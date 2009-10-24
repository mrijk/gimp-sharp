// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestImage.cs
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
  public class TestImage
  {
    int _width = 64;
    int _height = 128;
    ImageBaseType _type = ImageBaseType.Rgb;

    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, _type);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void NewImage()
    {
      // Fix me: test if there is a new image
    }

    [Test]
    public void WidthHeightImage()
    {
      Assert.AreEqual(_width, _image.Width);
      Assert.AreEqual(_height, _image.Height);
      Assert.AreEqual(_type, _image.BaseType);
    }

    [Test]
    public void Duplicate()
    {
      var copy = new Image(_image);
      Assert.AreEqual(_image.Width, copy.Width);
      Assert.AreEqual(_image.Height, copy.Height);
      Assert.AreEqual(_image.BaseType, copy.BaseType);
      copy.Delete();
    }

    [Test]
    public void Rotate()
    {
      _image.Rotate(RotationType.Rotate90);
      Assert.AreEqual(_width, _image.Height);
      Assert.AreEqual(_height, _image.Width);
    }

    [Test]
    public void ResizeOne()
    {
      _image.Resize(100, 100, 0, 0);
      Assert.AreEqual(100, _image.Width);
      Assert.AreEqual(100, _image.Height);
    }

    [Test]
    public void ResizeTwo()
    {
      _image.Resize(new Dimensions(99, 101), new Offset(0, 0));
      Assert.AreEqual(99, _image.Width);
      Assert.AreEqual(101, _image.Height);
    }

    [Test]
    public void ScaleOne()
    {
      _image.Scale(100, 100);
      Assert.AreEqual(100, _image.Width);
      Assert.AreEqual(100, _image.Height);
    }

    [Test]
    public void ScaleTwo()
    {
      _image.Scale(new Dimensions(99, 101));
      Assert.AreEqual(99, _image.Width);
      Assert.AreEqual(101, _image.Height);
    }

    [Test]
    public void CropOne()
    {
      _image.Crop(32, 33, 0, 0);
      Assert.AreEqual(32, _image.Width);
      Assert.AreEqual(33, _image.Height);
    }

    [Test]
    public void CropTwo()
    {
      _image.Crop(new Dimensions(32, 33), new Offset(0, 0));
      Assert.AreEqual(32, _image.Width);
      Assert.AreEqual(33, _image.Height);
    }

    [Test]
    public void CropThree()
    {
      var rectangle = new Rectangle(10, 10, 20, 20);
      _image.Crop(rectangle);
      Assert.AreEqual(rectangle.Width, _image.Width);
      Assert.AreEqual(rectangle.Height, _image.Height);
    }

    [Test]
    public void ActiveLayer()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);

      var active = _image.ActiveLayer;
      Assert.AreEqual(layer.Name, active.Name);
    }

    [Test]
    public void PickCorrelateLayer()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      var c = new Coordinate<int>(_width / 2, _height / 2);
      Assert.IsNull(_image.PickCorrelateLayer(c));

      _image.AddLayer(layer, 0);

      var picked = _image.PickCorrelateLayer(c);
      Assert.AreEqual(layer, picked);
    }

    // [Test]
    public void Channels()
    {
      ChannelList channels = _image.Channels;
    }
  }
}
