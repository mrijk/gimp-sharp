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
      _map["BckC"] = "background color";
      _map["Bckg"] = "background";
      _map["Blck"] = "black";
      _map["Cntr"] = "center";
      _map["CrnH"] = "Current History State";
      _map["FllD"] = "full document";
      _map["FrgC"] = "foreground color";
      _map["Gsn"]  = "gaussian";
      _map["Nrml"] = "normal";
      _map["Otsd"] = "outside";
      _map["Trns"] = "transparent";
      _map["Wrp"]  = "wrap";
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

