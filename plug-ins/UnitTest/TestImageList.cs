// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestImageList.cs
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
  public class TestImageList
  {
    int _width = 129;
    int _height = 65;
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
    public void TestConstructor()
    {
      var images = new ImageList();
      Assert.AreEqual(1, images.Count);  
    }

    [Test]
    public void TestRefresh()
    {
      var images = new ImageList();
      var image = CreateImage();

      Assert.AreEqual(1, images.Count);
      images.Refresh();
      Assert.AreEqual(2, images.Count);

      image.Delete(); 
    }

    [Test]
    public void TestEnumerator()
    {
      var image1 = CreateImage();
      var image2 = CreateImage();

      int count = 0;
      var images = new ImageList();
      foreach (var image in images)
	{
	  count++;
	}
      Assert.AreEqual(3, count);

      image2.Delete();
      image1.Delete();
    }

    [Test]
    public void TestForEach()
    {
      var image1 = CreateImage();
      var image2 = CreateImage();

      int count = 0;
      var images = new ImageList();
      
      images.ForEach(image => count++);
      Assert.AreEqual(3, count);

      image2.Delete();
      image1.Delete();
    }

    [Test]
    public void TestThis()
    {
      var images = new ImageList();
      Assert.AreEqual(_image, images[0]);
    }

    [Test]
    public void TestGetIndex()
    {
      var images = new ImageList();
      Assert.AreEqual(0, images.GetIndex(_image));
    }

    Image CreateImage()
    {
      return new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};
    }
  }
}
