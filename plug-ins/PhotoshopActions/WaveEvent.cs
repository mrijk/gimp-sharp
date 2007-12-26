// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// WaveEvent.cs
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
  public class WaveEvent : ActionEvent
  {
    [Parameter("Wvtp")]
    EnumParameter _waveType;
    [Parameter("NmbG")]
    int _numberOfGenerators;
    [Parameter("WLMn")]
    int _wavelengthMin;
    [Parameter("WLMx")]
    int _wavelengthMax;
    [Parameter("AmMn")]
    int _amplitudeMin;
    [Parameter("AmMx")]
    int _amplitudeMax;
    [Parameter("SclH")]
    int _scaleHorizontal;
    [Parameter("SclV")]
    int _scaleVertical;
    [Parameter("UndA")]
    EnumParameter _undefinedArea;
    [Parameter("RndS")]
    int _randomSeed;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_waveType, "Wvtp");
      yield return Format(_numberOfGenerators, "NmbG");
      yield return Format(_wavelengthMin, "WLMn");
      yield return Format(_wavelengthMax, "WLMx");
      yield return Format(_amplitudeMin, "AmMn");
      yield return Format(_amplitudeMax, "AmMx");
      yield return Format(_scaleHorizontal, "SclH");
      yield return Format(_scaleVertical, "SclV");
      yield return Format(_undefinedArea, "UndA");
      yield return Format(_randomSeed, "RndS");
    }

    override public bool Execute()
    {
      return false;
    }
  }
}
