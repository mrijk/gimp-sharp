// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// Abbreviations.cs
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
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  static class Abbreviations
  {
    static readonly Dictionary<string, string> _map = 
      new Dictionary<string, string>();

    static Abbreviations()
    {
      _map["AdBt"] = "bottom edges";
      _map["AdCH"] = "horizontal centers";
      _map["AdCV"] = "vertical centers";
      _map["BckC"] = "background color";
      _map["Bckg"] = "background";
      _map["Blck"] = "black";
      _map["BrbW"] = "brush wide blurry";
      _map["BrSm"] = "brush simple";
      _map["CBrn"] = "color burn";
      _map["Clr"]  = "color";
      _map["Cntr"] = "center";
      _map["CrnH"] = "Current History State";
      _map["Elps"] = "Ellipse";
      _map["FllD"] = "full document";
      _map["FrgC"] = "foreground color";
      _map["GlwE"] = "glowing edges";
      _map["GrnE"] = "grain enlarged";
      _map["GrnH"] = "grain horizontal";
      _map["GrnR"] = "grain regular";
      _map["Gsn"]  = "gaussian";
      _map["HrdL"] = "hard light";
      _map["Insd"] = "inside";
      _map["Lghn"] = "lighten";
      _map["Lmns"] = "luminosity";
      _map["LngL"] = "long lines";
      _map["Lnr"]  = "linear";
      _map["LngS"] = "long strokes";
      _map["Mltp"] = "multiply";
      _map["Nrml"] = "normal";
      _map["Otsd"] = "outside";
      _map["Ovrl"] = "overlay";
      _map["PntD"] = "paint daubs";
      _map["Prvs"] = "previous";
      _map["Rtcl"] = "reticulation";
      _map["SBMN"] = "smart blur mode normal";
      _map["SBQH"] = "smart blur quality high";
      _map["SBQH"] = "smart blur quality low";
      _map["SBQH"] = "smart blur quality medium";
      _map["Scrn"] = "screen";
      _map["SftL"] = "soft light";
      _map["Shdw"] = "shadow";
      _map["Sml"]  = "small";
      _map["Trns"] = "transparent";
      _map["Unfr"] = "uniform";
      _map["Wht"]  = "white";
      _map["Wrp"]  = "wrap";
      _map["WrpA"] = "wrap around";
      _map["WvSn"] = "wave sine";
      _map["WvSq"] = "wave square";
    }

    public static string Get(string key)
    {
      string fullString;

      if (_map.TryGetValue(key, out fullString))
	{
	  return fullString;
	}
      else
	{
	  return key;
	}
    }
  }
}

