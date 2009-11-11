// The Splitter plug-in
// Copyright (C) 2004-2009 Maurits Rijk
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

using System;

namespace Gimp.Splitter
{
  public class MyClassBase
  {
    protected double pi = Math.PI;
    protected double e = Math.E;

    public double w {get; set;}		// image width
    public double h {get; set;}		// image height

    public MyClassBase()
    {
    }

    public virtual double eval(double x, double y)
    {
      return 0.0;
    }

    protected double abs(double x)
    {
      return Math.Abs(x);
    }

    protected double acos(double x)
    {
      return Math.Acos(x);
    }

    protected double asin(double x)
    {
      return Math.Asin(x);
    }

    protected double atan(double x)
    {
      return Math.Atan(x);
    }

    protected double atan2(double y, double x)
    {
      return Math.Atan2(y, x);
    }

    protected double cos(double x)
    {
      return Math.Cos(x);
    }

    protected double cosh(double x)
    {
      return Math.Cosh(x);
    }

    protected double exp(double x)
    {
      return Math.Exp(x);
    }

    protected double floor(double x)
    {
      return Math.Floor(x);
    }

    protected double log(double x)
    {
      return Math.Log(x);
    }

    protected double log(double x, double y)
    {
      return Math.Log(x, y);
    }

    protected double log10(double x)
    {
      return Math.Log(x);
    }

    protected double pow(double x, double y)
    {
      return Math.Pow(x, y);
    }

    protected double sin(double x)
    {
      return Math.Sin(x);
    }

    protected double sinh(double x)
    {
      return Math.Sinh(x);
    }

    protected double sqrt(double x)
    {
      return Math.Sqrt(x);
    }

    protected double tan(double x)
    {
      return Math.Tan(x);
    }

    protected double tanh(double x)
    {
      return Math.Tanh(x);
    }
  }
}
