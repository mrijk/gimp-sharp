using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

using Gtk;
using GtkSharp;

namespace Gimp
  {
  abstract public class Plugin
  {
    protected string _name;
    bool _usesDrawable = false;
    bool _usesImage = false;
    
    protected Image _image;
    Drawable _drawable;

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

    [DllImport("libgimpui-2.0.so")]
    public static extern void gimp_ui_init(string prog_name, bool preview);

    [DllImport("libgimpwrapper.so")]
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

    [DllImport("libgimp-2.0.so")]
    public static extern bool gimp_plugin_menu_register(string procedure_name,
							string menu_path);


    static GimpPlugInInfo _info = new GimpPlugInInfo();
    static string[] myArgs = new String[6];

    IntPtr _dialogPtr;

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

    protected virtual void Init() 
    {
    }

    protected virtual void Quit() 
    {
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

    abstract protected void Query();

    abstract protected void Run(string name, GimpParam[] param,
				out GimpParam[] return_vals);

    virtual protected bool CreateDialog() {return true;}

    GimpParam[] _origParam;
    GimpParam[] _values = new GimpParam[1];

    public void Run(string name, int n_params, IntPtr paramPtr,
		    ref int n_return_vals, out GimpParam[] return_vals)
    {
      _name = name;

      // Get parameters
      _origParam = new GimpParam[n_params];
      for (int i = 0; i < n_params; i++)
	{
	_origParam[i] = (GimpParam) Marshal.PtrToStructure(paramPtr,
							   typeof(GimpParam));
	Console.WriteLine(_origParam[i].type);
	paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(_origParam[i]));
	}

      RunMode run_mode = (RunMode) _origParam[0].data.d_int32;
      _image = new Image(_origParam[1].data.d_image);
      _drawable = new Drawable(_origParam[2].data.d_drawable);
      
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

      _drawable.Detach();

      _values[0].type = PDBArgType.STATUS;
      _values[0].data.d_status = PDBStatusType.PDB_SUCCESS;
      return_vals = _values;

      n_return_vals = _values.Length;
    }

    [DllImport("libgimpwrapper.so")]
    public static extern bool wrapper_set_data(string identifier,
					       byte[] data,
					       int bytes);
    [DllImport("libgimpwrapper.so")]
    public static extern bool wrapper_get_data(string identifier,
					       byte[] data);
    [DllImport("libgimpwrapper.so")]
    public static extern int wrapper_get_data_size(string identifier);


    BinaryFormatter _formatter = new BinaryFormatter();

    protected void SetData()
    {
      MemoryStream memoryStream = new MemoryStream();

      foreach (FieldInfo field 
	       in GetType().GetFields(BindingFlags.Instance |  
				      BindingFlags.NonPublic | 
				      BindingFlags.Public))
	{
        foreach (object attribute in field.GetCustomAttributes(true))
	  {
	  if (attribute is SaveAttribute)
            {
	    _formatter.Serialize(memoryStream, field.GetValue(this));
            }
	  }
	}
      wrapper_set_data(_name, memoryStream.GetBuffer(),
		       (int) memoryStream.Length);		    
    }


    protected void GetData()
    {
      int size = wrapper_get_data_size(_name);
      if (size > 0)
	{
	byte[] data = new byte[size];
	wrapper_get_data(_name, data);

	MemoryStream memoryStream = new MemoryStream(data);

	foreach (FieldInfo field 
		 in GetType().GetFields(BindingFlags.Instance |  
					BindingFlags.NonPublic | 
					BindingFlags.Public))
	  {
	  foreach (object attribute in field.GetCustomAttributes(true))
	    {
	    if (attribute is SaveAttribute)
	      {
	      field.SetValue(this, _formatter.Deserialize(memoryStream));
	      }
	    }
	  }
	}
    }

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id,
				string button1, Gtk.ResponseType action1,
				string button2, Gtk.ResponseType action2)
    {
      _dialogPtr = gimp_dialog_new(title, role, parent, flags, 
				   help_func, help_id, 
				   button1, action1,
				   button2, action2, null);
      return new Dialog(_dialogPtr);
    }

    protected Dialog DialogNew( string title,
				string role,
				IntPtr parent,
				Gtk.DialogFlags flags,
				GimpHelpFunc help_func,
				string help_id)
    {
      return DialogNew (title, role, parent, flags, help_func, help_id,
			Stock.Cancel, ResponseType.Cancel,
			Stock.Ok, ResponseType.Ok);
    }

    void CallDoSomething()
    {
      GetRequiredParameters();
      
      if (_usesDrawable && _usesImage)
	{
	DoSomething(_drawable, _image);
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
    }
    
    virtual protected void DoSomething() {}
    virtual protected void DoSomething(Drawable drawable) {}
    virtual protected void DoSomething(Image image) {}
    virtual protected void DoSomething(Drawable drawable, Image image) {}

    virtual protected void GetParameters() {}

    protected bool DialogRun()
    {
      if (gimp_dialog_run(_dialogPtr) == ResponseType.Ok)
	{
	GetParameters();
	CallDoSomething();
	return true;
	}
      return false;
    }

    [DllImport("libgimp-2.0.so")]
    public static extern string gimp_directory();

    protected string GimpDirectory()
    {
      return gimp_directory();
    }

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

    protected void RunProcedure(string name, params object[] list)
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
	_params[1] = _origParam[1];
	_params[2] = _origParam[2];

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
  }
  }
