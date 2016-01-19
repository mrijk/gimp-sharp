// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// AddEvent.cs
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
  public class AddEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;
    
    string _name;

    public override string EventForDisplay
    {
      get 
	{
	  if (_obj.Set[0] is NameType)
	    {
	      _name = (_obj.Set[0] as NameType).Key;
	      return base.EventForDisplay + " channel \"" + _name + "\"";
	    }
	  else if (_obj.Set[0] is EnmrType)
	    {
	      _name = Abbreviations.Get((_obj.Set[0] as EnmrType).Value);
	      return base.EventForDisplay + " " + _name + " channel";
	    }
	  return base.EventForDisplay;
	}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "To: Selection";
    }

    override public bool Execute()
    {
      Console.WriteLine("Count: " + ActiveImage.Channels.Count);

      // Channel channel = ActiveImage.Channels[_name];

      Channel channel = new Channel(ActiveImage, ChannelType.Green, "Green");
      ActiveImage.AddChannel(channel, 0);

      ActiveImage.Selection.Combine(channel, ChannelOps.Add);
      return true;
    }
  }
}
