// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// AddGuideEvent.cs
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
  public class AddGuideEvent : ActionEvent
  {
    [Parameter("Pstn")]
    double _position;
    [Parameter("Ornt")]
    EnumParameter _orientation;

    string _units = "#Prc";	// Fix me!

    public AddGuideEvent(ActionEvent srcEvent, ObjcParameter myObject) 
      : base(srcEvent) 
    {
      myObject.Fill(this);
    }
    
    public override bool IsExecutable
    {
      get 
	{
	  return _units == "#Prc";
	}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "New: guide";
      yield return Format(_position, "Pstn");
      yield return Format(_orientation, "Ornt");
    }

    override public bool Execute()
    {
      Image image = ActiveImage;
      int position;

      if (image == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      if (_units != "#Prc")
	{
	  Console.WriteLine("Unit type {0} not supported", _units);
	  throw new GimpSharpException();
	}

      switch (_orientation.Value)
	{
	case "Vrtc":
	  position = (int) (_position * image.Width / 100);
	  new VerticalGuide(image, position);
	  break;
	case "Hrzn":
	  position = (int) (_position * image.Height / 100);
	  new HorizontalGuide(image, position);
	  break;
	default:
	  Console.WriteLine("AddGuideEvent: " + _orientation.Value);
	  break;
	}
      return true;
    }
  }
}
