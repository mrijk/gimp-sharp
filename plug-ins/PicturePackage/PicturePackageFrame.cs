// The PicturePackage plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// PicturePackageFrame.cs
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

namespace Gimp.PicturePackage
{
  public class PicturePackageFrame : GimpFrame
  {
    protected GimpTable _table;

    protected PicturePackageFrame(uint rows, uint columns, string label) : 
      base(label)
    {
      _table = new GimpTable(rows, columns, false);
      _table.ColumnSpacing = 6;
      _table.RowSpacing = 6;
      Add(_table);
    }

    protected ComboBox CreateComboBox(params string[] items)
    {
      ComboBox combo = ComboBox.NewText();

      foreach (string item in items)
	{
	  combo.AppendText(item);
	}
      combo.Active = 1;

      return combo;
    }

    protected void Attach(Widget widget, uint leftAttach, uint rightAttach, 
			  uint topAttach, uint bottomAttach)
    {
      _table.Attach(widget, leftAttach, rightAttach, topAttach, bottomAttach);
    }

    protected void AttachAligned(int column, int row, string labelText,
				 double xalign, double yalign, Widget widget,
				 int colspan, bool leftAlign)
    {
      _table.AttachAligned(column, row, labelText, xalign, yalign, widget,
			   colspan, leftAlign);
    }
  }
}
