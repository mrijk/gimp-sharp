// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestPatternList.cs
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
  public class TestPatternList
  {
    [Test]
    public void CountAll()
    {
      PatternList patterns = new PatternList(null);
      Assert.IsTrue(patterns.Count > 0);
    }

    [Test]
    public void CountNone()
    {
      // Test for non-existing patterns
      PatternList patterns = new PatternList("nonsense");
      Assert.IsTrue(patterns.Count == 0);
    }

    [Test]
    public void GetEnumerator()
    {
      PatternList patterns = new PatternList(null);
      int count = 0;
      foreach (Pattern pattern in patterns)
	{
	  count++;
	}
      Assert.IsTrue(patterns.Count == count);
    }
  }
}
