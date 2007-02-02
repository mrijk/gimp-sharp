// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Tile.cs
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
  public sealed class Tile
  {
    readonly IntPtr _tile;

    internal Tile(IntPtr tile)
    {
      _tile = tile;
    }

    public void Ref()
    {
      gimp_tile_ref(_tile);
    }

    public void RefZero()
    {
      gimp_tile_ref_zero(_tile);
    }

    public void Unref(bool dirty)
    {
      gimp_tile_unref(_tile, dirty);
    }

    public void Flush()
    {
      gimp_tile_flush(_tile);
    }

    static public ulong CacheSize
    {
      set
	{
	  gimp_tile_cache_size(value);
	}
    }

    static public ulong CacheNtiles
    {
      set
	{
	  gimp_tile_cache_ntiles(value);
	}
    }

    static public void CacheDefault(Drawable drawable)
    {
      Tile.CacheNtiles = (ulong) (2 * (drawable.Width / Gimp.TileWidth + 1));
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_ref(IntPtr tile);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_ref_zero(IntPtr tile);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_unref(IntPtr tile, bool dirty);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_flush(IntPtr tile);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_cache_size(ulong kilobytes);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_tile_cache_ntiles(ulong ntiles);
  }
}
