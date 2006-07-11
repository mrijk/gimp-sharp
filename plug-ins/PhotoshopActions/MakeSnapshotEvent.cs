// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// MakeSnapshotEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class MakeSnapshotEvent : MakeEvent
  {
    public MakeSnapshotEvent(MakeEvent srcEvent) : base(srcEvent)
    {
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " snapshot";}
    }

    protected override IEnumerable ListParameters()
    {
      // Fix me: snapshots from other states possible?
      yield return "From: Current History State";
    }

    override public bool Execute()
    {
      // Dummy event. Probably not needed in GIMP because we have
      // unlimited UNDO
      return true;
    }
  }
}
