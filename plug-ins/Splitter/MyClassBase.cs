// The Splitter plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// MyClassBase.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using static System.Math;

namespace Gimp.Splitter
{
  public class MyClassBase
  {
    protected double pi = PI;
    protected double e = E;

    public double w {get; set;}		// image width
    public double h {get; set;}		// image height

    public MyClassBase()
    {
    }

    public virtual double eval(double x, double y) => 0.0;

    protected double abs(double x) => Abs(x);

    protected double acos(double x) => Acos(x);

    protected double asin(double x) => Asin(x);

    protected double atan(double x) => Atan(x);

    protected double atan2(double y, double x) => Atan2(y, x);

    protected double cos(double x) => Cos(x);

    protected double cosh(double x) => Cosh(x);

    protected double exp(double x) => Exp(x);

    protected double floor(double x) => Floor(x);

    protected double log(double x) => Log(x);

    protected double log(double x, double y) => Log(x, y);

    protected double log10(double x) => Log(x);

    protected double pow(double x, double y) => Pow(x, y);

    protected double sin(double x) => Sin(x);

    protected double sinh(double x) => Sinh(x);

    protected double sqrt(double x) => Sqrt(x);

    protected double tan(double x) => Tan(x);

    protected double tanh(double x) => Tanh(x);
  }
}
