// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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

namespace Gimp
  {
  public class Gimp
    {
    static public string Directory
      {
      get
          {
	  // IntPtr bla = glib_check_version(1, 2, 3);
	  // System.Console.WriteLine("version: " + Marshal.PtrToStringAuto(bla));

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

    [DllImport("libglib.dll")]
    static extern IntPtr glib_check_version(int x, int y, int z);

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_version();
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
    static extern bool gimp_install_cmap();
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_min_colors();
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

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_register_load_handler(string procedural_name,
                                                  string extensions, 
                                                  string prefixes);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_register_save_handler(string procedural_name,
                                                  string extensions, 
                                                  string prefixes);
    }
  }
