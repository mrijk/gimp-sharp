// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// UnsharpMaskEvent.cs
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

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class UnsharpMaskEvent : ActionEvent
  {
    [Parameter("Amnt")]
    double _amount;
    [Parameter("Rds")]
    double _radius;
    [Parameter("Thsh")]
    double _threshold;

    protected override IEnumerable ListParameters()
    {
      yield return "Radius: " + _radius;
      yield return "Amount: " + _amount;
      yield return "Threshold: " + _threshold;
    }

    override public bool Execute()
    {
      if (ActiveImage == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      RunProcedure("plug_in_unsharp_mask", _radius, _amount, (int) _threshold);

      return true;
    }
  }
}
