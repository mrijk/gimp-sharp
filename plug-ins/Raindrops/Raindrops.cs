// The Raindrops plug-in
// Copyright (C) 2004-2007 Maurits Rijk, Massimo Perga
//
// Raindrops.cs
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

namespace Gimp.Raindrops
{
  class Raindrops : Plugin
  {
    DrawablePreview _preview;

    [SaveAttribute("drop_size")]
    int _dropSize = 80;
    [SaveAttribute("number")]
    int _number = 80;
    [SaveAttribute("fish_eye")]
    int _fishEye = 30;

    static void Main(string[] args)
    {
      new Raindrops(args);
    }

    Raindrops(string[] args) : base(args, "Raindrops")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList()
	{
	  new ParamDef("drop_size", 80, typeof(int), _("Size of raindrops")),
	  new ParamDef("number", 80, typeof(int), _("Number of raindrops")),
	  new ParamDef("fish_eye", 30, typeof(int), _("Fisheye effect"))
	};

      yield return new Procedure("plug_in_raindrops",
				 _("Generates raindrops"),
				 _("Generates raindrops"),
				 "Massimo Perga",
				 "(C) Massimo Perga",
				 "2006-2007",
				 _("Raindrops..."),
				 "RGB*, GRAY*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/" + _("Light and Shadow") + "/" + 
	    _("Glass"),
	  IconFile = "Raindrops.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Raindrops", true);

      GimpDialog dialog = DialogNew(_("Raindrops 0.1"), _("Raindrops"),
				    IntPtr.Zero, 0, Gimp.StandardHelpFunc,
				    _("Raindrops"));

      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      ScaleEntry _dropSizeEntry = new ScaleEntry(table, 0, 1,
						 _("_Drop size:"),
						 150, 3, _dropSize, 1.0,
						 256.0, 1.0, 8.0, 0);
      _dropSizeEntry.ValueChanged += delegate
	{
	  _dropSize = _dropSizeEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      ScaleEntry _numberEntry = new ScaleEntry(table, 0, 2,
					       _("_Number:"),
					       150, 3, _number, 1.0,
					       256.0, 1.0, 8.0, 0);
      _numberEntry.ValueChanged += delegate
	{
	  _number = _numberEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      ScaleEntry _fishEyeEntry = new ScaleEntry(table, 0, 3,
						_("_Fish eye:"),
						150, 3, _fishEye, 1.0,
						256.0, 1.0, 8.0, 0);
      _fishEyeEntry.ValueChanged += delegate
	{
	  _fishEye = _fishEyeEntry.ValueAsInt;
	  _preview.Invalidate();
	};

      return dialog;
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!
      Image clone = new Image(_image);
      clone.Crop(_preview.Bounds);

      Drawable drawable = clone.ActiveDrawable;
      RenderRaindrops(clone, drawable, true);
      _preview.Redraw(drawable);
      clone.Delete();
    }

    int Clamp(int x, int l, int u)
    {
      return (x < l) ? l : ((x > u) ? u : x);
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Image image, Drawable drawable)
    {
      RenderRaindrops(image, drawable, false);
    }

    void RenderRaindrops(Image image, Drawable drawable, bool isPreview)
    {
      Dimensions dimensions = image.Dimensions;
      Progress progress = (isPreview) ? null : new Progress(_("Raindrops..."));

      Rectangle bounds = drawable.MaskBounds;

      Tile.CacheDefault(drawable);
      PixelFetcher pf = new PixelFetcher(drawable, false);

      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.IterateSrcDest(src => src);

      double newCoeff = (double) Clamp(_fishEye, 1, 100) * 0.01;

      Random random = new Random();

      BoolMatrix boolMatrix = new BoolMatrix(dimensions.Width,
					     dimensions.Height);

      // TODO: find an upper bound so that
      // speed on big drop would be improved
      // int upper_bound = ;
      // upper bound for iteration in  blur search process

      for (int numBlurs = 0; numBlurs <= _number; numBlurs++)
	{
	  int newSize = random.Next(_dropSize);	// Size of current raindrop
	  int radius = newSize / 2;		// Half of current raindrop
	  double s = radius / Math.Log(newCoeff * radius + 1);

	  bool failed;
	  Coordinate<int> c = boolMatrix.Generate(radius, out failed);
	  if (failed)
	    {
	      break;
	    }

	  int x = c.X;
	  int y = c.Y;

	  for (int i = -radius; i < newSize - radius; i++)
	    {
	      for (int j = -radius; j < newSize - radius; j++)
		{
		  double r = Math.Sqrt(i * i + j * j);
		  double a = Math.Atan2(i, j);

		  if (r <= radius)
		    {
		      double oldRadius = r;
		      r = (Math.Exp (r / s) - 1) / newCoeff;

		      int k = x + (int) (r * Math.Sin(a));
		      int l = y + (int) (r * Math.Cos(a));

		      int m = x + i;
		      int n = y + j;

		      if (dimensions.IsInside(k, l) &&
			  dimensions.IsInside(m, n))
			{
			  boolMatrix[n, m] = true;

			  int bright = GetBright(radius, oldRadius, a);
			  Pixel newColor = pf[l, k] + bright;
			  newColor.Clamp0255();
			  pf[l, k] = newColor;
			}
		    }
		}
	    }

	  int blurRadius = newSize / 25 + 1;

	  for (int i = -radius - blurRadius;
	       i < newSize - radius + blurRadius; i++)
	    {
	      for (int j = -radius - blurRadius;
		   j < newSize - radius + blurRadius; j++)
		{
		  double r = Math.Sqrt(i * i + j * j);
		
		  if (r <= radius * 1.1)
		    {
		      Pixel average = drawable.CreatePixel();
		      int blurPixels = 0;
		      int m, n;

		      for (int k = -blurRadius; k < blurRadius + 1; k++)
			{
			  for (int l = -blurRadius; l < blurRadius + 1; l++)
			    {
			      m = x + i + k;
			      n = y + j + l;
			
			      if (dimensions.IsInside(m, n))
				{
				  average += pf[n, m];
				  blurPixels++;
				}
			    }
			}

		      m = x + i;
		      n = y + j;

		      if (dimensions.IsInside(m, n))
			{
			  pf[n, m] = average / blurPixels;
			}
		    }
		}
	    }

	  if (!isPreview)
	    progress.Update((double) numBlurs / _number);
	}

      pf.Dispose();

      drawable.Flush();
      // drawable.MergeShadow(true);
      drawable.Update(bounds);
    }

    int GetBright(double Radius, double OldRadius, double a)
    {
      int Bright = 0;

      if (OldRadius >= 0.9 * Radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    Bright = -80;
	  else if ((a <= -2.25) && (a > -2.5))
	    Bright = -40;
	  else if ((a <= 0.25) && (a > 0))
	    Bright = -40;
	}
      else if (OldRadius >= 0.8 * Radius)
	{
	  if ((a <= -0.75) && (a > -1.50))
	    Bright = -40;
	  else if ((a <= 0.10) && (a > -0.75))
	    Bright = -30;
	  else if ((a <= -1.50) && (a > -2.35))
	    Bright = -30;
	}
      else if (OldRadius >= 0.7 * Radius)
	{
	  if ((a <= -0.10) && (a > -2.0))
	    Bright = -20;
	  else if ((a <= 2.50) && (a > 1.90))
	    Bright = 60;
	}
      else if (OldRadius >= 0.6 * Radius)
	{
	  if ((a <= -0.50) && (a > -1.75))
	    Bright = -20;
	  else if ((a <= 0) && (a > -0.25))
	    Bright = 20;
	  else if ((a <= -2.0) && (a > -2.25))
	    Bright = 20;
	}
      else if (OldRadius >= 0.5 * Radius)
	{
	  if ((a <= -0.25) && (a > -0.50))
	    Bright = 30;
	  else if ((a <= -1.75 ) && (a > -2.0))
	    Bright = 30;
	}
      else if (OldRadius >= 0.4 * Radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    Bright = 40;
	}
      else if (OldRadius >= 0.3 * Radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    Bright = 30;
	}
      else if (OldRadius >= 0.2 * Radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    Bright = 20;
	}
      return Bright;
    }
  }
}

