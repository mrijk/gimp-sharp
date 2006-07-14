// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SelectEvent.cs
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
  public class SelectEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public SelectEvent()
    {
    }

    public SelectEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable
    {
      get {return _executable;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_obj != null)
	{
	  ReferenceType parameter = _obj.Set[0];

	  if (parameter is NameType)
	    {
	      NameType name = parameter as NameType;
	      if (name.ClassID2 == "Lyr")
		{
		  if (Parameters.Count > 2)
		    {
		      // TODO: implement multiple selection
		      Console.WriteLine("SelectEvent: multiple selection");
		    }

		  return new SelectLayerByNameEvent(this, name.Key);
		}
	      else
		{
		  Console.WriteLine("SelectEvent: " + name.ClassID2);
		}
	    }
	  else if (parameter is PropertyType)
	    {
	      PropertyType property = parameter as PropertyType;
	      if (property.Key == "Bckg")
		{
		  return new SelectLayerByNameEvent(this, "Background");
		}
	      else
		{
		  Console.WriteLine("Property: " + property.Key);
		}
	    }
	  else if (parameter is EnmrType)
	    {
	      EnmrType enmr = parameter as EnmrType;
	      switch (enmr.Key)
		{
		case "Chnl":
		  return new SelectChannelEvent(this, enmr.Value);
		case "Lyr":
		  return new SelectLayerEvent(this, enmr.Value);
		default:
		  Console.WriteLine("SelectEvent.Enmr: " + enmr.Key);
		  break;
		}
	    }
	  else if (parameter is ReleType)
	    {
	      ReleType rele = parameter as ReleType;
	      switch (rele.ClassID2)
		{
		case "Dcmn":
		  return new SelectDocumentEvent(this, rele.Offset);
		default:
		  Console.WriteLine("rele.ClassID2: " + rele.ClassID2);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("SelectEvent-1: " + parameter);
	    }
	}

      return myEvent;
    }
  }
}
