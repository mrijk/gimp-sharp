// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Dimensions.cs
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
  public struct Dimensions
  {
    int _width;
    int _height;

    public Dimensions(int width, int height)
    {
      _width = width;
      _height = height;
    }

    public int Width
    {
      get {return _width;}
      set {_width = value;}
    }

    public int Height
    {
      get {return _height;}
      set {_height = value;}
    }

    public bool IsInside(int x, int y)
    {
      return x >= 0 && x < _width && y >= 0 && y < _height;
    }

    public static bool operator==(Dimensions dimensions1, 
				  Dimensions dimensions2)
    {
      return dimensions1.Equals(dimensions2);
    }

    public static bool operator!=(Dimensions dimensions1, 
				  Dimensions dimensions2)
    {
      return !(dimensions1 == dimensions2);
    }

    public override string ToString()
    {
      return string.Format("({0} X {1})", Width, Height);
    }
  }
}
