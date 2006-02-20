// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// GimpColorButton.cs
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
using System.Collections;
using System.Runtime.InteropServices;

using GLib;
using Gtk;

namespace Gimp
{
  public enum ColorAreaType
  {
    COLOR_AREA_FLAT = 0,
    COLOR_AREA_SMALL_CHECKS,
    COLOR_AREA_LARGE_CHECKS
  };

  public class GimpColorButton : Button
  {
    internal GimpColorButton(string title,
			   int width,
			   int height,
			   GimpRGB color,
			   ColorAreaType type) : 
      base(gimp_color_button_new(title, width, height, ref color, type))
    {
    }

    public GimpColorButton(string title,
			   int width,
			   int height,
			   RGB color,
			   ColorAreaType type) : 
      this(title, width, height, color.GimpRGB, type)
    {
    }

    public RGB Color
    {
      get
	{
	  GimpRGB rgb = new GimpRGB();
	  gimp_color_button_get_color(Handle, ref rgb);
	  return new RGB(rgb);
	}
      set
	{
	  GimpRGB rgb = value.GimpRGB;
	  gimp_color_button_set_color(Handle, ref rgb);
	}
    }

    public bool Alpha
    {
      get
	{
	  return gimp_color_button_has_alpha(Handle);
	}
    }

    public ColorAreaType Type
    {
      set
	{
	  gimp_color_button_set_type(Handle, value);
	}
    }

    public bool Update
    {
      get
	{
	  return gimp_color_button_get_update(Handle);
	}
      set
	{
	  gimp_color_button_set_update(Handle, value);
	}
    }

    [GLib.Signal("color_changed")]
    public event EventHandler ColorChanged {
      add {
	GLib.Signal sig = GLib.Signal.Lookup (this, "color_changed");
	sig.AddDelegate (value);
      }
      remove {
	GLib.Signal sig = GLib.Signal.Lookup (this, "color_changed");
	sig.RemoveDelegate (value);     
      }
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_color_button_new(
					       string title,
					       int width,
					       int height,
					       ref GimpRGB color,
					       ColorAreaType type);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_color_button_set_color(IntPtr button,
						   ref GimpRGB color);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_color_button_get_color(IntPtr button,
						   ref GimpRGB color);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_color_button_has_alpha(IntPtr button);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_color_button_set_type(IntPtr button,
						  ColorAreaType type);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_color_button_set_update(IntPtr button,
						    bool continuous);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_color_button_get_update(IntPtr button);
  }
}
