// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// VariationsEvent.cs
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
  public class VariationsEvent : ActionEvent
  {
    [Parameter("RdGm")]
    double _redGamma;
    [Parameter("GrnG")]
    double _greenGamma;
    [Parameter("BlGm")]
    double _blueGamma;
    [Parameter("RdWh")]
    int _redWhitePoint;
    [Parameter("GrnW")]
    int _greenWhitePoint;
    [Parameter("BlWh")]
    int _blueWhitePoint;
    [Parameter("RdBl")]
    int _redBlackPoint;
    [Parameter("GrnB")]
    int _greenBlackPoint;
    [Parameter("BlBl")]
    int _blueBlackPoint;
    [Parameter("Strt")]
    int _saturation;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return String.Format("Red Gamma: {0:F3}", _redGamma);
      yield return String.Format("Green Gamma: {0:F3}", _greenGamma);
      yield return String.Format("Blue Gamma: {0:F3}", _blueGamma);
      yield return "Red White Point: " + _redWhitePoint;
      yield return "Green White Point: " + _greenWhitePoint;
      yield return "Blue White Point: " + _blueWhitePoint;
      yield return "Red Black Point: " + _redBlackPoint;
      yield return "Green Black Point: " + _greenBlackPoint;
      yield return "Blue Black Point: " + _blueBlackPoint;
      yield return "Saturation: " + _saturation;
    }

    override public bool Execute()
    {
      // TODO: implement this. Could be handled by the FilterPack plug-in,
      // but that one can't be run non-interactively :(
      return false;
    }
  }
}
