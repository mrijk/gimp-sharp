// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestPaletteList.cs
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
  public class TestPaletteList
  {
    [Test]
    public void CountAll()
    {
      var palettes = new PaletteList(null);
      Assert.IsTrue(palettes.Count > 0);
    }

    [Test]
    public void CountAllTwo()
    {
      var palettes = new PaletteList();
      Assert.IsTrue(palettes.Count > 0);
    }

    [Test]
    public void CountNone()
    {
      // Test for non-existing palettes
      var palettes = new PaletteList("nonsense");
      Assert.AreEqual(0, palettes.Count);
    }

    [Test]
    public void GetEnumerator()
    {
      var palettes = new PaletteList();
      int count = 0;
      foreach (var palette in palettes)
	{
	  count++;
	}
      Assert.AreEqual(count, palettes.Count);
    }

    [Test]
    public void ForEach()
    {
      var palettes = new PaletteList();
      int count = 0;
      palettes.ForEach(palette => count++);
      Assert.AreEqual(count, palettes.Count);
    }
  }
}
