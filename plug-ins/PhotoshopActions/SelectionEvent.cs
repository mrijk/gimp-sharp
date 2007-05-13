// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// SelectionEvent.cs
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
  public class SelectionEvent : SetEvent
  {
    [Parameter("T")]
    Parameter _parameter;

    readonly bool _executable;

    public SelectionEvent()
    {
    }

    public SelectionEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _executable = true;
    }

    public override bool IsExecutable
    {
      get {return _executable;}
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Selection";}
    }

    override public ActionEvent Parse(ActionParser parser)
    {
      if (_parameter is ObjcParameter)
	{
	  ObjcParameter objc = _parameter as ObjcParameter;
	  string classID2 = objc.ClassID2;

	  switch (classID2)
	    {
	    case "Elps":
	      return new SelectEllipseEvent(this, objc);
	    case "Plgn":
	      return new SelectPolygonEvent(this, objc);
	    case "Rctn":
	      return new SelectRectangleEvent(this, objc);
	    default:
	      Console.WriteLine("SelectionEvent Implement " + classID2);
	      break;
	    }
	}
      else if (_parameter is EnumParameter)
	{
	  string type = (_parameter as EnumParameter).Value;

	  switch (type)
	    {
	    case "Al":
	      return new SelectAllEvent(this);
	    case "None":
	      return new SelectNoneEvent(this);
	    default:
	      Console.WriteLine("SelectionEvent-1: " + type);
	      break;
	    }
	}
      else if (_parameter is ReferenceParameter)
	{
	  ReferenceParameter enmr = _parameter as ReferenceParameter;

	  if (enmr.Set[0] is EnmrType)
	    {
	      EnmrType type = enmr.Set[0] as EnmrType;
	      switch (type.Key)
		{
		case "Chnl":
		  return new SelectionFromChannelEvent(this, type.Value);
		  break;
		default:
		  Console.WriteLine("SelectionEvent-3: " + type.Key);
		  break;
		}
	    }
	  else
	    {
	      Console.WriteLine("SelectionEvent-4: " + enmr.Set[0]);
	    }
	}
      else
	{
	  Console.WriteLine("SelectionEvent-2: " + _parameter);
	}
      return this;
    }
  }
}
