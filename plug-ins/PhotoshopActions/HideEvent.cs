// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// HideEvent.cs
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
  public class HideEvent : ActionEvent
  {
    [Parameter("null")]
    ListParameter _list;

    readonly bool _executable;

    public HideEvent()
    {
    }

    public HideEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable => _executable;

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      var obj = _list.Set[0] as ReferenceParameter;

      if (obj.Set[0] is EnmrType)
	{
	  var enmr = obj.Set[0] as EnmrType;
	  
	  switch (enmr.Key)
	    {
	    case "Lyr":
	      return new HideLayerEvent(this);
	    case "Chnl":
	      return new HideChannelEvent(this, enmr.Value);
	    default:
	      Console.WriteLine("Can't hide " + enmr.Key);
	      break;
	    }
	}
      else if (obj.Set[0] is NameType)
	{
	  var name = obj.Set[0] as NameType;
	  
	  switch (name.ClassID2)
	    {
	    case "Chnl":
	      return new HideChannelEvent(this, name.Key);
	    case "Lyr":
	      return new HideLayerEvent(this, name.Key);
	    default:
	      Console.WriteLine("Can't hide " + name.ClassID2);
	      break;
	    }
	}
      else if (obj.Set[0] is PropertyType)
	{
	  var property = obj.Set[0] as PropertyType;
	  switch (property.ClassID2)
	    {
	    case "Lyr":
	      return new HideLayerEvent(this, property);
	    default:
	      Console.WriteLine("Can't hide " + property.Key);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("HideEvent: " + obj.Set[0]);
	}
      return this;
    }
  }
}
