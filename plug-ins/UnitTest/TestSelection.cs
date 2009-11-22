// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestSelection.cs
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

using NUnit.Framework;

namespace Gimp
{
  [TestFixture]
  public class TestSelection
  {
    int _width = 64;
    int _height = 128;
    Image _image;
    Drawable _drawable;
    Selection _selection;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      var layer = new Layer(_image, "test", _width, _height,
			    ImageType.Rgb, 100, 
			    LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);

      _drawable = _image.ActiveDrawable;
      _selection = _image.Selection;
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void Bounds()
    {
      bool nonEmpty;
      var bounds = _selection.Bounds(out nonEmpty);
      Assert.IsFalse(nonEmpty);
      Assert.AreEqual(_drawable.Bounds, bounds);
    }

    [Test]
    public void None()
    {
      _selection.None();
      Assert.IsTrue(_selection.Empty);
    }

    [Test]
    public void All()
    {
      _selection.All();
      Assert.IsFalse(_selection.Empty);
    }

    [Test]
    public void IsEmpty()
    {
      Assert.IsNotNull(_selection);
      Assert.IsTrue(_selection.Empty);
    }

    [Test]
    public void Shrink()
    {
      _selection.All();
      Assert.AreEqual(255, _selection[0, 0]);
      Assert.AreEqual(255, _selection[0, _height - 1]);
      Assert.AreEqual(255, _selection[_width - 1, 0]);
      Assert.AreEqual(255, _selection[_width - 1, _height - 1]);

      _selection.Shrink(1);
      Assert.AreEqual(0, _selection[0, 0]);
      Assert.AreEqual(0, _selection[0, _height - 1]);
      Assert.AreEqual(0, _selection[_width - 1, 0]);
      Assert.AreEqual(0, _selection[_width - 1, _height - 1]);
    }

    [Test]
    public void Grow()
    {
      Shrink();
      _selection.Grow(1);
      Assert.AreEqual(255, _selection[0, 0]);
      Assert.AreEqual(255, _selection[0, _height - 1]);
      Assert.AreEqual(255, _selection[_width - 1, 0]);
      Assert.AreEqual(255, _selection[_width - 1, _height - 1]);
    }

    [Test]
    public void Invert()
    {
      _selection.Invert();
      Assert.IsFalse(_selection.Empty);
      _selection.Invert();
      Assert.IsTrue(_selection.Empty);
    }

    [Test]
    public void Translate()
    {
      _selection.All();
      Assert.AreEqual(255, _selection[0, 0]);
      _selection.Translate(1, 1);
      Assert.AreEqual(0, _selection[0, 0]);
      Assert.AreEqual(255, _selection[1, 1]);
      _selection.Translate(new Offset(1, 1));
      Assert.AreEqual(0, _selection[1, 1]);      
    }

    [Test]
    public void Save()
    {
      var channels = _image.Channels;
      int count = channels.Count;
      _selection.Save();
      channels = _image.Channels;
      Assert.AreEqual(count + 1, channels.Count);
    }

    [Test]
    public void Value()
    {
      _selection.All();
      Assert.AreEqual(255, _selection.Value(_width / 2, _height / 2));
      _selection.None();
      Assert.AreEqual(0, _selection.Value(_width / 2, _height / 2));
    }

    [Test]
    public void This()
    {
      _selection.All();
      Assert.AreEqual(255, _selection[_width / 2, _height / 2]);
      _selection.None();
      Assert.AreEqual(0, _selection[_width / 2, _height / 2]);
    }
  }
}
