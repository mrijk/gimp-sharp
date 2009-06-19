// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestTextLayer.cs
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
  public class TestTextLayer
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
      var layer = new TextLayer(_image, "Hello World", "Sans", 
				new FontSize(32, Unit.Pixel));
      _image.AddLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void GetSetText()
    {
      var layer = new TextLayer(_image, "Hello World", "Sans", 
				new FontSize(32, Unit.Pixel));
      _image.AddLayer(layer, 0);
      Assert.AreEqual("Hello World", layer.Text);
      var text = "Something else";
      layer.Text = text;
      Assert.AreEqual(text, layer.Text);
    }

    [Test]
    public void GetSetFont()
    {
      var layer = new TextLayer(_image, "Hello World", "Sans", 
				new FontSize(32, Unit.Pixel));
      _image.AddLayer(layer, 0);
      Assert.AreEqual("Sans", layer.Font);
      var font = "Serif";
      layer.Font = font;
      Assert.AreEqual(font, layer.Font);
    }

    [Test]
    public void GetSetFontSize()
    {
      var fontSize = new FontSize(32, Unit.Pixel);
      var layer = new TextLayer(_image, "Hello World", "Sans", fontSize);
      _image.AddLayer(layer, 0);
      Assert.AreEqual(fontSize, layer.FontSize);
      var newFontSize = new FontSize(1, Unit.Inch);
      layer.FontSize = newFontSize;
      Assert.AreEqual(newFontSize, layer.FontSize);
    }

    [Test]
    public void GetSetAntialias()
    {
      var fontSize = new FontSize(32, Unit.Pixel);
      var layer = new TextLayer(_image, "Hello World", "Sans", fontSize);
      _image.AddLayer(layer, 0);
      layer.Antialias = true;
      Assert.IsTrue(layer.Antialias);
      layer.Antialias = false;
      Assert.IsFalse(layer.Antialias);
    }

    [Test]
    public void GetSetKerning()
    {
      var fontSize = new FontSize(32, Unit.Pixel);
      var layer = new TextLayer(_image, "Hello World", "Sans", fontSize);
      _image.AddLayer(layer, 0);
      layer.Kerning = true;
      Assert.IsTrue(layer.Kerning);
      layer.Kerning = false;
      Assert.IsFalse(layer.Kerning);
    }
  }
}
