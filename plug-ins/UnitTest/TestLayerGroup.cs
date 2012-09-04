// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// TestLayerGroup.cs
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
  public class TestLayerGroup
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void CreateLayerGroup()
    {
      var group = new LayerGroup(_image);
      Assert.IsTrue(group.IsGroup);
      Assert.IsTrue(group.IsLayer);
      Assert.AreEqual(_image, group.Image);
    }

    [Test]
    public void Parent()
    {
      var group = new LayerGroup(_image);
      _image.InsertLayer(group, null, 0);

      var layer = new Layer(_image, "test1", ImageType.Rgb);
      _image.InsertLayer(layer, group, 0);

      Assert.IsTrue(layer.Parent is LayerGroup);
      Assert.IsNull(layer.Parent.Parent);
    }

    [Test]
    public void GetChildren()
    {
      var group = new LayerGroup(_image);
      Assert.AreEqual(0, group.Children.Count);
      _image.InsertLayer(group, null, 0);

      var layer = new Layer(_image, "test1", ImageType.Rgb);
      _image.InsertLayer(layer, group, 0);
      Assert.AreEqual(1, group.Children.Count);

      Assert.IsTrue(group.Children[0] is Layer);
    }

    [Test]
    public void NestedLayerGroupOne()
    {
      var group1 = new LayerGroup(_image) {Name = "Foo"};
     _image.InsertLayer(group1, null, 0);      

      var group2 = new LayerGroup(_image) {Name = "Bar"};
      _image.InsertLayer(group2, group1, 0);

      Assert.IsNotNull(group2.Parent);
      Assert.AreEqual(group1, group2.Parent);
    }

    [Test]
    public void NestedLayerGroupTwo()
    {
      var group1 = new LayerGroup(_image) {Name = "Foo"};
     _image.InsertLayer(group1, 0);

      var group2 = new LayerGroup(_image) {Name = "Bar"};
      group1.Insert(group2, 0);

      Assert.IsNotNull(group2.Parent);
      Assert.AreEqual(group1, group2.Parent);
    }
  }
}
