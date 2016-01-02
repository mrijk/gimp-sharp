// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// GimpPreview.cs
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

using Gdk;
using GLib;
using Gtk;

namespace Gimp
{
  public class GimpPreview : VBox
  {
    public GimpPreview() {}

    public GimpPreview(IntPtr ptr) : base(ptr)
    {
    }

    virtual internal GimpPreview Instantiate(Drawable drawable) {return null;}

    public bool Update
    {
      get {return gimp_preview_get_update(Handle);}
      set {gimp_preview_set_update(Handle, value);}
    }

    public void SetBounds(int xmin, int ymin, int xmax, int ymax)
    {
      gimp_preview_set_bounds(Handle, xmin, ymin, xmax, ymax);
    }

    public void GetPosition(out int x, out int y)
    {
      gimp_preview_get_position(Handle, out x, out y);
    }

    public void Transform(int src_x, int src_y, out int dest_x, out int dest_y)
    {
      gimp_preview_transform(Handle, src_x, src_y, out dest_x, out dest_y);
    }

    public Coordinate<int> Transform(Coordinate<int> src)
    {
      int dest_x, dest_y;
      Transform(src.X, src.Y, out dest_x, out dest_y);
      return new Coordinate<int>(dest_x, dest_y);
    }

    public void Untransform(int src_x, int src_y, out int dest_x, 
			    out int dest_y)
    {
      gimp_preview_untransform(Handle, src_x, src_y, out dest_x, out dest_y);
    }

    public Coordinate<int> Untransform(Coordinate<int> src)
    {
      int dest_x, dest_y;
      Untransform(src.X, src.Y, out dest_x, out dest_y);
      return new Coordinate<int>(dest_x, dest_y);
    }

    public void GetSize(out int width, out int height)
    {
      gimp_preview_get_size(Handle, out width, out height);
    }

    public Offset Position
    {
      get
	{
	  int x, y;
	  GetPosition(out x, out y);
	  return new Offset(x, y);
	}
    }

    public Dimensions Size
    {
      get
	{
	  int width, height;
	  GetSize(out width, out height);
	  return new Dimensions(width, height);
	}
    }

    public Rectangle Bounds
    {
      set 
	{
	  SetBounds(value.X1, value.Y1, value.X2, value.Y2);
	}
      get
	{
	  int x, y, width, height;
	  GetPosition(out x, out y);
	  GetSize(out width, out height);
	  return new Rectangle(x, y, x + width, y + height);
	}
    }

    public PreviewArea Area => new PreviewArea(gimp_preview_get_area(Handle));

    public void Draw()
    {
      gimp_preview_draw(Handle);
    }

    public void Redraw(Drawable drawable)
    {
      var rectangle = Bounds;
      int bpp = drawable.Bpp;
      int rowStride = rectangle.Width * bpp;
      var buffer = new byte[rectangle.Area * bpp];

      foreach (var pixel in new ReadPixelIterator(drawable))
	{
	  int index = pixel.Y * rowStride + pixel.X * bpp;
	  pixel.CopyTo(buffer, index);
	}
      DrawBuffer(buffer, rowStride);
    }

    public void DrawBuffer(byte[] buffer, int rowstride)
    {
      gimp_preview_draw_buffer(Handle, buffer, rowstride);
    }

    public void Invalidate()
    {
      gimp_preview_invalidate(Handle);
    }

    public Cursor DefaultCursor
    {
      set {gimp_preview_set_default_cursor(Handle, value);}
    }

    public HBox Controls => new HBox(gimp_preview_get_controls(Handle));

    [GLib.Signal("invalidated")]
    public event EventHandler Invalidated {
      add {
	GLib.Signal sig = GLib.Signal.Lookup(this, "invalidated");
	sig.AddDelegate (value);
      }
      remove {
	GLib.Signal sig = GLib.Signal.Lookup(this, "invalidated");
	sig.RemoveDelegate(value);
      }
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_update(IntPtr preview,
					       bool update);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_preview_get_update(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_bounds(IntPtr preview,
					       int xmin, int ymin, 
					       int xmax, int ymax);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_get_position(IntPtr preview,
						 out int x, out int y);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_transform(IntPtr preview,
					      int src_x, int src_y,
					      out int dest_x, out int dest_y);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_untransform(IntPtr preview,
						int src_x, int src_y,
						out int dest_x, out int dest_y);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_get_size(IntPtr preview,
					     out int width, out int height);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_preview_get_area(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_draw(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_draw_buffer(IntPtr preview,
						byte[] buffer, 
						int rowstride);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_invalidate(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_default_cursor(IntPtr preview,
						       Cursor cursor);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_preview_get_controls(IntPtr preview);  
  }
}
