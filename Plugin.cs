using System;
using System.IO;
using System.Runtime.InteropServices;

using Gtk;
using GtkSharp;

namespace Gimp
  {
  [StructLayout(LayoutKind.Sequential)]
    public struct GimpRGB
    {
      double r, g, b, a;
    }

  [StructLayout(LayoutKind.Sequential)]
    public struct GimpParamRegion
    {
      public Int32 x;
      public Int32 y;
      public Int32 width;
      public Int32 height;
    };

  [StructLayout(LayoutKind.Sequential)]
    public struct GimpParasite
    {
      Int32 name;  
      Int32   flags; 
      Int32   size;  
      IntPtr  data;  
    };

  [StructLayout(LayoutKind.Explicit)]
    public struct GimpParamData
    {
      [FieldOffset(0)]
      public Int32	d_int32;
      [FieldOffset(0)]
      public Int16	d_int16;
      [FieldOffset(0)]
      public byte	d_int8;
      [FieldOffset(0)]
      public float d_float;
      [FieldOffset(0)]
      GimpRGB         d_color;
      [FieldOffset(0)]
      public GimpParamRegion d_region;
      [FieldOffset(0)]
      public Int32    d_image;
      [FieldOffset(0)]
      public Int32    d_drawable;
      [FieldOffset(0)]
      GimpParasite    d_parasite;
      [FieldOffset(0)]
      public Int32	d_status;	// Fix me!
    };

    public enum PDBProcType
    {
      INTERNAL,
      PLUGIN,
      EXTENSION,
      TEMPORARY
    }

    public enum GimpPDBArgType
      {
	INT32,
	INT16,
	INT8,
	FLOAT,
	STRING,
	INT32ARRAY,
	INT16ARRAY,
	INT8ARRAY,
	FLOATARRAY,
	STRINGARRAY,
	COLOR,
	REGION,
	DISPLAY,
	IMAGE,
	LAYER,
	CHANNEL,
	DRAWABLE,
	SELECTION,
	BOUNDARY,
	PATH,
	PARASITE,
	STATUS,
	END
      };

  [StructLayout(LayoutKind.Sequential)]
  public struct GimpParam
  {
    public GimpPDBArgType type;
    public GimpParamData  data;
  };

  abstract public class Plugin
  {
    public delegate void GimpMainProc(ref IntPtr info);

    public delegate void InitProc();
    public delegate void QuitProc();
    public delegate void QueryProc();
    public delegate void RunProc(string name, int n_params, 
				 IntPtr param,
				 ref int n_return_vals, 
				 out GimpParam[] return_vals);

    [StructLayout(LayoutKind.Sequential)]
    public struct GimpPlugInInfo
    {
      public InitProc Init;
      public QuitProc Quit;
      public QueryProc Query;
      public RunProc Run;
    }

    [DllImport("libgimp-2.0.so")]
    public static extern int gimp_main(ref IntPtr info, int argc, string[] args);

    [DllImport("libgimpui-2.0.so")]
    public static extern void gimp_ui_init(string prog_name, bool preview);

    [DllImport("gimpwrapper.so")]
    public static extern int fnInitGimp(ref GimpPlugInInfo info, 
					int argc, string[] args);

    protected delegate void GimpHelpFunc(string help_id);

    [DllImport("libgimpwidgets-2.0.so")]
    static extern IntPtr gimp_dialog_new(
      string title,
      string role,
      IntPtr parent,
      Gtk.DialogFlags  flags,
      GimpHelpFunc    help_func,
      string    help_id,
      string button1, Gtk.ResponseType acion1,
      string button2, Gtk.ResponseType action2,
      string end);

    [DllImport("libgimpwidgets-2.0.so")]
    static extern Gtk.ResponseType gimp_dialog_run(IntPtr dialog);

    [StructLayout(LayoutKind.Sequential)]
    public struct GimpParamDef
    {
      public GimpPDBArgType type;
      public string name;
      public string description;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
      public int x, y;
    }

    [DllImport("libgimp-2.0.so")]
    public static extern void gimp_install_procedure(
      string name,
      string blurb,
      string help,
      string author,
      string copyright,
      string date,
      string menu_path,
      string image_types,
      int	   type,	// Fix me!
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);

    static GimpPlugInInfo _info = new GimpPlugInInfo();
    static string[] myArgs = new String[6];

    public Plugin(string[] args)
    {
      _info.Init = new InitProc(Init);
      _info.Quit = new QuitProc(Quit);
      _info.Query = new QueryProc(Query);
      _info.Run = new RunProc(Run);

      string[] progargs = new string[args.Length + 1];
      progargs[0] = "gimp-sharp";
      args.CopyTo (progargs, 1);

      fnInitGimp(ref _info, progargs.Length, progargs);

      Close();		
    }

    void GimpMain(ref IntPtr info)
    {	
      gimp_main(ref info, myArgs.Length, myArgs);
    }

    public void Close()
    {
    }

    protected virtual void Init() 
    {
    }

    protected virtual void Quit() 
    {
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
      PDBProcType	   type,
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types,
				    GimpParamDef[] _params, 
				    GimpParamDef[] return_vals)
    {
      gimp_install_procedure(name, blurb, help, author, copyright, date, 
			     menu_path, image_types, PDBProcType.PLUGIN, 
			     _params.Length, return_vals.Length, _params, 
			     return_vals);
    }

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types,
				    GimpParamDef[] _params)
    {
      gimp_install_procedure(name, blurb, help, author, copyright, date, 
			     menu_path, image_types, PDBProcType.PLUGIN, 
			     _params.Length, 0, _params, null);
    }

    abstract protected void Query();

    abstract protected void Run(string name, GimpParam[] param,
				out GimpParam[] return_vals);

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, out GimpParam[] return_vals)
    {
      // Get parameters
      GimpParam[] param = new GimpParam[n_params];
      for (int i = 0; i < n_params; i++)
	{
	param[i] = (GimpParam) Marshal.PtrToStructure(paramPtr,
						      typeof(GimpParam));
	Console.WriteLine(param[i].type);
	paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(param[i]));
	}

      Run(name, param, out return_vals);
      n_return_vals = return_vals.Length;
    }

    protected IntPtr DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id,
				string button1, Gtk.ResponseType action1,
				string button2, Gtk.ResponseType action2)
    {
      return gimp_dialog_new(title, role, parent, flags, help_func, help_id, 
			     button1, action1,
			     button2, action2, null);
    }

    abstract protected void DoSomething();

    protected void DialogRun(IntPtr dialog)
    {
      if (gimp_dialog_run(dialog) == ResponseType.Ok)
	{
	DoSomething();
	}
    }
  }
  }
