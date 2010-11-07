// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// RandomCoordinateGenerator.cs
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
using System.Collections.Generic;

namespace Gimp
{
  public class RandomCoordinateGenerator
  {
    readonly int _width;
    readonly int _height;
    readonly int _count;

    readonly Random _random;

    public RandomCoordinateGenerator(Int32 seed, int width, int height, 
				     int count)
    {
      _random = new Random(seed);
      _width = width;
      _height = height;
      _count = count;
    }

    public RandomCoordinateGenerator(int width, int height, int count)
    {
      _random = new Random();
      _width = width;
      _height = height;
      _count = count;
    }

    public IEnumerator<IntCoordinate> GetEnumerator()
    {
      for (int i = 0; i < _count; i++)
	{
	  int x = _random.Next(0, _width - 1);
	  int y = _random.Next(0, _height - 1);
	  yield return new IntCoordinate(x, y);
	}
    }
  }
}
