// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestVersion.cs
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
  public class TestVersion
  {
    [Test]
    public void EqualsTrue()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.3");
      Assert.IsTrue(v1.Equals(v2));
    }

    [Test]
    public void EqualsFalse()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.4");
      Assert.IsFalse(v1.Equals(v2));
    }

    [Test]
    public void EqualsOperator()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.3");
      Assert.IsTrue(v1 == v2);
    }

    [Test]
    public void NotEqualsOperator()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.4");
      Assert.IsTrue(v1 != v2);
    }

    [Test]
    public void GreaterThan()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.4");
      Assert.IsTrue(v2 > v1);
    }

    [Test]
    public void LessThan()
    {
      var v1 = new Version("1.2.3");
      var v2 = new Version("1.2.4");
      Assert.IsTrue(v1 < v2);
    }
  }
}
