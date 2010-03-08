// The Ministeck plug-in
// Copyright (C) 2004-2010 Maurits Rijk
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

    readonly Random _random = new Random();

    protected void Combine(params ShapeDescription[] list)
    {
      var empty = new ShapeDescriptionSet();
      _set.Add(empty);

      foreach (var val in list)
	{
	  var copy = new List<ShapeDescriptionSet>();
	  foreach (var ele in _set)
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

    public bool Fits(BoolMatrix A, IntCoordinate c)
    {
      foreach (var shape in _set[_random.Next(0, _set.Count)])
	{
	  if (shape.Fits(Painter, A, c))
	    {
	      shape.Fill(A, c);
	      return true;
	    }
	}
      return false;
    }

    protected void LineStart(IntCoordinate c)
    {
      Painter.LineStart(c);
    }

    protected void Rectangle(IntCoordinate c, int w, int h)
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
