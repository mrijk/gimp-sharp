using System;
using System.Runtime.InteropServices;

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
      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_color_button_new(
	string title,
	int width,
	int height,
	ref GimpRGB color,
	ColorAreaType type);

      public GimpColorButton(string title,
			     int width,
			     int height,
			     GimpRGB color,
			     ColorAreaType type) : 
	base(gimp_color_button_new(title, width, height, ref color, type))
      {
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_color(IntPtr button,
						     ref GimpRGB color);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_get_color(IntPtr button,
						     ref GimpRGB color);

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

      [DllImport("libgimpwidgets-2.0.so")]
      extern static bool gimp_color_button_has_alpha(IntPtr button);
      public bool Alpha
      {
	get
	    {
	    return gimp_color_button_has_alpha(Handle);
	    }
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_type(IntPtr button,
						    ColorAreaType type);
      public ColorAreaType Type
      {
	set
	    {
	    gimp_color_button_set_type(Handle, value);
	    }
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_update(IntPtr button,
						      bool continuous);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static bool gimp_color_button_get_update(IntPtr button);

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
    }
  }
