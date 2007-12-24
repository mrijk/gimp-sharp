// The ncp plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// ncp.cs
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

namespace Gimp.ncp
{
  class ncp : PluginWithPreview
  {
    ScaleEntry _closestEntry;

    [SaveAttribute("seed")]
    UInt32 _seed;
    [SaveAttribute("random_seed")]
    bool _random_seed;
    [SaveAttribute("points")]
    int _points = 12;
    [SaveAttribute("closest")]
    int _closest = 1;
    [SaveAttribute("color")]
    bool _color = true;

    static void Main(string[] args)
    {
      new ncp(args);
    }

    ncp(string[] args) : base(args, "ncp")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList() {
	new ParamDef("points", 12, typeof(int), _("Number of points")),
	new ParamDef("closest", 1, typeof(int), _("Closest point")),
	new ParamDef("color", 1, typeof(bool), _("Color (true), B&W (false)"))
      };

      yield return new Procedure("plug_in_ncp",
				 _("Generates 2D textures"),
				 _("Generates 2D textures"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2007",
				 "NCP...",
				 "RGB*, GRAY*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "ncp.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("ncp", true);

      GimpDialog dialog = DialogNew("ncp", "ncp", IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, "ncp");

      GimpTable table = new GimpTable(4, 3, false)
	{
	  ColumnSpacing = 6, 
	  RowSpacing = 6
	};
      Vbox.PackStart(table, false, false, 0);

      RandomSeed seed = new RandomSeed(ref _seed, ref _random_seed);
      seed.Toggle.Toggled += delegate
	{
	  InvalidatePreview();
	};
      seed.SpinButton.ValueChanged += delegate
	{
	  InvalidatePreview();
	};

      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);

      ScaleEntry entry = new ScaleEntry(table, 0, 1, _("Po_ints:"), 150, 3, 
					_points, 1.0, 256.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      entry.ValueChanged += delegate
	{
	  _points = entry.ValueAsInt;
	  if (_points > _closestEntry.Upper)
	  {
	    _closestEntry.Upper = _points;
	  }
	  
	  if (_points < _closest)
	  {
	    _closest = _points;
	    _closestEntry.Upper = _closest;
	    _closestEntry.Value = _closest;
	  }
	  else
	  {
	    InvalidatePreview();
	  }
	};

      _closestEntry = new ScaleEntry(table, 0, 2, _("C_lose to:"), 150, 3, 
				     _closest, 1.0, _points, 1.0, 8.0, 0,
				     true, 0, 0, null, null);
      _closestEntry.ValueChanged += delegate
	{
	  _closest = _closestEntry.ValueAsInt;
	  InvalidatePreview();
	};

      CheckButton color = new CheckButton(_("_Use color"));
      color.Active = _color;
      color.Toggled += delegate
	{
	  _color = color.Active;
	  InvalidatePreview();
	};
      table.Attach(color, 0, 1, 3, 4);
			
      return dialog;
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      Initialize(_drawable);
      preview.Update(DoNCP);
    }

    Coordinate<int>[,] vp;

    int[] _distances;
    int[] _data, _under, _over;

    int _bpp;
    bool _hasAlpha;
    int _width, _height;
    Pixel _pixel;

    void Initialize(Drawable drawable)
    {
      Rectangle rectangle = drawable.MaskBounds;

      _bpp = drawable.Bpp;
      _pixel = drawable.CreatePixel();

      _hasAlpha = drawable.HasAlpha;
      if (_hasAlpha)
	{
	  _bpp--;
	  _pixel.Alpha = 255;
	}

      _width = rectangle.Width;
      _height = rectangle.Height;

      int xmid = _width / 2;
      int ymid = _height / 2;

      _distances = new int[4 * _points];
      _data = new int[4 * _points];
      _under = new int[4 * _points];
      _over = new int[4 * _points];

      vp = new Coordinate<int>[_bpp, 4 * _points];

      RandomCoordinateGenerator generator = 
	new RandomCoordinateGenerator((int) _seed, _width - 1, _height - 1, 
				      _points);
 
      for (int b = 0; b < _bpp; b++) 
	{
	  int i = 0;
	  foreach (Coordinate<int> c in generator)
	    {
	      int px = c.X;
	      int py = c.Y;

	      int offx = (px < xmid) ? _width : -_width;
	      int offy = (py < ymid) ? _height : -_height;

	      vp[b, i] = new Coordinate<int>(px, py);
	      vp[b, i + _points] = new Coordinate<int>(px + offx, py);
	      vp[b, i + 2 * _points] = new Coordinate<int>(px, py + offy);
	      vp[b, i + 3 * _points] = new Coordinate<int>(px + offx,
							   py + offy);
	      i++;
	    }
	}		
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress("NCP");
      iter.IterateDest(DoNCP);
			
      Display.DisplaysFlush();
    }

    int Select(int n)
    {
      int pivot = 0;
      int len = 4 * _points;
      _data = _distances;

      while (true)
	{
	  int j = 0;
	  int k = 0;
	  int pcount = 0;
	  
	  pivot = _data[0];

	  for (int i = 0; i < len; i++)
	    {
	      int elem = _data[i];

	      if (elem < pivot)
		_under[j++] = elem;
	      else if (elem > pivot)
		_over[k++] = elem;
	      else
		pcount++;	
	    }

	  if (n < j)
	    {
	      len = j;
	      _data = _under;
	    }
	  else if (n < j + pcount)
	    {
	      break;
	    }
	  else
	    {
	      len = k;
	      _data = _over;
	      n -= j + pcount;
	    }
	}
      return pivot;
    }

    Pixel DoNCP(int x, int y)
    {
      int b = 0;
      Pixel.FillDestFunc func = delegate {return Calc(b++, x, y);};

      if (_color)
	{
	  _pixel.Fill(func);
	}
      else
	{
	  _pixel.FillSame(func);
	}
      return _pixel;
    }

    int Calc(int b, int x, int y)
    {
      // compute distance to each point
      for (int k = 0; k < _points * 4; k++) 
	{
	  Coordinate<int> p = vp[b, k];
	  int x2 = x - p.X;
	  int y2 = y - p.Y;
	  _distances[k] = x2 * x2 + y2 * y2;
	}
      
      int val = (int) (255.0 * Math.Sqrt((double) Select(_closest) / 
					 (_width * _height)));      
      return 255 - val;	// invert
    }
  }
}
