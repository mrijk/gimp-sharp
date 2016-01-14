// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DarkStrokesEvent.cs
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
  public class DarkStrokesEvent : ActionEvent
  {
    [Parameter("Blnc")]
    int _balance;
    [Parameter("BlcI")]
    int _blackIntensity;
    [Parameter("WhtI")]
    int _whiteIntensity;

    public override bool IsExecutable => false;

    protected override IEnumerable ListParameters()
    {
      yield return "Balance: " + _balance;
      yield return "Black Intensity: " + _blackIntensity;
      yield return "White Intensity: " + _whiteIntensity;
    }

    override public bool Execute() => true;
  }
}
