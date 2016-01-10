// The Forge plug-in
// Copyright (C) 2006-2016 Maurits Rijk
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
  public enum ForgeType
  {
    Planet,
    Clouds,
    Stars
  }

  public class Dialog : GimpDialogWithPreview
  {
    // Flag for spin buttons values specified by the user
    bool dimspec, powerspec;
    // Flag for radio buttons values specified by the user
    bool glacspec, icespec, starspec, hourspec, inclspec, starcspec;

    ScaleEntry _dimensionEntry;
    ScaleEntry _powerEntry;

    readonly Variable<int> _type;

    public Dialog(Drawable drawable, VariableSet variables) : 
      base(_("Forge"), drawable, variables, () => new AspectPreview(drawable))
    {
      _type = GetVariable<int>("type");

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

      var vbox = new VBox(false, 1);
      frame.Add(vbox);

      var button = AddTypeButton(vbox, null, ForgeType.Planet, _("Pl_anet"));
      button = AddTypeButton(vbox, button, ForgeType.Clouds, _("C_louds"));
      AddTypeButton(vbox, button, ForgeType.Stars, _("_Night"));
    }

    RadioButton AddTypeButton(VBox vbox, RadioButton previous, ForgeType type, 
			      string description)
    {
      var button = new GimpRadioButton<int>(previous, description, (int) type, 
					    _type);
      vbox.Add(button);
      return button;
    }

    void PlanetRadioButtonEventHandler(object source, EventArgs e)
    {
      if (!dimspec)
        {
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 1.2;
        }
    }

    void CloudsRadioButtonEventHandler(object source, EventArgs e)
    {
      if (!dimspec)
        {
          _dimensionEntry.Value = 2.15;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 0.75;
        }
    }

    void NightRadioButtonEventHandler(object source, EventArgs e)
    {
      if (!dimspec)
        {
          _dimensionEntry.Value = 2.4;
        }

      if (!powerspec)
        {
          _powerEntry.Value = 1.2;
        }
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
      var entry = new ScaleEntry(table, 0, 0, _("_Dimension (0.0 - 3.0):"), 
				 150, 3, GetVariable<double>("dimension"), 
				 0.0, 3.0, 0.1, 1.0, 1);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet || IsClouds;
	};
    }
    void CreatePowerEntry(Table table)
    {
      var entry = new ScaleEntry(table, 3, 0, _("_Power:"), 150, 3,
				 GetVariable<double>("power"), 0.0, 
				 Double.MaxValue, 0.1, 1.0, 1);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet || IsClouds;
	};
    }

    void CreateGlaciersEntry(Table table)
    {
      var entry = new ScaleEntry(table, 0, 1, _("_Glaciers"), 150, 3, 
				 GetVariable<double>("glaciers"), 0.0, 
				 Double.MaxValue, 0.1, 1.0, 0);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet;
	};
    }

    void CreateIceEntry(Table table)
    {
      var entry = new ScaleEntry(table, 3, 1, _("_Ice"), 150, 3,
				 GetVariable<double>("ice_level"), 0.0, 
				 Double.MaxValue, 0.1, 1.0, 1);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet;
	};
    }

    void CreateHourEntry(Table table)
    {
      var entry = new ScaleEntry(table, 0, 2, _("Ho_ur (0 - 24):"), 150, 3, 
				 GetVariable<double>("hour"), 0.0, 24.0, 0.1, 
				 1.0, 0);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet;
	};
    }

    void CreateInclinationEntry(Table table)
    {
      var entry = new ScaleEntry(table, 3, 2, _("I_nclination (-90 - 90):"), 150, 
				 3, GetVariable<double>("inclination"), 
				 -90.0, 90.0, 1, 10.0, 0);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet;
	};
    }

    void CreateStarPercentageEntry(Table table)
    {
      var entry = new ScaleEntry(table, 0, 3, _("_Stars (0 - 100):"), 150, 3, 
				 GetVariable<double>("stars_fraction"), 1.0, 
				 100.0, 1.0, 8.0, 0);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet || IsStars;
	};
    }

    void CreateSaturationEntry(Table table)
    {
      var entry = new ScaleEntry(table, 3, 3, _("Sa_turation:"), 150, 3, 
				 GetVariable<double>("saturation"), 0.0, 
				 Int32.MaxValue, 1.0, 8.0, 0);
      _type.ValueChanged += delegate
	{
	  entry.Sensitive = IsPlanet;
	};
    }

    bool IsPlanet => IsType(ForgeType.Planet);

    bool IsClouds => IsType(ForgeType.Clouds);

    bool IsStars => IsType(ForgeType.Stars);

    bool IsType(ForgeType type) => _type.Value == (int) type;

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(preview as AspectPreview, Drawable);
    }
  }
}
