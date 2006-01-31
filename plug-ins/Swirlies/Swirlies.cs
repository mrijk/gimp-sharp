// The Swirlies plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Swirlies.cs
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

namespace Gimp.Swirlies
{
  public class Swirlies : Plugin
  {
    Random _random;
    byte[] _dest = new byte[3];
    int _width;
    int _height;
    List<Swirly> _swirlies = new List<Swirly>();

    AspectPreview _preview;

    [SaveAttribute]
    UInt32 _seed;
    [SaveAttribute]
    bool _random_seed;
    [SaveAttribute]
    int _points = 3;

    [STAThread]
    static void Main(string[] args)
    {
      new Swirlies(args);
    }

    public Swirlies(string[] args) : base(args)
    {
    }

    override protected ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();
      
      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_swirlies",
					  "Generates 2D textures",
					  "Generates 2D textures",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Swirlies...",
					  "RGB",
					  in_params);

      procedure.MenuPath = "<Image>/Filters/Render";
      procedure.IconFile = "Swirlies.png";

      set.Add(procedure);
      
      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Swirlies", true);

      Dialog dialog = DialogNew("Swirlies", "swirlies", IntPtr.Zero, 0, null, 
				"swirlies");

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
					_points, 1.0, 16.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      entry.ValueChanged += new EventHandler(PointsUpdate);
			
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

	  dest = DoSwirlies(x_orig, y_orig);
	  dest.CopyTo(buffer, index);
	  }
	}
      _preview.DrawBuffer(buffer, width * 3);
    }

    void PointsUpdate(object sender, EventArgs e)
    {
      _points = (int) (sender as Adjustment).Value;
      _preview.Invalidate();
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    void Initialize(Drawable drawable)
    {

      _random = new Random((int) _seed);
      Swirly.Random = _random;

      _width = drawable.Width;
      _height = drawable.Height;

      _swirlies.Clear();
      for (int i = 0; i < _points; i++)
	_swirlies.Add(Swirly.CreateRandom());
    }

    override protected void DoSomething(Drawable drawable)
    {
      Initialize(drawable);
      RgnIterator iter = new RgnIterator(drawable, RunMode.INTERACTIVE);
      iter.Progress = new Progress("Swirlies");
      iter.Iterate(new RgnIterator.IterFuncDest(DoSwirlies));
      
      Display.DisplaysFlush();
    }

    byte[] DoSwirlies(int x, int y)
    {
      double Fr = 0.0, Fg = 0.0, Fb = 0.0;

      double zoom = 0.5;
      int terms = 5;

      foreach (Swirly swirly in _swirlies)
	{
	swirly.CalculateOnePoint(terms, _width, _height, zoom, x, y, 
				 ref Fr, ref Fg, ref Fb);
	_dest[0] = FloatToIntPixel(RemapColorRange(Fr));
	_dest[1] = FloatToIntPixel(RemapColorRange(Fg));
	_dest[2] = FloatToIntPixel(RemapColorRange(Fb));
	}
      return _dest;
    }
    
    double RemapColorRange(double val)
    {
      double _post_gain = 0.35;
      double _pre_gain = 10000;

      val = Math.Abs(val);
      return Math.Tanh(_post_gain * Math.Log(1 + _pre_gain * val));
    }

    byte FloatToIntPixel(double val)
    {
      val *= 255;
      val += 1 - 2 * _random.NextDouble();
      val += 1 - 2 * _random.NextDouble();

      if (val < 0)
	{
	return 0;
	}
      else if (val > 255)
	{
	return 255;
	}
      else
	{
	return (byte) val;
	}
    }
  }
}
