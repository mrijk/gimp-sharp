// The Difference Clouds plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// Dialog.cs
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

namespace Gimp.DifferenceClouds
{
  class Dialog : GimpDialog
  {
    public Dialog(VariableSet variables) : 
      base(_("Difference Clouds"), variables)
    {
      var table = new GimpTable(3, 4) {ColumnSpacing = 6, RowSpacing = 6};
      VBox.PackStart(table, false, false, 0);

      CreateRandomSeedWidget(table);
      CreateTurbulenceEntry(table);
    }

    void CreateRandomSeedWidget(GimpTable table)
    {
      var seed = GetVariable<UInt32>("seed");
      var randomSeed = GetVariable<bool>("random_seed");
      var seedWidget = new RandomSeed(seed, randomSeed);
      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seedWidget, 2, 
			  true);
    }

    void CreateTurbulenceEntry(GimpTable table)
    {
      new ScaleEntry(table, 0, 1, _("_Turbulence"), 150, 3,
		     GetVariable<double>("turbulence"), 0.0, 7.0, 0.1, 1.0, 1);
    }
  }
}
