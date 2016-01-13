// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// PaletteKnifeEvent.cs
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
  public class PaletteKnifeEvent : ActionEvent
  {
    [Parameter("StrS")]
    int _strokeSize;
    [Parameter("StDt")]
    int _strokeDetail;
    [Parameter("Sftn")]
    int _softness;

    public override bool IsExecutable => false;

    protected override IEnumerable ListParameters()
    {
      yield return Format(_strokeSize, "StrS");
      yield return Format(_strokeDetail, "StDt");
      yield return Format(_softness, "Sftn");
    }

    override public bool Execute() => false;
  }
}
