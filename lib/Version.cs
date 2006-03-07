// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Version.cs
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

using System;

namespace Gimp
{
  public class Version : IComparable
  {
    readonly int _major;
    readonly int _minor;
    readonly int _micro;

    internal Version(string version)
    {
	string[] numbers = version.Split('.');
	_major = Convert.ToInt32(numbers[0]);
	_minor = Convert.ToInt32(numbers[1]);
	_micro = Convert.ToInt32(numbers[2]);
    }

    public int CompareTo(object obj)
    {
      Version version = obj as Version;

      int cmp = _major - version._major;
      if (cmp == 0) {
	cmp = _minor - version._minor;
	if (cmp == 0) {
	  cmp = _micro - version._micro;
	}
      }
      return cmp;
    }

    public uint Major
    {
      get {return (uint) _major;}
    }

    public uint Minor
    {
      get {return (uint) _minor;}
    }

    public uint Micro
    {
      get {return (uint) _micro;}
    }
  }
}
