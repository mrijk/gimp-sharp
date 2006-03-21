// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// PixelRgn.cs
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
  [StructLayout(LayoutKind.Sequential)]
  public struct GimpPixelRgn
  {
    public IntPtr	data;  	       /* pointer to region data */
    IntPtr		drawable;      /* pointer to drawable */
    public uint       bpp;           /* bytes per pixel */
    public uint       rowstride;     /* bytes per pixel row */
    public uint       x, y;          /* origin */
    public uint       w, h;          /* width and height of region */
    // uint         dirty : 1;     /* will this region be dirtied? */
    // uint         shadow : 1;    /* will this region use the shadow or normal tiles */
    public uint		 dirty_shadow;
    uint         process_count;      /* used internally */
  }

  public sealed class PixelRgn
  {
    GimpPixelRgn pr = new GimpPixelRgn();
    readonly byte[] _dummy;

    public PixelRgn(Drawable drawable, int x,
		    int y,
		    int width,
		    int height,
		    bool dirty,
		    bool shadow)
    {
      gimp_pixel_rgn_init (ref pr, drawable.Ptr, x, y, width, height, dirty, 
			   shadow);
      _dummy = new byte[pr.bpp];
    }

    public PixelRgn(Drawable drawable,  bool dirty, bool shadow) :
      this(drawable, 0, 0, drawable.Width, drawable.Height, dirty, shadow)
    {
    }

    public static IntPtr Register(PixelRgn rgn)
    {
      return gimp_pixel_rgns_register(1, ref rgn.pr);
    }

    public static IntPtr Register(PixelRgn rgn1, PixelRgn rgn2)
    {
      return gimp_pixel_rgns_register(2, ref rgn1.pr, ref rgn2.pr);
    }

    public static IntPtr Register(PixelRgn rgn1, PixelRgn rgn2, 
				  PixelRgn rgn3)
    {
      return gimp_pixel_rgns_register(3, ref rgn1.pr, ref rgn2.pr,
				      ref rgn3.pr);
    }

    public static IntPtr Process(IntPtr priPtr)
    {
      return gimp_pixel_rgns_process(priPtr);
    }

    public void GetPixel(byte[] buf, int x, int y)
    {
      gimp_pixel_rgn_get_pixel(ref pr, buf, x, y);
    }

    public void SetPixel(byte[] buf, int x, int y)
    {
      gimp_pixel_rgn_set_pixel(ref pr, buf, x, y);
    }

    public byte[] GetRect(int x, int y, int width, int height)
    {
      byte[] buf = new byte[width * pr.bpp * height];
      gimp_pixel_rgn_get_rect(ref pr, buf, x, y, width, height);
      return buf;
    }

    public void SetRect(byte[] buf, int x, int y, int width, int height)
    {
      gimp_pixel_rgn_set_rect(ref pr, buf, x, y, width, height);
    }

    public int X
    {
      get {return (int) pr.x;}
    }

    public int Y
    {
      get {return (int) pr.y;}
    }

    public int W
    {
      get {return (int) pr.w;}
    }

    public int H
    {
      get {return (int) pr.h;}
    }

    public int Rowstride
    {
      get {return (int) pr.rowstride;}
    }

    public GimpPixelRgn PR
    {
      get {return pr;}
    }

    public byte[] this[int row, int col]
    {
      set
	{
	  int bpp = (int) pr.bpp;
	  IntPtr dest = (IntPtr) ((int) pr.data + (row - Y) * Rowstride + 
				  (col - X) * bpp);
	  Marshal.Copy(value, 0, dest, bpp);
	}

      get
	{
	  int bpp = (int) pr.bpp;
	  IntPtr src = (IntPtr) ((int) pr.data + (row - Y) * Rowstride + 
				 (col - X) * bpp);
	  Marshal.Copy(src, _dummy, 0, bpp);
	  return _dummy;
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_init (ref GimpPixelRgn pr,
					    IntPtr drawable,
					    int x,
					    int y,
					    int width,
					    int height,
					    bool dirty,
					    bool shadow);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_rgns_register (int nrgns, 
						   ref GimpPixelRgn pr);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_rgns_register (int nrgns, 
						   ref GimpPixelRgn pr1,
						   ref GimpPixelRgn pr2);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_rgns_register (int nrgns, 
						   ref GimpPixelRgn pr1,
						   ref GimpPixelRgn pr2,
						   ref GimpPixelRgn pr3);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_pixel_rgns_process (IntPtr pri_ptr);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_get_pixel (ref GimpPixelRgn  pr,
						 byte[] buf,
						 int    x,
						 int    y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_set_pixel (ref GimpPixelRgn  pr,
						 byte[] buf,
						 int    x,
						 int    y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_get_rect (ref GimpPixelRgn pr,
						byte[] buf,
						int x,
						int y,
						int width,
						int height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_set_rect (ref GimpPixelRgn pr,
						byte[] buf,
						int x,
						int y,
						int width,
						int height);
  }
}
