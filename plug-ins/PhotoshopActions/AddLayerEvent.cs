// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
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

    static int _layerNr = 1;

    public AddLayerEvent(ActionEvent srcEvent) : base(srcEvent) 
    {
      Parameters.Fill(this);
      if (_objc != null)
	_objc.Fill(this);
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " layer";}
    }
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
      LayerModeEffects mode = LayerModeEffects.Normal;

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
      Image image = ActiveImage;

      string name;
      if (_name == null)
	{
	  name = "Layer " + _layerNr++;
	}
      else
	{
	  name = _name;
	}

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

      Layer layer = new Layer(image, name, imageType, 100, GetMode());
      image.AddLayer(layer, 0);
      image.ActiveLayer = layer;
      SelectedLayer = layer;

      layer.Fill(FillType.Transparent);

      return true;
    }
  }
}
