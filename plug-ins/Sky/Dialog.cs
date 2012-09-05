// The Sky plug-in
// Copyright (C) 2004-2012 Maurits Rijk
//
// Code ported from Physically Modeled Media Plug-In for The GIMP
//                  Copyright (c) 2000-2001 David A. Bartold
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

namespace Gimp.Sky
{
  public class Dialog : GimpDialogWithPreview
  {
    public Dialog(Drawable drawable, VariableSet variables) : 
      base("Sky", drawable, variables, () => new AspectPreview(drawable))
    {
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      CreateRandomEntry(vbox);
      CreateSunParameters(vbox);
      CreateCameraParameters(vbox);
      CreateColorParameters(vbox);
    }

    void CreateRandomEntry(VBox vbox)
    {
      var table = new GimpTable(1, 3) {ColumnSpacing = 6, RowSpacing = 6};
      vbox.Add(table);

      var seed = new RandomSeed(GetVariable<UInt32>("seed"),
				GetVariable<bool>("random_seed"));

      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);
    }

    void CreateSunParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Sun", 3, 2);

      var sunX = new ScaleEntry(table, 0, 1, _("_X:"), 150, 4, 
				GetVariable<double>("sun_x"),
				0.0, 1.0, 0.01, 0.1, 2);

      var sunY = new ScaleEntry(table, 0, 2, _("_Y:"), 150, 4, 
				GetVariable<double>("sun_y"),
				0.0, 1.0, 0.01, 0.1, 2);

      var sunShow = GetVariable<bool>("sun_show");
      sunShow.ValueChanged += delegate
	{
	  sunX.Sensitive = sunY.Sensitive = sunShow.Value;
	};

      table.Attach(new GimpCheckButton(_("_Show sun"), sunShow), 0, 2, 3, 4);

      Preview.ButtonPressEvent += (sender, args) =>
	{
	  if (sunShow.Value)
	  {
	    var size = Preview.Size;
	    sunX.Value = args.Event.X / size.Width;
	    sunY.Value = args.Event.Y / size.Height;
	  }
	};
    }

    void CreateCameraParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Camera", 2, 1);

      new ScaleEntry(table, 0, 1, _("_Rotation angle:"), 150, 3, 
		     GetVariable<double>("rotation"), 0.0, 90.0, 1.0, 8.0, 0);

      new ScaleEntry(table, 0, 2, _("_Tilt angle:"), 150, 3, 
		     GetVariable<double>("tilt"), 0.0, 90.0, 1.0, 8.0, 0);
    }

    void CreateColorParameters(VBox vbox)
    {
      var table = CreateFramedTable(vbox, "Colors", 3, 4);

      CreateColorButton(table, 0, 0, "_Horizon:", "horizon_color");
      CreateColorButton(table, 0, 1, "S_ky:", "sky_color");
      CreateColorButton(table, 0, 2, "S_un:", "sun_color");
      CreateColorButton(table, 2, 0, "C_loud:", "cloud_color");
      CreateColorButton(table, 2, 1, "Sh_adow:", "shadow_color");
    }

    GimpTable CreateFramedTable(VBox vbox, string label, 
				uint rows, uint columns)
    {
      var frame = new GimpFrame(_(label));
      vbox.Add(frame);

      var table = new GimpTable(rows, columns) 
	{ColumnSpacing = 6, RowSpacing = 6};
      frame.Add(table);

      return table;
    }

    void CreateColorButton(GimpTable table, int column, int row, string label,
			   string identifier)
    {
      var color = GetVariable<RGB>(identifier);
      var button = new GimpColorButton("", 16, 16, color, ColorAreaType.Flat) 
	{Update = true};
      table.AttachAligned(column, row, _(label), 0.0, 0.5, button, 1, true);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables, Drawable);
      renderer.Render(preview as AspectPreview);
    }
  }
}
