// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
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

using System;

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestRectangle
  {
    [Test]
    public void ConstructorOne()
    {
      Rectangle rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(13, rectangle.X1);
      Assert.AreEqual(14, rectangle.Y1);
      Assert.AreEqual(129, rectangle.X2);
      Assert.AreEqual(132, rectangle.Y2);
    }

    [Test]
    public void ConstructorTwo()
    {
      Coordinate<int> upperLeft = new Coordinate<int>(13, 14);
      Coordinate<int> lowerRight = new Coordinate<int>(129, 132);
      Rectangle rectangle = new Rectangle(upperLeft, lowerRight);
      Assert.AreEqual(13, rectangle.X1);
      Assert.AreEqual(14, rectangle.Y1);
      Assert.AreEqual(129, rectangle.X2);
      Assert.AreEqual(132, rectangle.Y2);
    }

    [Test]
    public void Width()
    {
      Rectangle rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(129 - 13, rectangle.Width);
    }

    [Test]
    public void Height()
    {
      Rectangle rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual(132 - 14, rectangle.Height);
    }

    [Test]
    public void Area()
    {
      Rectangle rectangle = new Rectangle(13, 14, 129, 132);
      Assert.AreEqual((129 - 13) * (132 - 14), rectangle.Area);
    }

    [Test]
    public void Equals()
    {
      Rectangle rectangle1 = new Rectangle(13, 14, 129, 132);
      Rectangle rectangle2 = new Rectangle(23, 24, 129, 132);
      Rectangle rectangle3 = new Rectangle(13, 14, 129, 132);

      Assert.IsFalse(rectangle1.Equals(rectangle2));
      Assert.IsTrue(rectangle1.Equals(rectangle3));
    }

    [Test]
    public void Operators()
    {
      Rectangle rectangle1 = new Rectangle(13, 14, 129, 132);
      Rectangle rectangle2 = new Rectangle(23, 24, 129, 132);
      Rectangle rectangle3 = new Rectangle(13, 14, 129, 132);

      Assert.IsFalse(rectangle1 == rectangle2);
      Assert.IsTrue(rectangle1 != rectangle2);
      Assert.IsTrue(rectangle1 == rectangle3);
      Assert.IsFalse(rectangle1 != rectangle3);
    }
  }
}
