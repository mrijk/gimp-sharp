// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// PreviewArea.cs
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

using Gtk;

namespace Gimp
{
  public class PreviewArea : DrawingArea
  {
    public PreviewArea() : base(gimp_preview_area_new())
    {
    }

    internal PreviewArea(IntPtr ptr) : base(ptr)
    {
    }

    public void Draw(int x, int y, int width, int height,
		     ImageType type, byte[] buf, int rowstride)
    {
      gimp_preview_area_draw(Handle, x, y, width, height, 
			     type, buf, rowstride);
    }

    public void Draw(Rectangle rectangle, ImageType type, byte[] buf, 
		     int rowstride)
    {
      Draw(rectangle.X1, rectangle.Y1, rectangle.Width, rectangle.Height, 
	   type, buf, rowstride);
    }

    public void Draw(Drawable drawable)
    {
      var rgn = new PixelRgn(drawable, false, false);

      var rectangle = drawable.Bounds;
      var buf = rgn.GetRect(rectangle);
      Draw(rectangle, drawable.Type, buf, rectangle.Width * drawable.Bpp);
    }

    public void SetMaxSize(int width, int height)
    {
      gimp_preview_area_set_max_size(Handle, width, height);
    }

    public Dimensions MaxSize
    {
      set {SetMaxSize(value.Width, value.Height);}
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_preview_area_new ();
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_area_draw(IntPtr area,
					      int x,
					      int y,
					      int width,
					      int height,
					      ImageType type,
					      byte[] buf,
					      int rowstride);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_area_set_max_size(IntPtr area,
						      int width,
						      int height);
  }
}
