// The Colorize plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Colorize.cs
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

namespace Gimp.Colorize
{
  public class Colorize : Plugin
  {
    [SaveAttribute("include_original")]
    bool _includeOriginal = false;
    [SaveAttribute("unselected_areas")]
    bool _unselectedAreas = true;
    [SaveAttribute("pure_white")]
    bool _pureWhite = false;
    [SaveAttribute("use_chroma")]
    bool _useChroma = false;
    [SaveAttribute("use_entire_image")]
    bool _useEntireImage = true;

    const int WindowRadius = 1;
    const int WindowWidth = 2 * WindowRadius + 1;
    const int WindowPixels = WindowWidth * WindowWidth;

    static void Main(string[] args)
    {
      new Colorize(args);
    }

    public Colorize(string[] args) : base(args, "Colorize")
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("points", 12, typeof(int),
				 _("Number of points")));

      Procedure procedure = new Procedure("plug_in_colorize",
          _("Fix me!"),
          _("Fix me!"),
          "Maurits Rijk",
          "(C) Maurits Rijk",
          "2004-2006",
          "Colorize...",
          "RGB*, GRAY*",
          in_params);
      procedure.MenuPath = "<Image>/Filters/Generic";
      procedure.IconFile = "Colorize.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Colorize", true);

      Dialog dialog = DialogNew("Colorize", "Colorize", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Colorize");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      GimpTable table = new GimpTable(6, 1, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, true, true, 0);

      DrawableComboBox combo = new DrawableComboBox(DialogMarkedConstrain, 
						    IntPtr.Zero);
      combo.Active = _drawable;
      table.Attach(combo, 0, 1, 0, 1);

      CheckButton includeOriginal = 
	new CheckButton(_("Marked images includes original image"));
      includeOriginal.Active = _includeOriginal;
      includeOriginal.Toggled += delegate(object sender, EventArgs args) {
	_includeOriginal = includeOriginal.Active;
      };
      table.Attach(includeOriginal, 0, 1, 1, 2);

      CheckButton unselectedAreas = 
	new CheckButton(_("Unselected areas are mask"));
      unselectedAreas.Active = _unselectedAreas;
      unselectedAreas.Toggled += delegate(object sender, EventArgs args) {
	_unselectedAreas = unselectedAreas.Active;
      };
      table.Attach(unselectedAreas, 0, 1, 2, 3);

      CheckButton pureWhite = 
	new CheckButton(_("Pure white is mask"));
      pureWhite.Active = _pureWhite;
      pureWhite.Toggled += delegate(object sender, EventArgs args) {
	_pureWhite = pureWhite.Active;
      };
      table.Attach(pureWhite, 0, 1, 3, 4);

      CheckButton useChroma = 
	new CheckButton(_("Use chroma in addition to luminance (for color images)"));
      useChroma.Active = _useChroma;
      useChroma.Toggled += delegate(object sender, EventArgs args) {
	_useChroma = useChroma.Active;
      };
      table.Attach(useChroma, 0, 1, 4, 5);

      CheckButton useEntireImage = 
	new CheckButton(_("Unselected areas are mask"));
      useEntireImage.Active = _useEntireImage;
      useEntireImage.Toggled += delegate(object sender, EventArgs args) {
	_useEntireImage = useEntireImage.Active;
      };
      table.Attach(useEntireImage, 0, 1, 5, 6);

      dialog.ShowAll();
      return DialogRun();
    }

    bool DialogMarkedConstrain(Int32 imageId, Int32 drawableId, IntPtr data)
    {
      Drawable drawable = new Drawable(drawableId);
      return drawable.IsRGB && drawable.HasAlpha;
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Progress progress = new Progress(_("Colorizing..."));

      int i, j, ii, jj;	// Fix me: replace with x1, y1, x2, y2
      bool hasSel = drawable.MaskIntersect(out j, out i, out jj, out ii);
      if (!hasSel || _useEntireImage) 
	{
	  j = i = 0;
	  jj = image.Width;
	  ii = image.Height;
	}

      Drawable sel = null;
      PixelRgn selRgn = null;

      if (hasSel) 
	{
	  sel = image.Selection;
	  selRgn = new PixelRgn(sel, j, i, jj, ii, false, false);
	}

      Drawable marked = null;	// Fix me!

      PixelRgn srcRgn = new PixelRgn(drawable, j, i, jj, ii, false, false);
      PixelRgn dstRgn = new PixelRgn(drawable, j, i, jj, ii, true, true);
      PixelRgn markRgn = new PixelRgn(marked, j, i, jj, ii, false, false);

      int h = srcRgn.H;
      int w = srcRgn.W;

      double[,] A = new double[WindowPixels, h * w];
      int[] AI = new int[WindowPixels * h * w];
      int[] AJ = new int[WindowPixels * h * w];

      double[,] Y = new double[h, w];
      double[,] I = new double[h, w];
      double[,] Q = new double[h, w];

      if (_useChroma) 
	{
	  double[,] inI = new double[h, w];
	  double[,] inQ = new double[h, w];
	}

      byte[,] mask = new byte[h, w];

      if (sel != null) 
	{
	  // Retarded check for selections, because gimp doesn't
	  // _REALLY_ return FALSE when there's no selection.
	  if (j == 0 && i == 0 && jj == image.Width && ii == image.Height) 
	    {
	      for (i = 0; i < h; i++) 
		{
		  byte[] selRow = selRgn.GetRow(selRgn.X, selRgn.Y + i, w);
		  for (j = 0; j < w; j++) 
		    {
		      int selIdx = j * sel.Bpp;
		      if (selRow[selIdx] != 0) goto good_selection;
		    }
		}
	      
	      // Nothing set in the entire selection.
	      sel.Detach();
	      sel = null;
	      
	      good_selection:
	      ;
	    }
	}

      for (i = 0; i < h; i++) 
	{
	  byte[] imgRow = srcRgn.GetRow(srcRgn.X, srcRgn.Y + i, w);
	  byte[] markRow = markRgn.GetRow(markRgn.X, markRgn.Y + i, w);

	  if (sel != null) 
	  {
	    byte[] selRow = selRgn.GetRow(selRgn.X, selRgn.Y + i, w);
	  }

	  for (j = 0; j < w; j++) 
	    {
	      int imgIdx = j * drawable.Bpp;
	      int markIdx = j * marked.Bpp;
	      int selIdx = j * sel.Bpp;
	      
	      double iY, iI, iQ;
	      double mY;
	      
	      int delta = 0;
	    }
      }
      Display.DisplaysFlush();
    }
  }
}
