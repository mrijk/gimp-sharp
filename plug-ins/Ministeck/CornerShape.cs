// The Ministeck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// CornerShape.cs
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

namespace Gimp.Ministeck
{
  public class CornerShape : Shape
  {
    ShapeDescription _shape1 = new ShapeDescription();
    ShapeDescription _shape2 = new ShapeDescription();
    ShapeDescription _shape3 = new ShapeDescription();
    ShapeDescription _shape4 = new ShapeDescription();

    public CornerShape()
    {
      _shape1.Add(0, 1);
      _shape1.Add(1, 0);

      _shape2.Add(1, 0);
      _shape2.Add(1, 1);

      _shape3.Add(0, 1);
      _shape3.Add(1, 1);

      _shape4.Add(0, 1);
      _shape4.Add(-1, 1);

      Combine(_shape1, _shape2, _shape3, _shape4);
    }

    protected override void Fill(int x, int y, ShapeDescription shape)
    {
      int _size = Shape._painter.Size;

      LineStart(x, y);
      if (shape == _shape1)
	{
	  HLine(2 * _size);
	  VLine(1 * _size);
	  HLine(-_size - 1);
	  VLine(_size + 1);
	  HLine(-_size);
	  VLine(-2 * _size);
	}
      else if (shape == _shape2)
	{
	  HLine(2 * _size);
	  VLine(2 * _size);
	  HLine(-_size);
	  VLine(-_size - 1);
	  HLine(-_size - 1);
	  VLine(-_size);
	}
      else if (shape == _shape3)
	{
	  HLine(_size);
	  VLine(_size + 1);
	  HLine(_size + 1);
	  VLine(_size);
	  HLine(-2 * _size);
	  VLine(-2 * _size);
	}
      else
	{
	  HLine(_size);
	  VLine(2 * _size);
	  HLine(-2 * _size);
	  VLine(-_size);
	  HLine(_size + 1);
	  VLine(-_size - 1);
	}
    }
  }
}
