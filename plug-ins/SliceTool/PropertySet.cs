// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public string this[string name]
    {
      get {return GetProperty(name).Value;} 
      set {GetProperty(name).Value = value;}
    }

    Property GetProperty(string name)
    {
      var property = _set[name];
      return property;
    }

    public bool Exists(string name)
    {
      return !String.IsNullOrEmpty(this[name]);
    }

    public void WriteHTML(StreamWriter w, string name)
    {
      GetProperty(name).WriteHTML(w);
    }

    public bool Changed
    {
      get {return _set.Any(kvp => kvp.Value.Changed);}

      set {_set.Values.ToList().ForEach(property => property.Changed = value);}
    }
  }
}
