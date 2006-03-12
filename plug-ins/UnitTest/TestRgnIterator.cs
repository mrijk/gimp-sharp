// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestRgnIterator.cs
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
  public class TestRgnIterator
  {
    int _width = 64;
    int _height = 128;
    Image _image;
    Drawable _drawable;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.RGB);

      Layer layer = new Layer(_image, "test", _width, _height,
			      ImageType.RGB, 100, 
			      LayerModeEffects.NORMAL);
      _image.AddLayer(layer, 0);

      _drawable = _image.ActiveDrawable;
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void TestCount()
    {
      RgnIterator iterator = new RgnIterator(_drawable, 
					     RunMode.NONINTERACTIVE);
      int count = 0;
      iterator.Iterate(delegate(byte[] src) 
      {
	count++;
      });
      Assert.AreEqual(_width * _height, count);
    }

    [Test]
    public void TestFill()
    {
      byte[] color = new byte[]{12, 13, 14};

      RgnIterator iterator = new RgnIterator(_drawable, 
					     RunMode.NONINTERACTIVE);
      iterator.Iterate(delegate(int x, int y)
      {
	return color;
      });

      iterator.Iterate(delegate(byte[] src)
      {
	Assert.AreEqual(color[0], src[0]);
	Assert.AreEqual(color[1], src[1]);
	Assert.AreEqual(color[2], src[2]);
      });
    }
  }
}
