// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
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

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      if (_monochrome)
	{
	  var red = _grey.Parameters["Rd"] as DoubleParameter;
	  var green = _grey.Parameters["Grn"] as DoubleParameter;
	  var blue = _grey.Parameters["Bl"] as DoubleParameter;

	  _r = red?.Value ?? 0;
	  _g = green?.Value ?? 0;
	  _b = blue?.Value ?? 0;
	}
      else
	{
	  var red = _red.Parameters["Rd"] as DoubleParameter;
	  var green = _green.Parameters["Grn"] as DoubleParameter;
	  var blue = _blue.Parameters["Bl"] as DoubleParameter;

	  _r = red?.Value ?? 0;
	  _g = green?.Value ?? 0;
	  _b = blue?.Value ?? 0;
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
