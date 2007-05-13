// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
//
// PhotocopyEvent.cs
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
  public class PhotocopyEvent : ActionEvent
  {
    [Parameter("GEfk")]
    EnumParameter _gefk;
    [Parameter("Dtl")]
    int _detail;
    [Parameter("Drkn")]
    int _darkness;

    protected override IEnumerable ListParameters()
    {
      if (_gefk != null)
	{
	  yield return "Effect: " + Abbreviations.Get(_gefk.Value);
	}
      yield return "Detail: " + _detail;
      yield return "Darkness: " + _darkness;
    }

    override public bool Execute()
    {
      // TODO: fine-tune these parameters
      double maskRadius = 8.5;
      double sharpness = _detail / 24.0;
      double pctBlack = 0;
      double pctWhite = (50.0 - _darkness) / 50.0;
      RunProcedure("plug_in_photocopy", maskRadius, sharpness, pctBlack,
		   pctWhite);
      return true;
    }
  }
}
