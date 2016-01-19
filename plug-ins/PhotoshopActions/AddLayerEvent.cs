// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// AddLayerEvent.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gimp.PhotoshopActions
{
  public class AddLayerEvent : ActionEvent
  {
    [Parameter("below")]
    bool _below;
    [Parameter("Usng")]
    ObjcParameter _objc;
    [Parameter("Nm")]
    string _name;
    [Parameter("Md")]
    EnumParameter _mode;

    public AddLayerEvent(ActionEvent srcEvent) : base(srcEvent) 
    {
      Parameters.Fill(this);
      _objc?.Fill(this);
    }

    public override string EventForDisplay => base.EventForDisplay + " layer";

    /*
    protected override IEnumerable ListParameters()
    {
      yield return "Using: layer";

      if (_name != null)
	yield return Format(_name, "Nm");
      
      if (_mode != null)
	yield return Format(_mode, "Md");

      if (Parameters["below"] != null)
	yield return Format(_below, "Below");
    }
    */
    LayerModeEffects GetMode()
    {
      var mode = LayerModeEffects.Normal;

      if (_mode == null)
	return mode;

      switch (_mode.Value)
	{
	case "Dfrn":
	  mode = LayerModeEffects.Difference;
	  break;
	case "Drkn":
	  mode = LayerModeEffects.DarkenOnly;
	  break;
	case "Lghn":
	  mode = LayerModeEffects.LightenOnly;
	  break;
	case "Mltp":
	  mode = LayerModeEffects.Multiply;
	  break;
	case "Ovrl":
	  mode = LayerModeEffects.Overlay;
	  break;
	case "Scrn":
	  mode = LayerModeEffects.Screen;
	  break;
	default:
	  Console.WriteLine("AddLayerEvent, unknown mode: " + _mode.Value);
	  mode = LayerModeEffects.Normal;
	  break;
	}

      return mode;
    }

    override public bool Execute()
    {
      var image = ActiveImage;
      var name = _name ?? GetNextUnnamedLayer();

      ImageType imageType;
      switch (image.BaseType)
	{
	case ImageBaseType.Rgb:
	  imageType = ImageType.Rgba;
	  break;
	case ImageBaseType.Gray:
	  imageType = ImageType.Gray;
	  break;
	default:
	  imageType = ImageType.Rgba;
	  break;
	}

      var layer = new Layer(image, name, imageType, 100, GetMode());
      image.InsertLayer(layer, 0);
      image.ActiveLayer = layer;
      SelectedLayer = layer;

      layer.Fill(FillType.Transparent);

      return true;
    }

    string GetNextUnnamedLayer()
    {
      int max = 1;

      var regex = new Regex(@"Layer ([0-9]+)");
      foreach (var layer in ActiveImage.Layers)
	{
	  if (regex.IsMatch(layer.Name)) {
	    var matches = regex.Matches(layer.Name);
	    int nr = Convert.ToInt32(matches[0].Groups[1].Value);
	    if (nr > max)
	      max = nr;
	  }
	}
      return "Layer " + (max + 1);
    }
  }
}
