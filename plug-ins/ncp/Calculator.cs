// The ncp plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// Calculator.cs
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

namespace Gimp.ncp
{
  using IntCoordinate = Coordinate<int>;

  class Calculator 
  {
    readonly int _points;
    readonly int _closest;

    readonly IntCoordinate[,] _vp;

    readonly int[] _distances;
    int[] _data, _under, _over;

    int _area;

    public Calculator(int points, int closest, int bpp, Rectangle rectangle, 
		      int seed)
    {
      _points = points;
      _closest = closest;

      int width = rectangle.Width;
      int height = rectangle.Height;
      _area = rectangle.Area;

      int xmid = width / 2;
      int ymid = height / 2;

      _distances = new int[4 * _points];
      _data = new int[4 * _points];
      _under = new int[4 * _points];
      _over = new int[4 * _points];

      _vp = new IntCoordinate[bpp, 4 * _points];

      var generator = new RandomCoordinateGenerator(seed, width - 1, 
						    height - 1, _points);
 
      for (int b = 0; b < bpp; b++) 
	{
	  int i = 0;
	  foreach (var c in generator)
	    {
	      int px = c.X;
	      int py = c.Y;

	      int offx = (px < xmid) ? width : -width;
	      int offy = (py < ymid) ? height : -height;

	      _vp[b, i] = new IntCoordinate(px, py);
	      _vp[b, i + _points] = new IntCoordinate(px + offx, py);
	      _vp[b, i + 2 * _points] = new IntCoordinate(px, py + offy);
	      _vp[b, i + 3 * _points] = new IntCoordinate(px + offx,
							  py + offy);
	      i++;
	    }
	}
    }

    public int Calc(int b, int x, int y)
    {
      // compute distance to each point
      for (int k = 0; k < _points * 4; k++) 
	{
	  var p = _vp[b, k];
	  int x2 = x - p.X;
	  int y2 = y - p.Y;
	  _distances[k] = x2 * x2 + y2 * y2;
	}
      
      int val = (int) (255.0 * Math.Sqrt((double) Select(_closest) / _area));

      return 255 - val;	// invert
    }

    int Select(int n)
    {
      int pivot = 0;
      int len = 4 * _points;
      _data = _distances;

      while (true)
	{
	  int j = 0;
	  int k = 0;
	  int pcount = 0;
	  
	  pivot = _data[0];

	  for (int i = 0; i < len; i++)
	    {
	      int elem = _data[i];

	      if (elem < pivot)
		_under[j++] = elem;
	      else if (elem > pivot)
		_over[k++] = elem;
	      else
		pcount++;	
	    }

	  if (n < j)
	    {
	      len = j;
	      _data = _under;
	    }
	  else if (n < j + pcount)
	    {
	      break;
	    }
	  else
	    {
	      len = k;
	      _data = _over;
	      n -= j + pcount;
	    }
	}
      return pivot;
    }
  }
}
