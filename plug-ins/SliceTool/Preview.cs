// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
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

using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Preview : PreviewArea
  {
    Gdk.GC _gc;
    Drawable _drawable;
    SliceTool _parent;
    PreviewRenderer _renderer;

    Cursor _cursor;

    public Preview(Drawable drawable, SliceTool parent)
    {
      SetMaxSize(drawable.Width, drawable.Height);

      _drawable = drawable;
      _parent = parent;

      ExposeEvent += OnExposed;
      Realized += OnRealized;
      SizeAllocated += OnSizeAllocated;

      Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
	EventMask.PointerMotionHintMask | EventMask.PointerMotionMask |
	EventMask.LeaveNotifyMask;
    }

    void OnExposed (object o, ExposeEventArgs args)
    {	
      _parent.Redraw(_renderer);
    }

    void OnRealized (object o, EventArgs args)
    {
      _gc = new Gdk.GC(GdkWindow);
      _renderer = new PreviewRenderer(this, _gc, 
				      _drawable.Width, _drawable.Height);
      FillWithImage();
    }

    void OnSizeAllocated(object o, SizeAllocatedArgs args)
    {
      FillWithImage();
    }

    void FillWithImage()
    {
      int width = _drawable.Width;
      int height = _drawable.Height;

      PixelRgn rgn = new PixelRgn(_drawable, 0, 0, width, height, 
				  false, false);
      
      byte[] buf = rgn.GetRect(0, 0, width, height);
      Draw(0, 0, width, height, ImageType.RGB, buf, width * _drawable.Bpp);
    }

    public PreviewRenderer Renderer
    {
      get {return _renderer;}
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
