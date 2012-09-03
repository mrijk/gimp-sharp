// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
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
      var version = Gimp.Version;
      Assert.IsNotNull(version);
    }

    [Test]
    public void MajorVersion()
    {
      var version = Gimp.Version;
      Assert.IsTrue(version.Major >= 2);
    }

    // [Test]
    public void MinorVersion()
    {
      var version = Gimp.Version;
      Assert.IsTrue(version.Minor >= 0);
    }

    // [Test]
    public void MicroVersion()
    {
      var version = Gimp.Version;
      Assert.IsTrue(version.Micro >= 0);
    }

    [Test]
    public void PID()
    {
      Assert.IsTrue(Gimp.PID > 0);
    }

    [Test]
    public void AttachParasite()
    {
      var parasite = new Parasite("foo", 0, 13);
      Gimp.AttachParasite(parasite);
      Assert.AreEqual(1, Gimp.ParasiteList.Count);
      Gimp.DetachParasite(parasite);
    }

    [Test]
    public void DetachParasite()
    {
      var parasite = new Parasite("foo", 0, 13);
      Gimp.AttachParasite(parasite);
      Assert.AreEqual(1, Gimp.ParasiteList.Count);
      Gimp.DetachParasite(parasite);
      Assert.AreEqual(0, Gimp.ParasiteList.Count);
    }

    [Test]
    public void GetParasite()
    {
      Assert.IsNull(Gimp.GetParasite("foo"));
      var parasite = new Parasite("foo", 0, 13);
      Gimp.AttachParasite(parasite);
      var found = Gimp.GetParasite("foo");
      Assert.IsNotNull(found);
      Assert.AreEqual(parasite, found);
      Gimp.DetachParasite(parasite);
    }

    [Test]
    public void MonitorResolution()
    {
      var resolution = Gimp.MonitorResolution;
      Assert.IsTrue(resolution.X > 0);
      Assert.IsTrue(resolution.Y > 0);
    }

    [Test]
    public void DefaultComment()
    {
      string comment = Gimp.DefaultComment;
      Assert.IsNotNull(comment);
    }

    [Test]
    public void ThemeDirectory()
    {
      Assert.IsNotNull(Gimp.ThemeDirectory);
    }

    [Test]
    public void LocaleDirectory()
    {
      Assert.IsNotNull(Gimp.LocaleDirectory);
    }

    [Test]
    public void PluginDirectory()
    {
      Assert.IsNotNull(Gimp.PluginDirectory);
    }
  }
}
