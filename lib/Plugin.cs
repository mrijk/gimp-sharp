using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp
  {
  abstract public class Plugin
  {
    protected string _name;
    bool _usesDrawable = false;
    bool _usesImage = false;
    
    protected Image _image;
    protected Drawable _drawable;

    public delegate void InitProc();
    public delegate void QuitProc();
    public delegate void QueryProc();
    public delegate void RunProc(string name, int n_params, 
				 IntPtr param,
				 ref int n_return_vals, 
				 ref GimpParam[] return_vals);
    [StructLayout(LayoutKind.Sequential)]
    public struct GimpPlugInInfo
    {
      public InitProc Init;
      public QuitProc Quit;
      public QueryProc Query;
      public RunProc Run;
    }

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

    protected void InstallProcedure(string name, string blurb, string help, 
				    string author, string copyright, 
				    string date, string menu_path, 
				    string image_types,
				    GimpParamDef[] _params, 
				    GimpParamDef[] return_vals)
    {
      _name = name;
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
      _name = name;
      GetRequiredParameters();
      
      int len = (_params == null) ? 0 : _params.Length;
      GimpParamDef[] args = new GimpParamDef[3 + len];
      
      args[0].type = PDBArgType.INT32;
      args[0].name = "run_mode";
      args[0].description = "Interactive, non-interactive";
      
      args[1].type = PDBArgType.IMAGE;
      args[1].name = "image";
      args[1].description = "Input image" + 
	((_usesImage) ?  "" : " (unused)");
      
      args[2].type = PDBArgType.DRAWABLE;
      args[2].name = "drawable";
      args[2].description = "Input drawable" + 
	((_usesDrawable) ?  "" : " (unused)");
      
      if (_params != null)
	_params.CopyTo(args, 3);
      
      gimp_install_procedure(name, blurb, help, author, copyright, date, 
			     menu_path, image_types, PDBProcType.PLUGIN, 
			     args.Length, 0, args, null);
    }
    
    void GetRequiredParameters()
    {
      foreach (MethodInfo method in 
	       GetType().GetMethods(BindingFlags.DeclaredOnly |
				    BindingFlags.NonPublic | 
				    BindingFlags.Instance))
	{
	if (method.Name.Equals("DoSomething"))
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

    protected bool MenuRegister(string procedure_name, string menu_path)
    {
      return gimp_plugin_menu_register(procedure_name, menu_path);
    }

    protected bool MenuRegister(string menu_path)
    {
      return MenuRegister(_name, menu_path);
    }

    Pixbuf LoadImageHelper(Assembly assembly, string filename)
    {
      Stream imageStream = assembly.GetManifestResourceStream(filename);

      PixbufLoader pixbufLoader = new Gdk.PixbufLoader();
      BinaryReader reader = new BinaryReader(imageStream);
      
      while (reader.PeekChar() != -1)
	{
	byte[] bytes = reader.ReadBytes(256);
	pixbufLoader.Write (bytes, (uint) bytes.Length);
	}
      
      Pixbuf pixbuf = pixbufLoader.Pixbuf;
      pixbufLoader.Close();

      return pixbuf;
    }

    protected Pixbuf LoadImage(string filename)
    {
      return LoadImageHelper(Assembly.GetCallingAssembly(), filename);
    }

    protected void IconRegister(string filename)
    {
      Pixbuf pixbuf = LoadImageHelper(Assembly.GetCallingAssembly(), filename);
      
      Pixdata data = new Pixdata();
      data.FromPixbuf(pixbuf, false);
      gimp_plugin_icon_register(_name, IconType.INLINE_PIXBUF, 
				data.Serialize());
    }

    abstract protected void Query();

    virtual protected void Run(string name, GimpParam[] inParam,
			       ref GimpParam[] outParam)
    {
      RunMode run_mode = (RunMode) inParam[0].data.d_int32;
      if (_usesImage)
	{
	_image = new Image(inParam[1].data.d_image);
	}
      if (_usesDrawable)
	{
	_drawable = new Drawable(inParam[2].data.d_drawable);
	}
      
      if (run_mode == RunMode.INTERACTIVE)
	{
	GetData();
	if (CreateDialog())
	  {
	  SetData();
	  }
	}
      else if (run_mode == RunMode.NONINTERACTIVE)
	{
	Console.WriteLine("RunMode.NONINTERACTIVE not implemented yet!");
	}
      else if (run_mode == RunMode.WITH_LAST_VALS)
	{
	GetData();
	CallDoSomething();
	}
      
      if (_usesDrawable)
	{
	_drawable.Detach();
	}
      
      outParam = new GimpParam[1];
      
      outParam[0].type = PDBArgType.STATUS;
      outParam[0].data.d_status = PDBStatusType.SUCCESS;
    }
    
    virtual protected bool CreateDialog() {return true;}

    GimpParam[] _origParam;

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, ref GimpParam[] return_vals)
    {
      _name = name;
      
      GetRequiredParameters();
      
      // Get parameters
      _origParam = new GimpParam[n_params];
      for (int i = 0; i < n_params; i++)
	{
	_origParam[i] = (GimpParam) Marshal.PtrToStructure(paramPtr,
							   typeof(GimpParam));
	Console.WriteLine(_origParam[i].type);
	paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(_origParam[i]));
	}
      
      Run(name, _origParam, ref return_vals);
      
      n_return_vals = return_vals.Length;
      Console.WriteLine("length: " + n_return_vals);
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

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id,
				string button1, Gtk.ResponseType action1,
				string button2, Gtk.ResponseType action2,
				string button3, Gtk.ResponseType action3)
    {
      _dialog = new GimpDialog(title, role, parent, flags, 
			       help_func, help_id, 
			       button1, action1,
			       button2, action2, 
			       button3, action3);
      return _dialog;
    }

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id)
    {
      return DialogNew (title, role, parent, flags, help_func, help_id,
			Stock.Help, ResponseType.Help,
			Stock.Cancel, ResponseType.Cancel,
			Stock.Ok, ResponseType.Ok);
    }

    void CallDoSomething()
    {
      int m_start = Environment.TickCount;

      GetRequiredParameters();
      
      if (_usesDrawable && _usesImage)
	{
	DoSomething(_image, _drawable);
	}
      else if (_usesDrawable)
	{
	DoSomething(_drawable);
	}
      else if (_usesImage)
	{
	DoSomething(_image);
	}
      else
	{
	DoSomething();
	}

      int m_time= Environment.TickCount - m_start;
      if (m_time<0)
	m_time &=0x7FFFFFFF;
      Console.WriteLine("Processing time {0:0.0} seconds.", 
			(double)m_time / 1000 );
    }
    
    virtual protected void DoSomething() {}
    virtual protected void DoSomething(Drawable drawable) {}
    virtual protected void DoSomething(Image image) {}
    virtual protected void DoSomething(Image image, Drawable drawable) {}

    virtual protected void GetParameters() {}

    virtual protected void DialogRun(ResponseType type) {}

    protected bool DialogRun()
    {
      while (true)
	{
	ResponseType type = _dialog.Run();
	if (type == ResponseType.Ok)
	  {
	  GetParameters();
	  CallDoSomething();
	  return true;
	  } 
	else if (type == ResponseType.Cancel || type == ResponseType.Close)
	  {
	  return false;
	  }
	else if (type == ResponseType.Help)
	  {
	  Console.WriteLine("Show help here!");
	  }
	else if (type >=0)		// User defined response
	  {
	  DialogRun(type);
	  Console.WriteLine("Type: " + type);
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
      string blurb;
      string help;
      string author;
      string copyright;
      string date;
      PDBProcType proc_type;
      int num_args;
      int num_values;
      IntPtr argsPtr;
      GimpParamDef[] return_vals;
    
      if (gimp_procedural_db_proc_info(name, 
				       out blurb, 
				       out help,
				       out author,
				       out copyright,
				       out date,
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

	_params[0].type = PDBArgType.INT32;
	_params[0].data.d_int32 = (Int32) RunMode.NONINTERACTIVE;	
	_params[1].type = PDBArgType.IMAGE;
	_params[1].data.d_image = image.ID;
	_params[2].type = PDBArgType.DRAWABLE;
	_params[2].data.d_drawable = drawable.ID;

	int i;

	for (i = 0; i < num_args; i++)
	  {
	  paramDef[i] = (GimpParamDef) Marshal.PtrToStructure(
	    argsPtr, typeof(GimpParamDef));
	  argsPtr = (IntPtr)((int)argsPtr + Marshal.SizeOf(paramDef[i]));
	  }

	i = 3;
	foreach (object obj in list)
	  {
	  switch (paramDef[i].type)
	    {
	    case PDBArgType.INT32:
	      _params[i].type = PDBArgType.INT32;
	      _params[i].data.d_int32 = (Int32) obj;
	      break;
	    default:
	      Console.WriteLine("Implement this!");
	      break;
	    }
	  i++;
	  }

	int n_return_vals;
	gimp_run_procedure2(name, out n_return_vals, num_args, _params);
	}
      else
	{
	Console.WriteLine(name + " not found!");
	}
    }

    [DllImport("libgimpui-2.0.so")]
    public static extern void gimp_ui_init(string prog_name, bool preview);
    [DllImport("libgimp-2.0.so")]
    public static extern bool gimp_plugin_menu_register(string procedure_name,
							string menu_path);
    [DllImport("libgimp-2.0.so")]
    public static extern bool gimp_plugin_icon_register(string procedure_name,
							IconType icon_type, 
							byte[] icon_data);
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
      PDBProcType	   type,
      int    n_params,
      int    n_return_vals,
      GimpParamDef[] _params,
      GimpParamDef[] return_vals);

    [DllImport("libgimp-2.0.so")]
    public static extern IntPtr gimp_run_procedure2(string name,
						    out int n_return_vals,
						    int n_params,
						    GimpParam[] _params);
    [DllImport("libgimp-2.0.so")]
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
      out GimpParamDef[] return_vals);

    [DllImport("libgimpwrapper.so")]
    public static extern int fnInitGimp(ref GimpPlugInInfo info, 
					int argc, string[] args);
    [DllImport("libgimpwrapper.so")]
    public static extern bool wrapper_set_data(string identifier,
					       byte[] data,
					       int bytes);
    [DllImport("libgimpwrapper.so")]
    public static extern bool wrapper_get_data(string identifier,
					       byte[] data);
    [DllImport("libgimpwrapper.so")]
    public static extern int wrapper_get_data_size(string identifier);
  }
  }
