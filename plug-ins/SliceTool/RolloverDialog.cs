// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
//
// RolloverDialog.cs
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

using Gtk;

namespace Gimp.SliceTool
{
  public class RolloverDialog : GimpDialog
  {
    RolloverEntry _mouseOver;
    RolloverEntry _mouseOut;
    RolloverEntry _mouseClick;
    RolloverEntry _mouseDoubleClick;
    RolloverEntry _mouseUp;
    RolloverEntry _mouseDown;

    public RolloverDialog() : base("Rollover Creator", "SliceTool",
				   IntPtr.Zero, 0, null, "SliceTool")
    {
      GimpTable table = new GimpTable(7, 3, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      _mouseOver = new RolloverEntry(table, "_Mouse over", 0);
      _mouseOut = new RolloverEntry(table, "Mo_use out", 1);
      _mouseClick = new RolloverEntry(table, "Mous_e click", 2);
      _mouseDoubleClick = new RolloverEntry(table, "Mouse dou_ble click", 3);
      _mouseUp = new RolloverEntry(table, "Mouse _up", 4);
      _mouseDown = new RolloverEntry(table, "Mouse _down", 5);

      Label label = new Label("If a file is not given for the rollover, the original file will be used.");
      table.Attach(label, 0, 2, 6, 7);
    }

    public void SetRectangleData(Rectangle rectangle)
    {
      _mouseOver.FileName = rectangle.GetProperty("MouseOver");
      _mouseOut.FileName = rectangle.GetProperty("MouseOut");
      _mouseClick.FileName = rectangle.GetProperty("MouseClick");
      _mouseDoubleClick.FileName = rectangle.GetProperty("MouseDoubleClick");
      _mouseUp.FileName = rectangle.GetProperty("MouseUp");
      _mouseDown.FileName = rectangle.GetProperty("MouseDown");
    }

    public void GetRectangleData(Rectangle rectangle)
    {
      rectangle.SetProperty("MouseOver", _mouseOver.FileName);
      rectangle.SetProperty("MouseOut", _mouseOut.FileName);
      rectangle.SetProperty("MouseClick", _mouseClick.FileName);
      rectangle.SetProperty("MouseDoubleClick", _mouseDoubleClick.FileName);
      rectangle.SetProperty("MouseUp", _mouseUp.FileName);
      rectangle.SetProperty("MouseDown", _mouseDown.FileName);
    }

    public bool Enabled
    {
      get {return false;}	// Fix me!
    }
  }
}
