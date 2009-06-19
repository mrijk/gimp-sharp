// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestProcedure.cs
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
  public class TestProcedure
  {
    [Test]
    public void New()
    {
      string name = "name";
      var procedure = new Procedure(name, "blurb", "help", 
				    "author", "copyright", 
				    "date", "menu_path", 
				    "RGB", null, null);
      Assert.AreEqual(name, procedure.Name);
    }

    [Test]
    public void Exists()
    {
      Assert.IsFalse(Procedure.Exists("non-existant"));
      Assert.IsTrue(Procedure.Exists("gimp-version"));
    }

    [Test]
    public void Run()
    {
      var procedure = new Procedure("plug_in_pixelize");
      // procedure.Run(image, drawable, 10);
    }
  }
}
