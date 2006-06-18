// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// SetColorEvent.cs
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
  public class SetColorEvent : ActionEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;

    [Parameter("Rd")]
    double _red;
    [Parameter("Grn")]
    double _green;
    [Parameter("Bl")]
    double _blue;

    [Parameter("H")]
    double _hue;
    [Parameter("Strt")]
    double _saturation;
    [Parameter("Brgh")]
    double _brightness;

    public SetColorEvent(ActionEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _objc.Fill(this);
    }
    
    protected RGB Color
    {
      get 
	{
	  switch (_objc.ClassID2)
	    {
	    case "RGBC":
	      return new RGB(_red / 255.0, _green / 255.0, _blue / 255.0);
	      break;
	    case "HSBC":
	      return new RGB(new HSV(_hue, _saturation, _brightness));
	      break;
	    default:
	      Console.WriteLine("*** Color model {0} not supported", 
				_objc.ClassID2);
	      return null;
	      break;
	    }
	}
    }
  }
}
