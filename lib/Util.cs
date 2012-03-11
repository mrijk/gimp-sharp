// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// Util.cs
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

namespace Gimp
{
  public sealed class Util
  {
    static public List<string> ToStringList(IntPtr ptr, int n)
    {
      return ToList<string>(ptr, n, (s) => s);
    }

    static public List<T> ToList<T>(IntPtr ptr, int n, 
				    Func<string, T> transform)
    {
      var set = new List<T>();
      for (int i = 0; i < n; i++)
	{
	  IntPtr tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
	  set.Add(transform(Marshal.PtrToStringAnsi(tmp)));
	  ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
        }
      return set;
    }

    static public void Iterate<T>(IntPtr ptr, int n,
				  Action<int, T> func)
    {
      for (int i = 0; i < n; i++)
	{
	  T tmp = (T) Marshal.PtrToStructure(ptr, typeof(T));
	  func(i, tmp);
	  ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
	}
    }
  }
}
