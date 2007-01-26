// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk  m.rijk@chello.nl
//
// TableDialog.cs
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

namespace Gimp.SliceTool
{
  public class TableDialog : GimpDialog
  {
    int _columns = 3;
    int _rows = 3;

    public TableDialog() : base(_("Insert Table"), _("SliceTool"), 
				IntPtr.Zero, 0, null, _("SliceTool"))
    {
      GimpTable table = new GimpTable(2, 3, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      new ScaleEntry(table, 0, 1, _("Co_lumns"), 150, 3,
		     _columns, 1.0, 16.0, 1.0, 1.0, 0,
		     true, 0, 0, null, null);

      new ScaleEntry(table, 0, 2, _("_Rows"), 150, 3,
		     _rows, 1.0, 16.0, 1.0, 1.0, 0,
		     true, 0, 0, null, null);
    }

    public int Columns
    {
      get {return _columns;}
    }

    public int Rows
    {
      get {return _rows;}
    }

  }
}
