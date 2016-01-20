// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// NeonGlowEvent.cs
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
  public class NeonGlowEvent : ActionEvent
  {
    [Parameter("Sz")]
    int _size;
    [Parameter("Brgh")]
    int _brightness;
    [Parameter("Clr")]
    ObjcParameter _color;

    public override bool IsExecutable => false;

    protected override IEnumerable ListParameters()
    {
      yield return Format(_size, "Sz");
      yield return Format(_brightness, "Brgh");
      yield return Format(_color.ClassID2, "Clr");
    }

    override public bool Execute() => true;
  }
}
