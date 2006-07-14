// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// CanvasSizeEvent.cs
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
  public class CanvasSizeEvent : ActionEvent
  {
    [Parameter("Wdth")]
    double _width;
    [Parameter("Hght")]
    double _height;
    [Parameter("Hrzn")]
    EnumParameter _horizontal;
    [Parameter("Vrtc")]
    EnumParameter _vertical;
    [Parameter("canvasExtensionColorType")]
    EnumParameter _extensionColorType;

    protected override IEnumerable ListParameters()
    {
      yield return "Width: " + _width;
      yield return "Height: " + _height;
      if (_horizontal != null)
	{
	  yield return "Horizontal: " + Abbreviations.Get(_horizontal.Value);
	}
      if (_horizontal != null)
	{
	  yield return "Vertical: " + Abbreviations.Get(_horizontal.Value);
	}
    }

    override public bool Execute()
    {
      int offsetX = 0;
      int offsetY = 0;

      double width = (Parameters["Wdth"] as DoubleParameter).GetPixels(ActiveDrawable.Width);
      double height = (Parameters["Hght"] as DoubleParameter).GetPixels(ActiveDrawable.Height);

      if (_horizontal != null)
	{
	  switch (_horizontal.Value)
	    {
	    case "Cntr":
	      offsetX = Math.Max(0, (int) (width - ActiveDrawable.Width) / 2);
	      break;
	    default:
	      Console.WriteLine("CanvasSizeEvent: " + _horizontal.Value);
	      break;
	    }
	}

      if (_vertical != null)
	{
	  switch (_vertical.Value)
	    {
	    case "Cntr":
	      offsetY = Math.Max(0, (int) (height - ActiveDrawable.Height) / 2);
	      break;
	    default:
	      Console.WriteLine("CanvasSizeEvent: " + _vertical.Value);
	      break;
	    }
	}

      ActiveImage.Resize((int) width, (int) height, offsetX, offsetY);
      return true;
    }
  }
}
