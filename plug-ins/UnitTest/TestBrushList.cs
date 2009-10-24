// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestBrushList.cs
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
  public class TestBrushList
  {
    [Test]
    public void CountAll()
    {
      var brushes = new BrushList(null);
      Assert.IsTrue(brushes.Count > 0);
    }

    [Test]
    public void CountNone()
    {
      // Test for non-existing brushes
      var brushes = new BrushList("nonsense");
      Assert.IsTrue(brushes.Count == 0);
    }

    [Test]
    public void GetEnumerator()
    {
      BrushList brushes = new BrushList(null);
      int count = 0;
      foreach (Brush brush in brushes)
	{
	  Assert.IsNotNull(brush.Name);
	  count++;
	}
      Assert.IsTrue(brushes.Count == count);
    }

    [Test]
    public void ForEach()
    {
      BrushList brushes = new BrushList(null);
      int count = 0;
      brushes.ForEach(brush => count++);
      Assert.IsTrue(brushes.Count == count);
    }
  }
}
