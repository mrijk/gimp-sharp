// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestVector3.cs
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
  public class TestVector3
  {
    [Test]
    public void Constructor()
    {
      Vector3 vector = new Vector3(13, 14, 15);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
      Assert.AreEqual(15, vector.Z);
    }

    [Test]
    public void DefaultConstructor()
    {
      Vector3 vector = new Vector3();
      Assert.AreEqual(0.0, vector.X);
      Assert.AreEqual(0.0, vector.Y);
      Assert.AreEqual(0.0, vector.Z);
    }

    [Test]
    public void Set()
    {
      Vector3 vector = new Vector3();
      vector.Set(13, 14, 15);
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
      Assert.AreEqual(15, vector.Z);
    }

    [Test]
    public void Length()
    {
      Vector3 vector = new Vector3();
      Assert.AreEqual(0.0, vector.Length);

      vector = new Vector3(3, 4, 0);
      Assert.AreEqual(5.0, vector.Length);
    }

    [Test]
    public void GetSetXYZ()
    {
      Vector3 vector = new Vector3();
      vector.X = 13;
      vector.Y = 14;
      vector.Z = 15;
      Assert.AreEqual(13, vector.X);
      Assert.AreEqual(14, vector.Y);
      Assert.AreEqual(15, vector.Z);
    }

    [Test]
    public void Mul()
    {
      Vector3 vector = new Vector3(1, 2, 3);
      vector.Mul(2);
      Assert.AreEqual(new Vector3(2, 4, 6), vector);
    }

    [Test]
    public void Normalize()
    {
      Vector3 vector = new Vector3(1, 2, 3);
      vector.Normalize();
      Assert.AreEqual(1, vector.Length);
    }

    [Test]
    public void Neg()
    {
      Vector3 vector = new Vector3(1, 2, 3);
      vector.Neg();
      Assert.AreEqual(new Vector3(-1, -2, -3), vector);
    }

    [Test]
    public void Add()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Vector3 v2 = new Vector3(1, 1, 1);
      v1.Add(v2);
      Assert.AreEqual(new Vector3(2, 3, 4), v1);
    }

    [Test]
    public void Sub()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Vector3 v2 = new Vector3(1, 1, 1);
      v1.Sub(v2);
      Assert.AreEqual(new Vector3(0, 1, 2), v1);
    }

    [Test]
    public void PlusOperator()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Vector3 v2 = new Vector3(1, 1, 1);
      Vector3 v = v1 + v2;
      Assert.AreEqual(new Vector3(2, 3, 4), v);
    }

    [Test]
    public void MinusOperator()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Vector3 v2 = new Vector3(1, 1, 1);
      Vector3 v = v1 - v2;
      Assert.AreEqual(new Vector3(0, 1, 2), v);
    }

    [Test]
    public void MultiplyOperator()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Vector3 v = v1 * 3;
      Assert.AreEqual(new Vector3(3, 6, 9), v);
    }    

    [Test]
    public void InnerProduct()
    {
      Vector3 v1 = new Vector3(1, 2, 3);
      Assert.AreEqual(1 * 1 + 2 * 2 + 3 * 3, v1.InnerProduct(v1));
    }
  }
}
