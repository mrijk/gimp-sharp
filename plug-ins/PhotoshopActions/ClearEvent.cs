// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// ClearEvent.cs
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
  public class ClearEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public ClearEvent()
    {
    }

    public ClearEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable
    {
      get {return _executable;}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      base.Parse(parser);

      if (_obj != null)
	{
	  if (_obj.Set[0] is PropertyType)
	    {
	      PropertyType property = _obj.Set[0] as PropertyType;

	      switch (property.ClassID2)
		{
		case "Prpr":
		  switch (property.Key)
		    {
		    case "QucM":
		      return new ClearQuickMaskEvent(this);
		    default:
		      Console.WriteLine("ClearEvent.Prpr: " + property.Key);
		      break;
		    }
		  break;
		default:
		  Console.WriteLine("ClearEvent.Parse: " + property.ClassID2);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("ClearEvent.Parse: {0} unknown type",
				_obj.Set[0]);
	    }
	}
      return this;
    }
  }
}
