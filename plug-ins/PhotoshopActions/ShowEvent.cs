// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ShowEvent.cs
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
  public class ShowEvent : ActionEvent
  {
    [Parameter("null")]
    ListParameter _list;

    public override bool IsExecutable
    {
      get 
	{
	  return false;
	}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      ReferenceParameter obj = _list.Set[0] as ReferenceParameter;

      if (obj.Set[0] is NameType)
	{
	  NameType name = obj.Set[0] as NameType;
	  
	  switch (name.ClassID2)
	    {
	    case "Lyr":
	      return new ShowLayerEvent(this, name.Key);
	      break;
	    case "Chnl":
	      return new ShowChannelEvent(name.Key);
	      break;
	    default:
	      Console.WriteLine("Can't show " + name.ClassID2);
	      break;
	    }
	}
      else if (obj.Set[0] is EnmrType)
	{
	  EnmrType enmr = obj.Set[0] as EnmrType;

	  switch (enmr.Key)
	    {
	    case "Lyr":
	      return new ShowLayerEvent(this);
	      break;
	    default:
	      Console.WriteLine("Can't show " + enmr.Key);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("ShowEvent: " + obj.Set[0]);
	}
      return this;
    }
  }
}
