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
using System.Collections;
using System.Collections.Generic;

namespace Gimp
{
  public class CoordinateList<T>
  {
    List<Coordinate<T>> _list = new List<Coordinate<T>>();

    public IEnumerator<Coordinate<T>> GetEnumerator()
    {
      return _list.GetEnumerator();
    }
	
    public void Add(Coordinate<T> coordinate)
    {
      _list.Add(coordinate);
    }
	
    public void Add(T x, T y)
    {
      _list.Add(new Coordinate<T>(x, y));
    }

    public T[] ToArray()
    {
      T[] array = new T[_list.Count * 2];
		
      int i = 0;
      foreach (Coordinate<T> coordinate in _list)
	{
	  array[i] = coordinate.X;
	  array[i + 1] = coordinate.Y;
	}
		
      return array;
    }
	
    public void Dump()
    {
      foreach (Coordinate<T> coordinate in _list)
	{
	  Console.WriteLine(coordinate);
	}
    }
  }
}
