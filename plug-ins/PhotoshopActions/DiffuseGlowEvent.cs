// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DiffuseGlowEvent.cs
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

using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class DiffuseGlowEvent : ActionEvent
  {
    [Parameter("Grns")]
    int _graininess;
    [Parameter("GlwA")]
    int _glowAmount;
    [Parameter("ClrA")]
    int _clearAmount;

    protected override IEnumerable ListParameters()
    {
      yield return "Graininess: " + _graininess;
      yield return "Glow Amount: " + _glowAmount;
      yield return "Clear Amount: " + _clearAmount;
    }

    override public bool Execute()
    {
      // TODO: check these parameters!
      RunProcedure("plug_in_softglow", (double) _glowAmount, 
		   _clearAmount / 100.0, _graininess / 100.0);
      return true;
    }
  }
}
