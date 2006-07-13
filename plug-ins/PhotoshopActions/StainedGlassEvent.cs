// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// StainedGlassEventEvent.cs
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
  public class StainedGlassEvent : ActionEvent
  {
    [Parameter("ClSz")]
    int _cellSize;
    [Parameter("BrdT")]
    int _borderThickness;
    [Parameter("LghI")]
    int _lightIntensity;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Cell Size: " + _cellSize;
      yield return "Border Thickness: " + _borderThickness;
      yield return "Light Intensity: " + _lightIntensity;
    }

    override public bool Execute()
    {
      return false;
    }
  }
}
