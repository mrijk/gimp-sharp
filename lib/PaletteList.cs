// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
//
// PaletteList.cs
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
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class PaletteList : IEnumerable
    {
      ArrayList _list = new ArrayList();

      public PaletteList(string filter)
      {
	int num_palettes;
	IntPtr ptr = gimp_palettes_get_list(filter, out num_palettes);

	for (int i = 0; i < num_palettes; i++)
	  {
          IntPtr tmp = (IntPtr) Marshal.PtrToStructure(ptr, typeof(IntPtr));
          _list.Add(new Palette(Marshal.PtrToStringAnsi(tmp), false));
          ptr = (IntPtr)((int)ptr + Marshal.SizeOf(tmp));
	  }
      }

      public PaletteList()
      {
      }

      public virtual IEnumerator GetEnumerator()
      {
	return _list.GetEnumerator();
      }

      static public void Refresh()
      {
	gimp_palettes_refresh();
      }

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_palettes_refresh();
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_palettes_get_list (string filter,
						   out int num_palettes);
    }
  }
