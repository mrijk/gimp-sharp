using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Gimp
    {
      static public string Directory()
      {
	IntPtr tmp = gimp_directory();
	return Marshal.PtrToStringAuto(tmp);
      }

      static public void RegisterLoadHandler(string procedural_name,
					     string extensions, 
					     string prefixes)
      {
	if (!gimp_register_load_handler(procedural_name, extensions,
					prefixes))
	  {
	  throw new Exception();
	  }
      }

      static public void RegisterSaveHandler(string procedural_name,
					     string extensions, 
					     string prefixes)
      {
	if (!gimp_register_save_handler(procedural_name, extensions,
					prefixes))
	  {
	  throw new Exception();
	  }
      }

      [DllImport("libgimpbase-2.0.so")]
      static extern IntPtr gimp_directory();
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_register_load_handler(string procedural_name,
						    string extensions, 
						    string prefixes);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_register_save_handler(string procedural_name,
						    string extensions, 
						    string prefixes);
    }
  }
