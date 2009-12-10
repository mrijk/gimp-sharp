// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Preview.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Preview : PreviewArea
  {
    public PreviewRenderer Renderer {get; set;}

    Cursor _cursor;

    public Preview(Drawable drawable, SliceTool parent)
    {
      MaxSize = drawable.Dimensions;

      ExposeEvent += delegate {parent.Redraw(Renderer);};
      Realized += delegate
	{
	  var gc = new Gdk.GC(GdkWindow);
	  Renderer = new PreviewRenderer(this, gc, drawable.Dimensions);
	  FillWithImage(drawable);
	};
      SizeAllocated += delegate {FillWithImage(drawable);};

      Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
	EventMask.PointerMotionHintMask | EventMask.PointerMotionMask |
	EventMask.LeaveNotifyMask;
    }

    void FillWithImage(Drawable drawable)
    {
      Draw(drawable, ImageType.Rgb);
    }

    public void SetCursor(Cursor cursor)
    {
      if (cursor != _cursor)
	{
	  _cursor = cursor;
	  GdkWindow.Cursor = cursor;
	}
    }
  }
}
