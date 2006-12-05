// The Colorize plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Ported from http://registry.gimp.org/plugin?id=5479
// copyright 2005 Christopher Lais
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

    Drawable _marked;

    DrawableComboBox _combo;

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

    void DialogMarkedCallback()
    {
      int active = (_combo as IntComboBox).Active;
      Console.WriteLine("ConnectMe: " + active);
      if (_marked != null)
	{
	  _marked.Detach();
	}
      _marked = new Drawable(active);
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

      _combo = new DrawableComboBox(DialogMarkedConstrain, IntPtr.Zero);
      _combo.Connect(-1, DialogMarkedCallback, IntPtr.Zero);
      _combo.Active = _drawable;
      // _marked = combo.Active;
      table.Attach(_combo, 0, 1, 0, 1);

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

    void rgb2yiq(Pixel pixel, out double y, out double i, out double q)
    {
      int r = pixel.Red;
      int g = pixel.Green;
      int b = pixel.Blue;
#if YUV
      y = (0.299 * r + 0.587 * g + 0.114 * b) / (double)255;
      i = (0.147 * r - 0.289 * g + 0.436 * b) / (double)255;
      q = (0.615 * r - 0.515 * g - 0.100 * b) / (double)255;
#else
      y = (0.299 * r + 0.587 * g + 0.114 * b) / (double)255;
      i = (0.596 * r - 0.274 * g - 0.322 * b) / (double)255;
      q = (0.212 * r - 0.523 * g + 0.311 * b) / (double)255;
#endif
    }

    void yiq2rgb(double y, double i, double q, Pixel pixel)
    {
	double dr, dg, db;
#if YUV
	dr = (y + 1.140 * q);
	dg = (y - 0.395 * i - 0.581 * q);
	db = (y + 2.032 * i);
#else
	dr = (y + 0.956 * i + 0.621 * q);
	dg = (y - 0.272 * i - 0.647 * q);
	db = (y - 1.105 * i + 1.702 * q);
#endif
	pixel.Red = (int) (255 * dr);
	pixel.Green = (int) (255 * dg);
	pixel.Blue = (int) (255 * db);
	pixel.Clamp0255();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Progress progress = new Progress(_("Colorizing..."));

      umfpack_wrapper_init();

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

      PixelRgn srcRgn = new PixelRgn(drawable, j, i, jj, ii, false, false);
      PixelRgn dstRgn = new PixelRgn(drawable, j, i, jj, ii, true, true);
      PixelRgn markRgn = new PixelRgn(_marked, j, i, jj, ii, false, false);

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

      Tile.CacheNtiles((ulong) (2 * (drawable.Width / Gimp.TileWidth + 1)));

      if (sel != null) 
	{
	  // Retarded check for selections, because gimp doesn't
	  // _REALLY_ return FALSE when there's no selection.
	  if (j == 0 && i == 0 && jj == drawable.Width && 
	      ii == drawable.Height) 
	    {
	      bool goodSelection = false;
	      foreach (Pixel pixel in 
		       new ReadPixelIterator(sel, RunMode.Noninteractive))
		{
		  if (pixel[0] != 0)
		    {
		      goodSelection = true;
		      break;
		    }
		}
	      if (!goodSelection)
		{
		  sel.Detach();
		  sel = null;
		}
	    }
	}

      Pixel[] selRow = null;
      Pixel whitePixel = new Pixel(255, 255, 255);

      for (i = 0; i < h; i++) 
	{
	  Pixel[] imgRow = srcRgn.GetRow(srcRgn.X, srcRgn.Y + i, w);
	  Pixel[] markRow = markRgn.GetRow(markRgn.X, markRgn.Y + i, w);

	  if (sel != null) 
	  {
	    selRow = selRgn.GetRow(selRgn.X, selRgn.Y + i, w);
	  }

	  for (j = 0; j < w; j++) 
	    {
	      Pixel imgPixel = imgRow[j];
	      Pixel markPixel = markRow[j];
	      int selIdx = (sel != null) ? j : 0;
	      
	      double iY, iI, iQ;
	      double mY;

	      rgb2yiq(imgPixel, out iY, out iI, out iQ);

	      if (_useChroma) 
		{
		  inI[i, j] = iI;
		  inQ[i, j] = iQ;
		}

	      if (_includeOriginal) 
		{
		  Pixel diff = imgPixel - markPixel;;
		  int delta = Math.Abs(diff.Red) + Math.Abs(diff.Green) +
		    Math.Abs(diff.Blue);
		}

	      // big dirty if statement
	      if (_pureWhite && markPixel.IsSameColor(whitePixel))
		{
		  mask[i, j] = true;
		} 
	      else if ((_includeOriginal && !imgPixel.IsSameColor(markPixel))
		       || (!_includeOriginal && markPixel.Alpha >= threshGuc)) 
		{
		  mask[i, j] = true;
		  rgb2yiq(markPixel, out mY, out iI, out iQ);
		} 
	      else if (sel != null && selRow[selIdx].Red < threshGuc) 
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
		  int min_ii = Math.Max(0, i - WindowRadius);
		  int max_ii = Math.Min(h - 1, i + WindowRadius);
		  int min_jj = Math.Max(0, j - WindowRadius);
		  int max_jj = Math.Min(w - 1, j + WindowRadius);
		  int[] vary = new int[WindowPixels];
		  int[] varx = new int[WindowPixels];
		  double[] var = new double[WindowPixels];
		  
		  int count = 0;
		  double sum_sq = 0;
		  double sum = 0;
		  double min_variance = 1.0;

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
		  
		  double sigma = 
		    (sum_sq - (sum * sum)/(double)(count + 1)) / (double)count;
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
		      A[n / (h * w) , n % (h * w)] = -var[ii] / sum;
		      ++n;
		    }
		}
	      
	      AI[n] = AJ[n] = i * w + j;
	      // Fix me: just A[i, j]?
	      A[n / (h * w), n % (h * w)] = 1.0;
	      ++n;
	    }
	}

      const int UMFPACK_CONTROL = 20;
      const int UMFPACK_INFO = 90;
      double[] control = new double[UMFPACK_CONTROL];
      double[] info = new double[UMFPACK_INFO];

      umfpack_di_defaults(control);

      double[,] Ax = new double[WindowPixels, h * w];
      int[] Ap = new int[h * w + 1];
      int[] Ai = new int[WindowPixels * h * w];
      int[] Map = new int[WindowPixels * h * w];

      int status = umfpack_di_triplet_to_col(h * w, h * w, n, AI, AJ, A, Ap, 
					     Ai, Ax, Map);

      IntPtr symbolic;
      status = umfpack_di_symbolic(h * w, h * w, Ap, Ai, Ax, out symbolic, 
				   control, info);

      IntPtr numeric;
      status = umfpack_di_numeric(Ap, Ai, Ax, symbolic, out numeric, control, 
				  info);

      umfpack_di_free_symbolic(ref symbolic);

      progress.Update(0.3);

      double[,] outI = new double[h, w];
      double[,] outQ = new double[h, w];

      const int UMFPACK_A = 0;
      status = umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, outI, I, numeric, 
				control, info);

      progress.Update(0.6);

      status = umfpack_di_solve(UMFPACK_A, Ap, Ai, Ax, outQ, Q, numeric, 
				control, info);

      umfpack_di_free_numeric(ref numeric);

      progress.Update(0.9);

      for (i = 0; i < h; i++) 
	{
	  // FIXME: This is only for the alpha channel..
	  Pixel[] imgRow = srcRgn.GetRow(srcRgn.X, srcRgn.Y + i, w);

	  for (j = 0; j < w; j++) 
	    {
	      yiq2rgb(Y[i, j], outI[i, j], outQ[i, j], imgRow[j]);
	    }
	  
	  dstRgn.SetRow(imgRow, dstRgn.X, dstRgn.Y + i);
      }

      drawable.Flush();
      drawable.MergeShadow(true);
      drawable.Update(dstRgn.X, dstRgn.Y, dstRgn.W, dstRgn.H);

      progress.Update(1.0);

      Display.DisplaysFlush();
    }

    // TODO: fix mappings from .so to .dll
    [DllImport("umfpackwrapper.so")]
    static extern void umfpack_wrapper_init();

    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_defaults(double[] control);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_triplet_to_col(int n_row,
						int n_col,
						int nz,
						int[] Ti,
						int[] Tj,
						double[,] Tx,
						int[] Ap,
						int[] Ai,
						double[,] Ax,
						int[] Map);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_symbolic(int n_row,
					  int n_col,
					  int[] Ap,
					  int[] Ai,
					  double[,] Ax,
					  out IntPtr Symbolic,
					  double[] Control,
					  double[] Info);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_numeric(int[] Ap,
					 int[] Ai,
					 double[,] Ax,
					 IntPtr Symbolic,
					 out IntPtr Numeric,
					 double[] Control,
					 double[] Info);
    [DllImport("libumfpack.dll")]
    static extern int umfpack_di_solve(int sys,
				       int[] Ap,
				       int[] Ai,
				       double[,] Ax,
				       double[,] X,
				       double[,] B,
				       IntPtr Numeric,
				       double[] Control,
				       double[] Info);
    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_free_symbolic(ref IntPtr Symbolic);
    [DllImport("libumfpack.dll")]
    static extern void umfpack_di_free_numeric(ref IntPtr Numeric);
  }
}
