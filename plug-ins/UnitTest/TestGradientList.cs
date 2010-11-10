// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestGradientList.cs
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
  public class TestGradientList
  {
    [Test]
    public void CountAll()
    {
      var gradients = new GradientList(null);
      Assert.IsTrue(gradients.Count > 0);
    }

    [Test]
    public void CountAllTwo()
    {
      var gradients = new GradientList();
      Assert.IsTrue(gradients.Count > 0);
    }

    [Test]
    public void CountNone()
    {
      // Test for non-existing gradients
      var gradients = new GradientList("nonsense");
      Assert.AreEqual(0, gradients.Count);
    }

    [Test]
    public void GetEnumerator()
    {
      var gradients = new GradientList();
      int count = 0;
      foreach (var gradient in gradients)
	{
	  count++;
	}
      Assert.AreEqual(count, gradients.Count);
    }

    [Test]
    public void ForEach()
    {
      var gradients = new GradientList();
      int count = 0;
      gradients.ForEach(gradient => count++);
      Assert.AreEqual(count, gradients.Count);
    }
  }
}
