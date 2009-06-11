// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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
  public enum EdgeMode
  {
    None,
    Wrap,
    Smear,
    Black,
    Background
  }

  public sealed class PixelFetcher: IDisposable
  {
    readonly IntPtr _ptr;
    readonly byte[] _dummy;

    public PixelFetcher(Drawable drawable, bool shadow)
    {
      _ptr = gimp_pixel_fetcher_new (drawable.Ptr, shadow);
      _dummy = new byte[drawable.Bpp];
    }

    public PixelFetcher(Drawable drawable) : this(drawable, false)
    {
    }

    ~PixelFetcher()
    {
      Dispose(false);
    }

    public EdgeMode EdgeMode
    {
      set {gimp_pixel_fetcher_set_edge_mode(_ptr, value);}
    }

    public RGB BackgroundColor
    {
      set
	{
	  GimpRGB rgb = value.GimpRGB;
	  gimp_pixel_fetcher_set_bg_color(_ptr, ref rgb);
	}
    }

    public Pixel GetPixel(int x, int y)
    {
      gimp_pixel_fetcher_get_pixel(_ptr, x, y, _dummy);
      return new Pixel(_dummy);
    }

    public Pixel GetPixel(Coordinate<int> c)
    {
      return GetPixel(c.X, c.Y);
    }

    public void PutPixel(int x, int y, Pixel pixel)
    {
      gimp_pixel_fetcher_put_pixel(_ptr, x, y, pixel.Bytes);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    void Dispose(bool disposing)
    {
      if (disposing)
	{
	  // _dummy.Dispose();
	}
      gimp_pixel_fetcher_destroy(_ptr);
    }

    public Pixel this[int row, int col]
    {
      set
	{
	  PutPixel(col, row, value);
	}

      get
	{
	  return GetPixel(col, row);
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_new(IntPtr drawable,
						bool shadow);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_fetcher_set_edge_mode(IntPtr pf,
							EdgeMode mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_fetcher_set_bg_color(IntPtr pf,
						       ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_destroy (IntPtr pf);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_get_pixel(IntPtr pf,
						      int x,
						      int y,
						      byte[] pixel);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_fetcher_put_pixel(IntPtr pf,
						      int x,
						      int y,
						      byte[] pixel);
  }
}
