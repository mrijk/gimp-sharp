// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// TestGrid.cs
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
  public class TestGrid
  {
    int _width = 64;
    int _height = 128;
    Image _image;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);
      var layer = new Layer(_image, "test", ImageType.Rgb);
      _image.InsertLayer(layer, 0);
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void GetGrid()
    {
      var grid = _image.Grid;
      Assert.IsNotNull(grid);
    }

    [Test]
    public void Offset()
    {
      var grid = _image.Grid;
      var offset = new DoubleOffset(12.12, 13.13);
      grid.Offset = offset;
      Assert.AreEqual(offset, grid.Offset);
    }

    [Test]
    public void Spacing()
    {
      var grid = _image.Grid;
      var spacing = new Spacing(12.12, 13.13);
      grid.Spacing = spacing;

      Assert.AreEqual(spacing, grid.Spacing);
    }

    [Test]
    public void ForegroundColor()
    {
      var grid = _image.Grid;
      RGB color = new RGB(11, 22, 33);
      grid.ForegroundColor = color;
      Assert.AreEqual(color, grid.ForegroundColor);
    }

    [Test]
    public void BackgroundColor()
    {
      var grid = _image.Grid;
      RGB color = new RGB(11, 22, 33);
      grid.BackgroundColor = color;
      Assert.AreEqual(color, grid.BackgroundColor);
    }

    [Test]
    public void Style()
    {
      var grid = _image.Grid;
      var style = GridStyle.OnOffDash;
      grid.Style = style;
      Assert.AreEqual(style, grid.Style);
    }
  }
}
