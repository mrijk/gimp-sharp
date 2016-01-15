// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// LightingEffectsEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class LightingEffectsEvent : ActionEvent
  {
    [Parameter("CrnL")]
    int _currentLight;
    [Parameter("Glos")]
    int _gloss;
    [Parameter("Mtrl")]
    int _material;
    [Parameter("Exps")]
    int _exposure;
    [Parameter("AmbB")]
    int _ambience;
    [Parameter("WhHi")]
    bool _whiteIsHigh;
    [Parameter("BmpA")]
    int _height;
    [Parameter("FrmW")]
    double _frameWidth;

    public override bool IsExecutable => false;

    override public bool Execute() => false;
  }
}
