// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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

using System;

namespace Gimp
{
  public class Coordinate<T>
  {
    T _x;
    T _y;
	
    public Coordinate()
    {
    }
	
    public Coordinate(T x, T y)
    {
      _x = x;
      _y = y;
    }

    public Coordinate(Coordinate<T> c)
    {
      _x = c._x;
      _y = c._y;
    }
	
    public T X
    {
      get {return _x;}
      set {_x = value;}
    }
	
    public T Y
    {
      get {return _y;}
      set {_y = value;}
    }
		
    public override string ToString()
    {
      return string.Format("({0}, {1})", _x, _y);
    }
  }
}
