// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// DrawablePreview.cs
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
  public class DrawablePreview : ScrolledPreview
  {
    readonly Drawable _drawable;

    public DrawablePreview(Drawable drawable, bool toggle) :
      base(gimp_drawable_preview_new(drawable.Ptr, toggle))
    {
      _drawable = drawable;
    }

    public Drawable Drawable
    {
      get {return _drawable;}
    }

    public void DrawRegion(PixelRgn region)
    {
      GimpPixelRgn pr = region.PR;
      gimp_drawable_preview_draw_region(Handle, ref pr);
    }

    public void Redraw(Drawable drawable)
    {
      Rectangle rectangle = Bounds;
      int bpp = drawable.Bpp;
      int rowStride = rectangle.Width * bpp;
      byte[] buffer = new byte[rectangle.Area * bpp];

      foreach (Pixel pixel in new ReadPixelIterator(drawable))
	{
	  int index = pixel.Y * rowStride + pixel.X * bpp;
	  pixel.CopyTo(buffer, index);
	}
      DrawBuffer(buffer, rowStride);
    }

    [DllImport("libgimpui-2.0-0.dll")]
    extern static IntPtr gimp_drawable_preview_new(IntPtr drawable, 
						   bool toggle);
    [DllImport("libgimpui-2.0-0.dll")]
    extern static void gimp_drawable_preview_draw_region(IntPtr preview,
							 ref GimpPixelRgn region);
  }
}
