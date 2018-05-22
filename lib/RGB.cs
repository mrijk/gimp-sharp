// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using GLib;

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

    public RGB(RGB rgb) : this(rgb.R, rgb.G, rgb.B)
    {
    }

    public RGB(HSV hsv)
    {
      var tmp = hsv.GimpHSV;
      gimp_hsv_to_rgb(ref tmp, ref _rgb);
    }

    public RGB(CMYK cmyk)
    {
      var tmp = cmyk.GimpCMYK;
      gimp_cmyk_to_rgb(ref tmp, ref _rgb);
    }

    public RGB(Gdk.Color color) : 
      this((double) color.Red / 0xFFFF, 
	   (double) color.Green / 0xFFFF, 
	   (double) color.Blue / 0xFFFF)
    {
    }

    internal RGB(GimpRGB rgb)
    {
      _rgb = rgb;
    }

    internal GimpRGB GimpRGB => _rgb;

    public override bool Equals(object o)
    {
      if (o is RGB)
	{
	  RGB rgb = o as RGB;
	  return R == rgb.R && G == rgb.G && B == rgb.B && Alpha == rgb.Alpha;
	}
      return false;
    }

    public override int GetHashCode() => _rgb.GetHashCode();

    public static bool operator==(RGB rgb1, RGB rgb2) => rgb1.Equals(rgb2);

    public static bool operator!=(RGB rgb1, RGB rgb2) => !(rgb1 == rgb2);

    public double R
    {
      get => _rgb.r;
      set => _rgb.r = value;
    }

    public double G
    {
      get => _rgb.g;
      set => _rgb.g = value;
    }

    public double B
    {
      get => _rgb.b;
      set => _rgb.b = value;
    }

    public double Alpha
    {
      get => _rgb.a;
      set {gimp_rgb_set_alpha(ref _rgb, value);}
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
      IntPtr tmp = Marshaller.StringToPtrGStrdup(name);
      if (!gimp_rgb_parse_name(ref _rgb, tmp, -1))
	{
	  Marshaller.Free(tmp);
	  throw new GimpSharpException();
	}
      Marshaller.Free(tmp);
    }

    public void ParseHex(string hex)
    {
      if (!gimp_rgb_parse_hex(ref _rgb, hex, -1))
	{
	  throw new GimpSharpException();
	}
    }

    public void ParseCss(string css)
    {
      if (!gimp_rgb_parse_css(ref _rgb, css, -1))
	{
	  throw new GimpSharpException();
	}
    }

    public static List<RGB> ListNames(out List<string> names)
    {
      IntPtr namesPtr;
      IntPtr colorsPtr;

      int len = gimp_rgb_list_names(out namesPtr, out colorsPtr);

      names = Util.ToStringList(namesPtr, len);
      Marshaller.Free(namesPtr);

      var colors = new List<RGB>();
      Util.Iterate<GimpRGB>(colorsPtr, len, (_, rgb) => colors.Add(new RGB(rgb)));
      Marshaller.Free(colorsPtr);

      return colors;
    }

    public void Add(RGB rgb)
    {
      gimp_rgb_add(ref _rgb, ref rgb._rgb);
    }

    public void Subtract(RGB rgb)
    {
      gimp_rgb_subtract(ref _rgb, ref rgb._rgb);
    }

    public void Multiply(double factor)
    {
      gimp_rgb_multiply(ref _rgb, factor);
    }

    public void Divide(double factor)
    {
      Multiply(1 / factor);
    }

    public static RGB operator*(RGB rgb, double factor)
    {
      RGB result = new RGB(rgb);
      result.Multiply(factor);
      return result;
    }

    public static RGB operator+(RGB rgb, double v)
    {
      RGB result = new RGB(rgb);
      result.R += v;
      result.G += v;
      result.B += v;
      return result;
    }

    public static RGB operator-(RGB rgb, double v) => rgb + (-v);

    public static RGB operator-(RGB rgb1, RGB rgb2)
    {
      RGB result = new RGB(rgb1);
      result.Subtract(rgb2);
      return result;
    }

    public static RGB operator+(RGB rgb1, RGB rgb2)
    {
      RGB result = new RGB(rgb1);
      result.Add(rgb2);
      return result;
    }

    public double Distance(RGB rgb) => gimp_rgb_distance(ref _rgb, ref rgb._rgb);

    public double Max => gimp_rgb_max(ref _rgb);

    public double Min => gimp_rgb_min(ref _rgb);

    public void Clamp()
    {
      gimp_rgb_clamp(ref _rgb);
    }

    public void Gamma(double gamma)
    {
      gimp_rgb_gamma(ref _rgb, gamma);
    }

    public double Luminance => gimp_rgb_luminance(ref _rgb);

    public double LuminanceUchar => gimp_rgb_luminance_uchar(ref _rgb);

    public static RGB Interpolate(double value, RGB rgb1, RGB rgb2)
    {
      double r = rgb1.R + value * (rgb2.R - rgb1.R);
      double g = rgb1.G + value * (rgb2.G - rgb1.G);
      double b = rgb1.B + value * (rgb2.B - rgb1.B);

      return new RGB(r, g, b);
    }

    public byte[] Bytes
    {
      get
	{
	  GetUchar(out byte r, out byte g, out byte b);
	  return new byte[]{r, g, b};
	}
    }

    public override string ToString()
    {
      GetUchar(out byte r, out byte g, out byte b);
      return $"({r} {g} {b})";
    }

    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_hsv_to_rgb(ref GimpHSV hsv,
				       ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_cmyk_to_rgb(ref GimpCMYK cmyk,
					ref GimpRGB rgb);
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
    static extern bool gimp_rgb_parse_css(ref GimpRGB rgb,
					  string css,
					  int len);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern int gimp_rgb_list_names(out IntPtr names,
					  out IntPtr colors);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_add(ref GimpRGB rgb1,
				      ref GimpRGB rgb2);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_subtract(ref GimpRGB rgb1,
					   ref GimpRGB rgb2);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_multiply(ref GimpRGB rgb,
					 double factor);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_distance(ref GimpRGB rgb1,
					   ref GimpRGB rgb2);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_max (ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_min (ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_clamp (ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_rgb_gamma (ref GimpRGB rgb,
				       double gamma);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern double gimp_rgb_luminance(ref GimpRGB rgb);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern byte gimp_rgb_luminance_uchar(ref GimpRGB rgb);
  }
}
