using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Gimp
  {
    [Serializable]
    public class RGB : ISerializable
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
      [DllImport("libgimp-2.0.so")]
      static extern void gimp_rgb_get_uchar (ref GimpRGB rgb,
					     out byte red,
					     out byte green,
					     out byte blue);

      public RGB(double red, double green, double blue)
      {
	gimp_rgb_set(ref _rgb, red, green, blue);
      }

      public RGB(byte red, byte green, byte blue)
      {
	gimp_rgb_set_uchar(ref _rgb, red, green, blue);
      }

      public RGB(GimpRGB rgb)
      {
	_rgb = rgb;
      }

      public GimpRGB GimpRGB
      {
	get {return _rgb;}
      }

      public void GetObjectData(SerializationInfo info, 
				StreamingContext context)
      {
	Console.WriteLine("Fixme: serialize RGB!");
      }

      public void GetUChar(out byte red, out byte green, out byte blue)
      {
	gimp_rgb_get_uchar(ref _rgb, out red, out green, out blue);
      }
    }
  }
