// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
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
  internal struct GimpPixelRgn
  {
    public IntPtr     data;  	       /* pointer to region data */
    public IntPtr     drawable;      /* pointer to drawable */
    public uint       bpp;           /* bytes per pixel */
    public uint       rowstride;     /* bytes per pixel row */
    public uint       x, y;          /* origin */
    public uint       w, h;          /* width and height of region */
    // uint         dirty : 1;     /* will this region be dirtied? */
    // uint         shadow : 1;    /* will this region use the shadow or normal tiles */
    public uint	      dirty_shadow;
    public uint        process_count;      /* used internally */
  }

  public unsafe sealed class PixelRgn
  {
    GimpPixelRgn* pr = null;
    readonly byte[] _dummy;
    readonly int _bpp;
    readonly bool _dirty;
    readonly Drawable _drawable;

    public PixelRgn(Drawable drawable, int x, int y, int width, int height,
		    bool dirty, bool shadow)
    {
      pr = (GimpPixelRgn*)g_malloc(sizeof(GimpPixelRgn));
      gimp_pixel_rgn_init(ref *pr, drawable.Ptr, x, y, width, height, dirty, 
        shadow);
      _bpp = (int) pr->bpp;
      _dummy = new byte[pr->bpp];
      _dirty = dirty;
      _drawable = drawable;
    }
    
    ~PixelRgn()
    {
      g_free((void*)pr);
      pr = null;
    }

    public PixelRgn(Drawable drawable, Rectangle rectangle, bool dirty,
		    bool shadow) : 
      this(drawable, rectangle.X1, rectangle.Y1, rectangle.Width,
	   rectangle.Height, dirty, shadow)
    {
    }

    public PixelRgn(Drawable drawable,  bool dirty, bool shadow) :
      this(drawable, 0, 0, drawable.Width, drawable.Height, dirty, shadow)
    {
    }

    public static IntPtr Register(PixelRgn rgn)
    {
      return gimp_pixel_rgns_register(1, ref *rgn.pr);
    }

    public static IntPtr Register(PixelRgn rgn1, PixelRgn rgn2)
    {
      return gimp_pixel_rgns_register(2, ref *rgn1.pr, ref *rgn2.pr);
    }

    public static IntPtr Register(PixelRgn rgn1, PixelRgn rgn2, 
				  PixelRgn rgn3)
    {
      return gimp_pixel_rgns_register(3, ref *rgn1.pr, ref *rgn2.pr,
              ref *rgn3.pr);
    }

    public static IntPtr Process(IntPtr priPtr)
    {
      return gimp_pixel_rgns_process(priPtr);
    }

    public void GetPixel(byte[] buf, int x, int y)
    {
      gimp_pixel_rgn_get_pixel(ref *pr, buf, x, y);
    }

    public Pixel GetPixel(int x, int y)
    {
      GetPixel(_dummy, x, y);
      return new Pixel(_dummy);
    }

    public void SetPixel(byte[] buf, int x, int y)
    {
      gimp_pixel_rgn_set_pixel(ref *pr, buf, x, y);
    }

    public byte[] GetRect(int x, int y, int width, int height)
    {
      var buf = new byte[width * _bpp * height];
      gimp_pixel_rgn_get_rect(ref *pr, buf, x, y, width, height);
      return buf;
    }

    public byte[] GetRect(Rectangle rectangle)
    {
      return GetRect(rectangle.X1, rectangle.Y1, rectangle.Width,
		     rectangle.Height);
    }

    public void SetRect(byte[] buf, int x, int y, int width, int height)
    {
      gimp_pixel_rgn_set_rect(ref *pr, buf, x, y, width, height);
    }

    public void SetRect(byte[] buf, Rectangle rectangle)
    {
      SetRect(buf, rectangle.X1, rectangle.Y1, rectangle.Width, 
	      rectangle.Height);
    }
   
    public Pixel[] GetRow(int x, int y, int width)
    {
      var buf = new byte[width * _bpp];
      gimp_pixel_rgn_get_row(ref *pr, buf, x, y, width);

      var row = new Pixel[width];

      int index = 0;
      for (int i = 0; i < width; i++)
	{
	  row[i] = new Pixel(_bpp);
	  row[i].CopyFrom(buf, index);
	  index += _bpp;
	}

      return row;
    }

    public void ForEachRow(Action<Pixel[]> action)
    {
      int width = _drawable.Width;
      for (int y = 0; y < _drawable.Height; y++) 
	{
	  action(GetRow(0, y, width));
	}
    }

    public void SetRow(Pixel[] row, int x, int y)
    {
      int width = row.Length;
      var buf = new byte[width * _bpp];

      int index = 0;
      for (int i = 0; i < width; i++)
	{
	  row[i].CopyTo(buf, index);
	  index += _bpp;
	}

      gimp_pixel_rgn_set_row(ref *pr, buf, x, y, width);
    }

    public void SetRow(byte[] row, int x, int y)
    {
      gimp_pixel_rgn_set_row(ref *pr, row, x, y, row.Length);
    }

    public Pixel[] GetColumn(int x, int y, int height)
    {
      var buf = new byte[height * _bpp];
      gimp_pixel_rgn_get_col(ref *pr, buf, x, y, height);

      var col = new Pixel[height];

      int index = 0;
      for (int i = 0; i < height; i++)
	{
	  col[i] = new Pixel(_bpp);
	  col[i].CopyFrom(buf, index);
	  index += _bpp;
	}

      return col;
    }

    public void ForEachColumn(Action<Pixel[]> action)
    {
      int height = _drawable.Height;
      for (int x = 0; x < _drawable.Width; x++) 
	{
	  action(GetColumn(x, 0, height));
	}
    }

    public void SetColumn(Pixel[] col, int x, int y)
    {
      int height = col.Length;
      var buf = new byte[height * _bpp];

      int index = 0;
      for (int i = 0; i < height; i++)
	{
	  col[i].CopyTo(buf, index);
	  index += _bpp;
	}

      gimp_pixel_rgn_set_col(ref *pr, buf, x, y, height);
    }

    public void SetColumn(byte[] col, int x, int y)
    {
      gimp_pixel_rgn_set_col(ref *pr, col, x, y, col.Length);
    }

    public int X
    {
      get {return (int) pr->x;}
    }

    public int Y
    {
      get {return (int) pr->y;}
    }

    public int W
    {
      get {return (int) pr->w;}
    }

    public int H
    {
      get {return (int) pr->h;}
    }

    public int Rowstride
    {
      get {return (int) pr->rowstride;}
    }

    public bool Dirty
    {
      get {return _dirty;}
    }

    internal GimpPixelRgn PR
    {
      get {return *pr;}
    }

    public Pixel this[int row, int col]
    {
      set
	{
	  IntPtr dest = (IntPtr) ((int) pr->data + (row - Y) * Rowstride + 
				  (col - X) * _bpp);
	  Marshal.Copy(value.Bytes, 0, dest, _bpp);
	}

      get
	{
	  IntPtr src = (IntPtr) ((int) pr->data + (row - Y) * Rowstride + 
				 (col - X) * _bpp);
	  Marshal.Copy(src, _dummy, 0, _bpp);
	  return new Pixel(this, _dummy) {X = col, Y = row};
	}
    }

    [DllImport("libglib-2.0-0.dll")]
    static extern void* g_malloc (long n_bytes); 
    [DllImport("libglib-2.0-0.dll")]
    static extern void	g_free (void* mem); 
    
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
    static extern void gimp_pixel_rgn_get_pixel(ref GimpPixelRgn  pr,
						byte[] buf,
						int    x,
						int    y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_set_pixel(ref GimpPixelRgn  pr,
						byte[] buf,
						int    x,
						int    y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_get_rect(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int width,
					       int height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_pixel_rgn_set_rect(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int width,
					       int height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void  gimp_pixel_rgn_get_row(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int width);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void  gimp_pixel_rgn_set_row(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int width);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void  gimp_pixel_rgn_get_col(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void  gimp_pixel_rgn_set_col(ref GimpPixelRgn pr,
					       byte[] buf,
					       int x,
					       int y,
					       int height);
  }
}
