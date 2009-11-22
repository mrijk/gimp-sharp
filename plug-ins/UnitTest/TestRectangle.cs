// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestRectangle.cs
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

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestRectangle
  {
    [Test]
    public void ConstructorOne()
    {
      var rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(13, rectangle.X1);
      Assert.AreEqual(14, rectangle.Y1);
      Assert.AreEqual(129, rectangle.X2);
      Assert.AreEqual(132, rectangle.Y2);
    }

    [Test]
    public void ConstructorTwo()
    {
      var upperLeft = new Coordinate<int>(13, 14);
      var lowerRight = new Coordinate<int>(129, 132);
      var rectangle = new Rectangle(upperLeft, lowerRight);
      Assert.AreEqual(13, rectangle.X1);
      Assert.AreEqual(14, rectangle.Y1);
      Assert.AreEqual(129, rectangle.X2);
      Assert.AreEqual(132, rectangle.Y2);
    }

    [Test]
    public void UpperLeftLowerRight()
    {
      var upperLeft = new Coordinate<int>(13, 14);
      var lowerRight = new Coordinate<int>(129, 132);
      var rectangle = new Rectangle(upperLeft, lowerRight);
      Assert.IsTrue(rectangle.UpperLeft == upperLeft);
      Assert.IsTrue(rectangle.LowerRight == lowerRight);
    }

    [Test]
    public void Width()
    {
      var rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(129 - 13, rectangle.Width);
    }

    [Test]
    public void Height()
    {
      var rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(132 - 14, rectangle.Height);
    }

    [Test]
    public void Area()
    {
      var rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual((129 - 13) * (132 - 14), rectangle.Area);
    }

    [Test]
    public void Equals()
    {
      var rectangle1 = new Rectangle(13, 14, 129, 132);
      var rectangle2 = new Rectangle(23, 24, 129, 132);
      var rectangle3 = new Rectangle(13, 14, 129, 132);

      Assert.IsFalse(rectangle1.Equals(rectangle2));
      Assert.IsTrue(rectangle1.Equals(rectangle3));
    }

    [Test]
    public void Operators()
    {
      var rectangle1 = new Rectangle(13, 14, 129, 132);
      var rectangle2 = new Rectangle(23, 24, 129, 132);
      var rectangle3 = new Rectangle(13, 14, 129, 132);

      Assert.IsFalse(rectangle1 == rectangle2);
      Assert.IsTrue(rectangle1 != rectangle2);
      Assert.IsTrue(rectangle1 == rectangle3);
      Assert.IsFalse(rectangle1 != rectangle3);
    }
  }
}
