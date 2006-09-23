// The Splitter plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// AdvancedDialog.cs
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

namespace Gimp.Splitter
{
  public class AdvancedDialog : GimpDialog
  {
    UInt32 _seed;
    bool _randomSeed;

    public AdvancedDialog(UInt32 seed, bool randomSeed) : 
      base(_("Advanced Settings"), _("Splitter"), IntPtr.Zero, 0, null, 
	   _("Splitter"))
    {
      _seed = seed;
      _randomSeed = randomSeed;

      GimpTable table = new GimpTable(1, 2, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      RandomSeed random = new RandomSeed(ref _seed, ref _randomSeed);
      
      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, random, 2, true);
    }

    public UInt32 Seed
    {
      get {return _seed;}
    }

    public bool RandomSeed
    {
      get {return _randomSeed;}
    }
  }
}
