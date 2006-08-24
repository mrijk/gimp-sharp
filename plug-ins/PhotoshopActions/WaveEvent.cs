// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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
      yield return "Wave Type: " + Abbreviations.Get(_waveType.Value);
      yield return "Number of Generators: " + _numberOfGenerators;
      yield return "Wave length min: " + _wavelengthMin;
      yield return "Wave length max: " + _wavelengthMax;
      yield return "Amplitude min: " + _amplitudeMin;
      yield return "Amplitude min: " + _amplitudeMax;
      yield return "Scale horizontal: " + _scaleHorizontal;
      yield return "Scale vertical: " + _scaleVertical;
      yield return "Undefined Area: " + 
	Abbreviations.Get(_undefinedArea.Value);
      yield return "Random Seed: " + _randomSeed;
    }

    override public bool Execute()
    {
      return false;
    }
  }
}
