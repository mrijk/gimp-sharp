// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// RGB.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Gimp
{
  [Serializable]
  public sealed class RGB
  {
    GimpRGB _rgb = new GimpRGB();

    public RGB(double red, double green, double blue)
    {
      gimp_rgb_set(ref _rgb, red, green, blue);
    }

    public RGB(byte red, byte green, byte blue)
    {
      SetUchar(red, green, blue);
    }

    internal RGB(GimpRGB rgb)
    {
      _rgb = rgb;
    }

    internal GimpRGB GimpRGB
    {
      get {return _rgb;}
    }

    public double R
    {
      get {return _rgb.r;}
    }

    public double G
    {
      get {return _rgb.g;}
    }

    public double B
    {
      get {return _rgb.b;}
    }

    public double Alpha
    {
      get {return _rgb.a;}
      set
	{
	  gimp_rgb_set_alpha(ref _rgb, value);
	}
    }

    public void SetUchar(byte red, byte green, byte blue)
    {
      gimp_rgb_set_uchar(ref _rgb, red, green, blue);
    }

    public void GetUchar(out byte red, out byte green, out byte blue)
    {
      gimp_rgb_get_uchar(ref _rgb, out red, out green, out blue);
    }

    public void ParseName(string name)
    {
    }

    public void ParseHex(string hex)
    {
      if (!gimp_rgb_parse_hex(ref _rgb, hex, -1))
	{
	  throw new Exception();
	}
    }

    public void Multiply(double factor)
    {
      gimp_rgb_multiply(ref _rgb, factor);
    }

    public double Distance(RGB rgb)
    {
      return gimp_rgb_distance(ref _rgb, ref rgb._rgb);
    }

    public double Max
    {
      get {return gimp_rgb_max(ref _rgb);}
    }

    public double Min
    {
      get {return gimp_rgb_min(ref _rgb);}
    }

    public void Clamp()
    {
      gimp_rgb_clamp(ref _rgb);
    }

    public byte[] Bytes
    {
      get
	{
	  byte r, g, b;
	  
	  GetUchar(out r, out g, out b);
	  return new byte[]{r, g, b};
	}
    }

    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_set(ref GimpRGB rgb,
				    double red,
				    double green,
				    double blue);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_set_alpha(ref GimpRGB rgb,
					  double alpha);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_set_uchar (ref GimpRGB rgb,
					   byte red,
					   byte green,
					   byte blue);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_get_uchar (ref GimpRGB rgb,
					   out byte red,
					   out byte green,
					   out byte blue);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern bool gimp_rgb_parse_name (ref GimpRGB rgb,
					    IntPtr name,
					    int len);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern bool gimp_rgb_parse_hex (ref GimpRGB rgb,
					   string hex,
					   int len);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_multiply (ref GimpRGB rgb,
					  double factor);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_distance (ref GimpRGB rgb1,
					    ref GimpRGB rgb2);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_max (ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_min (ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_clamp (ref GimpRGB rgb);
  }
}
