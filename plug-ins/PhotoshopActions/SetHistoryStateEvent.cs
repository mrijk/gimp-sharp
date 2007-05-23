// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// SetHistoryStateEvent.cs
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
  public class SetHistoryStateEvent : SetEvent
  {
    public SetHistoryStateEvent(SetEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " Current History State";}
    }
 
    protected override IEnumerable ListParameters()
    {
      Parameter type = Parameters["T"];

      if (type is ReferenceParameter)
	{
	  ReferenceParameter reference = type as ReferenceParameter;
	  string name = (reference.Set[0] as NameType).Key;
	  yield return Format(name, "Nm");
	}
      else
	{
	  yield break;
	}
    }

    override public bool Execute()
    {
      // Not really implemented, but we can safely ignore this
      return true;
    }
  }
}
