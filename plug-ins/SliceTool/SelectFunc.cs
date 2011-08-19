// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
    readonly PreviewRenderer _renderer;
    readonly SliceData _sliceData;
    Slice _slice;

    public SelectFunc(SliceData sliceData, Preview preview) :
      base(preview, false, false)
    {
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      if (slice == null)
	{
	  _sliceData.SelectRectangle(c);
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
		
    override protected void OnMove(IntCoordinate c)
    {
      _slice.Draw(_renderer);
      _slice.SetPosition(c);
      _slice.Draw(_renderer);		
    }

    override public Cursor GetCursor(IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) ? slice.Cursor : base.GetCursor(c);
    }
  }
}
