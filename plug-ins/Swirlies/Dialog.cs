// The Swirlies plug-in
// Copyright (C) 2004-2012 Maurits Rijk
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

namespace Gimp.Swirlies
{
  public class Dialog : GimpDialogWithPreview
  {
    readonly ProgressBar _progress;

    public Dialog(Drawable drawable, VariableSet variables) : 
      base(_("Swirlies"), drawable, variables, () => new AspectPreview(drawable))
    {
      _progress = new ProgressBar();
      Vbox.PackStart(_progress, false, false, 0);
      
      var table = new GimpTable(4, 3, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      var seed = new RandomSeed(GetVariable<UInt32>("seed"), 
				GetVariable<bool>("random_seed"));

      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);

      new ScaleEntry(table, 0, 1, _("Po_ints:"), 150, 3, 
		     GetVariable<int>("points"), 1.0, 16.0, 1.0, 8.0, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables, Drawable);
      renderer.Render(preview as AspectPreview);
    }
  }
}
