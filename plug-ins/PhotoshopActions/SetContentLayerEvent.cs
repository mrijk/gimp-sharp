// The PhotoshopActions plug-in
// Copyright (C) 2006-2013 Maurits Rijk
//
// SetContentLayerEvent.cs
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
  public class SetContentLayerEvent : SetEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;
    [Parameter("Clr")]
    ObjcParameter _color;

    public SetContentLayerEvent(SetEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
      _objc.Fill(this);
    }

    public override bool IsExecutable
    {
      get {return false;}
    }

    public override string EventForDisplay
    {
      get {return base.EventForDisplay + " current fill layer";}
    }

    protected override IEnumerable ListParameters()
    {
      // Fix me: contentlayer can also be a gradient layer and not only a fill layer!!!
      if (_color != null) 
	{
	  RGB rgb = _color.GetColor();
	  yield return "Slot Color: RGB color";
	  yield return String.Format("Red: {0:F3}", rgb.R * 255);
	  yield return String.Format("Green: {0:F3}", rgb.G * 255);
	  yield return String.Format("Blue: {0:F3}", rgb.B * 255);
	}
    }

    override public bool Execute()
    {
      return true;
    }
  }
}
