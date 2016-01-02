// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// Gimp.cs
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
using System.Runtime.InteropServices;

using GLib;

namespace Gimp
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpColorConfig 
  {
    public ColorManagementMode  mode;
    public string             	rgb_profile;
    public string              	cmyk_profile;
    public string              	display_profile;
    public bool               	display_profile_from_gdk;
    public string             	printer_profile;
    public ColorRenderingIntent	display_intent;
    public ColorRenderingIntent simulation_intent;
    
    public string               display_module;
    
    public bool               	simulation_gamut_check;
    public GimpRGB             	out_of_gamut_color;
  };

  public enum ColorManagementMode
  {
    Off,
    Display,
    Softproof
  };

  public enum ColorRenderingIntent
  {
    Perceptual,
    RelativeColorimetric,
    Saturation,
    AbsoluteColorimetric
  };

  public static class Gimp
  {
    static public Version Version => new Version(gimp_version());
    static public int PID => gimp_getpid();
    static public string PdbError => gimp_get_pdb_error();
    static public uint TileWidth => gimp_tile_width();
    static public uint TileHeight => gimp_tile_height();
    static public int ShmID => gimp_shm_ID();
    static public IntPtr ShmAddr => gimp_shm_addr();
    static public double Gamma => gimp_gamma();
    static public bool ShowToolTips => gimp_show_tool_tips();
    static public bool ShowHelpButton => gimp_show_help_button();
    static public CheckSize CheckSize => gimp_check_size();
    static public CheckType CheckType => gimp_check_type();
    static public Int32 DefaultDisplay => gimp_default_display();
    static public string DefaultComment => gimp_get_default_comment();
    static public Unit DefaultUnit => gimp_get_default_unit();
    static public string ModuleLoadInhibit => gimp_get_module_load_inhibit();

    static public string Directory
    {
      get
      {
        IntPtr tmp = gimp_directory();
        return Marshaller.FilenamePtrToString(tmp);
      }
    }

    static public void AttachParasite(Parasite parasite)
    {
      if (!gimp_attach_parasite(parasite.Ptr))
        {
	  throw new GimpSharpException();
        }
    }

    static public void DetachParasite(Parasite parasite)
    {
      if (!gimp_detach_parasite(parasite.Name))
        {
	  throw new GimpSharpException();
        }
    }

    static public Parasite GetParasite(string name)
    {
      IntPtr found = gimp_get_parasite(name);
      return (found == IntPtr.Zero) ? null : new Parasite(found);
    }

    static public ParasiteList ParasiteList
    {
      get
	{
	  int numParasites;

	  IntPtr ptr = gimp_get_parasite_list(out numParasites);

	  var parasites = new ParasiteList();

	  for (int i = 0; i < numParasites; i++)
	    {
	      IntPtr tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
	      var name = (string) Marshal.PtrToStringAnsi(tmp);
	      parasites.Add(GetParasite(name));
	      ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
	    }
	  return parasites;
	}
    }

    static public void Quit()
    {
      gimp_quit();
    }

    static public void RegisterLoadHandler(string procedural_name,
					   string extensions, 
					   string prefixes)
    {
      if (!gimp_register_load_handler(procedural_name, extensions,
				      prefixes))
	{
	  throw new GimpSharpException();
	}
    }

    static public void RegisterSaveHandler(string procedural_name,
					   string extensions, 
					   string prefixes)
      {
	if (!gimp_register_save_handler(procedural_name, extensions,
					prefixes))
	  {
	    throw new GimpSharpException();
	  }
      }

    static public void RegisterFileHandlerMime(string procedural_name,
					       string mime_type)
      {
	if (!gimp_register_file_handler_mime(procedural_name, mime_type))
      {
        throw new GimpSharpException();
      }
      }

    static public void RegisterThumbnailLoader(string load_proc,
					       string thumb_proc)
      {
	if (!gimp_register_thumbnail_loader(load_proc, thumb_proc))
	  {
	    throw new GimpSharpException();
	  }
    }

    static public void StandardHelpFunc(string help_id, IntPtr help_data)
    {
      gimp_standard_help_func(help_id, help_data);
    }

    // Implementation of gimprc

    static public string RcQuery(string token)
    {
      return gimp_gimprc_query(token);
    }

    static public void RcSet(string token, string value)
    {
      IntPtr tmp = Marshaller.StringToPtrGStrdup(value);
      if (!gimp_gimprc_set(token, tmp))
	{
	  Marshaller.Free(tmp);
	  throw new GimpSharpException();
	}
      Marshaller.Free(tmp);
    }

    // Fix me: not completely implemented yet

    static public IntPtr ColorConfiguration
    {
      get
	{
	  return gimp_get_color_configuration();
	}
    }

    static public Resolution MonitorResolution
    {
      get
	{
	  double xres, yres;
	  if (!gimp_get_monitor_resolution(out xres, out yres))
	    {
	      throw new GimpSharpException();
	    }
	  return new Resolution(xres, yres);
	}
    }

    static public string ThemeDirectory
    {
      get {return FilenamePtrToString(gimp_get_theme_dir());}
    }

    static public string LocaleDirectory
    {
      get {return FilenamePtrToString(gimp_locale_directory());}
    }

    static public string PluginDirectory
    {
      get {return FilenamePtrToString(gimp_plug_in_directory());}
    }

    static string FilenamePtrToString(IntPtr filenamePtr)
    {
      return Marshaller.FilenamePtrToString(filenamePtr);
    }

    static public void DisplaysFlush()
    {
      Display.DisplaysFlush();
    }
    
    [DllImport("libgimp-2.0-0.dll")]
      static extern void gimp_quit();
    [DllImport("libgimp-2.0-0.dll")]
      static extern string gimp_version();
    [DllImport("libgimp-2.0-0.dll")]
      static extern int gimp_getpid();
    [DllImport("libgimp-2.0-0.dll")]
      extern static bool gimp_attach_parasite(IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
      extern static bool gimp_detach_parasite(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_get_parasite(string name);
    [DllImport("libgimp-2.0-0.dll")]
      static extern IntPtr gimp_get_parasite_list(out int num_parasites);
    [DllImport("libgimp-2.0-0.dll")]
      static extern string gimp_get_pdb_error();
    [DllImport("libgimp-2.0-0.dll")]
      static extern uint gimp_tile_width();
    [DllImport("libgimp-2.0-0.dll")]
      static extern uint gimp_tile_height();
    [DllImport("libgimp-2.0-0.dll")]
      static extern int gimp_shm_ID();
    [DllImport("libgimp-2.0-0.dll")]
      static extern IntPtr gimp_shm_addr();
    [DllImport("libgimp-2.0-0.dll")]
      static extern double gimp_gamma();
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_show_tool_tips();
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_show_help_button();
    [DllImport("libgimp-2.0-0.dll")]
      static extern CheckSize gimp_check_size();
    [DllImport("libgimp-2.0-0.dll")]
      static extern CheckType gimp_check_type();
    [DllImport("libgimp-2.0-0.dll")]
      static extern Int32 gimp_default_display();

    [DllImport("libgimpbase-2.0-0.dll")]
      static extern IntPtr gimp_directory();
    [DllImport("libgimpbase-2.0-0.dll")]
      static extern IntPtr gimp_locale_directory();
    [DllImport("libgimpbase-2.0-0.dll")]
      static extern IntPtr gimp_plug_in_directory();

    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_register_load_handler(string procedural_name,
						    string extensions, 
						    string prefixes);
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_register_save_handler(string procedural_name,
						    string extensions, 
						    string prefixes);
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_register_file_handler_mime(string procedural_name,
							 string mime_type);
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_register_thumbnail_loader(string load_proc,
							string thumb_proc);
    [DllImport("libgimpwidgets-2.0-0.dll")]
      static extern void gimp_standard_help_func(string help_id,
						 IntPtr help_data);

    [DllImport("libgimp-2.0-0.dll")]
      static extern string gimp_gimprc_query(string token);
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_gimprc_set(string token, IntPtr value);
    [DllImport("libgimp-2.0-0.dll")]
      static extern IntPtr gimp_get_color_configuration();
    [DllImport("libgimp-2.0-0.dll")]
      static extern string gimp_get_default_comment();
    [DllImport("libgimp-2.0-0.dll")]
      static extern Unit gimp_get_default_unit();
    [DllImport("libgimp-2.0-0.dll")]
      static extern string gimp_get_module_load_inhibit();
    [DllImport("libgimp-2.0-0.dll")]
      static extern bool gimp_get_monitor_resolution(out double xres,
						     out double yres);
    [DllImport("libgimp-2.0-0.dll")]
      static extern IntPtr gimp_get_theme_dir();
  }
}
