// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
//
// SelectFunc.cs
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

namespace Gimp.SliceTool
{
  public class SelectFunc : MouseFunc
  {
    SliceTool _parent;
    PreviewRenderer _renderer;
    SliceData _sliceData;
    Slice _slice;
    Rectangle _rectangle;

    public SelectFunc(SliceTool parent, SliceData sliceData, Preview preview) :
      base(preview, false, false)
    {
      _parent = parent;
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(int x, int y) 
    {
      Slice slice = _sliceData.FindSlice(x, y);
      if (slice == null)
	{
	  Rectangle rectangle = _sliceData.SelectRectangle(x, y);
	  if (rectangle != _rectangle)
	    {
	      _parent.SetRectangleData(_rectangle);
	      _rectangle = rectangle;
	      Redraw();
	      _parent.GetRectangleData(rectangle);
	    }
	}
      else if (!slice.Locked)
	{
	  _slice = slice;
	  _renderer.Function = Gdk.Function.Equiv;
	  AddReleaseEvent();
	  AddMotionNotifyEvent();
	}
    }

    override protected void OnRelease() 
    {
      _renderer.Function = Gdk.Function.Copy;
      _sliceData.Cleanup(_slice);
      Redraw();
    }
		
    override protected void OnMove(int x, int y) 
    {
      _slice.Draw(_renderer);
      _slice.SetPosition(x, y);
      _slice.Draw(_renderer);		
    }

    override public Cursor GetCursor(int x, int y)
    {
      Slice slice = _sliceData.FindSlice(x, y);
      if (slice != null && !slice.Locked)
	{
	  return slice.Cursor;
	}
      else
	{
	  return base.GetCursor(x, y);
	}
    }
  }
}
