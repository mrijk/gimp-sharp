// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2017 Maurits Rijk
//
// Coordinate.cs
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

namespace Gimp
{
  public class Coordinate<T>
  {
    public T X {get; set;}	
    public T Y {get; set;}

    public Coordinate()
    {
    }
	
    public Coordinate(T x, T y)
    {
      X = x;
      Y = y;
    }

    public Coordinate(Coordinate<T> c) : this(c.X, c.Y)
    {
    }

    public override bool Equals(object o)
    {
      if (o is Coordinate<T>)
	{
	  var coordinate = o as Coordinate<T>;
	  return coordinate.X.Equals(X) && coordinate.Y.Equals(Y);
	}
      return false;
    }

    public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode();

    public static bool operator==(Coordinate<T> coordinate1, 
				  Coordinate<T> coordinate2)
    {
      if (ReferenceEquals(coordinate1, coordinate2))
	{
	  return true;
	}

      if (((object) coordinate1 == null) || ((object) coordinate2 == null))
	{
	  return false;
	}

      return coordinate1.Equals(coordinate2);
    }

    public static bool operator!=(Coordinate<T> coordinate1, 
				  Coordinate<T> coordinate2) =>
      !(coordinate1 == coordinate2);

    public override string ToString() => $"({X}, {Y})";
  }
}
