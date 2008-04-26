// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// ChannelMixerEvent.cs
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
  public class ChannelMixerEvent : ActionEvent
  {
    [Parameter("Mnch")]
    bool _monochrome;
    [Parameter("Gry")]
    ObjcParameter _grey;
    [Parameter("Rd")]
    ObjcParameter _red;
    [Parameter("Grn")]
    ObjcParameter _green;
    [Parameter("Bl")]
    ObjcParameter _blue;

    double _r, _g, _b;

    protected override IEnumerable ListParameters()
    {
      yield return Format(_monochrome, "Mnch");
      yield return "Red: " + _r + " %";
      yield return "Green: " + _g + " %";
      yield return "Blue: " + _b + " %";
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      if (_monochrome)
	{
	  DoubleParameter red, green, blue;

	  red = _grey.Parameters["Rd"] as DoubleParameter;
	  green = _grey.Parameters["Grn"] as DoubleParameter;
	  blue = _grey.Parameters["Bl"] as DoubleParameter;

	  if (red != null)
	    _r = red.Value;
	  if (green != null)
	    _g = green.Value;
	  if (blue != null)
	    _b = blue.Value;     
	}
      else
	{
	  DoubleParameter red, green, blue;

	  red = _red.Parameters["Rd"] as DoubleParameter;
	  green = _green.Parameters["Grn"] as DoubleParameter;
	  blue = _blue.Parameters["Bl"] as DoubleParameter;

	  if (red != null)
	    _r = red.Value;
	  if (green != null)
	    _g = green.Value;
	  if (blue != null)
	    _b = blue.Value;     
	}


      return this;
    }

    override public bool Execute()
    {
      double r = _r / 100;
      double g = _g / 100;
      double b = _b / 100;

      if (_monochrome)
	{
	  RunProcedure("plug_in_colors_channel_mixer", _monochrome,
		       r, g, b,
		       r, g, b,
		       r, g, b);
	}
      else
	{
	  RunProcedure("plug_in_colors_channel_mixer", _monochrome,
		       r, 0.0, 0.0,
		       0.0, g, 0.0,
		       0.0, 0.0, b);
	}
      return true;
    }
  }
}
