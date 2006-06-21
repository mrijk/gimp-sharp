// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// AddNoiseEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class AddNoiseEvent : ActionEvent
  {
    [Parameter("Dstr")]
    EnumParameter _distribution;
    [Parameter("Nose")]
    double _noise;
    [Parameter("Mnch")]
    bool _monochrome;

    override public bool Execute()
    {
      if (ActiveImage == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      double noise = _noise / 100;
      int independant = (_monochrome) ? 0 : 1;
      RunProcedure("plug_in_rgb_noise", independant, 0, noise, noise, noise, 
		   1.0);

      return true;
    }
  }
}
