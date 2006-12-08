// The Difference Clouds plug-in
// Copyright (C) 2006 Massimo Perga (massimo.perga@gmail.com)
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

using Gtk;


namespace Gimp.DifferenceClouds
{
  public class DifferenceClouds : Plugin
  {
    Random _random = null;
    ScaleEntry _turbulenceEntry = null;
    Progress _progressBar = null;
    private uint   _count;

    [SaveAttribute("seed")]
    UInt32 _rseed;	      		// Current random seed
    [SaveAttribute("random_seed")]
    bool _random_seed;
    [SaveAttribute("turbulence")]
    private double _turbulence;

    private int _progress;
    private int _maxProgress;
    private int _ix1, _ix2, _iy1, _iy2;
    private int _alpha, _bpp;
    private bool _hasAlpha;
    private RGB _foregroundColor, _backgroundColor;
    private byte [,] _indexedColorsMap = new byte[256, 3];

    delegate void GenericEventHandler(object o, EventArgs e);

    static void Main(string[] args)
    {
      new DifferenceClouds(args);
    }

    public DifferenceClouds(string[] args) : base(args, "DifferenceClouds")
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("turbulence", 0, typeof(double), 
				 _("Turbulence of the cloud")));

      Procedure procedure = new Procedure("plug_in_difference_clouds",
					  _("Creates difference clouds."),
					  _("Creates difference clouds."),
					  "Massimo Perga",
					  "(C) Massimo Perga",
					  "2006",
					  _("Difference Clouds..."),
					  "RGB*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Render/Clouds";
      procedure.IconFile = "DifferenceClouds.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Difference Clouds", true);

      Dialog dialog = DialogNew(_("Difference Clouds 0.1"),
				_("Difference Clouds"), IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, _("Difference Clouds"));

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      // Create the table widget
      GimpTable table = new GimpTable(3, 4, false);
      table.ColumnSpacing = 10;
      table.RowSpacing = 10;
      table.BorderWidth = 10;

      CreateLabelInTable(table, 0, 0, _("Seed:"));
      RandomSeed seed = new RandomSeed(ref _rseed, ref _random_seed);
      table.Attach(seed, 1, 3, 0, 1);

      _turbulenceEntry = new ScaleEntry(table, 0, 1, _("_Turbulence"), 150, 3,
					_turbulence, 0.0, 7.0, 0.1, 1.0, 1, 
					true, 0, 0, null, null);
      _turbulenceEntry.ValueChanged += TurbulenceChangedEventHandler;

      vbox.PackStart(table, false, false, 0);

      dialog.ShowAll();
      return DialogRun();
    }

    void TurbulenceChangedEventHandler(object source, EventArgs e)
    {
      _turbulence = _turbulenceEntry.Value;
    }

    override protected void Reset()
    {
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Tile.CacheDefault(drawable);

      _foregroundColor = Context.Foreground;
      _backgroundColor = Context.Background;
      if (_progressBar == null)
        _progressBar = new Progress(_("Difference Clouds..."));
      if (_random == null)
        _random = new Random((int)_rseed);

      Layer active_layer = image.ActiveLayer;
      Layer newLayer = new Layer(active_layer);
      newLayer.Name = "_DifferenceClouds_";      
      newLayer.Visible = false;
      newLayer.Mode = active_layer.Mode;
      newLayer.Opacity = active_layer.Opacity;

      // Initialization steps
      _bpp = drawable.Bpp;
      PixelFetcher _pf = new PixelFetcher(drawable, true);
      _progress = 0;
      _hasAlpha = newLayer.HasAlpha;
      _alpha = (_hasAlpha) ? _bpp - 1 : _bpp;
      InitializeIndexedColorsMap();

      drawable.MaskBounds(out _ix1, out _iy1, out _ix2, out _iy2);
      _maxProgress = (_ix2 - _ix1) * (_iy2 - _iy1);

      if (_ix1 != _ix2 && _iy1 != _iy2)
	{
	  //
	  // This first time only puts in the seed pixels - one in each
	  // corner, and one in the center of each edge, plus one in the
	  // center of the image.
	  //
	  DoDifferenceClouds(_pf, _ix1, _iy1, _ix2 - 1, _iy2 - 1, -1, 0);
	  
	  //
	  // Now we recurse through the images, going further each time.
	  //
	  int depth = 1;
	  while (!DoDifferenceClouds (_pf, _ix1, _iy1, _ix2 - 1, _iy2 - 1, 
				      depth, 0))
	    {
	      depth++;
	    }
	}
      
      _pf.Dispose();

      drawable.Flush();
      drawable.MergeShadow(true);
      
      DoDifference(drawable, newLayer);
      
      drawable.Update(_ix1, _iy1, _ix2 - _ix1, _iy2 - _iy1);
      
      Display.DisplaysFlush();
    }

    Label CreateLabelInTable(Table table, uint row, uint col, string text) 
    {
      Label label = new Label(text);
      label.SetAlignment(0.0f, 0.5f);
      table.Attach(label, col, col + 1, row, row + 1, Gtk.AttachOptions.Fill, 
		   Gtk.AttachOptions.Fill, 0, 0);

      return label;
    } 

    // Fix me: use Read/Write iterators
    void DoDifference(Drawable sourceDrawable, Drawable toDiffDrawable)
    {
      int x1, y1, x2, y2;
      sourceDrawable.MaskBounds(out x1, out y1, out x2, out y2);
      PixelRgn srcPR = new PixelRgn(sourceDrawable, x1, y1, x2 - x1, y2 - y1, 
				    true, true);
      PixelRgn destPR = new PixelRgn(toDiffDrawable, x1, y1, x2 - x1, y2 - y1, 
				     false, false);

      for (IntPtr pr = PixelRgn.Register(srcPR, destPR); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	      for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		{
		  srcPR[y, x] = MakeAbsDiff(destPR[y, x].Bytes, 
					    srcPR[y, x].Bytes);
		}
	    }				
	}
      sourceDrawable.Flush();
      sourceDrawable.MergeShadow(false);
      sourceDrawable.Update(x1, y1, x2 - x1, y2 - y1);
    }

    // Fix me: put this functionality in Pixel class

    Pixel MakeAbsDiff(byte[] dest, byte[] src)
    {
      byte []retVal = new byte[src.Length];
      int tmpVal = 0;
      for (int i = 0; i < src.Length; i++)
	{
	  tmpVal += src[i];
	}
      tmpVal /= src.Length;
      for (int i = 0; i < src.Length; i++)
	{
	  retVal[i] = (byte)Math.Abs(dest[i] - _indexedColorsMap[tmpVal,i]);
	}
        
      if (_hasAlpha)
	{
	  retVal[_bpp - 1] = 255;
	}
      return new Pixel(retVal);
    }   

    bool DoDifferenceClouds(PixelFetcher pf, int x1, int y1, int x2, int y2, 
			    int depth, int scaleDepth)
    {
      int xm = (x1 + x2) / 2;
      int ym = (y1 + y2) / 2;

      // Initial step
      if (depth == -1)
	{
	  pf[y1, x1] = RandomRGB();
	  pf[y1, x2] = RandomRGB();
	  pf[y2, x1] = RandomRGB();
	  pf[y2, x2] = RandomRGB();
	  pf[ym, x1] = RandomRGB();
	  pf[ym, x2] = RandomRGB();
	  pf[y1, xm] = RandomRGB();
	  pf[y2, xm] = RandomRGB();

	  _progress += 8;

	  return false;
	}

      if (depth == 0)
	{
	  int ran;

	  if (x1 == x2 && y1 == y2)
	    {
	      return false;
	    }

	  Pixel tl = pf[y1, x1];
	  Pixel tr = pf[y1, x2];
	  Pixel bl = pf[y2, x1];
	  Pixel br = pf[y2, x2];

	  ran = (int)((256.0 / (2.0 * scaleDepth)) * _turbulence);

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

	  /* Fix me: check with Max if this code is needed!

	  if (ym != y1 || ym != y2)
	    {
	      if (x1 != xm || ym != y2)
		{
		  // Bottom
		  bm = (bl + br) / 2;
		  AddRandom(bm, ran);
		  pf[y1, xm] = bm;
		  _progress++;
		}

	      if (y1 != y2)
		{
		  // Top
		  tm = (tl + tr) / 2;
		  AddRandom(tm, ran);
		  pf[y1, xm] = tm;
		  _progress++;
		}
	    }
	  */

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

	  return ((x2 - x1) < 3) && ((y2 - y1) < 3);
	}

      if (x1 < x2 || y1 < y2)
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

    Pixel AddRandom (Pixel pixel, int amount)
    {
      amount /= 2;

      if (amount > 0)
	{
	  for (int i = 0; i < _alpha; i++)
	    {
	      pixel[i] += _random.Next(-amount, amount);
	    }
	  pixel.Clamp0255();
	}
      return pixel;
    }

    void InitializeIndexedColorsMap()
    {
      for (int i = 0; i < 256; i++)
        {
          double ratio = i / 256.0; 
          for (int j = 0; j < 3; j++)
	    {
	      _indexedColorsMap[i,j] = (byte)(_foregroundColor.Bytes[j] + 
			       (byte)((double)(_foregroundColor.Bytes[j] - 
				     _backgroundColor.Bytes[j]) * ratio));
	    }
        }
    }
  }
}

