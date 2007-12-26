// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Plugin.cs
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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

using Mono.Unix;

using Gdk;
using Gtk;

namespace Gimp
{
  abstract public class Plugin
  {
    // Set of registered procedures
    protected ProcedureSet _procedures = new ProcedureSet();

    protected string _name;

    bool _usesDrawable = false;
    bool _usesImage = false;
    
    protected Image _image;
    protected Drawable _drawable;

    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void InitProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void QuitProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void QueryProc();
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void RunProc(string name, int n_params, 
				 IntPtr param,
				 ref int n_return_vals, 
				 out IntPtr return_vals);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct GimpPlugInInfo
    {
      public InitProc Init;
      public QuitProc Quit;
      public QueryProc Query;
      public RunProc Run;
    }
    
    static GimpPlugInInfo _info = new GimpPlugInInfo();

    public Plugin(string[] args, string package)
    {
      Catalog.Init(package, Gimp.LocaleDirectory);

      _info.Init = new InitProc(Init);
      _info.Quit = new QuitProc(Quit);
      _info.Query = new QueryProc(Query);
      _info.Run = new RunProc(Run);

      string[] progargs = new string[args.Length + 1];
      progargs[0] = "gimp-sharp";
      args.CopyTo (progargs, 1);

      gimp_main(ref _info, progargs.Length, progargs);
    }

    static protected string _(string s)
    {
      return Catalog.GetString(s);
    }

    public string Name
    {
      get {return _name;}
    }

    protected virtual void Init() 
    {
    }

    protected virtual void Quit() 
    {
    }

    protected abstract IEnumerable<Procedure> ListProcedures() ;

    void GetRequiredParameters()
    {
      foreach (MethodInfo method in 
	       GetType().GetMethods(BindingFlags.DeclaredOnly |
				    BindingFlags.Public | 
				    BindingFlags.NonPublic | 
				    BindingFlags.Instance))
	{
	  if (method.Name == "Render")
	    {
	      foreach (ParameterInfo parameter in method.GetParameters())
		{
		  if (parameter.ParameterType == typeof(Drawable))
		    {
		      _usesDrawable = true;
		    }
		  if (parameter.ParameterType == typeof(Image))
		    {
		      _usesImage = true;
		    }
		}
	    }
	}
    }

    protected Pixbuf LoadImage(string filename)
    {
      return new Pixbuf(Assembly.GetCallingAssembly(), filename);
    }

    virtual protected void Query()
    {
      GetRequiredParameters();

      foreach (Procedure procedure in ListProcedures())
	{
	  _procedures.Add(procedure);
	}

      _procedures.Install(_usesImage, _usesDrawable);
    }

    virtual protected void Run(string name, ParamDefList inParam,
			       out ParamDefList outParam)
    {
      RunMode run_mode = (RunMode) inParam[0].Value;
      if (_usesImage)
	{
	  _image = (Image) inParam[1].Value;
	}
      if (_usesDrawable)
	{
	  _drawable = (Drawable) inParam[2].Value;
	}
      
      if (run_mode == RunMode.Interactive)
	{
	  GetData();
	  _dialog = CreateDialog();
	  if (_dialog == null)
	    {
	      SetData();
	    }
	  else
	    {
	      _dialog.ShowAll();
	      if (DialogRun())
		{
		  SetData();
		}
	    }
	}
      else if (run_mode == RunMode.Noninteractive)
	{
	  if (ValidateParameters(inParam))
	    {
	      CallRender();
	    }
	}
      else if (run_mode == RunMode.WithLastVals)
	{
	  GetData();
	  CallRender();
	}
      
      if (_usesDrawable && _drawable != null)
	{
	  _drawable.Detach();
	}

      outParam = new ParamDefList(true);
      outParam.Add(new ParamDef(PDBStatusType.Success, typeof(PDBStatusType)));
    }

    virtual protected bool ValidateParameters(ParamDefList inParam)
    {
      foreach (SaveAttribute attribute in new SaveAttributeSet(GetType()))
	{
	  string name = attribute.Name;
	  if (name != null)
	    {
	      object value = inParam.GetValue(name);
	      if (value != null)
		{
		  Console.WriteLine("Setting " + name + ": " + value);
		  attribute.Field.SetValue(this, value);
		}
	    }
	}
      return true;
    }

    virtual protected GimpDialog CreateDialog() 
    {
      CallRender();
      return null;
    }

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, out IntPtr return_vals)
    {
      _name = name;

      GetRequiredParameters();

      ProcedureSet procedures = new ProcedureSet();

      foreach (Procedure p in ListProcedures())
	{
	  procedures.Add(p);
	}

      Procedure procedure = procedures[name];
      ParamDefList inParam = procedure.InParams;
      inParam.Marshall(paramPtr, n_params);

      ParamDefList outParam;
      Run(name, inParam, out outParam);
      outParam.Marshall(out return_vals, out n_return_vals);
    }

    protected void SetData()
    {
      PersistentStorage storage = new PersistentStorage(this);
      storage.SetData();
    }

    protected void GetData()
    {
      PersistentStorage storage = new PersistentStorage(this);
      storage.GetData();
    }

    GimpDialog _dialog;

    virtual protected GimpDialog DialogNew(string title,
				       string role,
				       IntPtr parent,
				       Gtk.DialogFlags flags,
				       GimpHelpFunc help_func,
				       string help_id,
				       string button1, ResponseType action1,
				       string button2, ResponseType action2,
				       string button3, ResponseType action3)
    {
      _dialog = new GimpDialog(title, role, parent, flags, 
			       help_func, help_id, 
			       button1, action1,
			       button2, action2, 
			       button3, action3);

      _dialog.SetTransient();
      return _dialog;
    }

    virtual protected GimpDialog DialogNew( string title,
					string role,
					IntPtr parent,
					Gtk.DialogFlags flags,
					GimpHelpFunc help_func,
					string help_id)
    {
      return DialogNew(title, role, parent, flags,
		       help_func, help_id,
		       GimpStock.Reset, (ResponseType) 1,
		       Stock.Cancel, ResponseType.Cancel,
		       Stock.Ok, ResponseType.Ok);
    }

    protected GimpDialog Dialog
    {
      get {return _dialog;}
    }

    void CallRender()
    {
      Stopwatch stopWatch = new Stopwatch();
      stopWatch.Start();

      GetRequiredParameters();

      if (_usesDrawable && _usesImage)
	{
	  Render(_image, _drawable);
	}
      else if (_usesDrawable)
	{
	  Render(_drawable);
	}
      else if (_usesImage)
	{
	  Render(_image);
	}
      else
	{
	  Render();
	}

      // TODO: maybe a check could/should be added here if we need to flush
      // the displays at all.
      Display.DisplaysFlush();

      stopWatch.Stop();
      TimeSpan ts = stopWatch.Elapsed;
      Console.WriteLine(String.Format("Processing time: {0:00}:{1:00}:{2:00}.{3:00}",
				      ts.Hours, ts.Minutes, ts.Seconds, 
				      ts.Milliseconds / 10));
    }
    
    virtual protected void Reset() {}
    virtual protected void Render() {}
    virtual protected void Render(Drawable drawable) {}
    virtual protected void Render(Image image) {}
    virtual protected void Render(Image image, Drawable drawable) {}

    virtual protected void GetParameters() {}

    virtual protected void DialogRun(ResponseType type) {}

    virtual protected bool OnClose()
    {
      return true;
    }

    protected bool DialogRun()
    {
      while (true)
	{
	  ResponseType type = _dialog.Run();
	  if (type == ResponseType.Ok)
	    {
	      GetParameters();
	      CallRender();
	      return true;
	    } 
	  else if (type == ResponseType.Cancel || type == ResponseType.Close
		   || type == ResponseType.DeleteEvent)
	    {
	      if (OnClose())
		{
		  return false;
		}
	    }
	  else if (type == ResponseType.Help)
	    {
	      Console.WriteLine("Show help here!");
	    }
	  else if (type == (ResponseType) 1)
	    {
	      Reset();
	    }
	  else if (type >=0)		// User defined response
	    {
	      DialogRun(type);
	      Console.WriteLine("Type: " + type);
	    }
	  else
	    {
	      Console.WriteLine("Unknown type: " + type);
	    }
	}
    }

    protected void RunProcedure(string name, params object[] list)
    {
      RunProcedure(name, _image, _drawable, list);
    }

    protected void RunProcedure(string name, Image image, Drawable drawable,
				params object[] list)
    {
      Procedure procedure = new Procedure(name);
      procedure.Run(image, drawable, list);
    }

    [DllImport("libgimpui-2.0-0.dll")]
    public static extern void gimp_ui_init(string prog_name, bool preview);
    [DllImport("libgimp-2.0-0.dll")]
    public static extern int gimp_main(ref GimpPlugInInfo info, 
				       int argc, string[] args);
  }
}
