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
    string _distribution;
    [Parameter("Nose")]
    double _noise;
    [Parameter("Mnch")]
    bool _monochrome;

    public AddNoiseEvent()
    {
    }

    public override bool IsExecutable
    {
      get 
	{
	  return _monochrome;
	}
    }
    
    override public ActionEvent Parse(ActionParser parser)
    {
      ParameterSet set = new ParameterSet();
      set.Parse(parser, NumberOfItems);

      _distribution = (set["Dstr"] as EnumParameter).Value;
      _noise = (set["Nose"] as DoubleParameter).Value;
      _monochrome = (set["Mnch"] as BoolParameter).Value;

      return this;
    }

    override public bool Execute()
    {
      if (Image == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      if (_monochrome)
	{
	  double noise = _noise / 100;
	  Procedure procedure = new Procedure("plug_in_rgb_noise");
	  procedure.Run(Image, Drawable, 0, 0, noise, noise, noise, 
			1.0);
	}
      else
	{
	  Console.WriteLine("AddNoiseEvent: implement non-monochromatic noise");
	  return false;
	}
      return true;
    }
  }
}
