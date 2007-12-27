// The Ministeck plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// TwoByTwoShape.cs
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
  public class TwoByTwoShape : Shape
  {
    public TwoByTwoShape()
    {
      ShapeDescription shape = new ShapeDescription();
      shape.Add(0, 1);
      shape.Add(1, 0);
      shape.Add(1, 1);

      Combine(shape);
    }

    protected override void Fill(Coordinate<int> c, ShapeDescription shape)
    {
      Rectangle(c, 2, 2);
    }
  }
}
