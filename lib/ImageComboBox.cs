using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class ImageComboBox : IntComboBox
    {
      public ImageComboBox() : 
	base(gimp_image_combo_box_new(IntPtr.Zero, IntPtr.Zero))
      {
      }

      public new Image Active
      {
	get 
	  {
	  return new Image(base.Active);
	  }
      }

      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_image_combo_box_new (
	IntPtr constraint,
	IntPtr data);
    }
  }
