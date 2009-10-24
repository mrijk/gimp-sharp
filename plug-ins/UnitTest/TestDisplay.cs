// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// TestDisplay.cs
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
  public class TestDisplay
  {
    [Test]
    public void NewAndDelete()
    {
      int width = 21;
      int height = 128;
      var image = new Image(width, height, ImageBaseType.Rgb);

      var images = new ImageList();
      int count = images.Count;
      Assert.AreEqual(1, count);

      var display = new Display(image);
      display.Delete();

      images.Refresh();
      Assert.AreEqual(count - 1, images.Count);
    }

    [Test]
    public void Reconnect()
    {
      int width = 21;
      int height = 128;
      var oldImage = new Image(width, height, ImageBaseType.Rgb);
      var newImage = new Image(width, height, ImageBaseType.Rgb);

      Assert.IsFalse(Display.Reconnect(oldImage, newImage));

      var display = new Display(oldImage);

      Assert.IsFalse(Display.Reconnect(newImage, oldImage));
      Assert.IsTrue(Display.Reconnect(oldImage, newImage));

      display.Delete();
    }
  }
}
