// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// CellPropertiesFrame.cs
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
  public class CellPropertiesFrame : GimpFrame
  {
    readonly Entry _url;
    readonly Entry _altText;
    readonly Entry _target;
    readonly CheckButton _include;

    readonly Label _left;
    readonly Label _right;
    readonly Label _top;
    readonly Label _bottom;

    public CellPropertiesFrame() : base(_("Cell Properties"))
    {
      var vbox = new VBox(false, 12);
      Add(vbox);

      var table = new GimpTable(3, 2, false) 
	{ColumnSpacing = 6, RowSpacing = 6};
      
      vbox.Add(table);
			
      _url = new Entry();
      table.AttachAligned(0, 0, _("_Link:"), 0.0, 0.5, _url, 3, false);
      
      _altText = new Entry();
      table.AttachAligned(0, 1, _("Alt_ernative text:"), 0.0, 0.5, _altText, 3,
			  false);
      
      _target = new Entry();
      table.AttachAligned(0, 2, _("_Target:"), 0.0, 0.5, _target, 3, false);

      var hbox = new HBox(false, 12);
      vbox.Add(hbox);

      table = new GimpTable(3, 4, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      hbox.PackStart(table, false, false, 0);

      _left = new Label("    ");
      table.AttachAligned(0, 0, _("Left:"), 0.0, 0.5, _left, 1, false);
      
      _right = new Label("    ");
      table.AttachAligned(0, 1, _("Right:"), 0.0, 0.5, _right, 1, false);
      
      _top = new Label("    ");
      table.AttachAligned(2, 0, _("Top:"), 0.0, 0.5, _top, 1, false);

      _bottom = new Label("    ");
      table.AttachAligned(2, 1, _("Bottom:"), 0.0, 0.5, _bottom, 1, false);
      
      _include = new CheckButton(_("_Include cell in table")) {Active = true};
      table.Attach(_include, 0, 3, 2, 3);
    }

    public void GetRectangleData(Rectangle rectangle)
    {
      _url.Text = rectangle.GetProperty("href");
      _altText.Text = rectangle.GetProperty("AltText");
      _target.Text = rectangle.GetProperty("Target");
      _include.Active = rectangle.Include;		

      _left.Text = rectangle.X1.ToString();
      _right.Text = rectangle.X2.ToString();
      _top.Text = rectangle.Y1.ToString();
      _bottom.Text = rectangle.Y2.ToString();
    }

    public void SetRectangleData(Rectangle rectangle)
    {
      rectangle.SetProperty("href", _url.Text);
      rectangle.SetProperty("AltText", _altText.Text);
      rectangle.SetProperty("Target", _target.Text);
      rectangle.Include = _include.Active;
    }
  }
}
