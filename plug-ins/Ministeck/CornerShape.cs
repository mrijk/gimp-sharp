// The Ministeck plug-in
// Copyright (C) 2004-2018 Maurits Rijk
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

namespace Gimp.Ministeck
{
  public class CornerShape : Shape
  {
    int Size {get;} = Painter.Size;

    public CornerShape()
    {
      Combine(new ShapeDescription(ShapeOne) {{0, 1}, {1, 0}},
	      new ShapeDescription(ShapeTwo) {{1, 0}, {1, 1}},
	      new ShapeDescription(ShapeThree) {{0, 1}, {1, 1}},
	      new ShapeDescription(ShapeFor) {{0, 1}, {-1, 1}});
    }

    void ShapeOne(IntCoordinate c)
    {
      Draw(c, 2 * Size, Size, -Size - 1, Size + 1, -Size, -2 * Size);
    }

    void ShapeTwo(IntCoordinate c)
    {
      Draw(c, 2 * Size, 2 * Size, -Size, -Size - 1, -Size - 1, -Size);
    }

    void ShapeThree(IntCoordinate c)
    {
      Draw(c, Size, Size + 1, Size + 1, Size, -2 * Size, -2 * Size);
    }

    void ShapeFor(IntCoordinate c)
    {
      Draw(c, Size, 2 * Size, -2 * Size, -Size, Size + 1, -Size - 1);
    }

    void Draw(IntCoordinate c, int hline1, int vline1, int hline2, int vline2, 
	      int hline3, int vline3)
    {
      LineStart(c);
      HLine(hline1);
      VLine(vline1);
      HLine(hline2);
      VLine(vline2);
      HLine(hline3);
      VLine(vline3);
    }
  }
}
