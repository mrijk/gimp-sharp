using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  public class Gimp
    {
    static public string Directory
      {
      get
          {
          IntPtr tmp = gimp_directory();
          return Marshal.PtrToStringAuto(tmp);
          }
      }

    static public string Version
      {
      get
          {
          IntPtr tmp = gimp_version();
          return Marshal.PtrToStringAuto(tmp);
          }
      }

    static public uint TileWidth
      {
      get {return gimp_tile_width();}
      }

    static public uint TileHeight
      {
      get {return gimp_tile_height();}
      }

    static public int ShmID
      {
      get {return gimp_shm_ID();}
      }

    static public IntPtr ShmAddr
      {
      get {return gimp_shm_addr();}
      }

    static public double Gamma
      {
      get {return gimp_gamma();}
      }

    static public bool InstallCmap
      {
      get {return gimp_install_cmap();}
      }

    static public int Mincolors
      {
      get {return gimp_min_colors();}
      }

    static public bool ShowToolTips
      {
      get {return gimp_show_tool_tips();}
      }

    static public bool ShowHelpButton
      {
      get {return gimp_show_help_button();}
      }

    static public CheckSize CheckSize
      {
      get {return gimp_check_size();}
      }

    static public CheckType CheckType
      {
      get {return gimp_check_type();}
      }

    static public Int32 DefaultDisplay
      {
      get {return gimp_default_display();}
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

    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_version();
    [DllImport("libgimp-2.0.so")]
    static extern uint gimp_tile_width();
    [DllImport("libgimp-2.0.so")]
    static extern uint gimp_tile_height();
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_shm_ID();
    [DllImport("libgimp-2.0.so")]
    static extern IntPtr gimp_shm_addr();
    [DllImport("libgimp-2.0.so")]
    static extern double gimp_gamma();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_install_cmap();
    [DllImport("libgimp-2.0.so")]
    static extern int gimp_min_colors();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_show_tool_tips();
    [DllImport("libgimp-2.0.so")]
    static extern bool gimp_show_help_button();
    [DllImport("libgimp-2.0.so")]
    static extern CheckSize gimp_check_size();
    [DllImport("libgimp-2.0.so")]
    static extern CheckType gimp_check_type();
    [DllImport("libgimp-2.0.so")]
    static extern Int32 gimp_default_display();

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
