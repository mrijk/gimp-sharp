// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2008 Maurits Rijk
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
using System.Runtime.InteropServices;

namespace Gimp
{
  public class CoordinateList<T>
  {
    readonly List<Coordinate<T>> _list = new List<Coordinate<T>>();

    public CoordinateList()
    {
    }

    internal CoordinateList(IntPtr ptr, int numCoords)
    {
      for (int i = 0; i < numCoords; i += 2)
	{
	  T x = (T) Marshal.PtrToStructure(ptr, typeof(T));
	  ptr = (IntPtr) ((int) ptr + Marshal.SizeOf(x));
	  T y = (T) Marshal.PtrToStructure(ptr, typeof(T));
	  ptr = (IntPtr) ((int) ptr + Marshal.SizeOf(y));
	  Add(new Coordinate<T>(x, y));
	}
    }

    public IEnumerator<Coordinate<T>> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public void ForEach(Action<Coordinate<T>> action)
    {
      _list.ForEach(action);
    }

    public int Count
    {
      get {return _list.Count;}
    }
	
    public void Add(Coordinate<T> coordinate)
    {
      _list.Add(coordinate);
    }
	
    public void Add(T x, T y)
    {
      _list.Add(new Coordinate<T>(x, y));
    }

    public Coordinate<T> this[int index]
    {
      get {return _list[index];}
      set {_list[index] = value;}
    }

    public override bool Equals(object o)
    {
      if (o is CoordinateList<T>)
	{
	  CoordinateList<T> list = o as CoordinateList<T>;
	  if (list.Count != Count)
	    {
	      return false;
	    }
	  else
	    {
	      for (int i = 0; i < Count; i++)
		{
		  if (_list[i] != list._list[i])
		    {
		      return false;
		    }
		}
	    }
	  return true;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return _list.GetHashCode();
    }

    public static bool operator==(CoordinateList<T> list1, 
				  CoordinateList<T> list2)
    {
      return list1.Equals(list2);
    }

    public static bool operator!=(CoordinateList<T> list1, 
				  CoordinateList<T> list2)
    {
      return !(list1 == list2);
    }

    public T[] ToArray()
    {
      if (Count == 0)
	{
	  return null;
	}

      T[] array = new T[Count * 2];
      
      int i = 0;
      _list.ForEach(coordinate =>
	{
	  array[i++] = coordinate.X;
	  array[i++] = coordinate.Y;
	});
      return array;
    }
  }
}
