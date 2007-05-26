// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// DeleteEvent.cs
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
  public class DeleteEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public DeleteEvent()
    {
    }

    public DeleteEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable
    {
      get {return _executable || !HasDescriptor;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_obj != null)
	{
	  if (_obj.Set[0] is EnmrType)
	    {
	      EnmrType type = _obj.Set[0] as EnmrType;
	      switch (type.Key)
		{
		case "Chnl":
		  return new DeleteChannelEvent(this);
		case "Gd":
		  return new DeleteGuideEvent(this, type.Value);
		case "Lyr":
		  return new DeleteLayerEvent(this);
		default:
		  Console.WriteLine("DeleteEvent-1: {0} unknown", type.Key);
		  break;
		}
	    }
	  else if (_obj.Set[0] is NameType)
	    {
	      NameType type = _obj.Set[0] as NameType;
	      switch (type.ClassID2)
		{
		case "Chnl":
		  return new DeleteChannelByNameEvent(this, type.Key);
		case "Lyr":
		  return new DeleteLayerEvent(this);
		default:
		  Console.WriteLine("DeleteEvent-2: {0} unknown", 
				    type.ClassID2);
		  break;
		}
	    }
	  else if (_obj.Set[0] is PropertyType)
	    {
	      PropertyType type = _obj.Set[0] as PropertyType;
	      switch (type.ClassID2)
		{
		case "Lyr":
		  return new DeleteLayerByNameEvent(this, type.Key);
		default:
		  Console.WriteLine("DeleteEvent-3: {0} unknown", 
				    type.ClassID2);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("DeleteEvent: " + _obj.Set[0]);
	    }
	}
      return myEvent;
    }

    override public bool Execute()
    {
      if (!HasDescriptor)
	{
	  ActiveDrawable.EditClear();
	}
      return true;
    }
  }
}
