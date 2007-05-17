// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// ColorPaletteEvent.cs
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
  public class ColorPaletteEvent : ActionEvent
  {
    [Parameter("Pncl")]
    int _pencilWidth;
    [Parameter("StrP")]
    int _strokePressure;
    [Parameter("PprB")]
    int _paperBrightness;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_pencilWidth, "Pncl");
      yield return Format(_strokePressure, "StrP");
      yield return Format(_paperBrightness, "PprB");
    }

    override public bool Execute()
    {
      return false;
    }
  }
}
