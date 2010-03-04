// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestVector2.cs
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
  public class TestVector2
  {
    [Test]
    public void Constructor()
    {
      var vector = new Vector2(13, 14);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void DefaultConstructor()
    {
      var vector = new Vector2();
      Assert.AreEqual(0.0, vector.X);
      Assert.AreEqual(0.0, vector.Y);
    }

    [Test]
    public void Set()
    {
      var vector = new Vector2();
      vector.Set(13, 14);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void Length()
    {
      var vector = new Vector2();
      Assert.AreEqual(0.0, vector.Length);

      vector = new Vector2(3, 4);
      Assert.AreEqual(5.0, vector.Length);
    }

    [Test]
    public void GetSetXYZ()
    {
      var vector = new Vector2() {X = 13, Y = 14};
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void Mul()
    {
      var vector = new Vector2(1, 2);
      vector.Mul(2);
      Assert.AreEqual(new Vector2(2, 4), vector);
    }

    [Test]
    public void Normalize()
    {
      var vector = new Vector2(1, 2);
      vector.Normalize();
      Assert.AreEqual(1.0, vector.Length, 0.0001);
    }

    [Test]
    public void Neg()
    {
      var vector = new Vector2(1, 2);
      vector.Neg();
      Assert.AreEqual(new Vector2(-1, -2), vector);
    }

    [Test]
    public void Add()
    {
      var v1 = new Vector2(1, 2);
      var v2 = new Vector2(1, 1);
      v1.Add(v2);
      Assert.AreEqual(new Vector2(2, 3), v1);
    }

    [Test]
    public void Sub()
    {
      var v1 = new Vector2(1, 2);
      var v2 = new Vector2(1, 1);
      v1.Sub(v2);
      Assert.AreEqual(new Vector2(0, 1), v1);
    }

    [Test]
    public void PlusOperator()
    {
      var v1 = new Vector2(1, 2);
      var v2 = new Vector2(1, 1);
      var v = v1 + v2;
      Assert.AreEqual(new Vector2(2, 3), v);
    }

    [Test]
    public void MinusOperator()
    {
      var v1 = new Vector2(1, 2);
      var v2 = new Vector2(1, 1);
      var v = v1 - v2;
      Assert.AreEqual(new Vector2(0, 1), v);
    }

    [Test]
    public void MultiplyOperator()
    {
      var v1 = new Vector2(1, 2);
      var v = v1 * 3;
      Assert.AreEqual(new Vector2(3, 6), v);
    }    

    [Test]
    public void InnerProduct()
    {
      var v1 = new Vector2(1, 2);
      Assert.AreEqual(1 * 1 + 2 * 2, v1.InnerProduct(v1));
    }

    [Test]
    public void Rotate()
    {
      var v1 = new Vector2(10, 0);
      v1.Rotate(Math.PI);
      Assert.AreEqual(-10, v1.X, 1e-6);
      Assert.AreEqual(0, v1.Y, 1e-6);
    }
  }
}
