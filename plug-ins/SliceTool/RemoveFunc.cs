// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
//
// RemoveFunc.cs
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
  public class RemoveFunc : MouseFunc
  {
    static readonly Cursor _cursor;
    SliceData _sliceData;

    public RemoveFunc(SliceData sliceData, Preview preview) : 
      base(preview, false, false)
    {
      _sliceData = sliceData;
    }

    static RemoveFunc()
    {
      _cursor = LoadCursor("cursor-eraser.png");
    }

    override protected void OnPress(int x, int y) 
    {
      Slice slice = _sliceData.MayRemove(x, y);
      if (slice != null)
	{
	  _sliceData.Remove(slice);
	  Redraw();
	}
    }

    override public Cursor GetCursor(int x, int y)
    {
      Slice slice = _sliceData.MayRemove(x, y);
      if (slice == null)
	{
	  return base.GetCursor(x, y);
	}
      else
	{
	  return _cursor;
	}
    }
  }
}
