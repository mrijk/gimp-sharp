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
	  int imageID = base.Active;
	  return (imageID == -1) ? null : new Image(imageID);
	  }
      }

      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_image_combo_box_new (
	IntPtr constraint,
	IntPtr data);
    }
  }
