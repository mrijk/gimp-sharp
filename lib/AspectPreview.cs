using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class AspectPreview : GimpPreview
    {
      public AspectPreview(Drawable drawable, bool toggle) : 
	base(gimp_aspect_preview_new(drawable.Ptr, toggle))
      {
      }

      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_aspect_preview_new (IntPtr drawable,
						    bool toggle);
    }
  }
