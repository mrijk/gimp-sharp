// The Ministeck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// TwoByOneShape.cs
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
  public class TwoByOneShape : Shape
  {
    ShapeDescription _shape1 = new ShapeDescription();
    ShapeDescription _shape2 = new ShapeDescription();

    public TwoByOneShape()
    {
      _shape1.Add(0, 1);
      _shape2.Add(1, 0);

      Combine(_shape1, _shape2);
    }

    protected override void Fill(int x, int y, ShapeDescription shape)
    {
      if (shape == _shape1)	// Vertical
	{
	  Rectangle(x, y, 1, 2);
	}
      else			// Horizontal
	{
	  Rectangle(x, y, 2, 1);
	}
    }
  }
}
