// The Ministeck plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Shape.cs
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

namespace Gimp.Ministeck
{
  abstract public class Shape
  {
    List<ShapeDescriptionSet> _set = new List<ShapeDescriptionSet>(); 
    static public Painter Painter {protected get; set;}

    Random _random = new Random();

    public Shape()
    {
    }

    protected void Combine(params ShapeDescription[] list)
    {
      var empty = new ShapeDescriptionSet();
      _set.Add(empty);

      foreach (var val in list)
	{
	  var copy = new List<ShapeDescriptionSet>();
	  foreach (ShapeDescriptionSet ele in _set)
	    {
	      for (int i = 0; i <= ele.Count; i++)
		{
		  var tmp = new ShapeDescriptionSet(ele);
		  tmp.Insert(i, val);
		  copy.Add(tmp);
		}
	    }
	  _set = copy;
	}
    }

    public bool Fits(bool[,] A, Coordinate<int> c)
    {
      int index = _random.Next(0, _set.Count);

      foreach (var shape in _set[index])
	{
	  if (Fits(A, c.X, c.Y, shape))
	    {
	      Fill(A, c, shape);
	      return true;
	    }
	}
      return false;
    }

    bool Fits(bool[,] A, int x, int y, ShapeDescription shape)
    {
      var color = Painter.GetPixel(x, y);

      int width = A.GetLength(0);
      int height = A.GetLength(1);

      foreach (var c in shape)
	{
	  int cx = x + c.X;
	  int cy = y + c.Y;
	  if (cx < 0 || cx >= width || cy < 0 || cy >= height || A[cx, cy])
	    {
	      return false;
	    }

	  var pixel = Painter.GetPixel(cx, cy);
	  if (!pixel.IsSameColor(color))
	    {
	      return false;
	    }
	}
      return true;
    }

    abstract protected void Fill(Coordinate<int> c, ShapeDescription shape) ;

    void Fill(bool[,] A, Coordinate<int> c, ShapeDescription shape)
    {
      Fill(c, shape);
      A[c.X, c.Y] = true;
      shape.ForEach(sc => {A[c.X + sc.X, c.Y + sc.Y] = true;});
    }

    protected void LineStart(Coordinate<int> c)
    {
      Painter.LineStart(c);
    }

    protected void Rectangle(Coordinate<int> c, int w, int h)
    {
      Painter.Rectangle(c, w, h);
    }

    protected void HLine(int len)
    {
      Painter.HLine(len);
    }

    protected void VLine(int len)
    {
      Painter.VLine(len);
    }
  }
}
