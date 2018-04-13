// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// Rectangle.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

namespace Gimp
{
  public class Rectangle
  {
    public Coordinate<int> UpperLeft {get;} = new Coordinate<int>();
    public Coordinate<int> LowerRight {get;} = new Coordinate<int>();

    public int X1 => UpperLeft.X;
    public int Y1 => UpperLeft.Y;
    public int X2 => LowerRight.X;
    public int Y2 => LowerRight.Y;

    public int Width => X2 - X1;
    public int Height => Y2 - Y1;
    public int Area => Width * Height;

    public Rectangle(int x1, int y1, int x2, int y2)
    {
      UpperLeft.X = x1;
      UpperLeft.Y = y1;
      LowerRight.X = x2;
      LowerRight.Y = y2;
    }

    public Rectangle(Coordinate<int> upperLeft, int width, int height)
    {
      UpperLeft = upperLeft;
      LowerRight = new Coordinate<int>(upperLeft.X + width,
				       upperLeft.Y + height);
    }

    public Rectangle(Coordinate<int> upperLeft, Coordinate<int> lowerRight)
    {
      UpperLeft = upperLeft;
      LowerRight = lowerRight;
    }

    public override bool Equals(object o)
    {
      var rectangle = o as Rectangle;
      return rectangle?.UpperLeft == UpperLeft && 
	rectangle?.LowerRight == LowerRight;
    }

    public override int GetHashCode() =>
      UpperLeft.GetHashCode() + LowerRight.GetHashCode();

    public static bool operator==(Rectangle rectangle1, Rectangle rectangle2) =>
      rectangle1.Equals(rectangle2);

    public static bool operator!=(Rectangle rectangle1, Rectangle rectangle2) =>
      !(rectangle1 == rectangle2);

    public override string ToString() => $"({X1}, {Y1}, {X2}, {Y2})";
  }
}
