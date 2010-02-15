// The PhotoshopActions plug-in
// Copyright (C) 2006-2010 Maurits Rijk
//
// TrimEvent.cs
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
  public class TrimEvent : ActionEvent
  {
    [Parameter("trimBasedOn")]
    EnumParameter _trimBasedOn;
    [Parameter("Top")]
    bool _top;
    [Parameter("Btom")]
    bool _bottom;
    [Parameter("Left")]
    bool _left;
    [Parameter("Rght")]
    bool _right;

    public override bool IsExecutable
    {
      get {return _top && _bottom && _left && _right;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_trimBasedOn, "trimBasedOn");
      yield return Format(_top, "Trim Top");
      yield return Format(_bottom, "Trim Bottom");
      yield return Format(_left, "Trim Left");
      yield return Format(_right, "Trim Right");
    }

    override public bool Execute()
    {
      // Fix me: only works if we want to trim all edges based on upper left
      // color!
      RunProcedure("plug_in_autocrop");
      return true;
    }
  }
}
