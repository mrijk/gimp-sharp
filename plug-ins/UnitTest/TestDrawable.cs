// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
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
      _image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};
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
    public void IsValid()
    {
      Assert.IsTrue(_drawable.IsValid);
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
      var offset = _drawable.Offsets;
      Assert.AreEqual(0, offset.X);
      Assert.AreEqual(0, offset.Y);      
    }

    [Test]
    public void Bounds()
    {
      var bounds = _drawable.Bounds;
      Assert.AreEqual(_width, bounds.Width);
      Assert.AreEqual(_height, bounds.Height);
    }

    [Test]
    public void Dimensions()
    {
      var dimensions = _drawable.Dimensions;
      Assert.AreEqual(_width, dimensions.Width);
      Assert.AreEqual(_height, dimensions.Height);
    }

    [Test]
    public void MaskBounds()
    {
      var bounds = _drawable.MaskBounds;
      Assert.AreEqual(_width, bounds.Width);
      Assert.AreEqual(_height, bounds.Height);
    }

    [Test]
    public void MaskIntersect()
    {
      var bounds = _drawable.MaskIntersect;
      Assert.AreEqual(_drawable.Bounds, bounds);	// Select == All
    }

    [Test]
    public void Offset()
    {
      _drawable.Offset(false, OffsetType.Transparent, 13, 14);
      var offset = _drawable.Offsets;
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
    public void ParasiteList()
    {
      var list = _drawable.ParasiteList;
      Assert.AreEqual(0, list.Count);

      string name = "parasite";
      int data = 13;
      var parasite = new Parasite(name, 0, data);

      _drawable.ParasiteAttach(parasite);

      list = _drawable.ParasiteList;
      Assert.AreEqual(1, list.Count);
      Assert.AreEqual(parasite, list[0]);
    }

    [Test]
    public void ParasiteAttach()
    {
      string name = "parasite";
      int data = 13;
      var parasite = new Parasite(name, 0, data);

      _drawable.ParasiteAttach(parasite);
      var found = _drawable.ParasiteFind(name);
      Assert.IsNotNull(found);
      Assert.AreEqual(name, found.Name);
    }

    [Test]
    public void ParasiteDetach()
    {
      string name = "parasite";
      int data = 13;
      var parasite = new Parasite(name, 0, data);

      _drawable.ParasiteAttach(parasite);
      _drawable.ParasiteDetach(name);
      var found = _drawable.ParasiteFind(name);
      Assert.IsNull(found);
    }

    [Test]
    public void GetThumbnailData()
    {
      var dimensions = new Dimensions(16, 32);
      var thumbnail = _drawable.GetThumbnailData(dimensions);
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
      var pixel = _drawable.CreatePixel();
      Assert.AreEqual(_drawable.Bpp, pixel.Bpp);
    }
    
    // Edit opereations
    // Maybe move these to a seperate file later

    [Test]
    public void EditCut()
    {
      int count = _image.Layers.Count;
      _drawable.EditCut();
      Assert.AreEqual(count - 1, _image.Layers.Count);
    }

    [Test]
    public void EditCopy()
    {
      int count = _image.Layers.Count;
      _drawable.EditCopy();
      Assert.AreEqual(count, _image.Layers.Count);
    }

    /* Fix me: check if this is a function of an image or a drawable!
    [Test]
    public void EditCopyVisible()
    {
      int count = _image.Layers.Count;
      _drawable.EditCopyVisible();
      Assert.AreEqual(count, _image.Layers.Count);
    }
    */

    [Test]
    public void EditPaste()
    {
      int count = _image.Layers.Count;
      _drawable.EditCopy();
      Assert.AreEqual(count, _image.Layers.Count);
      var selection = _drawable.EditPaste(true);
      Assert.AreEqual(count + 1, _image.Layers.Count);
      Assert.AreEqual(_image.FloatingSelection, selection);
    }

    [Test]
    public void EditNamedCopy()
    {
      int count = Buffer.GetBuffers(null).Count;
      var buffer = _drawable.EditNamedCopy("foo");
      Assert.AreEqual(count + 1, Buffer.GetBuffers(null).Count);
      Assert.AreEqual(buffer.Name, "foo");
      Assert.AreEqual(_image.Width, buffer.Width);
      Assert.AreEqual(_image.Height, buffer.Height);
      Assert.AreEqual(_drawable.Bpp, buffer.Bytes);
      // Assert.AreEqual(_drawable.Type, buffer.ImageType);
      buffer.Delete();
      Assert.AreEqual(count, Buffer.GetBuffers(null).Count);
    }

    [Test]
    public void EditNamedCut()
    {
      int count = Buffer.GetBuffers(null).Count;
      int bpp = _drawable.Bpp;
      var buffer = _drawable.EditNamedCut("foo");
      Assert.AreEqual(count + 1, Buffer.GetBuffers(null).Count);
      Assert.AreEqual(buffer.Name, "foo");
      Assert.AreEqual(_image.Width, buffer.Width);
      Assert.AreEqual(_image.Height, buffer.Height);
      Assert.AreEqual(bpp, buffer.Bytes);
      // Assert.AreEqual(_drawable.Type, buffer.ImageType);
      buffer.Delete();
    }

    [Test]
    public void EditNamedPaste()
    {
      int count = Buffer.GetBuffers(null).Count;
      var buffer = _drawable.EditNamedCopy("foo");
      Assert.AreEqual(count + 1, Buffer.GetBuffers(null).Count);
      FloatingSelection selection = _drawable.EditNamedPaste("foo", false);
      Assert.AreEqual(_image.FloatingSelection, selection);
      Assert.AreEqual(count + 1, Buffer.GetBuffers(null).Count);
      Assert.AreEqual(_drawable.Width, selection.Width);
      Assert.AreEqual(_drawable.Height, selection.Height);      
      buffer.Delete();
    }
  }
}
