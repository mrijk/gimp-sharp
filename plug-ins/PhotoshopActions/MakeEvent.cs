// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// MakeEvent.cs
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
  public class MakeEvent : ActionEvent
  {
    [Parameter("Nw")]
    Parameter _object;
    [Parameter("null")]
    ReferenceParameter _obj;

    public override bool IsExecutable
    {
      get {return false;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);
      string classID = null;

      if (_object != null)
	{
	  if (_object is ObjcParameter)
	    {
	      classID = (_object as ObjcParameter).ClassID2;
	    }
	}
      else if (_obj != null)
	{
	  ClassType classType = _obj.Set[0] as ClassType;
	  switch (classType.ClassID2)
	    {
	    case "AdjL":
	      return new AddAdjustmentLayerEvent(this);
	      break;
	    case "Lyr":
	      return new AddLayerEvent(this, _obj.Set);
	      break;
	    case "TxLr":
	      return new AddTextLayerEvent(this);
	      break;
	    default:
	      Console.WriteLine("MakeEvent: {0} not implemented", 
				classType.ClassID2);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("Disaster!");
	}

      switch (classID)
	{
	case "Dcmn":
	  return new NewDocumentEvent(this, _object as ObjcParameter);
	  break;
	case "Gd":
	  return new AddGuideEvent(this, _object as ObjcParameter);
	  break;
	default:
	  Console.WriteLine("MakeEvent: {0} not implemented", classID);
	  break;
	}
      return myEvent;
    }
  }
}
