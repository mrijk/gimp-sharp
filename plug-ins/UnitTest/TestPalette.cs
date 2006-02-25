// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestPalette.cs
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
  public class TestPalette
  {
    [Test]
    public void NewPalette()
    {
      Palette palette = new Palette("UnitTestPalette");
      // TODO: Check if number of palettes has increased by 1
      palette.Delete();
    }

    [Test]
    public void Rename()
    {
      Palette palette = new Palette("UnitTestPalette");
      string name = palette.Rename("UnitTestPaletteTwo");
      Assert.AreEqual(name, "UnitTestPaletteTwo");
      Assert.IsTrue(palette.Name == "UnitTestPaletteTwo");
      palette.Delete();
    }

    [Test]
    public void GetInfo()
    {
      Palette palette = new Palette("UnitTestPalette");
      int num_colors;
      palette.GetInfo(out num_colors);
      Assert.AreEqual(num_colors, 0);
      palette.Delete();
    }

    [Test]
    public void AddEntry()
    {
      Palette palette = new Palette("UnitTestPalette");
      palette.AddEntry("black", new RGB(0, 0, 0));
      Assert.AreEqual(palette.NumberOfColors, 1);
      palette.Delete();
    }

    [Test]
    public void DeleteEntry()
    {
      Palette palette = new Palette("UnitTestPalette");
      palette.AddEntry("black", new RGB(0, 0, 0));
      palette.DeleteEntry(0);
      Assert.AreEqual(palette.NumberOfColors, 0);
      palette.Delete();
    }

    [Test]
    public void This()
    {
      Palette palette = new Palette("UnitTestPalette");
      palette.AddEntry("black", new RGB(0, 0, 0));
      PaletteEntry black = palette[0];
      Assert.IsNotNull(black);
      palette.Delete();
    }

    [Test]
    [ExpectedException(typeof(Exception))]
    public void ThisOutOfRange()
    {
      Palette palette = new Palette("UnitTestPalette");
      palette.AddEntry("black", new RGB(0, 0, 0));
      // TODO: no range check yet
      PaletteEntry white = palette[1];
      try {
	string name = white.Name;
      }
      catch {
	palette.Delete();
	throw;
      }
    }

    [Test]
    public void GetEnumerator()
    {
      Palette palette = new Palette("UnitTestPalette");
      palette.AddEntry("black", new RGB(0, 0, 0));
      int count = 0;
      foreach (PaletteEntry entry in palette)
	{
	  count++;
	  Assert.AreEqual(entry.Name, "black");
	}
      Assert.AreEqual(count, 1);
      palette.Delete();
    }
  }
}
