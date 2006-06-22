// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// HueSaturationEvent.cs
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

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class HueSaturationEvent : ActionEvent
  {
    bool _colorization;
    int _hue, _saturation, _lightness;

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      string with = _colorization ? "With" : "Without";
      store.AppendValues(iter, with + " Colorize");
      store.AppendValues(iter, "Hue: " + _hue);
      store.AppendValues(iter, "Saturation: " + _saturation);
      store.AppendValues(iter, "Lightness: " + _lightness);
    }
    
    override public ActionEvent Parse(ActionParser parser)
    {
      // 1: colorization

      _colorization = parser.ParseBool("Clrz");

      // 2: adjstments

      parser.ParseToken("Adjs");
      parser.ParseFourByteString("VlLs");

      int numberOfItems = parser.ReadInt32();

      parser.ParseFourByteString("Objc");
      string classID = parser.ReadUnicodeString();
      string classID2 = parser.ReadTokenOrString();

      numberOfItems = parser.ReadInt32();

      _hue = parser.ReadLong("H");
      _saturation = parser.ReadLong("Strt");
      _lightness = parser.ReadLong("Lght");

      return this;
    }

    override public bool Execute()
    {
      if (ActiveDrawable == null)
	{
	  Console.WriteLine("Please open image first");
	  return false;
	}

      ActiveDrawable.HueSaturation(HueRange.All, (double) _hue, 
				   (double) _saturation,
				   (double) _lightness);
      return true;
    }
  }
}
