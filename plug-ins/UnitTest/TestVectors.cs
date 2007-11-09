// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestVectors.cs
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
  public class TestVectors
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      Layer layer = new Layer(_image, "test", _width, _height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void Constructor()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Assert.AreEqual("firstVector", vectors.Name);
    }

    [Test]
    public void IsValid()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Assert.IsTrue(vectors.IsValid);
    }

    [Test]
    public void GetImage()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      _image.AddVectors(vectors, -1);
      Assert.AreEqual(_image, vectors.Image);
    }    

    [Test]
    public void GetSetLinked()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Assert.IsFalse(vectors.Linked);
      vectors.Linked = true;
      Assert.IsTrue(vectors.Linked);
    }

    [Test]
    public void GetSetTattoo()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Tattoo tattoo =  = new Tattoo(13);
      vectors.Tattoo = tattoo;
      Assert.AreEqual(tattoo, vectors.Tattoo);
    }

    [Test]
    public void GetSetVisible()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Assert.IsFalse(vectors.Visible);
      vectors.Visible = true;
      Assert.IsTrue(vectors.Visible);
    }

    [Test]
    public void GetSetName()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      vectors.Name = "renamedVector";
      Assert.AreEqual("renamedVector", vectors.Name);
    }

    [Test]
    public void ParasiteAttach()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Parasite parasite = new Parasite("foo", 0, 13);
      vectors.ParasiteAttach(parasite);
      Assert.AreEqual(1, vectors.ParasiteList.Count);
    }

    [Test]
    public void ParasiteDetach()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Parasite parasite = new Parasite("foo", 0, 13);
      vectors.ParasiteAttach(parasite);
      vectors.ParasiteDetach(parasite);
      Assert.AreEqual(0, vectors.ParasiteList.Count);
    }

    [Test]
    public void ParasiteFind()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      Parasite parasite = new Parasite("foo", 0, 13);
      vectors.ParasiteAttach(parasite);
      Parasite found = vectors.ParasiteFind("foo");
      Assert.AreEqual(parasite, found);
      Assert.IsNull(vectors.ParasiteFind("bar"));
    }

    [Test]
    public void Add()
    {
      Vectors vectors = new Vectors(_image, "firstVector");
      _image.AddVectors(vectors, -1);
      Assert.IsTrue(vectors.IsValid);
    }
  }
}
