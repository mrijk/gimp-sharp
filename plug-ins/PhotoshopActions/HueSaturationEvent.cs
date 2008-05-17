// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// HueSaturationEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class HueSaturationEvent : ActionEvent
  {
    [Parameter("Clrz")]
    bool _colorization;
    [Parameter("Adjs")]
    ListParameter _adjustment;

    int _hue, _saturation, _lightness;

    public override bool IsExecutable
    {
      get 
	{
	  return _adjustment == null || _adjustment.Count == 1;
	}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      if (_adjustment == null)
	{
	  Console.WriteLine("HueSaturationEvent: implement colorization only");
	}
      else if (_adjustment.Count > 1)
	{
	  Console.WriteLine("HueSaturationEvent: implement for > 1 params");
	}
      else if (_adjustment[0] is ObjcParameter)
	{
	  ObjcParameter objc = _adjustment[0] as ObjcParameter;
	  
	  _hue = (int) objc.GetValueAsLong("H");
	  _saturation = (int) objc.GetValueAsLong("Strt");
	  _lightness = (int) objc.GetValueAsLong("Lght");
	}
      return this;
    }

    override public bool Execute()
    {
      ActiveDrawable.HueSaturation(HueRange.All, (double) _hue, 
				   (double) _lightness,
				   (double) _saturation);
      return true;
    }
  }
}
