// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestPattern.cs
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
  public class TestPattern
  {
    [Test]
    public void GetInfo()
    {
      PatternList patterns = new PatternList(null);
      foreach (Pattern pattern in patterns)
	{
	  int width, height, bpp;
	  pattern.GetInfo(out width, out height, out bpp);
	  Assert.IsTrue(width > 0);
	  Assert.IsTrue(height > 0);
	  Assert.IsTrue(bpp > 0);
	}
    }

    [Test]
    public void GetPixels()
    {
      PatternList patterns = new PatternList(null);
      foreach (Pattern pattern in patterns)
	{
	  int width, height, bpp, numBytes;
	  pattern.GetPixels(out width, out height, out bpp, out numBytes);
	  Assert.AreEqual(width * height * bpp, numBytes);
	}
    }
  }
}
