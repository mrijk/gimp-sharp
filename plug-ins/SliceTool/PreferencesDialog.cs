// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// PreferencesDialog.cs
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

namespace Gimp.SliceTool
{
  public class PreferencesDialog : GimpDialog
  {
    public PreferencesDialog(Variable<RGB> active, Variable<RGB> inactive) : 
      base(_("Slice Preferences"), _("SliceTool"))
    {
      var table = new GimpTable(2, 2, false)
	{BorderWidth = 12, ColumnSpacing = 6, RowSpacing = 6};
      VBox.PackStart(table, true, true, 0);

      CreateColorButton(table, 0, _("Active tile border color:"), active);
      CreateColorButton(table, 1, _("Inactive tile border color:"), inactive);
    }

    void CreateColorButton(GimpTable table, int row, string label, 
			   Variable<RGB> color)
    {
      var button = new GimpColorButton("", 16, 16, color, ColorAreaType.Flat)
	{Update = true};
      table.AttachAligned(0, row, label, 0.0, 0.5, button, 1, true);
    }
  }
}
