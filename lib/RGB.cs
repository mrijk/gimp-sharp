using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class RGB
    {
      GimpRGB _rgb = new GimpRGB();

      [DllImport("libgimpcolor-2.0.so")]
      static extern void gimp_rgb_set(ref GimpRGB rgb,
				      double red,
				      double green,
				      double blue);
      [DllImport("libgimp-2.0.so")]
      static extern void gimp_rgb_set_uchar (ref GimpRGB rgb,
					     byte red,
					     byte green,
					     byte blue);

      public RGB(double red, double green, double blue)
      {
	gimp_rgb_set(ref _rgb, red, green, blue);
      }

      public RGB(byte red, byte green, byte blue)
      {
	gimp_rgb_set_uchar(ref _rgb, red, green, blue);
      }

      public GimpRGB GimpRGB
      {
	get {return _rgb;}
      }
    }
  }
