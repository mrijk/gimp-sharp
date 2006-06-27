// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SetEvent.cs
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
  public class SetEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    public override bool IsExecutable
    {
      get {return false;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      if (_obj != null)
	{
	  if (_obj.Set[0] is PropertyType)
	    {
	      PropertyType property = _obj.Set[0] as PropertyType;

	      switch (property.ClassID2)
		{
		case "Clr":
		  switch (property.Key)
		    {
		    case "BckC":
		      return new SetBackgroundColorEvent(this);
		      break;
		    case "FrgC":
		      return new SetForegroundColorEvent(this);
		      break;
		    default:
		      break;
		    }
		  break;
		case "Chnl":
		  if (property.Key == "fsel")
		    {
		      return new SelectionEvent(this);
		    }
		  break;
		case "Lyr":
		  return new SetLayerPropertyEvent(this);
		  break;
		default:
		  Console.WriteLine("SetEvent.Parse: " + property.ClassID2);
		  break;
		}
	    }
	  else if (_obj.Set[0] is EnmrType)
	    {
	      EnmrType enmr = _obj.Set[0] as EnmrType;
	      switch (enmr.Key)
		{
		case "Chnl":
		  return new SetChannelPropertyEvent(this);
		  break;
		case "Lyr":
		  return new SetLayerPropertyEvent(this);
		  break;
		default:
		  Console.WriteLine("SetEvent.Parse: unknown key " + enmr.Key);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("SetEvent.Parse: {0} unknown type",
				_obj.Set[0]);
	    }
	}

      return this;
    }
  }
}
