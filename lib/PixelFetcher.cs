using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class PixelFetcher
    {
      IntPtr _ptr;

      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_fetcher_new (IntPtr drawable,
						   bool shadow);
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_fetcher_destroy (IntPtr drawable);
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_fetcher_get_pixel (IntPtr pf,
							 int x,
							 int y,
							 byte[] pixel);
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_pixel_fetcher_put_pixel (IntPtr pf,
							 int x,
							 int y,
							 byte[] pixel);


      public PixelFetcher(Drawable drawable, bool shadow)
      {
	_ptr = gimp_pixel_fetcher_new (drawable.Ptr, shadow);
      }

      public void Destroy()
      {
	gimp_pixel_fetcher_destroy (_ptr);
      }

      public void GetPixel(int x, int y, byte[] pixel)
      {
	gimp_pixel_fetcher_get_pixel (_ptr, x, y, pixel);
      }

      public void PutPixel(int x, int y, byte[] pixel)
      {
	gimp_pixel_fetcher_put_pixel (_ptr, x, y, pixel);
      }
    }
  }
