// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// RoughPastelsEvent.cs
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
  public class RoughPastelsEvent : ActionEvent
  {
    [Parameter("StrL")]
    int _strokeLength;
    [Parameter("StDt")]
    int _strokeDetail;
    // [Parameter("TxtT")]
    EnumParameter _textureType;
    [Parameter("Scln")]
    int _scaling;
    [Parameter("Rlf")]
    int _relief;
    [Parameter("InvT")]
    bool _invertTexture;
    [Parameter("LghD")]
    EnumParameter _lightDirection;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return Format(_strokeLength, "StrL");
      yield return Format(_strokeDetail, "StDt");
      // yield return Format(_textureType, "TxtT");
      yield return "Texture type: fix me!";
      yield return Format(_scaling, "Scln");
      yield return Format(_relief, "Rlf");
      yield return Format(_invertTexture, "InvT");
      yield return Format(_lightDirection, "LghD");
    }

    override public bool Execute()
    {
      return true;
    }
  }
}
