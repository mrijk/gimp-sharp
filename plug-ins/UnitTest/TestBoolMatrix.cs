// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestBoolMatrix.cs
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
  public class TestBoolMatrix
  {
    [Test]
    public void Constructor()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      Assert.AreEqual(width, b.Width);
      Assert.AreEqual(height, b.Height);
    }

    [Test]
    public void This()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      Assert.IsFalse(b[11, 13]);
      b[11, 13] = true;
      Assert.IsTrue(b[11, 13]);
      b[11, 13] = false;
      Assert.IsFalse(b[11, 13]);
    }

    [Test]
    public void Get()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      b[13, 11] = true;
      Assert.IsTrue(b.Get(new IntCoordinate(11, 13)));
    }

    [Test]
    public void Set()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      var c = new IntCoordinate(11, 13);
      b.Set(c, true);
      Assert.IsTrue(b.Get(c));
    }

    [Test]
    public void IsInside()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      Assert.IsTrue(b.IsInside(new IntCoordinate(11, 13)));
      Assert.IsTrue(b.IsInside(new IntCoordinate(width - 1, height - 1)));
      Assert.IsTrue(b.IsInside(new IntCoordinate(0, 0)));
    }

    [Test]
    public void IsNotInside()
    {
      const int width = 43;
      const int height = 29;

      var b = new BoolMatrix(width, height);
      Assert.IsFalse(b.IsInside(new IntCoordinate(111, 133)));
      Assert.IsFalse(b.IsInside(new IntCoordinate(width, height)));
      Assert.IsFalse(b.IsInside(new IntCoordinate(-1, -1)));
    }
  }
}
