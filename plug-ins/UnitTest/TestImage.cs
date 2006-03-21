// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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
    [Test]
    public void NewImage()
    {
      int width = 64;
      int height = 128;
      Image image = new Image(width, height, ImageBaseType.RGB);
      // Fix me: this if there is a new image
      image.Delete();
    }

    [Test]
    public void WidthHeightImage()
    {
      int width = 64;
      int height = 128;
      ImageBaseType type = ImageBaseType.RGB;
      Image image = new Image(width, height, type);
      Assert.AreEqual(width, image.Width);
      Assert.AreEqual(height, image.Height);
      Assert.AreEqual(type, image.BaseType);
      image.Delete();
    }

    [Test]
    public void Duplicate()
    {
      int width = 64;
      int height = 128;
      ImageBaseType type = ImageBaseType.RGB;
      Image image = new Image(width, height, type);
      Image copy = new Image(image);
      Assert.AreEqual(width, copy.Width);
      Assert.AreEqual(height, copy.Height);
      Assert.AreEqual(type, copy.BaseType);
      copy.Delete();
      image.Delete();
    }

    [Test]
    public void Rotate()
    {
      int width = 64;
      int height = 128;
      ImageBaseType type = ImageBaseType.RGB;
      Image image = new Image(width, height, type);

      image.Rotate(RotationType.Rotate90);
      Assert.AreEqual(width, image.Height);
      Assert.AreEqual(height, image.Width);

      image.Delete();
    }

    [Test]
    public void Resize()
    {
      int width = 64;
      int height = 128;
      ImageBaseType type = ImageBaseType.RGB;
      Image image = new Image(width, height, type);

      image.Resize(100, 100, 0, 0);
      Assert.AreEqual(image.Width, 100);
      Assert.AreEqual(image.Height, 100);

      image.Delete();
    }

    // [Test]
    public void Channels()
    {
      int width = 64;
      int height = 128;
      ImageBaseType type = ImageBaseType.RGB;
      Image image = new Image(width, height, type);

      ChannelList channels = image.Channels;
      
      image.Delete();
    }
  }
}
