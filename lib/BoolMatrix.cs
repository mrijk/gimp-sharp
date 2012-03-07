// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// BoolMatrix.cs
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

namespace Gimp
{
  public class BoolMatrix
  {
    public int Width {get; set;}
    public int Height {get; set;}
    readonly bool [,] _matrix;

    readonly Random _random = new Random();

    public BoolMatrix(int width, int height)
    {
      Width = width;
      Height = height;
      _matrix = new bool[height, width];
    }

    public bool this[int row, int col]
    {
      set {_matrix[row, col] = value;}
      get {return _matrix[row, col];}
    }

    public bool Get(IntCoordinate c)
    {
      return _matrix[c.Y, c.X];
    }

    public void Set(IntCoordinate c, bool value)
    {
      _matrix[c.Y, c.X] = value;
    }

    public bool IsInside(IntCoordinate c)
    {
      return c.X >= 0 && c.X < Width && c.Y >= 0 && c.Y < Height;
    }

    public IntCoordinate Generate(int radius, int maxTries = 10000)
    {
      for (int tries = 0; tries < maxTries; tries++)
	{
	  bool findAnother = false;

	  int x = _random.Next(Width);
	  int y = _random.Next(Height);
	  
	  if (_matrix[y, x])
	    {
	      findAnother = true;
	    }
	  else
	    {
	      for (int i = x - radius ; i <= x + radius ; i++)
		{
		  for (int j = y - radius ; j <= y + radius ; j++)
		    {
		      if (i >= 0 && i < Width && j >= 0 && j < Height)
			{
			  if (_matrix[j, i])
			    {
			      findAnother = true;
			    }
			}
		    }
		}
	    }
	  
	  if (!findAnother)
	    {
	      return new IntCoordinate(x, y);
	    }
	}
      return null;
    }
  }
}
