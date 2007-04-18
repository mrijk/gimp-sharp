// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestDimensions.cs
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
  public class TestDimensions
  {
    [Test]
    public void Constructor()
    {
      Dimensions dimensions = new Dimensions(13, 14);
      Assert.AreEqual(13, dimensions.Width);
      Assert.AreEqual(14, dimensions.Height);
    }

    [Test]
    public void Inside()
    {
      Dimensions dimensions = new Dimensions(13, 14);
      Assert.IsTrue(dimensions.IsInside(5, 6));
    }

    [Test]
    public void Equals()
    {
      Dimensions dimensions1 = new Dimensions(13, 14);
      Dimensions dimensions2 = new Dimensions(23, 24);
      Dimensions dimensions3 = new Dimensions(13, 14);

      Assert.IsTrue(dimensions1.Equals(dimensions3));
      Assert.IsFalse(dimensions1.Equals(dimensions2));
    }

    [Test]
    public void Operators()
    {
      Dimensions dimensions1 = new Dimensions(13, 14);
      Dimensions dimensions2 = new Dimensions(23, 24);
      Dimensions dimensions3 = new Dimensions(13, 14);

      Assert.IsTrue(dimensions1 == dimensions3);
      Assert.IsFalse(dimensions1 == dimensions2);
    }
  }
}
