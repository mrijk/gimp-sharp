// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestProceduralDb.cs
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
using System.Linq;

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestProceduralDb
  {
    [Test]
    public void TempName()
    {
      string tempName = ProceduralDb.TempName();
      Assert.IsNotNull(tempName);
      Assert.Greater(tempName.Length, 0);
    }

    [Test]
    public void SetGetData()
    {
      string identifier = ProceduralDb.TempName();
      var encoding = new System.Text.ASCIIEncoding();

      const string testString = "This is a test";
      var data = encoding.GetBytes(testString);
      ProceduralDb.SetData(identifier, data);

      data = ProceduralDb.GetData(identifier);
      Assert.AreEqual(testString, encoding.GetString(data));
    }

    [Test]
    public void GetDataSize()
    {
      string identifier = ProceduralDb.TempName();
      var encoding = new System.Text.ASCIIEncoding();

      const string testString = "This is a test";
      var data = encoding.GetBytes(testString);
      ProceduralDb.SetData(identifier, data);

      Assert.AreEqual(data.Length, ProceduralDb.GetDataSize(identifier));
    }

    [Test]
    public void QueryOne()
    {
      var procedures = ProceduralDb.Query(".*", ".*", ".*", ".*", ".*", ".*",
					  ".*");
      Assert.Greater(procedures.Count, 0);
      Assert.IsTrue(procedures.All(p => ProceduralDb.ProcExists(p)));
    }

    [Test]
    public void QueryTwo()
    {
      var procedures = ProceduralDb.Query("nonsense", ".*", ".*", ".*", ".*", 
					  ".*", ".*");
      Assert.AreEqual(0, procedures.Count);
    }

    [Test]
    public void Exists()
    {
      Assert.IsFalse(ProceduralDb.ProcExists("nonsense"));
      Assert.IsTrue(ProceduralDb.ProcExists("gimp-image-new"));
    }
  }
}
