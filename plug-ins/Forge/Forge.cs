// The Forge plug-in
// Copyright (C) 2006-2011 Massimo Perga (massimo.perga@gmail.com)
//
// Forge.cs
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

namespace Gimp.Forge
{
  class Forge : PluginWithPreview
  {
    const double planetAmbient = 0.05;
    bool _randomSeed = true;
    bool _previewAllowed = false;

    RadioButton _PlanetRadioButton;
    RadioButton _CloudsRadioButton;
    RadioButton _NightRadioButton;
    ScaleEntry _dimensionEntry;
    ScaleEntry _glacierEntry;
    ScaleEntry _iceEntry;
    ScaleEntry _powerEntry;
    ScaleEntry _hourEntry;
    ScaleEntry _inclinationEntry;
    ScaleEntry _starsEntry;
    ScaleEntry _saturationEntry;

    // Flag for spin buttons values specified by the user
    private bool dimspec, powerspec;
    // Flag for radio buttons values specified by the user
    private bool glacspec, icespec, starspec, hourspec, inclspec, starcspec;
    [SaveAttribute("clouds")]
    bool _clouds;	      	// Just generate clouds
    [SaveAttribute("stars")]
    bool _stars;	      		// Just generate stars
    [SaveAttribute("dimension")]
    double _fracdim;		// Fractal dimension
    [SaveAttribute("power")]
    double _powscale; 	      	// Power law scaling exponent
    [SaveAttribute("glaciers")]
    double _glaciers;
    [SaveAttribute("icelevel")]
    double _icelevel;
    [SaveAttribute("hour")]
    double _hourangle;
    [SaveAttribute("inclination")]
    double _inclangle;
    [SaveAttribute("starsfraction")]
    double _starfraction;
    [SaveAttribute("saturation")]
    double _starcolour;
    [SaveAttribute("seed")]
    uint _rseed;	      		// Current random seed
    private int forced;
    private const int meshsize = 256;	      	// FFT mesh size

    Random _random;

    delegate void GenericEventHandler(object o, EventArgs e);

    static void Main(string[] args)
    {
      GimpMain<Forge>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList()
	{
	  new ParamDef("clouds", false, typeof(bool), 
		       _("Clouds (true), Planet or Stars (false)")),
	  new ParamDef("stars", false, typeof(bool), 
		       _("Stars (true), Planet or Clouds (false)")),
	  new ParamDef("dimension", 2.4, typeof(double), 
		       _("Fractal dimension factor")),
	  new ParamDef("power", 1.0, typeof(double), 
		       _("Power factor")),
	  new ParamDef("glaciers", 0.75, typeof(double), 
		       _("Glaciers factor")),
	  new ParamDef("ice", 0.4, typeof(double), 
		       _("Ice factor")),
	  new ParamDef("hour", 0.0, typeof(double), 
		       _("Hour factor")),
	  new ParamDef("inclination", 0.0, typeof(double), 
		       _("Inclination factor")),
	  new ParamDef("stars", 100.0, typeof(double), 
		       _("Stars factor")),
	  new ParamDef("saturation", 100.0, typeof(double), 
		       _("Saturation factor")),
	  new ParamDef("seed", 0, typeof(uint), 
		       _("Random generated seed"))
	};
      yield return new Procedure("plug_in_forge",
				 _("Creates an artificial world."),
				 _("Creates an artificial world."),
				 "Massimo Perga, Maurits Rijk",
				 "(C) Massimo Perga, Maurits Rijk",
				 "2006-2011",
				 _("Forge..."),
				 "RGB*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "Forge.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Forge", true);

      var dialog = DialogNew(_("Forge 0.2"), _("Forge"), IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, _("Forge"));

      var hbox = new HBox(false, 12);
      Vbox.PackStart(hbox);

      CreateTypeFrame(hbox);
      CreateRandomSeedEntry(hbox);
      CreateParametersTable();

      return dialog;
    }

    void CreateTypeFrame(HBox hbox)
    {
      var frame = new GimpFrame(_("Type"));
      hbox.PackStart(frame, false, false, 0);

      var typeBox = new VBox(false, 1);
      frame.Add(typeBox);

      _PlanetRadioButton = CreateRadioButtonInVBox(typeBox, null,
          PlanetRadioButtonEventHandler, _("Pl_anet"));

      _CloudsRadioButton = CreateRadioButtonInVBox(typeBox, _PlanetRadioButton,
          CloudsRadioButtonEventHandler, _("C_louds"));

      _NightRadioButton = CreateRadioButtonInVBox(typeBox, _PlanetRadioButton,
          NightRadioButtonEventHandler, _("_Night"));
    }

    void CreateRandomSeedEntry(HBox hbox)
    {
      var randomBox = new VBox(false, 1);
      hbox.PackStart(randomBox, false, false, 0);

      var seed = new RandomSeed(ref _rseed, ref _randomSeed);
      randomBox.PackStart(seed, false, false, 0);
    }

    void CreateParametersTable()
    {
      var table = new GimpTable(4, 6)
	{ColumnSpacing = 10, RowSpacing = 10};
      Vbox.PackEnd(table);

      CreateDimensionEntry(table);
      CreatePowerEntry(table);
      CreateGlaciersEntry(table);
      CreateIceEntry(table);
      CreateHourEntry(table);
      CreateInclinationEntry(table);
      CreateStarPercentageEntry(table);
      CreateSaturationEntry(table);
    }

    void CreateDimensionEntry(Table table)
    {
      _dimensionEntry = new ScaleEntry(table, 0, 0, 
				       _("_Dimension (0.0 - 3.0):"), 
				       150, 3, 
				       _fracdim, 0.0, 3.0, 
				       0.1, 1.0, 1);
      _dimensionEntry.ValueChanged += delegate
	{
	  if (forced > 0)
	    forced--;
	  else
	    dimspec = true;
	  _fracdim = _dimensionEntry.Value;
	}; 
    }

    void CreatePowerEntry(Table table)
    {
      _powerEntry = new ScaleEntry(table, 3, 0, _("_Power:"), 
				   150, 3, 
				   _powscale, 0.0, Double.MaxValue, 
				   0.1, 1.0, 1);
      _powerEntry.ValueChanged += delegate
	{
	  if (forced > 0)
	    forced--;
	  else
            powerspec = true;
	  _powscale = _powerEntry.Value;
	};
    }

    void CreateGlaciersEntry(Table table)
    {
      _glacierEntry = new ScaleEntry(table, 0, 1, _("_Glaciers"), 
				     150, 3, 
				     _glaciers, 0.0, Double.MaxValue, 
				     0.1, 1.0, 0);
      _glacierEntry.ValueChanged += delegate
	{
	  glacspec = true;
	  _glaciers = _glacierEntry.Value;
	  InvalidatePreview();
	};
    }

    void CreateIceEntry(Table table)
    {
      _iceEntry = new ScaleEntry(table, 3, 1, _("_Ice"), 
				 150, 3, 
				 _icelevel, 0.0, Double.MaxValue, 
				 0.1, 1.0, 1);
      _iceEntry.ValueChanged += delegate
	{
	  icespec = true;
	  _icelevel = _iceEntry.Value;
	  InvalidatePreview();
	};
    }

    void CreateHourEntry(Table table)
    {
      _hourEntry = new ScaleEntry(table, 0, 2, _("Ho_ur (0 - 24):"), 
				  150, 3, 
				  _hourangle, 0.0, 24.0, 0.1, 1.0, 0);
      _hourEntry.ValueChanged += delegate
	{
	  hourspec = true;
	  _hourangle = _hourEntry.Value;
	  InvalidatePreview();
	};
    }

    void CreateInclinationEntry(Table table)
    {
      _inclinationEntry = new ScaleEntry(table, 3, 2, 
					 _("I_nclination (-90 - 90):"), 
					 150, 3, 
					 _inclangle, -90.0, 90.0, 1, 10.0, 0);
      _inclinationEntry.ValueChanged += delegate
	{
	  inclspec = true;
	  _inclangle = _inclinationEntry.Value;  
	  InvalidatePreview();
	};
    }

    void CreateStarPercentageEntry(Table table)
    {
      _starsEntry = new ScaleEntry(table, 0, 3, _("_Stars (0 - 100):"), 
				   150, 3, 
				   _starfraction, 1.0, 100.0, 1.0, 8.0, 0);
      _starsEntry.ValueChanged += delegate
	{
	  starspec = true;
	  _starfraction = _starsEntry.Value;
	  InvalidatePreview();
	};
    }

    void CreateSaturationEntry(Table table)
    {
      _saturationEntry = new ScaleEntry(table, 3, 3, _("Sa_turation:"), 
					150, 3, 
					_starcolour, 0.0, Int32.MaxValue, 
					1.0, 8.0, 0);
      _saturationEntry.ValueChanged += delegate
	{
	  starcspec = true;
	  _starcolour = _saturationEntry.Value;
	  InvalidatePreview();
	};
    }

    RadioButton CreateRadioButtonInVBox(VBox vbox, 
        RadioButton radioButtonGroup,
        GenericEventHandler radioButtonEventHandler, 
        string radioButtonLabel)
    { 
      RadioButton radioButton = new RadioButton(radioButtonGroup, 
						radioButtonLabel);
      radioButton.Clicked += new EventHandler(radioButtonEventHandler);
      vbox.PackStart(radioButton, true, true, 10);

      return radioButton;
    }

    void SetDefaultValues()
    {
      _clouds = _stars = false;

      _PlanetRadioButton.Active = true;
      _glacierEntry.Value = 0.75;
      _iceEntry.Value = 0.4;
      _hourEntry.Value = 0;
      _inclinationEntry.Value = 0;
      _starsEntry.Value = 100.0;
      _saturationEntry.Value = 125.0;
    }

    void PlanetRadioButtonEventHandler(object source, EventArgs e)
    {
      if (!_PlanetRadioButton.Active)
	return;

      _clouds = _stars = false;

      if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 1.2;
        }

      SetSlidersSensitivity();

      if(_previewAllowed)
        InvalidatePreview();
    }

    void CloudsRadioButtonEventHandler(object source, EventArgs e)
    {
      _clouds = _CloudsRadioButton.Active;
      if (!_clouds)
	return;

      _stars = false;

      if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.15;
        }

      if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 0.75;
        }

      SetSlidersSensitivity();

      if(_previewAllowed)
        InvalidatePreview();
    }

    void NightRadioButtonEventHandler(object source, EventArgs e)
    {
      _stars = _NightRadioButton.Active;
      if (!_stars)
	return;

      _clouds = false;

      if (!dimspec)
        {
          forced++;
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          forced++;
          _powerEntry.Value = 1.2;
        }

      SetSlidersSensitivity();
      InvalidatePreview();
    }

    void SetSlidersSensitivity()
    {
      bool planet = !_stars && !_clouds;

      _dimensionEntry.Sensitive = planet || _clouds; 
      _powerEntry.Sensitive = planet || _clouds;
      _glacierEntry.Sensitive = planet; 
      _iceEntry.Sensitive = planet;
      _hourEntry.Sensitive = planet; 
      _inclinationEntry.Sensitive = planet;
      _starsEntry.Sensitive = planet || _stars;
      _saturationEntry.Sensitive = planet;
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      Console.WriteLine("UpdatePreview!");

      var dimensions = preview.Size;

      int width = dimensions.Width;
      int height = dimensions.Height;

      var pixelArray = new byte[width * height * 3];
      RenderForge(null, pixelArray, dimensions);

      preview.DrawBuffer(pixelArray, width * 3);
    }

    override protected void Reset()
    {
      SetDefaultValues(); 
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var dimensions = drawable.Dimensions;

      if (dimensions.Width < dimensions.Height)
      {
        new Message(_("This filter can be applied just if height <= width"));
        return;
      }

      Tile.CacheDefault(drawable);

      RenderForge(drawable, null, dimensions);
    }

    void RenderForge(Drawable drawable, byte[] pixelArray,
		     Dimensions dimensions)
    {
      InitParameters();
      Planet(drawable, pixelArray, dimensions);
    }

    void InitParameters()
    {
      // Set defaults when explicit specifications were not given.
      // The  default  fractal  dimension  and  power  scale depend upon
      // whether we're generating a planet or clouds.

      _random = new Random((int) _rseed);

      for (int i = 0; i < 7; i++)
	{
	  _random.Next();
	}

      if (!dimspec)
      {
        _fracdim = _clouds ? Cast(1.9, 2.3) : Cast(2.0, 2.7);
	_dimensionEntry.Value = _fracdim;
      }

      if (!powerspec)
      {
        _powscale = _clouds ? Cast(0.6, 0.8) : Cast(1.0, 1.5);
	_powerEntry.Value = _powscale;
      }

      if (!icespec)
	{
	  _icelevel = Cast(0.2, 0.6);
	  _iceEntry.Value = _icelevel;
	}

      if (!glacspec)
	{
	  _glaciers = Cast(0.6, 0.85);
	  _glacierEntry.Value = _glaciers;
	}

      if (!starspec)
	{
	  _starfraction = Cast(75, 125);
	  _starsEntry.Value = _starfraction;
	}

      if (!starcspec) 
	{
	  _starcolour = Cast(100, 150);
	  _saturationEntry.Value = _starcolour;
	}
      _previewAllowed = true;
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }

    void Planet(Drawable drawable, byte[] pixelArray, Dimensions dimensions)
    {
      new Planet(drawable, pixelArray, dimensions, 
		 _stars, _starfraction, _starcolour,
		 _clouds, _random, _icelevel, _glaciers,
		 _fracdim, hourspec, _hourangle, inclspec, _inclangle,
		 _powscale);
    }
  }
}
