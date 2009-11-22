// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestPatternList
  {
    [Test]
    public void CountAll()
    {
      var patterns = new PatternList(null);
      Assert.IsTrue(patterns.Count > 0);
    }

    [Test]
    public void CountAllTwo()
    {
      var patterns = new PatternList();
      Assert.IsTrue(patterns.Count > 0);
    }

    [Test]
    public void CountNone()
    {
      // Test for non-existing patterns
      var patterns = new PatternList("nonsense");
      Assert.AreEqual(0, patterns.Count);
    }

    [Test]
    public void GetEnumerator()
    {
      var patterns = new PatternList();
      int count = 0;
      foreach (var pattern in patterns)
	{
	  count++;
	}
      Assert.AreEqual(count, patterns.Count);
    }

    [Test]
    public void ForEach()
    {
      var patterns = new PatternList();
      int count = 0;
      patterns.ForEach(pattern => count++);
      Assert.AreEqual(count, patterns.Count);
    }
  }
}
