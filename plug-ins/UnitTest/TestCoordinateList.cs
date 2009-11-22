// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestCoordinateList.cs
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
  public class TestCoordinateList
  {
    [Test]
    public void Constructor()
    {
      var list = new CoordinateList<int>();
      Assert.AreEqual(0, list.Count);      
    }

    [Test]
    public void Enumerator()
    {
      var list = new CoordinateList<int>();
      for (int i = 0; i < 10; i++)
	{
	  list.Add(new Coordinate<int>(i, 2 * i));
	}

      int count = 0;
      foreach (var c in list)
	{
	  Assert.IsTrue(c == new Coordinate<int>(count, 2 * count));
	  count++;
	}
      Assert.AreEqual(list.Count, count);
    }

    [Test]
    public void ForEach()
    {
      var list = new CoordinateList<int>();
      for (int i = 0; i < 10; i++)
	{
	  list.Add(new Coordinate<int>(i, 2 * i));
	}

      int count = 0;
      list.ForEach(c =>
	{
	  Assert.IsTrue(c == new Coordinate<int>(count, 2 * count));
	  count++;
	});
      Assert.AreEqual(list.Count, count);
    }

    [Test]
    public void Add()
    {
      var list = new CoordinateList<int>();
      var c = new Coordinate<int>(13, 14);
      list.Add(c);
      Assert.AreEqual(1, list.Count);      

      list.Add(23, 24);
      Assert.AreEqual(2, list.Count);      
    }

    [Test]
    public void Equals()
    {
      var list1 = new CoordinateList<int>();
      var list2 = new CoordinateList<int>();
      var c = new Coordinate<int>(13, 14);

      list1.Add(c);
      Assert.IsFalse(list1.Equals(list2));

      list2.Add(c);
      Assert.IsTrue(list1.Equals(list2));
    }

    [Test]
    public void ToArray()
    {
      var list = new CoordinateList<int>();
      var c = new Coordinate<int>(13, 14);

      int[] array = list.ToArray();
      Assert.IsNull(array);

      list.Add(c);
      array = list.ToArray();
      Assert.AreEqual(2, array.Length);
    }
  }
}
