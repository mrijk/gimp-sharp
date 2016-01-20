// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// NewChannelEvent.cs
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
  public class NewChannelEvent : MakeEvent
  {
    [Parameter("ClrI")]
    EnumParameter _colorIndicates;
    [Parameter("Clr")]
    ObjcParameter _objc;
    [Parameter("Opct")]
    int _opacity;

    public NewChannelEvent(MakeEvent srcEvent, ObjcParameter myObject) : 
      base(srcEvent)
    {
      myObject.Fill(this);
    }

    protected override IEnumerable ListParameters()
    {
      RGB rgb = _objc.GetColor();
      byte red, green, blue;
      rgb.GetUchar(out red, out green, out blue);

      yield return "New: channel";
      yield return "Color Indicates: " + _colorIndicates;
      yield return "Color:";
      yield return "Red: " + red;
      yield return "Green: " + green;
      yield return "Blue: " + blue;
      yield return "Opacity: " + _opacity;
    }

    override public bool Execute()
    {
      RGB rgb = _objc.GetColor();
      var channel = new Channel(ActiveImage, "Alpha 1", 
				ActiveImage.Width, ActiveImage.Height,
				_opacity, rgb);
      ActiveImage.AddChannel(channel, -1);
      return true;
    }
  }
}
