// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// NewDocumentEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class NewDocumentEvent : ActionEvent
  {
    [Parameter("Md")]
    TypeParameter _mode;
    [Parameter("Wdth")]
    double _width;
    [Parameter("Hght")]
    double _height;
    [Parameter("Rslt")]
    double _resolution;
    [Parameter("pixelScaleFactor")]
    double _pixelScaleFactor;
    [Parameter("Fl")]
    EnumParameter _fill;
    [Parameter("Dpth")]
    int _depth;
    [Parameter("profile")]
    string _profile;

    public NewDocumentEvent(ActionEvent srcEvent, ObjcParameter myObject) : 
      base(srcEvent)
    {
      myObject.Fill(this);
    }

    protected override IEnumerable ListParameters()
    {
      yield return "New: document";
      if (_mode != null)
	{
	  yield return "Mode: " + Abbreviations.Get(_mode.Value);
	}
      yield return "Width: " + _width;
      yield return "Height: " + _height;
      yield return "Resolution: " + _resolution;

      yield return "Pixel Aspect Ratio: " + _pixelScaleFactor;
      if (_fill != null)
	{
	  yield return "Fill: " + Abbreviations.Get(_fill.Value);
	}
      yield return "Depth: " + _depth;
      if (_profile != null)
	{
	  yield return "Profile: \"" + _profile + "\"";
	}
    }

    override public bool Execute()
    {
      ImageBaseType type = ImageBaseType.Rgb;	// Fix me!
      int width = (int) _width;
      int height = (int) _height;

      ImageType imageType;
      FillType fillType;

      switch (_fill.Value)
	{
	case "Trns":
	  imageType = ImageType.Rgba;
	  fillType = FillType.Transparent;
	  break;
	default:
	  imageType = ImageType.Rgb;
	  fillType = FillType.White;
	  break;
	}

      Image image = new Image(width, height, type);
      Layer layer = new Layer(image, "Layer 1", width, height,
			      imageType, 100, LayerModeEffects.Normal);
      image.AddLayer(layer, 0);

      layer.Fill(fillType);

      ActiveImage = image;
      ActiveDrawable = layer;

      new Display(image);

      return true;
    }
  }
}
