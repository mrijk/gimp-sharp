// The PhotoshopActions plug-in
// Copyright (C) 2006-2012 Maurits Rijk
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

    override protected IEnumerable ListParameters()
    {
      DoubleParameter.Resolution = _resolution;
      return base.ListParameters();
    }

    override public bool Execute()
    {
      ImageBaseType type = ImageBaseType.Rgb;	// Fix me!

      switch (_mode.Value)
	{
	case "Grys":
	  type = ImageBaseType.Gray;
	  break;
	default:
	  Console.WriteLine("Type: " + _mode.Value);
	  type = ImageBaseType.Rgb;
	  break;
	}

      int width = (int) (Parameters["Wdth"] as DoubleParameter).GetPixels(0);
      int height = (int) (Parameters["Hght"] as DoubleParameter).GetPixels(0);

      ImageType imageType;
      FillType fillType;

      switch (_fill.Value)
	{
	case "Trns":
	  imageType = (type == ImageBaseType.Gray) 
	    ? ImageType.Graya : ImageType.Rgba;
	  fillType = FillType.Transparent;
	  break;
	default:
	  imageType = (type == ImageBaseType.Gray) 
	    ? ImageType.Gray : ImageType.Rgb;
	  fillType = FillType.White;
	  break;
	}

      var image = new Image(width, height, type);
      var layer = new Layer(image, "Layer 1", width, height, imageType);
      image.InsertLayer(layer, 0);

      layer.Fill(fillType);

      ActiveImage = image;
      ActiveDrawable = layer;

      new Display(image);

      return true;
    }
  }
}
