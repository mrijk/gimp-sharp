// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// BrushList.cs
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
  public sealed class BrushList
  {
    readonly List<Brush> _list = new List<Brush>();

    public BrushList(string filter)
    {
      int num_brushes;
      IntPtr ptr = gimp_brushes_get_list(filter, out num_brushes);
      for (int i = 0; i < num_brushes; i++)
        {
	  IntPtr tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
	  _list.Add(new Brush(Marshal.PtrToStringAnsi(tmp), false));
	  ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
        }
    }

    public IEnumerator<Brush> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public void ForEach(Action<Brush> action)
    {
      _list.ForEach(action);
    }

    public int Count
    {
      get {return _list.Count;}
    }

    static public void Refresh()
    {
      gimp_brushes_refresh();
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static void gimp_brushes_refresh();
    [DllImport("libgimp-2.0-0.dll")]
    extern static IntPtr gimp_brushes_get_list(string filter, 
					       out int num_brushes);
  }
}
