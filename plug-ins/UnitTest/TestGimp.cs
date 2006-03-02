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
      string version = Gimp.Version;
      Assert.IsTrue(version.Length > 0);
    }

    [Test]
    public void MajorVersion()
    {
      Assert.IsTrue(Gimp.MajorVersion >= 2);
    }

    [Test]
    public void MinorVersion()
    {
      Assert.IsTrue(Gimp.MinorVersion >= 0);
    }

    [Test]
    public void MicroVersion()
    {
      Assert.IsTrue(Gimp.MicroVersion >= 0);
    }

    [Test]
    public void VersionCombined()
    {
      string version = string.Format("{0}.{1}.{2}", Gimp.MajorVersion, 
				     Gimp.MinorVersion, Gimp.MicroVersion);
      Assert.AreEqual(Gimp.Version, version);
    }
  }
}
