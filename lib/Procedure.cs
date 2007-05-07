// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
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
using System.Reflection;
using System.Runtime.InteropServices;

using Gdk;

namespace Gimp
{
  public class Procedure
  {
    string _name;
    string _blurb;
    string _help;
    string _author;
    string _copyright;
    string _date;
    string _menu_path;
    string _image_types;

    string _menuPath;	// Fix me: this name looks to much like _menu_path
    string _iconFile;

    ParamDefList _inParams;
    ParamDefList _outParams;

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types,
		     ParamDefList inParams,
		     ParamDefList outParams)
    {
      _name = name;
      _blurb = blurb;
      _help = help;
      _author = author;
      _copyright = copyright;
      _date = date;
      _menu_path = menu_path;
      _image_types = image_types;
      _inParams = inParams;
      _outParams = outParams;
    }

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types,
		     ParamDefList inParams) : 
      this(name, blurb, help, author, copyright, date, menu_path,
	   image_types, inParams, null)
    {
    }

    public Procedure(string name, string blurb, string help, 
		     string author, string copyright, 
		     string date, string menu_path, 
		     string image_types) :
      this(name, blurb, help, author, copyright, date, menu_path,
	   image_types, new ParamDefList(), null)
    {
    }

    public Procedure(string name)
    {
      _name = name;
    }

    public void Install(bool usesImage, bool usesDrawable)
    {
      GimpParamDef[] args = _inParams.GetGimpParamDef(usesImage, usesDrawable);

      GimpParamDef[] returnVals;
      int returnLen;
      if (_outParams == null)
	{
	  returnVals = null;
	  returnLen = 0;
	}
      else
	{
	  returnVals = _outParams.GetGimpParamDef(usesImage, usesDrawable);
	  returnLen = returnVals.Length;
	}
      
      gimp_install_procedure(_name, _blurb, _help, _author, _copyright, _date, 
			     _menu_path, _image_types, PDBProcType.Plugin, 
			     args.Length, returnLen, args, returnVals);
      MenuRegister();
      IconRegister();
    }

    public void Run(Image image, Drawable drawable, params object[] list)
    {
      PDBProcType proc_type;
      int num_args;
      int num_values;
      IntPtr argsPtr;
      IntPtr return_vals;
    
      if (gimp_procedural_db_proc_info(_name, 
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
	  // Get parameter types
	  GimpParamDef[] paramDef = new GimpParamDef[num_args];
	  GimpParam[] _params = new GimpParam[num_args];
	  
	  // First 3 parameters are default

	  _params[0].type = PDBArgType.Int32;
	  _params[0].data.d_int32 = (Int32) RunMode.Noninteractive;	
	  _params[1].type = PDBArgType.Image;
	  _params[1].data.d_image = image.ID;
	  _params[2].type = PDBArgType.Drawable;
	  _params[2].data.d_drawable = drawable.ID;

	  int i;

	  for (i = 0; i < num_args; i++)
	    {
	      paramDef[i] = (GimpParamDef) 
		Marshal.PtrToStructure(argsPtr, typeof(GimpParamDef));
	      argsPtr = (IntPtr)((int)argsPtr + Marshal.SizeOf(paramDef[i]));
	    }

	  i = 3;
	  foreach (object obj in list)
	    {
	      switch (paramDef[i].type)
		{
		case PDBArgType.Int32:
		  _params[i].type = PDBArgType.Int32;
		  if (obj is bool)
		    {
		      Int32 val = ((bool) obj) ? 1 : 0;
		      _params[i].data.d_int32 = val;
		    }
		  else
		    {
		      _params[i].data.d_int32 = (Int32) obj;
		    }
		  break;
		case PDBArgType.Float:
		  _params[i].type = PDBArgType.Float;
		  if (obj is int)
		    {
		      _params[i].data.d_float = (double) (int) obj;
		    }
		  else
		    {
		      _params[i].data.d_float = (double) obj;
		    }
		  break;
		default:
		  Console.WriteLine("Implement this!");
		  break;
		}
	      i++;
	    }

	  int n_return_vals;
	  gimp_run_procedure2(_name, out n_return_vals, i, _params);
	}
      else
	{
	  Console.WriteLine(_name + " not found!");
	}
    }

    public void MenuRegister()
    {
      if (_menuPath != null)
	{
	  if (!gimp_plugin_menu_register(_name, _menuPath))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void IconRegister()
    {
      if (_iconFile != null)
	{
	  Pixbuf pixbuf = new Pixbuf(Assembly.GetEntryAssembly(), _iconFile);
	  
	  Pixdata data = new Pixdata();
	  data.FromPixbuf(pixbuf, false);
	  if (!gimp_plugin_icon_register(_name, IconType.InlinePixbuf, 
					 data.Serialize()))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public string Name
    {
      get {return _name;}
      set {_name = value;}
    }

    public string MenuPath
    {
      set {_menuPath = value;}
    }

    public string IconFile
    {
      set {_iconFile = value;}
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
						    GimpParam[] _params);
  }
}
