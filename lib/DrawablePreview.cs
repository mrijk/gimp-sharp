using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class DrawablePreview : ScrolledPreview
    {
      public DrawablePreview(Drawable drawable, bool toggle) :
	base(gimp_drawable_preview_new(drawable.Ptr, toggle))
      {
      }

      public Drawable Drawable
      {
	get {return new Drawable(gimp_drawable_preview_get_drawable(Handle));}
      }

      public void DrawRegion(PixelRgn region)
      {
	GimpPixelRgn pr = region.PR;
	gimp_drawable_preview_draw_region(Handle, ref pr);
      }

      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_drawable_preview_new(IntPtr drawable, 
						     bool toggle);
      [DllImport("libgimpui-2.0.so")]
      extern static IntPtr gimp_drawable_preview_get_drawable(IntPtr preview);
      [DllImport("libgimpui-2.0.so")]
      extern static void gimp_drawable_preview_draw_region(IntPtr preview,
							   ref GimpPixelRgn region);
    }
  }
