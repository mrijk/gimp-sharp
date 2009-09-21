// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestGimpUnit.cs
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
  public class TestGimpUnit
  {
    [Test]
    public void GetNumberOfUnits()
    {
      Assert.AreEqual((int) Unit.End + 1, GimpUnit.NumberOfUnits);
    }

    [Test]
    public void GetNumberOfBuiltInUnits()
    {
      Assert.AreEqual((int) Unit.End, GimpUnit.NumberOfBuiltInUnits);
    }

    [Test]
    public void NewGimpUnit()
    {
      int numberOfUnits = GimpUnit.NumberOfUnits;
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, "%", "abbr", 
				   "foo", "foos");
      Assert.AreEqual(numberOfUnits + 1, GimpUnit.NumberOfUnits);
      Assert.AreEqual((int) Unit.End, GimpUnit.NumberOfBuiltInUnits);
    }

    [Test]
    public void DeletionFlag()
    {
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, "%", "abbr", 
				   "foo", "foos");
      Assert.IsTrue(unit.DeletionFlag);
      unit.DeletionFlag = false;
      Assert.IsFalse(unit.DeletionFlag);
      unit.DeletionFlag = true;
      Assert.IsTrue(unit.DeletionFlag);
    }

    [Test]
    public void Factor()
    {
      double factor = 3.1415927;
      GimpUnit unit = new GimpUnit("foo", factor, 3, "%", "abbr", 
				   "foo", "foos");
      Assert.AreEqual(factor, unit.Factor);
    }

    [Test]
    public void Digits()
    {
      int digits = 3;
      GimpUnit unit = new GimpUnit("foo", 3.1415927, digits, "%", "abbr", 
				   "foo", "foos");
      Assert.AreEqual(digits, unit.Digits);
    }

    [Test]
    public void Identifier()
    {
      string identifier = "foo";
      GimpUnit unit = new GimpUnit(identifier, 3.1415927, 3, "%", "abbr", 
				   "foo", "foos");
      Assert.AreEqual(identifier, unit.Identifier);
    }

    [Test]
    public void Symbol()
    {
      string symbol = "%";
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, symbol, "abbr", 
				   "foo", "foos");
      Assert.AreEqual(symbol, unit.Symbol);
    }

    [Test]
    public void Abbreviation()
    {
      string abbreviation = "abbr";
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, "%", abbreviation,
				   "foo", "foos");
      Assert.AreEqual(abbreviation, unit.Abbreviation);
    }

    [Test]
    public void Singular()
    {
      string singular = "foo";
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, "%", "abbr",
				   singular, "foos");
      Assert.AreEqual(singular, unit.Singular);
    }

    [Test]
    public void Plural()
    {
      string plural = "foos";
      GimpUnit unit = new GimpUnit("foo", 3.1415927, 3, "%", "abbr",
				   "foo", plural);
      Assert.AreEqual(plural, unit.Plural);
    }
  }
}
