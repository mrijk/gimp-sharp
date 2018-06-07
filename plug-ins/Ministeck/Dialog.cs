// The Ministeck plug-in
// Copyright (C) 2004-2018 Maurits Rijk
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

using Gtk;

namespace Gimp.Ministeck
{
  public class Dialog : GimpDialogWithPreview
  {
    readonly Image _image;

    public Dialog(Image image, Drawable drawable, VariableSet variables) : 
      base("Ministeck", drawable, variables, () => new DrawablePreview(drawable))
    {
      _image = image;

      var table = new GimpTable(2, 2) 
	{ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      var size = new GimpSpinButton(3, 100, 1, GetVariable<int>("size"));
      table.AttachAligned(0, 0, _("_Size:"), 0.0, 0.5, size, 2, true);

      var limit = new GimpCheckButton(_("_Limit Shapes"), 
				      GetVariable<bool>("limit"));
      table.Attach(limit, 2, 3, 0, 1);

      var colorButton = new GimpColorButton("", 16, 16, GetVariable<RGB>("color"),
					    ColorAreaType.Flat) 
	{Update = true};

      table.AttachAligned(0, 1, _("C_olor:"), 0.0, 0.5, colorButton, 1, true);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!

      var clone = new Image(_image);
      clone.Crop(preview.Bounds);

      var drawable = clone.ActiveDrawable;

      var renderer = new Renderer(Variables);
      renderer.Render(clone, drawable, true);

      preview.Redraw(drawable);
      clone.Delete();
    }
  }
}

