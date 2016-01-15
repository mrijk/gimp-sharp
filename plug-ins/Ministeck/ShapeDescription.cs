// The Ministeck plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// ShapeDescription.cs
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
using System.Linq;

namespace Gimp.Ministeck
{
  public class ShapeDescription : CoordinateList<int>
  {
    readonly Action<IntCoordinate> _fillFunc;

    public ShapeDescription(Action<IntCoordinate> fillFunc)
    {
      _fillFunc = fillFunc;
    }

    public bool Fits(Painter painter, BoolMatrix A, IntCoordinate p)
    {
      var color = painter.GetPixel(p);
      return TrueForAll(offset => 
	{
	  var c = new IntCoordinate(p.X + offset.X, p.Y + offset.Y);
	  return A.IsInside(c) && !A.Get(c) && painter.IsSameColor(c, color);
	});
    }

    public void Fill(BoolMatrix A, IntCoordinate c)
    {
      _fillFunc(c);
      A.Set(c, true);
      ForEach(sc => {A[c.Y + sc.Y, c.X + sc.X] = true;});
    }
  }
}
