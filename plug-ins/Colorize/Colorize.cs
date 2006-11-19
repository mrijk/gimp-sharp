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
using System.Runtime.InteropServices;
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

    const double _thresh = 0.5;
    const double  LN_100 = 4.60517018598809136804;

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

    void rgb2yiq(byte r, byte g, byte b,
		 out double y, out double i, out double q)
    {
#if YUV
      y = (0.299*r + 0.587*g + 0.114*b) / (double)255;
      i = (0.147*r - 0.289*g + 0.436*b) / (double)255;
      q = (0.615*r - 0.515*g - 0.100*b) / (double)255;
#else
      y = (0.299*r + 0.587*g + 0.114*b) / (double)255;
      i = (0.596*r - 0.274*g - 0.322*b) / (double)255;
      q = (0.212*r - 0.523*g + 0.311*b) / (double)255;
#endif
    }

    void yiq2rgb(double y, double i, double q,
		 out byte r, out byte g, out byte b) 
    {
	double dr, dg, db;
#if YUV
	dr = (y + 1.140*q);
	dg = (y - 0.395*i - 0.581*q);
	db = (y + 2.032*i);
#else
	dr = (y + 0.956*i + 0.621*q);
	dg = (y - 0.272*i - 0.647*q);
	db = (y - 1.105*i + 1.702*q);
#endif
	dr = Math.Min(1.0, Math.Max(0.0, dr));
	dg = Math.Min(1.0, Math.Max(0.0, dg));
	db = Math.Min(1.0, Math.Max(0.0, db));
	r = (byte) (255 * dr);
	g = (byte) (255 * dg);
	b = (byte) (255 * db);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Progress progress = new Progress(_("Colorizing..."));

      umfpack_wrapper_init();

      Console.WriteLine("1");

      int i, j, ii, jj;	// Fix me: replace with x1, y1, x2, y2
      bool hasSel = drawable.MaskIntersect(out j, out i, out jj, out ii);
      if (!hasSel || _useEntireImage) 
	{
	  j = i = 0;
	  jj = image.Width;
	  ii = image.Height;
	}

      Drawable sel = null;
      int threshGuc = (int) (_thresh * 255);
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

      double[,] inI = null;
      double[,] inQ = null;
      if (_useChroma) 
	{
	  inI = new double[h, w];
	  inQ = new double[h, w];
	}

      bool[,] mask = new bool[h, w];

      byte[] selRow = null;
      if (sel != null) 
	{
	  // Retarded check for selections, because gimp doesn't
	  // _REALLY_ return FALSE when there's no selection.
	  if (j == 0 && i == 0 && jj == image.Width && ii == image.Height) 
	    {
	      for (i = 0; i < h; i++) 
		{
		  selRow = selRgn.GetRow(selRgn.X, selRgn.Y + i, w);
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
	    selRow = selRgn.GetRow(selRgn.X, selRgn.Y + i, w);
	  }

	  for (j = 0; j < w; j++) 
	    {
	      int imgIdx = j * drawable.Bpp;
	      int markIdx = j * marked.Bpp;
	      int selIdx = j * sel.Bpp;
	      
	      double iY, iI, iQ;
	      double mY;
	      
	      int delta = 0;

	      rgb2yiq(imgRow[imgIdx + 0], 
		      imgRow[imgIdx + 1], 
		      imgRow[imgIdx + 2],
		      out iY, out iI, out iQ);

	      if (_useChroma) 
		{
		  inI[i, j] = iI;
		  inQ[i, j] = iQ;
		}

	      if (_includeOriginal) 
		{
		  int v;
		  v = imgRow[imgIdx + 0] - markRow[markIdx + 0];
		  delta += Math.Abs(v);
		  v = imgRow[imgIdx + 1] - markRow[markIdx + 1];
		  delta += Math.Abs(v);
		  v = imgRow[imgIdx + 2] - markRow[markIdx + 2];
		  delta += Math.Abs(v);
		}

	      // big dirty if statement
	      if (_pureWhite
		  && markRow[markIdx + 0] >= 255
		  && markRow[markIdx + 1] >= 255
		  && markRow[markIdx + 2] >= 255) 
		{
		  mask[i, j] = true;
		} 
	      else if ((_includeOriginal &&
			(imgRow[imgIdx + 0] != markRow[markIdx + 0] ||
			 imgRow[imgIdx + 1] != markRow[markIdx + 1] ||
			 imgRow[imgIdx + 2] != markRow[markIdx + 2]))
		       || (!_includeOriginal
			   && markRow[markIdx + 3] >= threshGuc)) 
		{
		  mask[i, j] = true;
		  rgb2yiq(markRow[markIdx + 0],
			  markRow[markIdx + 1],
			  markRow[markIdx + 2],
			  out mY, out iI, out iQ);
		} 
	      else if (sel != null && selRow[selIdx] < threshGuc) 
		{
		  mask[i, j] = true;
		} 
	      else {
		  mask[i, j] = false;
		  iI = iQ = 0;
	      }
	      
	      Y[i, j] = iY;
	      I[i, j] = iI;
	      Q[i, j] = iQ;
	    }
	}

      if (sel != null) 
	{
	  sel.Detach();
	}

      progress.Update(0.1);

      int n = 0;
      for (i = 0; i < h; i++) 
	{
	  for (j = 0; j < w; j++) 
	    {
	      if (!mask[i, j]) 
		{
		  double sum_sq, sum;
		  double min_variance;
		  double sigma;
		  
		  int min_ii = Math.Max(0, i - WindowRadius);
		  int max_ii = Math.Min(h - 1, i + WindowRadius);
		  int min_jj = Math.Max(0, j - WindowRadius);
		  int max_jj = Math.Min(w - 1, j + WindowRadius);
		  int[] vary = new int[WindowPixels];
		  int[] varx = new int[WindowPixels];
		  double[] var = new double[WindowPixels];
		  int count;
		  
		  count = 0;
		  sum_sq = sum = 0;
		  min_variance = 1.0;
		  for (ii = min_ii; ii <= max_ii; ii++) 
		    {
		      for (jj = min_jj; jj <= max_jj; jj++) 
			{
			  double val = Y[ii, jj];
			  sum += val;
			  sum_sq += val * val;
			  
			  if (ii == i && jj == j) 
			    continue;
			  
			  vary[count] = i * w + j;
			  varx[count] = ii * w + jj;
			  var[count] = val - Y[i, j];
			  var[count] *= var[count];
			  if (_useChroma) 
			    {
			      val = inI[ii, jj] - inI[i, j];
			      var[count] += val * val;
			      val = inQ[ii, jj] - inQ[i, j];
			      var[count] += val * val;
			    }
			  if (var[count] < min_variance) 
			    min_variance = var[count];
			  ++count;
			}
		    }
		  
		  sigma = (sum_sq - (sum*sum)/(double)(count+1))/(double)count;
		  if (sigma < 0.000002) 
		    sigma = 0.000002;
		  else if (sigma < (min_variance / LN_100))
		    sigma = min_variance / LN_100;
		  
		  sum = 0;
		  for (ii = 0; ii < count; ii++) 
		    {
		      var[ii] = Math.Exp(-var[ii] / sigma);
		      sum += var[ii];
		    }
		  for (ii = 0; ii < count; ii++) 
		    {
		      AI[n] = vary[ii];
		      AJ[n] = varx[ii];
		      // Fix me: just A[i, j]?
		      A[n / w, n % w] = -var[ii] / sum;
		      ++n;
		    }
		}
	      
	      AI[n] = AJ[n] = i * w + j;
	      // Fix me: just A[i, j]?
	      A[n / w, n % w] = 1.0;
	      ++n;
	    }
	}

      const int UMFPACK_CONTROL = 20;
      double[] control = new double[UMFPACK_CONTROL];
      umfpack_di_defaults(ref control);

      double[,] Ax = new double[WindowPixels, h * w];
      int[] Ap = new int[h * w];
      int[] Ai = new int[h * w];
      int[] Map = new int[WindowPixels * h * w];

      // umfpack_di_triplet_to_col(h * w, h * w, n, AI, AJ, A, Ap, Ai, Ax, 
      // Map);

      // umfpack_di_symbolic(h * w, h * w, Ap, Ai, Ax, &symbolic, control, 
      // info);
      // umfpack_di_numeric(Ap, Ai, Ax, symbolic, &numeric, control, info);

      // umfpack_di_free_symbolic(&symbolic);

      progress.Update(0.3);

      double[,] outI = new double[h, w];
      double[,] outQ = new double[h, w];

      // umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, outI, I, numeric, control, info);

      progress.Update(0.6);

      // umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, outQ, Q, numeric, control, info);

      // umfpack_di_free_numeric(&numeric);

      progress.Update(0.9);

      for (i = 0; i < h; i++) 
	{
	  // FIXME: This is only for the alpha channel..
	  byte[] imgRow = srcRgn.GetRow(srcRgn.X, srcRgn.Y + i, w);
	
	  for (j = 0; j < w; j++) 
	    {
	      int imgIdx = j * drawable.Bpp;
	      yiq2rgb(Y[i, j],
		      outI[i, j],
		      outQ[i, j],
		      out imgRow[imgIdx + 0],
		      out imgRow[imgIdx + 1],
		      out imgRow[imgIdx + 2]);
	    }
	  
	  dstRgn.SetRow(imgRow, dstRgn.X, dstRgn.Y + i, w);
      }

      drawable.Flush();
      drawable.MergeShadow(true);
      drawable.Update(dstRgn.X, dstRgn.Y, dstRgn.W, dstRgn.H);

      progress.Update(1.0);
    }

    // TODO: fix mappings from .so to .dll
    [DllImport("umfpackwrapper.so")]
    static extern void umfpack_wrapper_init();

    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_defaults(ref double[] control);
  }
}
