// The Raindrops plug-in
// Copyright (C) 2004-2007 Maurits Rijk, Massimo Perga
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

using Gtk;

namespace Gimp.Raindrops
{
  public class BoolMatrix
  {
    readonly int _width;
    readonly int _height;
    readonly bool [,] _matrix;

    Random _random = new Random();

    public BoolMatrix(int width, int height)
    {
      _width = width;
      _height = height;
      _matrix = new bool[height, width];
    }

    public bool this[int row, int col]
    {
      set {_matrix[row, col] = value;}
      get {return _matrix[row, col];}
    }

    public Coordinate<int> Generate(int radius, out bool failed)
    {
      int tries = 0;
      
      while (true)
	{
	  bool findAnother = false;

	  int x = _random.Next(_width);
	  int y = _random.Next(_height);
	  
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
		      if (i >= 0 && i < _width && j >= 0 && j < _height)
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
	      failed = false;
	      return new Coordinate<int>(x, y);
	    }
	  else if (++tries >= 10000)
	    {
	      failed = true;
	      return null;
	    }
	}
    }
  }
}
