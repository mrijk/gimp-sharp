// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
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
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(1, _image.Layers.Count);
    }

    [Test]
    public void GetSetText()
    {
      var layer = CreateTextLayer();
      Assert.AreEqual("Hello World", layer.Text);
      var text = "Something else";
      layer.Text = text;
      Assert.AreEqual(text, layer.Text);
    }

    [Test]
    public void GetMarkup()
    {
      var layer = CreateTextLayer();
      Assert.IsTrue(false);
    }

    [Test]
    public void GetSetHintStyle()
    {
      var layer = CreateTextLayer();
      Assert.IsTrue(false);
    }

    [Test]
    public void GetSetFont()
    {
      var layer = CreateTextLayer();
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
      _image.InsertLayer(layer, 0);
      Assert.AreEqual(fontSize, layer.FontSize);
      var newFontSize = new FontSize(1, Unit.Inch);
      layer.FontSize = newFontSize;
      Assert.AreEqual(newFontSize, layer.FontSize);
    }

    [Test]
    public void GetSetHinting()
    {
      var layer = CreateTextLayer();
      layer.Hinting = new FontHinting(true, true);
      Assert.IsTrue(layer.Hinting.Hinting);
      Assert.IsTrue(layer.Hinting.Autohint);
      layer.Hinting = new FontHinting(false, false);
      Assert.IsFalse(layer.Hinting.Hinting);
      Assert.IsFalse(layer.Hinting.Autohint);
    }

    [Test]
    public void GetSetAntialias()
    {
      var layer = CreateTextLayer();
      layer.Antialias = true;
      Assert.IsTrue(layer.Antialias);
      layer.Antialias = false;
      Assert.IsFalse(layer.Antialias);
    }

    [Test]
    public void GetSetKerning()
    {
      var layer = CreateTextLayer();
      layer.Kerning = true;
      Assert.IsTrue(layer.Kerning);
      layer.Kerning = false;
      Assert.IsFalse(layer.Kerning);
    }

    [Test]
    public void GetSetBaseDirection()
    {
      var layer = CreateTextLayer();
      Assert.AreEqual(TextDirection.Ltr, layer.BaseDirection);
      layer.BaseDirection = TextDirection.Rtl;
      Assert.AreEqual(TextDirection.Rtl, layer.BaseDirection);
    }

    [Test]
    public void GetSetJustification()
    {
      var layer = CreateTextLayer();
      Assert.AreEqual(TextJustification.Left, layer.Justification);
      layer.Justification = TextJustification.Right;
      Assert.AreEqual(TextJustification.Right, layer.Justification);
    }

    [Test]
    public void GetSetColor()
    {
      var layer = CreateTextLayer();
      var color = new RGB(11, 22, 33);
      layer.Color = color;
      Assert.AreEqual(color, layer.Color);
    }

    [Test]
    public void GetSetIndent()
    {
      var layer = CreateTextLayer();
      double indent = 13;
      layer.Indent = indent;
      Assert.AreEqual(indent, layer.Indent);
    }

    [Test]
    public void GetLineSpacing()
    {
      var layer = CreateTextLayer();
      double lineSpacing = 13;
      layer.LineSpacing = lineSpacing;
      Assert.AreEqual(lineSpacing, layer.LineSpacing);
    }

    [Test]
    public void GetLetterSpacing()
    {
      var layer = CreateTextLayer();
      double letterSpacing = 13;
      layer.LetterSpacing = letterSpacing;
      Assert.AreEqual(letterSpacing, layer.LetterSpacing);
    }

    TextLayer CreateTextLayer()
    {
      var fontSize = new FontSize(32, Unit.Pixel);
      var layer = new TextLayer(_image, "Hello World", "Sans", fontSize);
      _image.InsertLayer(layer, 0);
      return layer;
    }

    [Test]
    public void Resize()
    {
      var layer = CreateTextLayer();
      layer.Resize(2 * _width, 2 * _height, 0, 0);
      Assert.AreEqual(2 * _width, layer.Width);
      Assert.AreEqual(2 * _height, layer.Height);
    }
  }
}
