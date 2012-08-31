// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// TestLayer.cs
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
  public class TestLayer
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
    public void ConstructorOne()
    {
      var layer = new Layer(_image, "test", _width, _height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      Assert.AreEqual(ImageType.Rgb, layer.Type);
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void ConstructorTwo()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb, 100, 
			    LayerModeEffects.Normal);
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
      Assert.AreEqual(_image.Width, layer.Width);
      Assert.AreEqual(_image.Height, layer.Height);
    }

    [Test]
    public void ConstructorThree()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
      Assert.AreEqual(_image.Width, layer.Width);
      Assert.AreEqual(_image.Height, layer.Height);
      Assert.AreEqual(100, layer.Opacity);
      Assert.AreEqual(LayerModeEffects.Normal, layer.Mode);
    }

    [Test]
    public void ConstructorFour()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);

      var image = new Image(_width, _height, ImageBaseType.Rgb);
      layer = new Layer(_image, image, "new_from_visible");
      image.InsertLayer(layer, 0);

      Assert.AreEqual(1, image.Layers.Count);
      Assert.AreEqual(image.Width, layer.Width);
      Assert.AreEqual(image.Height, layer.Height);
      Assert.AreEqual(100, layer.Opacity);
      Assert.AreEqual(LayerModeEffects.Normal, layer.Mode);

      image.Delete();
    }

    [Test]
    public void AddConstructor()
    {
      var image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};
      Assert.AreEqual(1, image.Layers.Count);
      image.Delete();
    }

    [Test]
    public void CopyConstructor()
    {
      var layerOne = new Layer(_image, "test", ImageType.Rgb);
      var layerTwo = new Layer(layerOne);
      Assert.AreEqual(layerOne.Width, layerTwo.Width);
    }

    [Test]
    public void ScaleOne()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
      layer.Scale(2 * _width, 2 * _height, false);
      Assert.AreEqual(2 * _width, layer.Width);
      Assert.AreEqual(2 * _height, layer.Height);
    }

    [Test]
    public void ScaleTwo()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
      layer.Scale(new Dimensions(2 * _width, 2 * _height), false,
		  InterpolationType.None);
      Assert.AreEqual(2 * _width, layer.Width);
      Assert.AreEqual(2 * _height, layer.Height);
    }

    [Test]
    public void Resize()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
      layer.Resize(2 * _width, 2 * _height, 0, 0);
      Assert.AreEqual(2 * _width, layer.Width);
      Assert.AreEqual(2 * _height, layer.Height);
    }

    [Test]
    public void ResizeToImageSize()
    {
      var layer = new Layer(_image, "test", _width / 2, _height / 2,
			    ImageType.Rgb, 100, 
			    LayerModeEffects.Normal);
      _image.InsertLayer(layer, 0);
      layer.ResizeToImageSize();
      Assert.AreEqual(_image.Width, layer.Width);
      Assert.AreEqual(_image.Height, layer.Height);
    }

    [Test]
    public void Translate()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
      layer.Translate(-10, 10);

      var offset = layer.Offsets;
      Assert.AreEqual(-10, offset.X);
      Assert.AreEqual(10, offset.Y);
    }

    [Test]
    public void AddAlpha()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);

      int bpp = layer.Bpp;
      Assert.AreEqual(3, bpp);

      layer.AddAlpha();
      Assert.AreEqual(ImageType.Rgba, layer.Type);
      Assert.AreEqual(bpp + 1, layer.Bpp);
    }

    [Test]
    public void Flatten()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(ImageType.Rgba, layer.Type);
      layer.Flatten();
      Assert.AreEqual(ImageType.Rgb, layer.Type);
    }

    [Test]
    public void SetOffsets()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);

      var offset = new Offset(13, 14);

      layer.Offsets = offset;
      Assert.AreEqual(offset, layer.Offsets);
    }
    
    [Test]
    public void CreateMask()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      Assert.AreEqual(null, layer.Mask);

      var mask = layer.CreateMask(AddMaskType.White);
      layer.Mask = mask;
      Assert.AreEqual(mask, layer.Mask);
    }

    [Test]
    public void RemoveMask()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      var mask = layer.CreateMask(AddMaskType.White);
      layer.Mask = mask;
      Assert.AreEqual(mask, layer.Mask);

      layer.RemoveMask(MaskApplyMode.Discard);
      Assert.AreEqual(null, layer.Mask);
    }

    [Test]
    public void NewFromDrawable()
    {
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);

      var copy = new Layer(layer, _image);
      Assert.AreEqual(layer.Dimensions, copy.Dimensions);
    }

    [Test]
    public void LockAlpha()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.LockAlpha = true;
      Assert.IsTrue(layer.LockAlpha);

      layer.LockAlpha = false;
      Assert.IsFalse(layer.LockAlpha);
    }

    [Test]
    public void ApplyMask()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.Mask = layer.CreateMask(AddMaskType.White);
      layer.ApplyMask = true;
      Assert.IsTrue(layer.ApplyMask);

      layer.ApplyMask = false;
      Assert.IsFalse(layer.ApplyMask);
    }

    [Test]
    public void ShowMask()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.Mask = layer.CreateMask(AddMaskType.White);
      layer.ShowMask = true;
      Assert.IsTrue(layer.ShowMask);

      layer.ShowMask = false;
      Assert.IsFalse(layer.ShowMask);
    }

    [Test]
    public void EditMask()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.Mask = layer.CreateMask(AddMaskType.White);
      layer.EditMask = true;
      Assert.IsTrue(layer.EditMask);

      layer.EditMask = false;
      Assert.IsFalse(layer.EditMask);
    }

    [Test]
    public void Opacity()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.Opacity = 3.14;
      Assert.AreEqual(3.14, layer.Opacity, 0.0001);
    }

    [Test]
    public void Mode()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      layer.Mode = LayerModeEffects.Dodge;
      Assert.AreEqual(LayerModeEffects.Dodge, layer.Mode);
    }

    [Test]
    public void IsFloatingSelection()
    {
      var layer = new Layer(_image, "test", ImageType.Rgba);
      layer.AddAlpha();
      _image.InsertLayer(layer, 0);

      Assert.IsFalse(layer.IsFloatingSelection);
    }
  }
}
