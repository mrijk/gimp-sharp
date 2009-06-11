// The Ministeck plug-in
// Copyright (C) 2004-2009 Maurits Rijk
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

    [SaveAttribute("limit")]
    bool _limit = true;
    [SaveAttribute("size")]
    int _size = 16;
    [SaveAttribute("color")]
    RGB _color = new RGB(0, 0, 0);

    static void Main(string[] args)
    {
      new Ministeck(args);
    }

    Ministeck(string[] args) : base(args, "Ministeck")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList()
	{
	  new ParamDef("limit", true, typeof(bool), 
		       _("Use real life ratio for number of pieces if true")),
	  new ParamDef("size", 16, typeof(int), 
		       _("Default size")),
	  new ParamDef("color", new RGB(0, 0, 0), typeof(RGB), 
		       _("Color for the outline"))
	};
      yield return new Procedure("plug_in_ministeck",
				 _("Generates Ministeck"),
				 _("Generates Ministeck"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2009",
				 _("Ministeck..."),
				 "RGB*, GRAY*",
				 inParams)
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

      var table = new GimpTable(2, 2, false) 
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      var size = new SpinButton(3, 100, 1) {Value = _size};
      table.AttachAligned(0, 0, _("_Size:"), 0.0, 0.5, size, 2, true);
      size.ValueChanged += delegate
	{
	  _size = size.ValueAsInt;
	  _preview.Invalidate();
	};

      var limit = new CheckButton(_("_Limit Shapes"));
      table.Attach(limit, 2, 3, 0, 1);
      limit.Active = _limit;
      limit.Toggled += delegate 
	{
	  _limit = limit.Active;
	  _preview.Invalidate();
	};

      var colorButton = new GimpColorButton("", 16, 16, _color, 
					    ColorAreaType.Flat);
      colorButton.Update = true;
      colorButton.ColorChanged += delegate
	{
	  _color = colorButton.Color;
	  _preview.Invalidate();
	};
      table.AttachAligned(0, 1, _("C_olor:"), 0.0, 0.5, colorButton, 1, true);
#if false      
      Expander expander = new Expander(_("Colors Needed"));
      vbox.PackStart(expander, false, false, 0);
      GimpTable colorTable = new GimpTable(4, 6, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      for (uint i = 0; i < 6; i++)
	{
	  for (uint j = 0; j < 6; j++)
	    {
	      RGB rgb = new RGB(i * 40 / 256.0, 
				j * 40 / 256.0, 
				(i + j) * 20 / 256.0);
	      GimpColorButton color = new GimpColorButton("", 16, 16, rgb,
							  ColorAreaType.Flat);
	      colorTable.Attach(color, i, i + 1, j, j + 1);
	    }
	}
      expander.Add(colorTable);
#endif
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
      image.UndoGroupStart();
      RunProcedure("plug_in_pixelize", image, drawable, _size);

      var palette = new MinisteckPalette();
      image.ConvertIndexed(ConvertDitherType.No, ConvertPaletteType.Custom,
			   0, false, false, "Ministeck");
      palette.Delete();

      image.ConvertRgb();
      image.UndoGroupEnd();

      // And finally calculate the Ministeck pieces
	
      int width = drawable.Width / _size;
      int height = drawable.Height / _size;

      using (var painter = new Painter(drawable, _size, _color))
	{
	  Shape.Painter = painter;

	  bool[,] A = new bool[width, height];

	  // Fill in shapes
      
	  var shapes = new ShapeSet();

	  if (_limit)
	    {
	      shapes.Add(2, new TwoByTwoShape());
	      shapes.Add(8, new ThreeByOneShape());
	      shapes.Add(3, new TwoByOneShape());
	      shapes.Add(2, new CornerShape());
	      shapes.Add(1, new OneByOneShape());
	    }
	  else
	    {
	      shapes.Add(new TwoByTwoShape());
	      shapes.Add(new ThreeByOneShape());
	      shapes.Add(new TwoByOneShape());
	      shapes.Add(new CornerShape());
	      shapes.Add(new OneByOneShape());
	    }

	  Progress progress = null;
	  if (!preview)
	    progress = new Progress(_("Ministeck..."));

	  for (int y = 0; y < height; y++)
	    {
	      for (int x = 0; x < width; x++)
		{
		  if (!A[x, y])
		    {
		      shapes.Fits(A, new Coordinate<int>(x, y));
		    }
		}
	      if (!preview)
		progress.Update((double) y / height);
	    }
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
