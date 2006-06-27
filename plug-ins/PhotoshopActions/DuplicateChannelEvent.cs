// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// DuplicateChannelEvent.cs
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
  public class DuplicateChannelEvent : ActionEvent
  {
    string _name;

    public DuplicateChannelEvent(ActionEvent srcEvent, string name) : 
      base(srcEvent) 
    {
      _name = name;
    }

    public override bool IsExecutable
    {
      get 
	{
	  return Gimp.Version > new Version("2.3.0");
	}
    }

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      store.AppendValues(iter, "Channel: " + _name);
    }

    override public bool Execute()
    {
      Channel channel = null;

      switch (_name)
	{
	case "Rd":
	  channel = new Channel(ActiveImage, ChannelType.Red, "Red copy");
	  break;
	case "Grn":
	  channel = new Channel(ActiveImage, ChannelType.Green, "Green copy");
	  break;
	case "Bl":
	  channel = new Channel(ActiveImage, ChannelType.Blue, "Blue copy");
	  break;	  
	default:
	  Console.WriteLine("DuplicateChannel: " + _name);
	  break;
	}
      if (channel != null)
	{
	  ActiveImage.AddChannel(channel, -1);
	}

      return true;
    }
  }
}