// The Difference Clouds plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
//
// DifferenceClouds.cs
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

namespace Gimp.DifferenceClouds
{
  class DifferenceClouds : Plugin
  {
    Random _random = null;
    Progress _progressBar = null;
    uint _count;

    [SaveAttribute("seed")]
    UInt32 _rseed;
    [SaveAttribute("random_seed")]
    bool _random_seed;
    [SaveAttribute("turbulence")]
    double _turbulence;

    int _progress;
    int _maxProgress;
    int _alpha, _bpp;
    bool _hasAlpha;
    byte [,] _indexedColorsMap = new byte[256, 3];

    static void Main(string[] args)
    {
      new DifferenceClouds(args);
    }

    DifferenceClouds(string[] args) : base(args, "DifferenceClouds")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList()
	{
	  new ParamDef("turbulence", 0, typeof(double),
		       _("Turbulence of the cloud"))
	};
      yield return new Procedure("plug_in_difference_clouds",
				 _("Creates difference clouds."),
				 _("Creates difference clouds."),
				 "Massimo Perga",
				 "(C) Massimo Perga",
				 "2006-2007",
				 _("Difference Clouds..."),
				 "RGB*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/Render/Clouds",
	    IconFile = "DifferenceClouds.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Difference Clouds", true);

      GimpDialog dialog = DialogNew(_("Difference Clouds 0.2"),
				    _("Difference Clouds"), IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, 
				    _("Difference Clouds"));

      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      GimpTable table = new GimpTable(3, 4, false)
	{ColumnSpacing = 6, RowSpacing = 6};

      RandomSeed seed = new RandomSeed(ref _rseed, ref _random_seed);
      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);

      ScaleEntry _turbulenceEntry = new ScaleEntry(table, 0, 1, 
						   _("_Turbulence"), 150, 3,
						   _turbulence, 0.0, 7.0, 0.1, 
						   1.0, 1);
      _turbulenceEntry.ValueChanged += delegate
	{
	  _turbulence = _turbulenceEntry.Value;
	};

      vbox.PackStart(table, false, false, 0);

      return dialog;
    }

    override protected void Reset()
    {
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Tile.CacheDefault(drawable);

      _progressBar = new Progress(_("Difference Clouds..."));
      _random = new Random((int)_rseed);

      Layer activeLayer = image.ActiveLayer;
      Layer newLayer = new Layer(activeLayer)
	{
	  Name = "_DifferenceClouds_", Visible = false,
	  Mode = activeLayer.Mode, Opacity = activeLayer.Opacity
	};

      // Initialization steps
      _bpp = drawable.Bpp;
      PixelFetcher pf = new PixelFetcher(drawable, true);
      _progress = 0;
      _hasAlpha = newLayer.HasAlpha;
      _alpha = (_hasAlpha) ? _bpp - 1 : _bpp;
      InitializeIndexedColorsMap();

      Rectangle rectangle = drawable.MaskBounds;
      _maxProgress = rectangle.Area;

      if (rectangle.Width > 0 && rectangle.Height > 0)
	{
	  //
	  // This first time only puts in the seed pixels - one in each
	  // corner, and one in the center of each edge, plus one in the
	  // center of the image.
	  //
	  InitSeedPixels(pf, rectangle);

	  //
	  // Now we recurse through the images, going further each time.
	  //
	  int depth = 1;
	  while (!DoDifferenceClouds(pf, rectangle.X1, rectangle.Y1, 
				     rectangle.X2 - 1, rectangle.Y2 - 1, 
				     depth, 0))
	    {
	      depth++;
	    }
	}
      
      pf.Dispose();

      drawable.Flush();
      drawable.MergeShadow(true);
      
      DoDifference(drawable, newLayer);
      
      drawable.Update(rectangle);
    }

    void DoDifference(Drawable sourceDrawable, Drawable toDiffDrawable)
    {
      Rectangle rectangle = sourceDrawable.MaskBounds;
      PixelRgn srcPR = new PixelRgn(sourceDrawable, rectangle, true, true);
      PixelRgn destPR = new PixelRgn(toDiffDrawable, rectangle, false, false);

      RegionIterator iterator = new RegionIterator(srcPR, destPR);
      iterator.ForEach((src, dest) => src.Set(MakeAbsDiff(dest, src)));

      sourceDrawable.Flush();
      sourceDrawable.MergeShadow(false);
      sourceDrawable.Update(rectangle);
    }

    Pixel MakeAbsDiff(Pixel dest, Pixel src)
    {
      Pixel pixel = new Pixel(_bpp);

      int tmpVal = 0;
      for (int i = 0; i < _bpp; i++)
	{
	  tmpVal += src[i];
	}
      tmpVal /= _bpp;

      for (int i = 0; i < _bpp; i++)
	{
	  pixel[i] = (byte)Math.Abs(dest[i] - _indexedColorsMap[tmpVal, i]);
	}
        
      if (_hasAlpha)
	{
	  pixel[_bpp - 1] = 255;
	}
      return pixel;
    }   

    void InitSeedPixels(PixelFetcher pf, Rectangle rectangle)
    {
      int x1 = rectangle.X1;
      int y1 = rectangle.Y1;
      int x2 = rectangle.X2 - 1;
      int y2 = rectangle.Y2 - 1;

      int xm = (x1 + x2) / 2;
      int ym = (y1 + y2) / 2;

      pf[y1, x1] = RandomRGB();
      pf[y1, x2] = RandomRGB();
      pf[y2, x1] = RandomRGB();
      pf[y2, x2] = RandomRGB();
      pf[ym, x1] = RandomRGB();
      pf[ym, x2] = RandomRGB();
      pf[y1, xm] = RandomRGB();
      pf[y2, xm] = RandomRGB();

      _progress += 8;
    }

    bool DoDifferenceClouds(PixelFetcher pf, int x1, int y1, int x2, int y2, 
			    int depth, int scaleDepth)
    {
      int xm = (x1 + x2) / 2;
      int ym = (y1 + y2) / 2;

      if (depth == 0)
	{
	  if (x1 == x2 && y1 == y2)
	    {
	      return false;
	    }

	  Pixel tl = pf[y1, x1];
	  Pixel tr = pf[y1, x2];
	  Pixel bl = pf[y2, x1];
	  Pixel br = pf[y2, x2];

	  int ran = (int)((256.0 / (2.0 * scaleDepth)) * _turbulence);

	  if (xm != x1 || xm != x2)
	    {
	      // Left
	      pf[ym, x1] = AddRandom((tl + bl) / 2, ran);
	      _progress++;

	      if (x1 != x2)
		{
		  // Right
		  pf[ym, x2] = AddRandom((tr + br) / 2, ran);
		  _progress++;
		}
	    }

	  if (ym != y1 || ym != y2)
	    {
	      if (x1 != xm || ym != y2)
		{
		  // Bottom
		  pf[y2, xm] = AddRandom((bl + br) / 2, ran);
		  _progress++;
		}

	      if (y1 != y2)
		{
		  // Top
		  pf[y1, xm] = AddRandom((tl + tr) / 2, ran);
		  _progress++;
		}
	    }

	  if (y1 != y2 || x1 != x2)
	    {
	      // Middle pixel
	      pf[ym, xm] = AddRandom((tl + tr + bl + br) / 4, ran);
	      _progress++;
	    }

	  _count++;

	  if (_count % 2000 == 0)
	    {
	      _progressBar.Update((double)_progress / (double) _maxProgress);
	    }

	  return (x2 - x1 < 3) && (y2 - y1 < 3);
	}
      else if (x1 < x2 || y1 < y2)
	{
	  depth--;
	  scaleDepth++;

	  // Top left
	  DoDifferenceClouds(pf, x1, y1, xm, ym, depth, scaleDepth);
	  // Bottom left
	  DoDifferenceClouds(pf, x1, ym, xm ,y2, depth, scaleDepth);
	  // Top right
	  DoDifferenceClouds(pf, xm, y1, x2 , ym, depth, scaleDepth);
	  // Bottom right
	  return DoDifferenceClouds(pf, xm, ym, x2, y2, depth, scaleDepth);
	}
      else
	{
	  return true;
	}
    }

    Pixel RandomRGB()
    {
      Pixel pixel = new Pixel(_bpp);

      for (int i = 0; i < _bpp; i++)
	{
	  pixel[i] = _indexedColorsMap[_random.Next(256), i];
	} 
      
      if (_hasAlpha)
	{
	  pixel[_alpha] = 255;
	}
      return pixel;
    }

    Pixel AddRandom(Pixel pixel, int amount)
    {
      pixel.AddNoise(amount / 2);
      return pixel;
    }

    void InitializeIndexedColorsMap()
    {

      byte[] fgBytes = Context.Foreground.Bytes;
      byte[] bgBytes = Context.Background.Bytes;

      for (int i = 0; i < 256; i++)
        {
          double ratio = i / 256.0; 
          for (int j = 0; j < 3; j++)
	    {
	      _indexedColorsMap[i, j] = (byte)
		(fgBytes[j] + (byte)((fgBytes[j] - bgBytes[j]) * ratio));
	    }
        }
    }
  }
}

