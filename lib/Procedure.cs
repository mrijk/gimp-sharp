// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// Procedure.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using Gdk;

namespace Gimp
{
  public class Procedure
  {
    public string Name {get; set;}
    string _blurb;
    string _help;
    string _author;
    string _copyright;
    string _date;
    string _menu_path;
    string _image_types;

    // Fix me: this name looks to much like _menu_path
    public string MenuPath {get; set;}
    public string IconFile {get; set;}

    ParamDefList _inParams;
    ParamDefList _outParams;

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types,
		     ParamDefList inParams = null,
		     ParamDefList outParams = null)
    {
      Name = name;
      _blurb = blurb;
      _help = help;
      _author = author;
      _copyright = copyright;
      _date = date;
      _menu_path = menu_path;
      _image_types = image_types;
      _inParams = inParams ?? new ParamDefList();
      _outParams = outParams ?? new ParamDefList(false);
    }

    public Procedure(string name)
    {
      Name = name;
    }

    static public bool Exists(string name)
    {
      return ProceduralDb.ProcExists(name);
    }

    public void Install(bool usesImage, bool usesDrawable)
    {
      gimp_install_procedure(Name, _blurb, _help, _author, _copyright, _date, 
			     _menu_path, _image_types, PDBProcType.Plugin, 
			     _inParams.Count, _outParams.Count,
			     _inParams.GetGimpParamDef(), 
			     _outParams.GetGimpParamDef());
      MenuRegister();
      IconRegister();
    }

    public List<object> Run(params object[] list)
    {
      PDBProcType proc_type;
      int num_args;
      int num_values;
      IntPtr argsPtr;
      IntPtr return_vals;
    
      if (gimp_procedural_db_proc_info(Name, 
				       out _blurb, 
				       out _help,
				       out _author,
				       out _copyright,
				       out _date,
				       out proc_type,
				       out num_args,
				       out num_values,
				       out argsPtr,
				       out return_vals))
	{
	  var parameters = new GimpParamSet() {
	    new GimpParam(PDBArgType.Int32, RunMode.Noninteractive)
	  };

	  ParseParameters(parameters, num_args, argsPtr, list);
	  // Todo: destroy argsPtr!

	  return RunProcedure2(Name, parameters);
	}
      else
	{
	  Console.WriteLine(Name + " not found!");
	}
      return null;
    }

    List<object> ParseReturnArgs(IntPtr argsPtr, int num_args)
    {
      var list = new List<object>();
      var status = PDBStatusType.Success;
      var statusString = "";

      // first parameter contains Status!
      for (int i = 0; i < num_args; i++)
	{
	  var param = (GimpParam) Marshal.PtrToStructure(argsPtr, typeof(GimpParam));
	  argsPtr = (IntPtr)((int)argsPtr + Marshal.SizeOf(param));
	  switch (param.type) 
	    {
	    case PDBArgType.String:
	      statusString = Marshal.PtrToStringAnsi(param.data.d_string); 
	      break;
	    case PDBArgType.Status:
	      status = param.data.d_status;
	      break;
	    case PDBArgType.Image:
	      list.Add(new Image(param.data.d_image));
	      break;
	    default:
	      Console.WriteLine("ParseReturnArgs: Implement this: " + param.type);
	      break;
	  }
	}
      if (status != PDBStatusType.Success)
	{
	  throw new GimpSharpException(statusString);
	}

      // Fix me! gimp_destroy_params(argsPtr, num_args);
      return list;
    }

    public void Run(Image image, Drawable drawable, params object[] list)
    {
      PDBProcType proc_type;
      int num_args;
      int num_values;
      IntPtr argsPtr;
      IntPtr return_vals;
    
      if (gimp_procedural_db_proc_info(Name, 
				       out _blurb, 
				       out _help,
				       out _author,
				       out _copyright,
				       out _date,
				       out proc_type,
				       out num_args,
				       out num_values,
				       out argsPtr,
				       out return_vals))
	{	
	  Console.WriteLine("Run 1");
	  // First 3 parameters are default
	  var parameters = new GimpParamSet() {
	    new GimpParam(PDBArgType.Int32, RunMode.Noninteractive),
	    new GimpParam(PDBArgType.Image, image),
	    new GimpParam(PDBArgType.Drawable, drawable)
	  };
	  Console.WriteLine("Run 2");

	  ParseParameters(parameters, num_args, argsPtr, list);
	  // Todo: destroy argsPtr!
	  Console.WriteLine("Run 3");

	  RunProcedure2(Name, parameters);
	}
      else
	{
	  Console.WriteLine(Name + " not found!");
	}
    }

    List<object> RunProcedure2(string Name, GimpParamSet parameters)
    {
      int n_return_vals;
      IntPtr returnArgsPtr = gimp_run_procedure2(Name, out n_return_vals, 
						 parameters.Count, 
						 parameters.ToStructArray());
      Console.WriteLine("RunProcedure2");
      return ParseReturnArgs(returnArgsPtr, n_return_vals);
    }

    void ParseParameters(GimpParamSet parameters, int num_args, IntPtr argsPtr, 
			 params object[] list)
    {
      var paramDef = GetParamDef(num_args, argsPtr);

      int i = 0;

      foreach (object obj in list)
	{
	  Console.WriteLine("ParseParameters: " + paramDef[i].type);
	  switch (paramDef[i].type)
	    {
	    case PDBArgType.Int32:
	      parameters.Add(GimpParam.GetIntParam(obj));
	      break;
	    case PDBArgType.Float:
	      parameters.Add(GimpParam.GetFloatParam(obj));
	      break;
	    case PDBArgType.String:
	      parameters.Add(GimpParam.GetStringParam(obj));
	      break;
	    default:
	      Console.WriteLine("Procedure: Implement this: " +
				paramDef[i].type);
	      break;
	    }
	  i++;
	}
    }

    GimpParamDef[] GetParamDef(int num_args, IntPtr argsPtr)
    {
      var paramDef = new GimpParamDef[num_args];
      for (int i = 0; i < num_args; i++)
	{
	  paramDef[i] = (GimpParamDef) 
	    Marshal.PtrToStructure(argsPtr, typeof(GimpParamDef));
	  argsPtr = (IntPtr)((int)argsPtr + Marshal.SizeOf(paramDef[i]));
	}
      return paramDef;
    }

    public void MenuRegister()
    {
      if (MenuPath != null)
	{
	  if (!gimp_plugin_menu_register(Name, MenuPath))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void IconRegister()
    {
      if (IconFile != null)
	{
	  Pixbuf pixbuf;
	  try
	    {
	      pixbuf = new Pixbuf(Assembly.GetEntryAssembly(), IconFile);
	    }
	  catch
	    {
	      Console.WriteLine("Icon file: " + IconFile + " not found!");
	      return;
	    }
	  var data = new Pixdata();
	  data.FromPixbuf(pixbuf, false);
	  if (!gimp_plugin_icon_register(Name, IconType.InlinePixbuf, 
					 data.Serialize()))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public ParamDefList InParams
    {
      get {return _inParams;}
    }
    
    [DllImport("libgimp-2.0-0.dll")]
    public static extern void gimp_install_procedure(
      string name,
      string blurb,
      string help,
      string author,
      string copyright,
      string date,
      string menu_path,
      string image_types,
      PDBProcType type,
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_menu_register(string procedure_name,
							string menu_path);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_plugin_icon_register(string procedure_name,
							IconType icon_type, 
							byte[] icon_data);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern bool gimp_procedural_db_proc_info (
      string procedure,
      out string blurb,
      out string help,
      out string author,
      out string copyright,
      out string date,
      out PDBProcType proc_type,
      out int num_args,
      out int num_values,
      out IntPtr args,
      out IntPtr return_vals);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern IntPtr gimp_run_procedure2(string name,
						    out int n_return_vals,
						    int n_params,
						    IntPtr _params);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern void gimp_destroy_params(IntPtr _params, int n_params);
  }
}
