// The Forge plug-in
// Copyright (C) 2006-2021 Maurits Rijk
//
// Dialog.cs
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

namespace Gimp.Forge
{
  public class Dialog : GimpDialogWithPreview<AspectPreview>
  {
    delegate void GenericEventHandler(object o, EventArgs e);

    // Flag for spin buttons values specified by the user
    bool dimspec, powerspec;
    // Flag for radio buttons values specified by the user
    bool glacspec, icespec, starspec, hourspec, inclspec, starcspec;

    bool _clouds, _stars;

    ScaleEntry _dimensionEntry;
    ScaleEntry _glacierEntry;
    ScaleEntry _iceEntry;
    ScaleEntry _powerEntry;
    ScaleEntry _hourEntry;
    ScaleEntry _inclinationEntry;
    ScaleEntry _starsEntry;
    ScaleEntry _saturationEntry;

    public Dialog(Drawable drawable, VariableSet variables) : 
      base(_("Forge"), drawable, variables)
    {
      var hbox = new HBox(false, 12);
      Vbox.PackStart(hbox);

      CreateTypeFrame(hbox);
      CreateRandomSeedEntry(hbox);
      CreateParametersTable();
    }

    void CreateTypeFrame(HBox hbox)
    {
      var frame = new GimpFrame(_("Type"));
      hbox.PackStart(frame, false, false, 0);

      var typeBox = new VBox(false, 1);
      frame.Add(typeBox);

      var button = CreateRadioButtonInVBox(typeBox, null,
					   PlanetRadioButtonEventHandler, 
					   _("Pl_anet"));

      button = CreateRadioButtonInVBox(typeBox, button,
				       CloudsRadioButtonEventHandler, 
				       _("C_louds"));

      button = CreateRadioButtonInVBox(typeBox, button,
				       NightRadioButtonEventHandler, _("_Night"));
    }

    void PlanetRadioButtonEventHandler(object source, EventArgs e)
    {
      if (!(source as RadioButton).Active)
	return;

      _clouds = _stars = false;

      if (!dimspec)
        {
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 1.2;
        }

      SetSlidersSensitivity();
    }

    void CloudsRadioButtonEventHandler(object source, EventArgs e)
    {
      _clouds = (source as RadioButton).Active;
      if (!_clouds)
	return;

      _stars = false;

      if (!dimspec)
        {
          _dimensionEntry.Value = 2.15;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 0.75;
        }

      SetSlidersSensitivity();
    }

    void NightRadioButtonEventHandler(object source, EventArgs e)
    {
      _stars = (source as RadioButton).Active;
      if (!_stars)
	return;

      _clouds = false;

      if (!dimspec)
        {
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 1.2;
        }

      SetSlidersSensitivity();
    }

    void CreateRandomSeedEntry(HBox hbox)
    {
      var vbox = new VBox(false, 1);
      hbox.PackStart(vbox, false, false, 0);

      var seed = GetVariable<UInt32>("seed");
      var randomSeed = GetVariable<bool>("random_seed");
      vbox.PackStart(new RandomSeed(seed, randomSeed), false, false, 0);
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
      new ScaleEntry(table, 0, 0, _("_Dimension (0.0 - 3.0):"), 150, 3, 
		     GetVariable<double>("dimension"), 0.0, 3.0, 0.1, 1.0, 1);
    }

    void CreatePowerEntry(Table table)
    {
      new ScaleEntry(table, 3, 0, _("_Power:"), 150, 3,
		     GetVariable<double>("power"), 0.0, Double.MaxValue, 
		     0.1, 1.0, 1);
    }

    void CreateGlaciersEntry(Table table)
    {
      new ScaleEntry(table, 0, 1, _("_Glaciers"), 150, 3, 
		     GetVariable<double>("glaciers"), 0.0, Double.MaxValue, 
		     0.1, 1.0, 0);
    }

    void CreateIceEntry(Table table)
    {
      new ScaleEntry(table, 3, 1, _("_Ice"), 150, 3,
		     GetVariable<double>("ice_level"), 0.0, Double.MaxValue, 
		     0.1, 1.0, 1);
    }

    void CreateHourEntry(Table table)
    {
      new ScaleEntry(table, 0, 2, _("Ho_ur (0 - 24):"), 150, 3, 
		     GetVariable<double>("hour"), 0.0, 24.0, 0.1, 1.0, 0);
    }

    void CreateInclinationEntry(Table table)
    {
      new ScaleEntry(table, 3, 2, _("I_nclination (-90 - 90):"), 150, 3, 
		     GetVariable<double>("inclination"), -90.0, 90.0, 1, 10.0, 0);
    }

    void CreateStarPercentageEntry(Table table)
    {
      new ScaleEntry(table, 0, 3, _("_Stars (0 - 100):"), 150, 3, 
		     GetVariable<double>("stars_fraction"), 1.0, 100.0, 
		     1.0, 8.0, 0);
    }

    void CreateSaturationEntry(Table table)
    {
      new ScaleEntry(table, 3, 3, _("Sa_turation:"), 150, 3, 
		     GetVariable<double>("saturation"), 0.0, Int32.MaxValue, 
		     1.0, 8.0, 0);
    }

    RadioButton CreateRadioButtonInVBox(VBox vbox, RadioButton group,
					GenericEventHandler radioButtonEventHandler, 
					string label)
    { 
      var button = new RadioButton(group, label);
      button.Clicked += new EventHandler(radioButtonEventHandler);
      vbox.PackStart(button, true, true, 10);

      return button;
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

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(preview as AspectPreview);
    }
  }
}
