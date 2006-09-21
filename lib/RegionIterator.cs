// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// RegionIterator.cs
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
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class RegionIterator
  {
    readonly IntPtr _iter;

    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void RgnFuncSrc(int x, int y, byte[] src, int bpp,
				    IntPtr data);

    public RegionIterator(Drawable drawable)
    {
      _iter = gimp_rgn_iterator_new(drawable.ID, RunMode.Interactive);
    }

    public void Iterate(RgnFuncSrc func)
    {
      gimp_rgn_iterator_src(_iter, func, IntPtr.Zero);
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_rgn_iterator_new(Int32 drawable,
					       RunMode unused);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_rgn_iterator_src(IntPtr iter,
					     RgnFuncSrc func,
					     IntPtr data);
  }
}
