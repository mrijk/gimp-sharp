// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DeselectEvent.cs
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
  public class DeselectEvent : ActionEvent
  {
    [Parameter("null")]
    ReferenceParameter _obj;

    readonly bool _executable;

    public DeselectEvent()
    {
    }

    public DeselectEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      _executable = true;
    }

    public override bool IsExecutable => _executable;

    override public ActionEvent Parse(ActionParser parser)
    {
      ActionEvent myEvent = base.Parse(parser);

      ReferenceType parameter = _obj.Set[0];

      if (parameter is ClassType)
	{
	  var type = parameter as ClassType;
	  switch (type.ClassID2)
	    {
	    case "Path":
	      return new DeselectPathEvent(this);
	    default:
	      Console.WriteLine("DeselectEvent.class: " + type.ClassID2);
	      break;
	    }
	}
      else
	{
	  Console.WriteLine("DeselectEvent: " + parameter);
	}

      return myEvent;
    }

    protected override IEnumerable ListParameters()
    {
      yield break;
    }

    override public bool Execute() => true;
  }
}
