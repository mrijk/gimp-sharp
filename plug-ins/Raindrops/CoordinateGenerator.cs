// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// CoordinateGenerator.cs
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

namespace Gimp.Raindrops
{
  public class CoordinateGenerator
  {
    readonly Rectangle _rectangle;

    public CoordinateGenerator(Rectangle rectangle)
    {
      _rectangle = rectangle;
    }

    public IEnumerator<IntCoordinate> GetEnumerator()
    {
      for (int y = _rectangle.Y1; y < _rectangle.Y2; y++)
	{
	  for (int x = _rectangle.X1; x < _rectangle.X2; x++)
	    {
	      yield return new IntCoordinate(x, y);
	    }
	}
    }
  }
}
