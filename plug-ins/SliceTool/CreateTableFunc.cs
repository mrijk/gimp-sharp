// The SliceTool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class CreateTableFunc : MouseFunc
  {
    static readonly Cursor _cursor;

    public CreateTableFunc(SliceData sliceData, Preview preview) : 
      base(sliceData, preview, false, false)
    {
    }

    static CreateTableFunc()
    {
      _cursor = LoadCursor("cursor-table.png");
    }

    override protected void OnPress(IntCoordinate c) 
    {
      var rows = new Variable<int>(3);
      var columns = new Variable<int>(3);
      var dialog = new TableDialog(rows, columns);
      dialog.ShowAll();
      if (dialog.Run() == ResponseType.Ok)
	{
	  _sliceData.CreateTable(c, rows.Value, columns.Value);
	  Redraw();
	}
      dialog.Destroy();
    }

    override public Cursor GetCursor(IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) ? slice.Cursor : _cursor;
    }

    override public MouseFunc GetActualFunc(IntCoordinate c)
    {
      var slice = _sliceData.FindSlice(c);
      return (SliceIsSelectable(slice)) 
	? new SelectFunc(_sliceData, _preview) : (MouseFunc) this;
    }
  }
}
