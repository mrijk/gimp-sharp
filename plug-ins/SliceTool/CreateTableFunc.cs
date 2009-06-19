// The SliceTool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// CreateTableFunc.cs
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
  public class CreateTableFunc : MouseFunc
  {
    static readonly Cursor _cursor;
    readonly SliceData _sliceData;

    public CreateTableFunc(SliceData sliceData, Preview preview) : 
      base(preview, false, false)
    {
      _sliceData = sliceData;
    }

    static CreateTableFunc()
    {
      _cursor = LoadCursor("cursor-table.png");
    }

    override protected void OnPress(Coordinate<int> c) 
    {
      TableDialog dialog = new TableDialog();
      dialog.ShowAll();
      if (dialog.Run() == ResponseType.Ok)
	{
	  _sliceData.CreateTable(c, dialog.Rows, dialog.Columns);
	  Redraw();
	}
      dialog.Destroy();
    }

    override public Cursor GetCursor(Coordinate<int> c)
    {
      Slice slice = _sliceData.FindSlice(c);
      return (slice != null && !slice.Locked) ? slice.Cursor : _cursor;
    }

    override public MouseFunc GetActualFunc(SliceTool parent,
					    Coordinate<int> c)
    {
      Slice slice = _sliceData.FindSlice(c);
      if (slice == null || slice.Locked)
	{
	  return this;
	}
      else
	{
	  return new SelectFunc(parent, _sliceData);
	}
    }
  }
}
