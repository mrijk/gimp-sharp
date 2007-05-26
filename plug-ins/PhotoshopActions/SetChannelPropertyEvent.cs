// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// SetChannelPropertyEvent.cs
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
  public class SetChannelPropertyEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    public SetChannelPropertyEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
    }

    public override string EventForDisplay
    {
      get 
	{
	  return base.EventForDisplay + " current channel";
	}
    }

    protected override IEnumerable ListParameters()
    {
      foreach (Parameter parameter in _objc.Parameters)
	{
	  switch (parameter.Name)
	    {
	    case "Nm":
	      string name = (parameter as TextParameter).Value;
	      yield return Format(name, "Nm");
	      break;
	    default:
	      Console.WriteLine("SetChannelProperty: " + parameter.Name);
	      break;
	    }
	}
    }

    override public bool Execute()
    {
      Channel channel = ActiveImage.ActiveChannel;
      foreach (Parameter parameter in _objc.Parameters)
	{
	  switch (parameter.Name)
	    {
	    case "Nm":
	      channel.Name = (parameter as TextParameter).Value;
	      break;
	    default:
	      Console.WriteLine("SetChannelPropertyEvent: " + parameter.Name);
	      break;
	    }
	}
      return true;
    }
  }
}
