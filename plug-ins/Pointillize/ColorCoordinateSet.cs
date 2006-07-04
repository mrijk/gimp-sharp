// The Pointillize plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ColorCoordinateSet.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Gimp.Pointillize
{
  public class ColorCoordinateSet : CoordinateList<int>
  {
    readonly int _cellSize;
    readonly int _width;
    readonly int _height;
    readonly int _matrixRows;
    readonly int _matrixColumns;

    readonly byte[] _backgroundColor;

    readonly List<ColorCoordinate>[,] _matrix;

    public ColorCoordinateSet(Drawable drawable, int cellSize)
    {
      _cellSize = cellSize;

      _backgroundColor = Context.Background.Bytes;

      PixelFetcher pf = new PixelFetcher(drawable, false);
      Random random = new Random();

      _width = drawable.Width;
      _height = drawable.Height;
      
      int nrOfCells = (int) (_width * _height / 
			     (Math.PI * _cellSize * _cellSize));

      Console.WriteLine("nrOfCells: " + nrOfCells);

      _matrixColumns = (int) Math.Sqrt(nrOfCells * _width / 8.0 / _height);
      _matrixRows = _matrixColumns * _height / _width;

      _matrixColumns = Math.Max(_matrixColumns, 1);
      _matrixRows = Math.Max(_matrixRows, 1);
      
      _matrix = new List<ColorCoordinate>[_matrixRows, _matrixColumns];

      for (int i = 0; i < nrOfCells; i++)
	{
	  int x = random.Next(0, _width - 1);
	  int y = random.Next(0, _height - 1);
	  byte[] color = new byte[drawable.Bpp];
	  pf.GetPixel(x, y, color);

	  // Add some noise to red and green
	  // color[0] += random.Next(0, 10);
	  // color[1] += random.Next(0, 10);

	  ColorCoordinate coordinate = new ColorCoordinate(x, y, color);
	  Add(coordinate);

	  int row = y * _matrixRows / _height;
	  int col = x * _matrixColumns / _width;
	  
	  if (_matrix[row, col] == null)
	    {
	      _matrix[row, col] = new List<ColorCoordinate>();
	    }
	  _matrix[row, col].Add(coordinate);

	  int top = row * _height / _matrixRows;
	  int left = col * _width / _matrixColumns;
	  int bottom = (row + 1) * _height / _matrixRows;
	  int right = (col + 1) * _width / _matrixColumns;
	  
	  Intersects(left, top, coordinate);
	  Intersects(x, top, coordinate);
	  Intersects(right, top, coordinate);
	  Intersects(left, y, coordinate);
	  Intersects(right, y, coordinate);
	  Intersects(left, bottom, coordinate);
	  Intersects(x, bottom, coordinate);
	  Intersects(right, bottom, coordinate);
	}

      pf.Dispose();
    }

    void Intersects(int x, int y, ColorCoordinate coordinate)
    {
      if (x < 0 || x >= _width || y < 0 || y >= _height)
	{
	  return;
	}
      
      int row = y * _matrixRows / _height;
      int col = x * _matrixColumns / _width;
	  
      if (coordinate.Distance(x, y) < _cellSize * _cellSize / 4)
	{
	  Console.WriteLine("Ok");
	  if (_matrix[row, col] == null)
	    {
	      _matrix[row, col] = new List<ColorCoordinate>();
	    }
	  _matrix[row, col].Add(coordinate);
	}
    }

    public byte[] GetColor(int x, int y)
    {
      int distance = int.MaxValue;
      ColorCoordinate closest = null;

      int row = y * _matrixRows / _height;
      int col = x * _matrixColumns / _width;
      
      List<ColorCoordinate> list = _matrix[row, col];

      foreach (ColorCoordinate coordinate in list)
	{
	  int d = coordinate.Distance(x, y);
	  if (d < distance)
	    {
	      distance = d;
	      closest = coordinate;
	    }
	}

      return distance < _cellSize * _cellSize 
	? closest.Color : _backgroundColor;
    }
  }
}
