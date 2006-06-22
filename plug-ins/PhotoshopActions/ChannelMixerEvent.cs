// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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
using Gtk;

namespace Gimp.PhotoshopActions
{
  public class ChannelMixerEvent : ActionEvent
  {
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
      _r = (_red.Parameters["Rd"] as DoubleParameter).Value;
      _g = (_green.Parameters["Grn"] as DoubleParameter).Value;
      _b = (_blue.Parameters["Bl"] as DoubleParameter).Value;
      return this;
    }

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      store.AppendValues(iter, "Red: " + _r + " %");
      store.AppendValues(iter, "Green: " + _g + " %");
      store.AppendValues(iter, "Blue: " + _b + " %");
    }

    override public bool Execute()
    {
      RunProcedure("plug_in_colors_channel_mixer", 0,
		   _r / 100, 0.0, 0.0,
		   0.0, _g / 100, 0.0,
		   0.0, 0.0, _b / 100);
      return true;
    }
  }
}
