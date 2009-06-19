// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// PropertySet.cs
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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Gimp.SliceTool
{
  public class PropertySet
  {
    Dictionary<string, Property> _set = new Dictionary<string, Property>();

    public PropertySet()
    {
      Add("href");
      Add("AltText");
      Add("Target");

      AddJavaScript("MouseOver");
      AddJavaScript("MouseOut");
      AddJavaScript("MouseClick");
      AddJavaScript("MouseDoubleClick");
      AddJavaScript("MouseUp");
      AddJavaScript("MouseDown");
    }

    void Add(string name)
    {
      _set[name] = new Property(name);
    }

    void AddJavaScript(string name)
    {
      _set[name] = new JavaScriptProperty(name);
    }

    Property GetProperty(string name)
    {
      Property property = _set[name];
      Debug.Assert(property != null, "Property not in hashset!");
      return property;
    }

    public void Set(string name, string value)
    {
      GetProperty(name).Value = value;
    }

    public string Get(string name)
    {
      return GetProperty(name).Value;
    }

    public bool Exists(string name)
    {
      return Get(name).Length > 0;
    }

    public void WriteHTML(StreamWriter w, string name)
    {
      GetProperty(name).WriteHTML(w);
    }

    public bool Changed
    {
      get
	{
          foreach (Property property in _set.Values)
            {
	      if (property.Changed)
		{
		  return true;
		}
            }
          return false;
	}

      set
	{
          foreach (Property property in _set.Values)
            {
	      property.Changed = value;
            }
	}
    }
  }
}
