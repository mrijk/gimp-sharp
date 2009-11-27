// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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
  public class Version
  {
    readonly int _major;
    readonly int _minor;
    readonly int _micro;

    public Version(string version)
    {
      var numbers = version.Split('.');
      if (numbers.Length > 0)
	_major = Convert.ToInt32(numbers[0]);
      if (numbers.Length > 1)
	_minor = Convert.ToInt32(numbers[1]);
      if (numbers.Length > 2)
	_micro = Convert.ToInt32(numbers[2]);
    }

    public override bool Equals(object o)
    {
      if (o is Version)
	{
	  var v = o as Version;
	  return v._major == _major &&
	    v._minor == _minor &&
	    v._micro == _micro;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return _major ^ _minor ^ _micro;
    }

    public static bool operator==(Version v1, Version v2)
    {
      return v1.Major == v2.Major &&
	v1.Minor == v2.Minor &&
	v1.Micro == v2.Micro;
    }

    public static bool operator!=(Version v1, Version v2)
    {
      return !(v1 == v2);
    }

    public static bool operator>(Version v1, Version v2)
    {
      if (v1.Major > v2.Major)
	{
	  return true;
	}
      else if (v1.Major == v2.Major)
	{
	  if (v1.Minor > v2.Minor)
	    {
	      return true;
	    }
	  else if (v1.Minor == v2.Minor)
	    {
	      return v1.Micro > v2.Micro;
	    }
	}
      return false;
    }

    public static bool operator<(Version v1, Version v2)
    {
      return !(v1 > v2 || v1 == v2);
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

    public override string ToString()
    {
      return string.Format("{0}.{1}.{2}", Major, Minor, Micro);
    }
  }
}
