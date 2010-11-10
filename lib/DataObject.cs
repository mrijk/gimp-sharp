// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// DataObject.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

namespace Gimp
{
  public abstract class DataObject
  {
    string _name;

    protected DataObject(string name)
    {
      _name = name;
    }

    public string Name
    {
      get {return _name;}
      set {Rename(value);}
    }

    public string Rename(string newName)
    {
      return _name = TryRename(newName);
    }

    protected abstract string TryRename(string newName);
    
    // Fix me: this fails when comparing Brush and Pattern with same name!
    public override bool Equals(object o)
    {
      if (o is DataObject)
	{
	  return (o as DataObject).Name == Name;
	}
      return false;
    }
  }
}
