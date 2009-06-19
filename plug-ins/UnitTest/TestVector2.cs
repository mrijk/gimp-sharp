// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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
      Vector2 vector = new Vector2(13, 14);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void DefaultConstructor()
    {
      Vector2 vector = new Vector2();
      Assert.AreEqual(0.0, vector.X);
      Assert.AreEqual(0.0, vector.Y);
    }

    [Test]
    public void Set()
    {
      Vector2 vector = new Vector2();
      vector.Set(13, 14);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void Length()
    {
      Vector2 vector = new Vector2();
      Assert.AreEqual(0.0, vector.Length);

      vector = new Vector2(3, 4);
      Assert.AreEqual(5.0, vector.Length);
    }

    [Test]
    public void GetSetXYZ()
    {
      Vector2 vector = new Vector2();
      vector.X = 13;
      vector.Y = 14;
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
    }

    [Test]
    public void Mul()
    {
      Vector2 vector = new Vector2(1, 2);
      vector.Mul(2);
      Assert.AreEqual(new Vector2(2, 4), vector);
    }

    [Test]
    public void Normalize()
    {
      Vector2 vector = new Vector2(1, 2);
      vector.Normalize();
      Assert.AreEqual(1.0, vector.Length, 0.0001);
    }

    [Test]
    public void Neg()
    {
      Vector2 vector = new Vector2(1, 2);
      vector.Neg();
      Assert.AreEqual(new Vector2(-1, -2), vector);
    }

    [Test]
    public void Add()
    {
      Vector2 v1 = new Vector2(1, 2);
      Vector2 v2 = new Vector2(1, 1);
      v1.Add(v2);
      Assert.AreEqual(new Vector2(2, 3), v1);
    }

    [Test]
    public void Sub()
    {
      Vector2 v1 = new Vector2(1, 2);
      Vector2 v2 = new Vector2(1, 1);
      v1.Sub(v2);
      Assert.AreEqual(new Vector2(0, 1), v1);
    }

    [Test]
    public void PlusOperator()
    {
      Vector2 v1 = new Vector2(1, 2);
      Vector2 v2 = new Vector2(1, 1);
      Vector2 v = v1 + v2;
      Assert.AreEqual(new Vector2(2, 3), v);
    }

    [Test]
    public void MinusOperator()
    {
      Vector2 v1 = new Vector2(1, 2);
      Vector2 v2 = new Vector2(1, 1);
      Vector2 v = v1 - v2;
      Assert.AreEqual(new Vector2(0, 1), v);
    }

    [Test]
    public void MultiplyOperator()
    {
      Vector2 v1 = new Vector2(1, 2);
      Vector2 v = v1 * 3;
      Assert.AreEqual(new Vector2(3, 6), v);
    }    

    [Test]
    public void InnerProduct()
    {
      Vector2 v1 = new Vector2(1, 2);
      Assert.AreEqual(1 * 1 + 2 * 2, v1.InnerProduct(v1));
    }
  }
}
