// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestCoordinate.cs
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
  public class TestCoordinate
  {
    [Test]
    public void ConstructorOne()
    {
      var c = new Coordinate<int>();
      Assert.AreEqual(0, c.X);
      Assert.AreEqual(0, c.Y);
    }

    [Test]
    public void ConstructorTwo()
    {
      var c = new Coordinate<int>(13, 14);
      Assert.AreEqual(13, c.X);
      Assert.AreEqual(14, c.Y);
    }

    [Test]
    public void ConstructorThree()
    {
      var c1 = new Coordinate<int>(13, 14);
      var c2 = new Coordinate<int>(c1);
      Assert.AreEqual(13, c2.X);
      Assert.AreEqual(14, c2.Y);
    }

    [Test]
    public void getAndSetXandY()
    {
      var c = new Coordinate<int>();
      c.X = 13;
      c.Y = 14;
      Assert.AreEqual(13, c.X);
      Assert.AreEqual(14, c.Y);
    }

    [Test]
    public void Equals()
    {
      var c1 = new Coordinate<int>(13, 14);
      var c2 = new Coordinate<int>(c1);
      var c3 = new Coordinate<int>(23, 24);
      Assert.IsTrue(c1.Equals(c2));
      Assert.IsFalse(c1.Equals(c3));
    }

    [Test]
    public void DifferentTypes()
    {
      var c1 = new Coordinate<int>(13, 14);
      var c2 = new Coordinate<double>(13, 14);

      Assert.IsFalse(c1.Equals(c2));     
    }

    [Test]
    public void Operators()
    {
      var c1 = new Coordinate<int>(13, 14);
      var c2 = new Coordinate<int>(c1);
      var c3 = new Coordinate<int>(23, 24);

      Assert.IsTrue(c1 == c2);
      Assert.IsFalse(c1 != c2);
      Assert.IsFalse(c1 == c3);
      Assert.IsTrue(c1 != c3);
    }
  }
}
