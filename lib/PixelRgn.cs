using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  [StructLayout(LayoutKind.Sequential)]
  struct GimpPixelRgn
  {
    // byte[]       data;          /* pointer to region data */
    public IntPtr		 data;
    IntPtr		 drawable;      /* pointer to drawable */
    public uint         bpp;           /* bytes per pixel */
    public uint         rowstride;     /* bytes per pixel row */
    public uint         x, y;          /* origin */
    public uint         w, h;          /* width and height of region */
    // uint         dirty : 1;     /* will this region be dirtied? */
    // uint         shadow : 1;    /* will this region use the shadow or normal tiles */
    public uint		 dirty_shadow;
    uint         process_count; /* used internally */
  }

    public class PixelRgn
    {
      [DllImport("libgimp-2.0.so")]
      static extern void gimp_pixel_rgn_init (ref GimpPixelRgn pr,
					      IntPtr drawable,
					      int x,
					      int y,
					      int width,
					      int height,
					      bool dirty,
					      bool shadow);

      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_rgns_register  (int nrgns, ref GimpPixelRgn pr);

      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_rgns_register  (int nrgns, ref GimpPixelRgn pr1,
						      ref GimpPixelRgn pr2);

      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_rgns_process (IntPtr pri_ptr);

      GimpPixelRgn pr = new GimpPixelRgn();

      public PixelRgn(Drawable drawable, int x,
		      int y,
		      int width,
		      int height,
		      bool dirty,
		      bool shadow)
      {
	gimp_pixel_rgn_init (ref pr, drawable.Ptr, x, y, width, height, dirty, shadow);
      }

      public static IntPtr Register(PixelRgn rgn)
      {
	return gimp_pixel_rgns_register(1, ref rgn.pr);
      }

      public static IntPtr Register(PixelRgn rgn1, PixelRgn rgn2)
      {
	return gimp_pixel_rgns_register(2, ref rgn1.pr, ref rgn2.pr);
      }

      public static IntPtr Process(IntPtr priPtr)
      {
	return gimp_pixel_rgns_process(priPtr);
      }

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_pixel_rgn_get_pixel (ref GimpPixelRgn  pr,
						   byte[] buf,
						   int        x,
						   int        y);

      public void GetPixel(byte[] buf, int x, int y)
      {
	gimp_pixel_rgn_get_pixel(ref pr, buf, x, y);
      }

      public int X
      {
	get {return (int) pr.x;}
      }

      public int Y
      {
	get {return (int) pr.y;}
      }

      public int W
      {
	get {return (int) pr.w;}
      }

      public int H
      {
	get {return (int) pr.h;}
      }

      public IntPtr Data
      {
	get {return pr.data;}
      }
			
      public int Bpp
      {
	get {return (int) pr.bpp;}
      }

      public int Rowstride
      {
	get {return (int) pr.rowstride;}
      }
    }
  }
