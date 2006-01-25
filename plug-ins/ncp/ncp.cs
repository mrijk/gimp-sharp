// The ncp plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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

using Gtk;

namespace Gimp.ncp
{
  public class ncp : Plugin
  {
    AspectPreview _preview;
    ScaleEntry _closestEntry;

    [SaveAttribute]
    UInt32 _seed;
    [SaveAttribute]
    bool _random_seed;
    [SaveAttribute]
    int _points = 12;
    [SaveAttribute]
    int _closest = 1;
    [SaveAttribute]
    bool _color = true;

    [STAThread]
    static void Main(string[] args)
    {
      new ncp(args);
    }

    public ncp(string[] args) : base(args)
    {
    }

    struct Point 
    {
      public int x;
      public int y;
    }

    override protected void Query()
    {
      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("points", 12, typeof(int),
				 "Number of points"));

      InstallProcedure("plug_in_ncp",
		       "Generates 2D textures",
		       "Generates 2D textures",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2004-2006",
		       "NCP...",
		       "RGB*, GRAY*",
		       in_params);

      MenuRegister("<Image>/Filters/Render");
      IconRegister("ncp.png");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("ncp", true);

      Dialog dialog = DialogNew("ncp", "ncp", IntPtr.Zero, 0, null, "ncp");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new AspectPreview(_drawable, false);
      _preview.Invalidated += new EventHandler(UpdatePreview);
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(4, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      RandomSeed seed = new RandomSeed(ref _seed, ref _random_seed);

      table.AttachAligned(0, 0, "Random _Seed:", 0.0, 0.5, seed, 2, true);

      ScaleEntry entry = new ScaleEntry(table, 0, 1, "Po_ints:", 150, 3,
					_points, 1.0, 256.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      entry.ValueChanged += new EventHandler(PointsUpdate);

      _closestEntry = new ScaleEntry(table, 0, 2, "C_lose to:", 150, 3,
				     _closest, 1.0, _points, 1.0, 8.0, 0,
				     true, 0, 0, null, null);
      _closestEntry.ValueChanged += new EventHandler(CloseToUpdate);

      CheckButton color = new CheckButton("_Use color");
      color.Active = _color;
      color.Toggled += new EventHandler(ColorToggled);
      table.Attach(color, 0, 1, 3, 4);
			
      dialog.ShowAll();
      return DialogRun();
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      Initialize(_drawable);

      int width, height;
      _preview.GetSize(out width, out height);

      byte[] buffer = new byte[width * height * 3];
      byte[] dest = new byte[3];
      for (int y = 0; y < height; y++)
	{
	int y_orig = _height * y / height;
	for (int x = 0; x < width; x++)
	  {
	  long index = 3 * (y * width + x);
	  int x_orig = _width * x / width;

	  dest = DoNCP(x_orig, y_orig);
	  dest.CopyTo(buffer, index);
	  }
	}
      _preview.DrawBuffer(buffer, width * 3);
    }

    void PointsUpdate(object sender, EventArgs e)
    {
      _points = (int) (sender as Adjustment).Value;
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
	_preview.Invalidate();
	}
    }

    void CloseToUpdate(object sender, EventArgs e)
    {
      _closest = (int) (sender as Adjustment).Value;
      _preview.Invalidate();
    }

    void ColorToggled (object sender, EventArgs args)
    {
      _color = (sender as CheckButton).Active;
      _preview.Invalidate();
    }
		
    Point[,] vp;

    int[] _distances;
    int[] _data, _under, _over;

    byte[] _dest;
    int _bpp;
    bool _has_alpha;
    int _width, _height;

    void Initialize(Drawable drawable)
    {
      int x1, y1, x2, y2;
      drawable.MaskBounds(out x1, out y1, out x2, out y2);

      Random random = new Random((int) _seed);

      _bpp = drawable.Bpp;
      _has_alpha = drawable.HasAlpha();
      if (_has_alpha)
	_bpp--;
      _dest = new byte[_bpp];

      _width = x2 - x1;
      _height = y2 - y1;

      int xmid = _width / 2;
      int ymid = _height / 2;

      _distances = new int[4 * _points];
      _data = new int[4 * _points];
      _under = new int[4 * _points];
      _over = new int[4 * _points];

      vp = new Point[_bpp, 4 * _points];

      for (int b = 0; b < _bpp; b++) 
	{
	for (int i = 0; i < _points; i++)
	  {
	  int px = random.Next(0, _width - 1);
	  int py = random.Next(0, _height - 1);

	  vp[b, i].x = px;
	  vp[b, i].y = py ;
	  vp[b, i + _points].x = (px < xmid) ? (vp[b, i].x + _width) 
	    : (vp[b, i].x - _width);
	  vp[b, i + _points].y = vp[b, i].y;
	  vp[b, i + 2 * _points].x = vp[b, i].x;
	  vp[b, i + 2 * _points].y = (py < ymid) ? (vp[b, i].y + _height) 
	    : (vp[b, i].y - _height);
	  vp[b, i + 3 * _points].x = (px < xmid) ? (vp[b, i].x + _width) 
	    : (vp[b, i].x - _width);
	  vp[b, i + 3 * _points].y = (py < ymid) ? (vp[b, i].y + _height) 
	    : (vp[b, i].y - _height);
	  }
	}		
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void DoSomething(Drawable drawable)
    {
      Initialize(drawable);
      RgnIterator iter = new RgnIterator(drawable, RunMode.INTERACTIVE);
      iter.Progress = new Progress("NCP");
      iter.Iterate(new RgnIterator.IterFuncDest(DoNCP));
			
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

    byte[] DoNCP(int x, int y)
    {
      for (int b = 0; b < _bpp; b++) 
	{
	// compute distance to each point
	for (int k = 0; k < _points * 4; k++) 
	  {
	  Point p = vp[b, k];
	  int x2 = x - p.x;
	  int y2 = y - p.y;
	  _distances[k] = x2 * x2 + y2 * y2;
	  }

	byte val = (byte) (255.0 * Math.Sqrt((double) Select(_closest) / 
					     (_width * _height)));

	// invert
	val = (byte) (255 - val);
	if (_color) 
	  { 
	  _dest[b] = val;
	  }
	else 
	  {
	  for (int l = 0; l < _bpp; l++) 
	    _dest[l] = val;
	  break;
	  }
	}
      if (_has_alpha) 
	_dest[_bpp]= 255;

      return _dest;
    }
  }
  }
