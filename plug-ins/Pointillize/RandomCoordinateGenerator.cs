// The Pointillize plug-in
// Copyright (C) 2007 Maurits Rijk
//
// RandomCoordinateGenerator.cs
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

namespace Gimp.Pointillize
{
  class RandomCoordinateGenerator
  {
    readonly int _width;
    readonly int _height;
    readonly int _count;

    Random _random = new Random();

    public RandomCoordinateGenerator(int width, int height, int count)
    {
      _width = width;
      _height = height;
      _count = count;
    }

    public IEnumerator<Coordinate<int>> GetEnumerator()
    {
      for (int i = 0; i < _count; i++)
	{
	  int x = _random.Next(0, _width - 1);
	  int y = _random.Next(0, _height - 1);
	  yield return new Coordinate<int>(x, y);
	}
    }
  }
}
