// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
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
    public void NewGrayImage()
    {
      var image = new Image(_width, _height, ImageBaseType.Rgb);
      Assert.AreEqual(ImageBaseType.Rgb, image.BaseType);
      image.Delete();
    }

    [Test]
    public void NewIndexedImage()
    {
      var image = new Image(_width, _height, ImageBaseType.Indexed);
      Assert.AreEqual(ImageBaseType.Indexed, image.BaseType);
      image.Delete();
    }

    [Test]
    public void WidthHeightImage()
    {
      Assert.AreEqual(_width, _image.Width);
      Assert.AreEqual(_height, _image.Height);
      Assert.AreEqual(_type, _image.BaseType);
    }

    [Test]
    public void Bounds()
    {
      Assert.AreEqual(new Rectangle(0, 0, _width, _height), _image.Bounds);
    }

    [Test]
    public void Dimensionss()
    {
      Assert.AreEqual(new Dimensions(_width, _height), _image.Dimensions);
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

    [Test]
    public void ActiveDrawable()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);
      Assert.AreEqual(layer, _image.ActiveDrawable);
    }

    [Test]
    public void RemoveLayer()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
      _image.RemoveLayer(layer);
      Assert.AreEqual(0, _image.Layers.Count);
    }

    [Test]
    public void RaiseLayer()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(1, layer1.Position);
      Assert.AreEqual(0, layer2.Position);
      _image.RaiseLayer(layer1);
      Assert.AreEqual(0, layer1.Position);
      Assert.AreEqual(1, layer2.Position);
    }

    [Test]
    public void LowerLayer()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(1, layer1.Position);
      Assert.AreEqual(0, layer2.Position);
      _image.LowerLayer(layer2);
      Assert.AreEqual(0, layer1.Position);
      Assert.AreEqual(1, layer2.Position);
    }

    [Test]
    public void RaiseLayerToTop()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(1, layer1.Position);
      Assert.AreEqual(0, layer2.Position);
      _image.RaiseLayerToTop(layer1);
      Assert.AreEqual(0, layer1.Position);
      Assert.AreEqual(1, layer2.Position);
    }

    [Test]
    public void LowerLayerToBottom()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(1, layer1.Position);
      Assert.AreEqual(0, layer2.Position);
      _image.LowerLayerToBottom(layer2);
      Assert.AreEqual(0, layer1.Position);
      Assert.AreEqual(1, layer2.Position);
    }

    [Test]
    public void GetLayerPosition()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(1, _image.GetLayerPosition(layer1));
      Assert.AreEqual(0, _image.GetLayerPosition(layer2));
    }

    [Test]
    public void AddChannel()
    {
      int before = _image.Channels.Count;
      var channel = new Channel(_image, "test", new RGB(0, 255, 0));
      _image.AddChannel(channel);
      Assert.AreEqual(before + 1, _image.Channels.Count);
    }

    [Test]
    public void RemoveChannel()
    {
      int before = _image.Channels.Count;
      var channel = new Channel(_image, "test", new RGB(0, 255, 0));
      _image.AddChannel(channel);
      _image.RemoveChannel(channel);
      Assert.AreEqual(before, _image.Channels.Count);
    }

    [Test]
    public void RaiseChannel()
    {
      var channel1 = new Channel(_image, "test1", new RGB(0, 255, 0));
      _image.AddChannel(channel1);
      var channel2 = new Channel(_image, "test2", new RGB(0, 255, 0));
      _image.AddChannel(channel2);

      int position = channel1.Position;
      _image.RaiseChannel(channel1);
      Assert.AreEqual(position - 1, channel1.Position);
      Assert.AreEqual(position, channel2.Position);
    }

    [Test]
    public void LowerChannel()
    {
      var channel1 = new Channel(_image, "test1", new RGB(0, 255, 0));
      _image.AddChannel(channel1);
      var channel2 = new Channel(_image, "test2", new RGB(0, 255, 0));
      _image.AddChannel(channel2);

      int position = channel1.Position;
      _image.LowerChannel(channel2);
      Assert.AreEqual(position - 1, channel1.Position);
      Assert.AreEqual(position, channel2.Position);
    }

    [Test]
    public void GetChannelPosition()
    {
      var channel1 = new Channel(_image, "test1", new RGB(0, 255, 0));
      _image.AddChannel(channel1);
      var channel2 = new Channel(_image, "test2", new RGB(0, 255, 0));
      _image.AddChannel(channel2);
      Assert.AreEqual(1, _image.GetChannelPosition(channel1));
      Assert.AreEqual(0, _image.GetChannelPosition(channel2));
    }

    [Test]
    public void Flatten()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(2, _image.Layers.Count);
      _image.Flatten();
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void MergeVisibleLayers()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(2, _image.Layers.Count);
      _image.MergeVisibleLayers(MergeType.ClipToImage);
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void MergeDown()
    {
      var layer1 = new Layer(_image, "test1", ImageType.Rgb);
      _image.AddLayer(layer1, 0);
      var layer2 = new Layer(_image, "test2", ImageType.Rgb);
      _image.AddLayer(layer2, 0);

      Assert.AreEqual(2, _image.Layers.Count);
      _image.MergeDown(layer1, MergeType.ClipToImage);
      Assert.AreEqual(2, _image.Layers.Count);
      _image.MergeDown(layer2, MergeType.ClipToImage);
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void Filename()
    {
      string filename = "foobar.jpg";
      _image.Filename = filename;
      Assert.AreEqual(filename, _image.Filename);
    }

    [Test]
    public void GetThumbnail()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);
      var dimensions = new Dimensions(19, 23);
      var thumbnail = _image.GetThumbnail(dimensions, Transparency.KeepAlpha);
      int width = thumbnail.GetLength(1);
      int height = thumbnail.GetLength(0);
      Assert.AreEqual(dimensions, new Dimensions(width, height));
    }

    [Test]
    public void ConvertGrayscale()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);

      _image.ConvertGrayscale();
      // Fix me: next assert fails!
      Assert.Equals(ImageBaseType.Gray, _image.BaseType);
    }

    [Test]
    public void ConvertIndexed()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.AddLayer(layer, 0);

      _image.ConvertIndexed(ConvertDitherType.No, ConvertPaletteType.Web,
			    0, false, false, "");
      // Fixe me: next assert fails!
      Assert.Equals(ImageBaseType.Indexed, _image.BaseType);
    }
  }
}
