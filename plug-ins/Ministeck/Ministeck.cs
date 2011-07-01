// The Ministeck plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Ministeck.cs
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
using System.Collections.Generic;

using Gtk;

namespace Gimp.Ministeck
{
  class Ministeck : Plugin
  {
    DrawablePreview _preview;

    Variable<bool> _limit = 
    new Variable<bool>("limit", 
		       _("Use real life ratio for number of pieces if true"), 
		       true);
    Variable<int> _size = new Variable<int>("size", _("Default size"), 16);
    Variable<RGB> _color = new Variable<RGB>("color", 
					     _("Color for the outline"), 
					     new RGB(0, 0, 0));

    static void Main(string[] args)
    {
      GimpMain<Ministeck>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_ministeck",
				 _("Generates Ministeck"),
				 _("Generates Ministeck"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2011",
				 _("Ministeck..."),
				 "RGB*, GRAY*",
				 new ParamDefList(_limit, _size, _color))
	{
	  MenuPath = "<Image>/Filters/Artistic",
	  IconFile = "Ministeck.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("ministeck", true);

      var dialog = DialogNew(_("Ministeck"), _("ministeck"), 
			     IntPtr.Zero, 0, null, _("ministeck"));
	
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      var table = new GimpTable(2, 2) 
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      var size = new GimpSpinButton(3, 100, 1, _size);
      table.AttachAligned(0, 0, _("_Size:"), 0.0, 0.5, size, 2, true);

      var limit = new GimpCheckButton(_("_Limit Shapes"), _limit);
      table.Attach(limit, 2, 3, 0, 1);

      var colorButton = new GimpColorButton("", 16, 16, _color, 
					      ColorAreaType.Flat) 
	{Update = true};

      table.AttachAligned(0, 1, _("C_olor:"), 0.0, 0.5, colorButton, 1, true);

      _size.ValueChanged += UpdatePreview;
      _limit.ValueChanged += UpdatePreview;
      _color.ValueChanged += UpdatePreview;

      return dialog;
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!

      var clone = new Image(_image);
      clone.Crop(_preview.Bounds);

      var drawable = clone.ActiveDrawable;
      RenderMinisteck(clone, drawable, true);
      _preview.Redraw(drawable);
      clone.Delete();
    }

    void RenderMinisteck(Image image, Drawable drawable, bool preview)
    {
      int size = _size.Value;

      image.UndoGroupStart();
      RunProcedure("plug_in_pixelize", image, drawable, size);

      var palette = new MinisteckPalette();
      image.ConvertIndexed(ConvertDitherType.No, ConvertPaletteType.Custom,
			   0, false, false, "Ministeck");
      palette.Delete();

      image.ConvertRgb();
      image.UndoGroupEnd();

      // And finally calculate the Ministeck pieces
	
      using (var painter = new Painter(drawable, size, _color.Value))
	{
	  Shape.Painter = painter;

	  int width = drawable.Width / size;
	  int height = drawable.Height / size;

	  var A = new BoolMatrix(width, height);

	  // Fill in shapes
	  bool limit = _limit.Value;
	  var shapes = new ShapeSet();
	  shapes.Add((limit) ? 2 : 1, new TwoByTwoShape());
	  shapes.Add((limit) ? 8 : 1, new ThreeByOneShape());
	  shapes.Add((limit) ? 3 : 1, new TwoByOneShape());
	  shapes.Add((limit) ? 2 : 1, new CornerShape());
	  shapes.Add((limit) ? 1 : 1, new OneByOneShape());

	  Action<int> update = null;
	  if (!preview)
	    {
	      var progress = new Progress(_("Ministeck..."));
	      update = y => progress.Update((double) y / height);
	    }

	  var generator = 
	    new CoordinateGenerator(new Rectangle(0, 0, width, height), update);
	  generator.ForEach(c => {if (!A.Get(c)) shapes.Fits(A, c);});
	}

      drawable.Flush();
      drawable.Update();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      RenderMinisteck(image, drawable, false);
    }
  }
}
