using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class PreviewArea : DrawingArea
    {
      public PreviewArea() : base(gimp_preview_area_new())
      {
      }

      public void Draw(int x, int y, int width, int height,
		       ImageType type, byte[] buf, int rowstride)
      {
	gimp_preview_area_draw(Handle, x, y, width, height, 
			       type, buf, rowstride);
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_preview_area_new ();
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_preview_area_draw(
	IntPtr area,
	int x,
	int y,
	int width,
	int height,
	ImageType type,
	byte[] buf,
	int rowstride);
    }
  }
