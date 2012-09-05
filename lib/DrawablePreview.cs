// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
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
    public Drawable Drawable {get; private set;}
    readonly int _bpp;

    public DrawablePreview(Drawable drawable, bool toggle = false) :
      base(gimp_drawable_preview_new(drawable.Ptr, toggle))
    {
      Drawable = drawable;
      _bpp = drawable.Bpp;
    }

    public void DrawRegion(PixelRgn region)
    {
      var pr = region.PR;
      gimp_drawable_preview_draw_region(Handle, ref pr);
    }

    public new void Update(Func<Pixel, Pixel> func)
    {
      var rectangle = Bounds;

      int rowStride = rectangle.Width * _bpp;
      byte[] buffer = new byte[rectangle.Area * _bpp];

      var srcPR = new PixelRgn(Drawable, rectangle, false, false);

      var iterator = new RegionIterator(srcPR);
      iterator.ForEach(src =>
	{
	  int x = src.X;
	  int y = src.Y;
	  var pixel = func(src);
	  
	  int index = 
	  (y - rectangle.Y1) * rowStride + (x - rectangle.X1) * _bpp;
	  pixel.CopyTo(buffer, index);
	});
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
