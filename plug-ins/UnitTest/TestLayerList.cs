// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// TestLayerList.cs
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
  public class TestLayerList
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
    public void AddTwoDifferentLayerTypes()
    {
      var layer = new Layer(_image, "test1", ImageType.Rgb);
      _image.InsertLayer(layer, null, 0);

      var fontSize = new FontSize(32, Unit.Pixel);
      var textLayer = new TextLayer(_image, "Hello World", "Sans", fontSize);
      _image.InsertLayer(textLayer, null, 0);

      Assert.AreEqual(2, _image.Layers.Count);
      Assert.IsTrue(_image.Layers[0] is TextLayer);
      Assert.IsTrue(_image.Layers[1] is Layer);
    }
  }
}
