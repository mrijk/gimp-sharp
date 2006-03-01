// The Ministeck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
using System.Collections;
using System.Collections.Generic;

using Gtk;

namespace Gimp.Ministeck
{
  public class Ministeck : Plugin
  {
    DrawablePreview _preview;

    [SaveAttribute]
    bool _limit = true;
    [SaveAttribute]
    int _size = 16;
    [SaveAttribute]
    RGB _color = new RGB(0, 0, 0);

    [STAThread]
    static void Main(string[] args)
    {
      new Ministeck(args);
    }

    public Ministeck(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("size", 16, typeof(int), "Default size"));

      Procedure procedure = new Procedure("plug_in_ministeck",
					  "Generates Ministeck",
					  "Generates Ministeck",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2004-2006",
					  "Ministeck...",
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Artistic";
      procedure.IconFile = "Ministeck.png";
	
      set.Add(procedure);
	
      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("ministeck", true);

      Dialog dialog = DialogNew("Ministeck", "ministeck",
				IntPtr.Zero, 0, null, "ministeck");
	
      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      SpinButton size = new SpinButton(3, 100, 1);
      size.Value = _size;
      table.AttachAligned(0, 0, "_Size:", 0.0, 0.5, size, 2, true);
      size.ValueChanged += SizeChanged;

      CheckButton limit = new CheckButton("_Limit Shapes");
      table.Attach(limit, 2, 3, 0, 1);
      limit.Active = _limit;
      limit.Toggled += delegate(object sender, EventArgs args)
	{
	  _limit = limit.Active;
	};

      GimpColorButton colorButton = 
	new GimpColorButton("", 16, 16, _color, ColorAreaType.COLOR_AREA_FLAT);
      colorButton.Update = true;
      colorButton.ColorChanged += delegate(object sender, EventArgs e)
	{
	  _color = colorButton.Color;
	  _preview.Invalidate();
	};
      table.AttachAligned(0, 1, "C_olor:", 0.0, 0.5, colorButton, 1, true);

      dialog.ShowAll();
      return DialogRun();
    }

    void SizeChanged(object sender, EventArgs e)
    {
      _size = (sender as SpinButton).ValueAsInt;
      _preview.Invalidate();
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      int x, y, width, height;
 	
      _preview.GetPosition(out x, out y);
      _preview.GetSize(out width, out height);
      Image clone = new Image(_image);
      clone.Crop(width, height, x, y);

      RenderMinisteck(clone, clone.ActiveDrawable, true);
      PixelRgn rgn = new PixelRgn(clone.ActiveDrawable, 0, 0, width, height, 
				  false, false);
      _preview.DrawRegion(rgn);
	
      clone.Delete();
    }

    void RenderMinisteck(Image image, Drawable drawable, bool preview)
    {
      image.UndoGroupStart();
      RunProcedure("plug_in_pixelize", image, drawable, _size);

      MinisteckPalette palette = new MinisteckPalette();
      image.ConvertIndexed(ConvertDitherType.NO, 
			   ConvertPaletteType.CUSTOM,
			   0, false, false, "Ministeck");
      palette.Delete();

      image.ConvertRgb();
      image.UndoGroupEnd();

      // And finally calculate the Ministeck pieces
	
      int width = drawable.Width / _size;
      int height = drawable.Height / _size;

      Painter painter = new Painter(drawable, _size, _color);
      Shape.Painter = painter;

      bool[,] A = new bool[width, height];
      Array.Clear(A, 0, width * height);

      // Fill in shapes
      
      ShapeSet shapes = new ShapeSet();

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
	progress = new Progress("Ministeck...");

      for (int y = 0; y < height; y++)
	{
	  for (int x = 0; x < width; x++)
	    {
	      if (!A[x, y])
		{
		  foreach (Shape shape in shapes)
		    {
		      if (shape.Fits(A, x, y))
			{
			  break;
			}
		    }
		}
	    }
	  if (!preview)
	    progress.Update((double) y / height);
	}
	
      painter.Destroy();

      drawable.Flush();
      drawable.Update(0, 0, drawable.Width, drawable.Height);

      if (!preview)
	Display.DisplaysFlush();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      RenderMinisteck(image, drawable, false);
    }
  }
}
