// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// PixelFetcher.cs
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
  public sealed class PixelFetcher
  {
    readonly IntPtr _ptr;
    readonly byte[] _dummy;

    public PixelFetcher(Drawable drawable, bool shadow)
    {
      _ptr = gimp_pixel_fetcher_new (drawable.Ptr, shadow);
      _dummy = new byte[drawable.Bpp];
    }

    public void Destroy()
    {
      gimp_pixel_fetcher_destroy (_ptr);
    }

    public void GetPixel(int x, int y, byte[] pixel)
    {
      gimp_pixel_fetcher_get_pixel (_ptr, x, y, pixel);
    }

    public void PutPixel(int x, int y, byte[] pixel)
    {
      gimp_pixel_fetcher_put_pixel (_ptr, x, y, pixel);
    }

    public byte[] this[int row, int col]
    {
      set 
	{
	  PutPixel(col, row, value);
	}

      get
	{
	  GetPixel(col, row, _dummy);
	  return _dummy;
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_new (IntPtr drawable,
						 bool shadow);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_destroy (IntPtr drawable);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_get_pixel (IntPtr pf,
						       int x,
						       int y,
						       byte[] pixel);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_put_pixel (IntPtr pf,
						       int x,
						       int y,
						       byte[] pixel);
  }
}
