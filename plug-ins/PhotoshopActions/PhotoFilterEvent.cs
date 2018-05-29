// The PhotoshopActions plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// PhotoFilterEvent.cs
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
  public class PhotoFilterEvent : ActionEvent
  {
    [Parameter("Clr")]
    ObjcParameter _color;
    [Parameter("Dnst")]
    int _density;
    [Parameter("PrsL")]
    bool _preserveLuminosity;

    public override bool IsExecutable => false;

    protected override IEnumerable ListParameters()
    {
      // Fix me: output color
      yield return "Filter Color";
      yield return Format(_density, "Dnst");
      yield return Format(_preserveLuminosity, "PrsL");
    }

    override public bool Execute() => true;
  }
}
