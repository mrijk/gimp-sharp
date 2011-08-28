// The ncp plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

namespace Gimp.ncp
{
  public class Dialog : GimpDialogWithPreview<AspectPreview>
  {
    ScaleEntry _closestEntry;

    public Dialog(Drawable drawable, VariableSet variables) : 
      base("ncp", drawable, variables)
    {
      var table = new GimpTable(4, 3) {ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      var pointsVariable = GetVariable<int>("points");
      var closest = GetVariable<int>("closest");
      var color = GetVariable<bool>("color");

      CreateRandomSeedWidget(table);
      CreatePointsWidget(table, pointsVariable);
      CreateClosestEntryWidget(table, closest, pointsVariable);
      CreateUseColorWidget(table, color);

      pointsVariable.ValueChanged += delegate {
	int points = pointsVariable.Value;
	if (points > _closestEntry.Upper)
	{
	  _closestEntry.Upper = points;
	}
	
	if (points < closest.Value)
	  {
	    closest.Value = points;
	    _closestEntry.Upper = closest.Value;
	    _closestEntry.Value = closest.Value;
	  }
	else
	{
	  InvalidatePreview();
	}
      };
      closest.ValueChanged += delegate {InvalidatePreview();};
      color.ValueChanged += delegate {InvalidatePreview();};
    }

    void CreateRandomSeedWidget(GimpTable table)
    {
      var seed = GetVariable<UInt32>("seed");
      var randomSeed = GetVariable<bool>("random_seed");
      var seedWidget = new RandomSeed(seed, randomSeed);
      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seedWidget, 2, true);
    }

    void CreatePointsWidget(GimpTable table, Variable<int> points)
    {
      new ScaleEntry(table, 0, 1, _("Po_ints:"), 150, 3, 
		     points, 1.0, 256.0, 1.0, 8.0, 0);
    }

    void CreateClosestEntryWidget(GimpTable table, Variable<int> closest,
				  Variable<int> points)
    {
      _closestEntry = new ScaleEntry(table, 0, 2, _("C_lose to:"), 150, 3, 
				     closest, 1.0, points.Value, 1.0, 8.0, 0);
    }

    void CreateUseColorWidget(GimpTable table, Variable<bool> color)
    {
      table.Attach(new GimpCheckButton(_("_Use color"), color), 0, 1, 3, 4);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables, Drawable);
      renderer.Render(preview as AspectPreview);
    }
  }
}
