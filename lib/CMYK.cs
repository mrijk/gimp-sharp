// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2017 Maurits Rijk
//
// CMYK.cs
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
  public sealed class CMYK
  {
    GimpCMYK _cmyk = new GimpCMYK();

    public double Cyan => _cmyk.c;
    public double Magenta => _cmyk.m;
    public double Yellow => _cmyk.y;
    public double Black => _cmyk.k;

    internal GimpCMYK GimpCMYK =>  _cmyk;

    public CMYK(double cyan, double magenta, double yellow, double black)
    {
      gimp_cmyk_set(ref _cmyk, cyan, magenta, yellow, black);
    }

    public CMYK(CMYK cmyk)
    {
      _cmyk = cmyk._cmyk;
    }

    public override bool Equals(object o) => _cmyk.Equals((o as CMYK)?._cmyk);

    public override int GetHashCode() => _cmyk.GetHashCode();

    public static bool operator==(CMYK cmyk1, CMYK cmyk2) =>
      cmyk1._cmyk.Equals(cmyk2._cmyk);

    public static bool operator!=(CMYK cmyk1, CMYK cmyk2) => !(cmyk1 == cmyk2);

    public void Clamp()
    {
      gimp_cmyk_clamp(ref _cmyk);
    }

    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_cmyk_set(ref GimpCMYK cmyk,
				    double cyan,
				    double magenta,
				    double yellow,
				    double black);
    [DllImport("libgimpcolor-2.0-0.dll")]
    static extern void gimp_cmyk_clamp(ref GimpCMYK cmyk);
  }

  internal struct GimpCMYK
  {
    internal double c, m, y, k, a;
  }
}
