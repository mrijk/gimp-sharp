// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// SelectChannelEvent.cs
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
  public class SelectChannelEvent : ActionEvent
  {
    string _name;

    public SelectChannelEvent(ActionEvent srcEvent, string name) : 
      base(srcEvent)
    {
      _name = name;
    }

    public override string EventForDisplay
    {
      get 
	{
	  var channel = ActiveImage.Channels[_name];
	  if (channel == null)	// Default channel
	    {
	      return base.EventForDisplay + " " + Abbreviations.Get(_name) + 
		" channel";
	    }
	  else
	    {
	      return base.EventForDisplay + " channel \"" + _name + "\"";
	    }
	}
    }

    override public bool Execute()
    {
      var channel = ActiveImage.Channels[_name];
      if (channel == null)	// Default channel
	{
	  SelectedChannelName = _name;
	  SelectedChannel = null;
	}
      else
	{
	  SelectedChannel = channel;
	  Console.WriteLine("SelectChannelEvent: " + _name);
	}

      return true;
    }
  }
}
