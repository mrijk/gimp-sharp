// The ncp plug-in
// Copyright (C) 2004-2010 Maurits Rijk
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

    Pixel _pixel;
    Calculator _calculator;

    static void Main(string[] args)
    {
      new ncp(args);
    }

    ncp(string[] args) : base(args, "ncp")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList() {
	new ParamDef("points", 12, typeof(int), _("Number of points")),
	new ParamDef("closest", 1, typeof(int), _("Closest point")),
	new ParamDef("color", 1, typeof(bool), _("Color (true), B&W (false)"))
      };

      yield return new Procedure("plug_in_ncp",
				 _("Generates 2D textures"),
				 _("Generates 2D textures"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2010",
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

      var dialog = DialogNew("ncp", "ncp", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "ncp");

      var table = new GimpTable(4, 3)
	{
	  ColumnSpacing = 6, 
	  RowSpacing = 6
	};
      Vbox.PackStart(table, false, false, 0);

      CreateRandomSeedWidget(table);
      CreatePointsWidget(table);
      CreateClosestEntryWidget(table);
      CreateUseColorWidget(table);
			
      return dialog;
    }

    void CreateRandomSeedWidget(GimpTable table)
    {
      var seed = new RandomSeed(ref _seed, ref _random_seed);
      seed.Toggle.Toggled += delegate {InvalidatePreview();};
      seed.SpinButton.ValueChanged += delegate {InvalidatePreview();};
      table.AttachAligned(0, 0, _("Random _Seed:"), 0.0, 0.5, seed, 2, true);
    }

    void CreatePointsWidget(GimpTable table)
    {
      var entry = new ScaleEntry(table, 0, 1, _("Po_ints:"), 150, 3, 
				 _points, 1.0, 256.0, 1.0, 8.0, 0);
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
    }

    void CreateClosestEntryWidget(GimpTable table)
    {
      _closestEntry = new ScaleEntry(table, 0, 2, _("C_lose to:"), 150, 3, 
				     _closest, 1.0, _points, 1.0, 8.0, 0);
      _closestEntry.ValueChanged += delegate
	{
	  _closest = _closestEntry.ValueAsInt;
	  InvalidatePreview();
	};
    }

    void CreateUseColorWidget(GimpTable table)
    {
      var color = new CheckButton(_("_Use color")) {Active = _color};
      color.Toggled += delegate
	{
	  _color = color.Active;
	  InvalidatePreview();
	};
      table.Attach(color, 0, 1, 3, 4);
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      Initialize(_drawable);
      preview.Update(DoNCP);
    }

    void Initialize(Drawable drawable)
    {
      int bpp = drawable.Bpp;
      _pixel = drawable.CreatePixel();

      if (drawable.HasAlpha)
	{
	  bpp--;
	  _pixel.Alpha = 255;
	}

      _calculator = new Calculator(_points, _closest, bpp, drawable.MaskBounds,
				   (int) _seed);
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);
      var iter = new RgnIterator(drawable, "NCP");
      iter.IterateDest(DoNCP);
    }

    Pixel DoNCP(IntCoordinate c)
    {
      int b = 0;
      Func<int> func = () => _calculator.Calc(b++, c);

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
  }
}
