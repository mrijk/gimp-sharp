// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// ImageSizeEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class ImageSizeEvent : ActionEvent
  {
    [Parameter("Hght")]
    double _height;
    [Parameter("Wdth")]
    double _width;
    [Parameter("scaleStyles")]
    bool _scaleStyles;
    [Parameter("CnsP")]
    bool _constrainProportions;
    [Parameter("Intr")]
    EnumParameter _interpolation;

    override public bool Execute()
    {      
      var width = Parameters["Wdth"] as DoubleParameter; 
      _width = width?.GetPixels(ActiveImage.Width) ?? 0;

      var height = Parameters["Hght"] as DoubleParameter; 
      _height = height?.GetPixels(ActiveImage.Height) ?? 0;
      
      if (_constrainProportions)
	{
	  if (_width != 0 && _height == 0)
	    _height = ActiveImage.Height * _width / ActiveImage.Width;
	  if (_width == 0 && _height != 0)
	    _width = ActiveImage.Width * _height / ActiveImage.Height;
	}
      
      ActiveImage.Scale((int) _width, (int) _height);
      return true;
    }
  }
}
