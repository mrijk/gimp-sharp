// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestParasite.cs
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
  public class TestParasite
  {
    [Test]
    public void Constructor()
    {
      string name = "parasite";
      // string foo = "foo";
      int data = 13;
      Parasite parasite = new Parasite(name, 0, data);
      Assert.AreEqual(name, parasite.Name);
    }

    [Test]
    public void Data()
    {
      string name = "parasite";
      int data = 13;
      Parasite parasite = new Parasite(name, 0, data);
      // int value = (int) parasite.Data;
    }

    [Test]
    public void DataSize()
    {
      string name = "parasite";
      int data = 13;
      Parasite parasite = new Parasite(name, 0, data);

      long size = parasite.DataSize;
      Assert.AreEqual(sizeof(int), size);
    }

    [Test]
    public void Equals()
    {
      int data = 13;
      Parasite parasite1 = new Parasite("parasite", 0, data);
      Parasite parasite2 = new Parasite("parasite", 0, data);
      Assert.IsTrue(parasite1.Equals(parasite2));

      data = 14;
      Parasite parasite3 = new Parasite("parasite", 0, data);
      Assert.IsFalse(parasite1.Equals(parasite3));
    }
  }
}
