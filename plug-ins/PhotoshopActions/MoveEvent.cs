// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// MoveEvent.cs
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
  public class MoveEvent : ActionEvent
  {
    [Parameter("null")]
    Parameter _obj;
    [Parameter("T")]
    Parameter _type;

    readonly bool _executable;

    public MoveEvent()
    {
    }

    public MoveEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable => _executable;

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_type != null)
	{
	  if (_type is ReferenceParameter)
	    {
	      ReferenceParameter type = _type as ReferenceParameter;
	      
	      if (type.Set[0] is EnmrType)
		{
		  var enmr = type.Set[0] as EnmrType;
		  
		  switch (enmr.Key)
		    {
		    case "Lyr":
		      return new MoveLayerEvent(this, enmr.Value);
		    default:
		      Console.WriteLine("MoveEvent, unknown key: " + enmr.Key);
		      break;
		    }
		}
	      else if (type.Set[0] is IndexType)
		{
		  var index = type.Set[0] as IndexType;
		  switch (index.Key)
		    {
		    case "Lyr":
		      return new MoveLayerByIndexEvent(this, index.Index);
		    default:
		      Console.WriteLine("MoveEvent, unknown key: " + 
					index.Key);
		      break;
		    }
		}
	    }
	  else if (_type is ObjcParameter)
	    {
	      var type = _type as ObjcParameter;

	      if (type.ClassID2 == "Ofst")
		{
		  return new MoveOffsetEvent(this, type);
		}
	      else
		{
		  Console.WriteLine("MoveEvent-1: " + type.ClassID2);
		}
	    }
	  else
	    {
	      Console.WriteLine("MoveEvent-2");
	    }
	}
      else
	{
	  Console.WriteLine("MoveEvent-3");
	}
      return myEvent;
    }
  }
}
