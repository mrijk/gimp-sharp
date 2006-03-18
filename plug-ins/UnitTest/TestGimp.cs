// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestGimp.cs
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
  public class TestGimp
  {
    [Test]
    public void Version()
    {
      Version version = Gimp.Version;
      Assert.IsTrue(version != null);
    }

    // [Test]
    public void MajorVersion()
    {
      Version version = Gimp.Version;
      Assert.IsTrue(version.Major >= 2);
    }

    // [Test]
    public void MinorVersion()
    {
      Version version = Gimp.Version;
      Assert.IsTrue(version.Minor >= 0);
    }

    // [Test]
    public void MicroVersion()
    {
      Version version = Gimp.Version;
      Assert.IsTrue(version.Micro >= 0);
    }

    [Test]
    public void MonitorResolution()
    {
      double xres, yres;
      Gimp.GetMonitorResolution(out xres, out yres);
      Assert.IsTrue(xres > 0);
      Assert.IsTrue(yres > 0);
    }

    [Test]
    public void DefaultComment()
    {
      string comment = Gimp.DefaultComment;
      Assert.IsTrue(comment != null);
    }

    [Test]
    public void ThemeDirectory()
    {
      string directory = Gimp.ThemeDirectory;
      Assert.IsTrue(directory != null);
    }

    [Test]
    public void RcQueryOne()
    {
      string value = Gimp.RcQuery("nonsense");
      Assert.IsTrue(value == null);
    }

    [Test]
    public void RcQueryTwo()
    {
      string value = Gimp.RcQuery("show-tips");
      Assert.IsTrue(value != null);
    }

    [Test]
    public void RcSet()
    {
      Gimp.RcSet("foo", "bar");
      string value = Gimp.RcQuery("foo");
      Assert.AreEqual("bar", value);
    }
  }
}
