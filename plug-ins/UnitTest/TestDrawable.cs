// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// TestDrawable.cs
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
  public class TestDrawable
  {
    int _width = 64;
    int _height = 128;
    Image _image;
    Drawable _drawable;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      Layer layer = new Layer(_image, "test", _width, _height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);

      _drawable = _image.ActiveDrawable;
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void NewDrawable()
    {
      Assert.IsNotNull(_drawable);
      Assert.AreEqual(_image, _drawable.Image);
    }

    [Test]
    public void WidthAndHeight()
    {
      Assert.AreEqual(_drawable.Width, _width);
      Assert.AreEqual(_drawable.Height, _height);
    }

    [Test]
    public void IsColorType()
    {
      Assert.IsTrue(_drawable.IsRGB);
      Assert.IsFalse(_drawable.IsGray);
      Assert.IsFalse(_drawable.IsIndexed);
    }

    [Test]
    public void Name()
    {
      Assert.AreEqual(_drawable.Name, "test");
    }

    [Test]
    public void GetSetName()
    {
      _drawable.Name = "foobar";
      Assert.AreEqual("foobar", _drawable.Name);
    }

    [Test]
    public void GetSetVisible()
    {
      _drawable.Visible = false;
      Assert.IsFalse(_drawable.Visible);
      _drawable.Visible = true;
      Assert.IsTrue(_drawable.Visible);
    }

    [Test]
    public void GetSetLinked()
    {
      Assert.IsFalse(_drawable.Linked);
      _drawable.Linked = true;
      Assert.IsTrue(_drawable.Linked);
      _drawable.Linked = false;
      Assert.IsFalse(_drawable.Linked);
    }

    [Test]
    public void HasAlpha()
    {
      Assert.IsFalse(_drawable.HasAlpha);
    }

    [Test]
    public void TypeWithAlpha()
    {
      Assert.AreEqual(ImageType.Rgba, _drawable.TypeWithAlpha);
    }

    [Test]
    public void Type()
    {
      Assert.AreEqual(ImageType.Rgb, _drawable.Type);
    }

    [Test]
    public void Offsets()
    {
      Offset offset = _drawable.Offsets;
      Assert.AreEqual(0, offset.X);
      Assert.AreEqual(0, offset.Y);      
    }

    [Test]
    public void Bounds()
    {
      Rectangle bounds = _drawable.Bounds;
      Assert.AreEqual(_width, bounds.Width);
      Assert.AreEqual(_height, bounds.Height);
    }

    [Test]
    public void Dimensions()
    {
      Dimensions dimensions = _drawable.Dimensions;
      Assert.AreEqual(_width, dimensions.Width);
      Assert.AreEqual(_height, dimensions.Height);
    }

    [Test]
    public void MaskBounds()
    {
      Rectangle bounds = _drawable.MaskBounds;
      Assert.AreEqual(_width, bounds.Width);
      Assert.AreEqual(_height, bounds.Height);
    }

    [Test]
    public void MaskIntersect()
    {
      Rectangle bounds = _drawable.MaskIntersect;
      Assert.AreEqual(_drawable.Bounds, bounds);	// Select == All
    }

    [Test]
    public void Offset()
    {
      _drawable.Offset(false, OffsetType.Transparent, 13, 14);
      Offset offset = _drawable.Offsets;
      Assert.AreEqual(13, offset.X);
      Assert.AreEqual(14, offset.Y);
    }

    [Test]
    public void IsLayer()
    {
      Assert.IsTrue(_drawable.IsLayer());
    }

    [Test]
    public void IsChannel()
    {
      Assert.IsFalse(_drawable.IsChannel());
    }

    [Test]
    public void GetThumbnailData()
    {
      Dimensions dimensions = new Dimensions(16, 32);
      Pixel[,] thumbnail = _drawable.GetThumbnailData(dimensions);
      Assert.AreEqual(32, thumbnail.GetLength(0));
      Assert.AreEqual(16, thumbnail.GetLength(1));
      Assert.AreEqual(_drawable.Bpp, thumbnail[0, 0].Bpp);
    }

    // TODO: this test fails!
    [Test]
    public void Fill()
    {
      // Fill with some color
      RGB foreground = new RGB(22, 55, 77);
      Context.Push();
      Context.Foreground = foreground;
      _drawable.Fill(FillType.Foreground);
      Context.Pop();

      using (PixelFetcher pf = new PixelFetcher(_drawable, false))
	{
	  Pixel pixel = pf[_height / 2, _width / 2];
	  Assert.AreEqual(foreground, pixel.Color);
	}
    }

    [Test]
    public void CreatePixel()
    {
      Pixel pixel = _drawable.CreatePixel();
      Assert.AreEqual(_drawable.Bpp, pixel.Bpp);
    }
  }
}
