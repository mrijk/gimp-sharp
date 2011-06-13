// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// IntCoordinate.cs
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

namespace Gimp
{
  public class IntCoordinate : Coordinate<int>
  {
    public IntCoordinate(int x, int y) : base(x, y)
    {
    }

    public IntCoordinate(IntCoordinate c) : this(c.X, c.Y)
    {
    }

    public static IntCoordinate operator + (IntCoordinate c1, IntCoordinate c2)
    {
      return new IntCoordinate(c1.X + c2.X, c1.Y + c2.Y);
    }

    public int Distance(int x, int y)
    {
      x -= X;
      y -= Y;
      return x * x + y * y;
    }

    public int Distance(IntCoordinate c)
    {
      return Distance(c.X, c.Y);
    }

    public double Radius
    {
      get {return Math.Sqrt(X * X + Y * Y);}
    }

    public double Angle
    {
      get {return Math.Atan2(X, Y);}
    }
  }
}
