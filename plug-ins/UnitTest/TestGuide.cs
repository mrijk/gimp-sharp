// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestGuide.cs
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
  public class TestGuide
  {
    int _width = 117;
    int _height = 118;
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
    public void NewHorizontalGuide()
    {
      var guide = new HorizontalGuide(_image, _height / 2);
      Assert.AreEqual(OrientationType.Horizontal, guide.Orientation);
      Assert.AreEqual(1, CountGuides());

      guide.Delete();
      Assert.AreEqual(0, CountGuides());
    }

    [Test]
    public void NewVerticalGuide()
    {
      var guide = new VerticalGuide(_image, _height / 2);
      Assert.AreEqual(OrientationType.Vertical, guide.Orientation);
      Assert.AreEqual(1, CountGuides());

      guide.Delete();
      Assert.AreEqual(0, CountGuides());
    }

    [Test]
    public void Position()
    {
      var guide = new HorizontalGuide(_image, _height / 2);
      Assert.AreEqual(OrientationType.Horizontal, guide.Orientation);
      Assert.AreEqual(_height / 2, guide.Position);
    }

    int CountGuides()
    {
      int count = 0;
      foreach (Guide g in new GuideCollection(_image))
	count++;
      return count;
    }
  }
}
