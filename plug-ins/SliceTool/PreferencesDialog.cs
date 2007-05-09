// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk  m.rijk@chello.nl
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

using System;

namespace Gimp.SliceTool
{
  public class PreferencesDialog : GimpDialog
  {
    GimpColorButton _active;
    GimpColorButton _inactive;

    public PreferencesDialog() : base(_("Slice Preferences"), _("SliceTool"), 
				      IntPtr.Zero, 0, null, _("SliceTool"))
    {
      GimpTable table = new GimpTable(2, 2, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      _active = new GimpColorButton("", 16, 16, new RGB(255, 0, 0),
				    ColorAreaType.Flat);
      _active.Update = true;
      table.AttachAligned(0, 0, _("Active tile border color:"),
			  0.0, 0.5, _active, 1, true);

      _inactive = new GimpColorButton("", 16, 16, new RGB(0, 255, 0),
				      ColorAreaType.Flat);
      _inactive.Update = true;
      table.AttachAligned(0, 1, _("Inactive tile border color:"), 
			  0.0, 0.5, _inactive, 1, true);
    }

    public RGB ActiveColor
    {
      get {return _active.Color;}
      set {_active.Color = value;}
    }

    public RGB InactiveColor
    {
      get {return _inactive.Color;}
      set {_inactive.Color = value;}
    }
  }
}
