// The Raindrops plug-in
// Copyright (C) 2004-2012 Maurits Rijk, Massimo Perga
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

namespace Gimp.Raindrops
{
  public class Dialog : GimpDialogWithPreview
  {
    readonly Image _image;

    public Dialog(Image image, Drawable drawable, VariableSet variables) : 
      base("Raindrops", drawable, variables, () => new AspectPreview(drawable))
    {
      _image = image;

      var table = new GimpTable(2, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      CreateDropSizeEntry(table);
      CreateNumberEntry(table);
      CreateFishEyeEntry(table);

      // variables.ValueChanged += delegate {InvalidatePreview();};
    }

    void CreateDropSizeEntry(Table table)
    {
      new ScaleEntry(table, 0, 1, _("_Drop size:"), 150, 3, 
		     GetVariable<int>("drop_size"), 1.0, 256.0, 1.0, 8.0, 0);
    }

    void CreateNumberEntry(Table table)
    {
      new ScaleEntry(table, 0, 2, _("_Number:"), 150, 3, GetVariable<int>("number"), 1.0,
		     256.0, 1.0, 8.0, 0);
    }

    void CreateFishEyeEntry(Table table)
    {
      new ScaleEntry(table, 0, 3, _("_Fish eye:"), 150, 3, GetVariable<int>("fish_eye"), 
		     1.0, 256.0, 1.0, 8.0, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!
      var clone = new Image(_image);
      clone.Crop(preview.Bounds);

      var drawable = clone.ActiveDrawable;

      var renderer = new Renderer(Variables);
      renderer.Render(clone, drawable, null);
      preview.Redraw(drawable);
      clone.Delete();
    }
  }
}
