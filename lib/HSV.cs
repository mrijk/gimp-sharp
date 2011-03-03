// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// HSV.cs
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

using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class HSV
  {
    GimpHSV _hsv = new GimpHSV();

    public HSV(double hue, double saturation, double value)
    {
      gimp_hsv_set(ref _hsv, hue, saturation, value);
    }

    public HSV(HSV hsv)
    {
      _hsv = hsv._hsv;
    }

    internal GimpHSV GimpHSV
    {
      get {return _hsv;}
    }

    public override bool Equals(object o)
    {
      if (o is HSV)
	{
	  return _hsv.Equals((o as HSV)._hsv);
	}
      return false;
    }

    public override int GetHashCode()
    {
      return _hsv.GetHashCode();
    }

    public static bool operator==(HSV hsv1, HSV hsv2)
    {
      return hsv1._hsv.Equals(hsv2._hsv);
    }

    public static bool operator!=(HSV hsv1, HSV hsv2)
    {
      return !(hsv1 == hsv2);
    }

    public double Hue
    {
      get {return _hsv.h;}
    }

    public double Saturation
    {
      get {return _hsv.s;}
    }

    public double Value
    {
      get {return _hsv.v;}
    }

    public double Alpha
    {
      get {return _hsv.a;}
    }

    public void Clamp()
    {
      gimp_hsv_clamp(ref _hsv);
    }

    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_hsv_set(ref GimpHSV hsv,
				    double hue,
				    double saturation,
				    double value);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_hsv_clamp(ref GimpHSV hsv);
  }

  internal struct GimpHSV
  {
    internal double h, s, v, a;
  }
}
