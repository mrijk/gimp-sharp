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
	RGB color,
	ColorAreaType type);

      public GimpColorButton(string title,
			     int width,
			     int height,
			     RGB color,
			     ColorAreaType type) : 
	base(gimp_color_button_new(title, width, height, color, type))
      {
      }
    }
  }
