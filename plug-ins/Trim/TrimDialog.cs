// The Trim plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// TrimDialog.cs
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

using Gtk;

namespace Gimp.Trim
{
  public class TrimDialog : GimpDialog
  {
    readonly Drawable _drawable;

    public TrimDialog(Drawable drawable, VariableSet variables) : 
      base("Trim", variables)
    {
      _drawable = drawable;
      
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      CreateBasedOnWidget(vbox);
      CreateTrimAwayWidget(vbox);

      SetTransient();
    }

    void CreateBasedOnWidget(VBox parent)
    {
      var frame = new GimpFrame(_("Based On"));
      parent.PackStart(frame, true, true, 0);

      var vbox = new VBox(false, 12);
      frame.Add(vbox);
      
      var button = AddBasedOnButton(vbox, null, 0, _("Transparent Pixels"));
      button.Sensitive = _drawable.HasAlpha;
      button = AddBasedOnButton(vbox, button, 1, _("Top Left Pixel Color"));
      AddBasedOnButton(vbox, button, 2, _("Bottom Right Pixel Color"));
    }

    RadioButton AddBasedOnButton(VBox vbox, RadioButton previous, int type, 
				 string description)
    {
      var button = new GimpRadioButton<int>(previous, description, type, 
					    GetVariable<int>("based-on"));
      vbox.Add(button);
      return button;
    }

    void CreateTrimAwayWidget(VBox parent)
    {
      var frame = new GimpFrame(_("Trim Away"));
      parent.PackStart(frame, true, true, 0);

      var table = new GimpTable(2, 2)
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);

      CreateTrimWidget(table, 0, 0, _("_Top"), "top");
      CreateTrimWidget(table, 1, 0, _("_Left"), "left");
      CreateTrimWidget(table, 0, 1, _("_Bottom"), "bottom");
      CreateTrimWidget(table, 1, 1, _("_Right"), "right");
    }

    void CreateTrimWidget(GimpTable table, uint column, uint row, string label, 
			  string identifier)
    {
      table.Attach(new GimpCheckButton(label, GetVariable<bool>(identifier)),
		   column, column + 1, row, row + 1);
    }
  }
}
