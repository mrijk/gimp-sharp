using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class GimpFontSelectWidget : Widget
    {
      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_font_select_widget_new(
	string title,
	string font_name,
	IntPtr callback,
	IntPtr data);

      public GimpFontSelectWidget(string title, string font_name) : 
	base(gimp_font_select_widget_new(title, font_name, 
					 IntPtr.Zero, IntPtr.Zero))
      {
      }

      public void Close()
      {
      }

      public void Set(string font_name)
      {
      }
    }
  }
