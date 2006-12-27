// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestPixel.cs
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
  public class TestPixel
  {
    [Test]
    public void ConstructorOne()
    {
      Pixel pixel = new Pixel(3);
      Assert.AreEqual(0, pixel.Red);
      Assert.AreEqual(0, pixel.Green);
      Assert.AreEqual(0, pixel.Blue);
    }

    [Test]
    public void ConstructorTwo()
    {
      byte[] rgb = new byte[]{11, 12, 13};
      Pixel pixel = new Pixel(rgb);
      Assert.AreEqual(11, pixel.Red);
      Assert.AreEqual(12, pixel.Green);
      Assert.AreEqual(13, pixel.Blue);
    }

    [Test]
    public void ConstructorThree()
    {
      Pixel pixel = new Pixel(11, 12, 13);
      Assert.AreEqual(11, pixel.Red);
      Assert.AreEqual(12, pixel.Green);
      Assert.AreEqual(13, pixel.Blue);
    }

    [Test]
    public void ConstructorFor()
    {
      Pixel pixel = new Pixel(11, 12, 13, 255);
      Assert.AreEqual(11, pixel.Red);
      Assert.AreEqual(12, pixel.Green);
      Assert.AreEqual(13, pixel.Blue);
      Assert.AreEqual(255, pixel.Alpha);
    }

    [Test]
    public void IsSameColor()
    {
      Pixel pixel1 = new Pixel(11, 12, 13);
      Pixel pixel2 = new Pixel(11, 12, 13);
      Pixel pixel3 = new Pixel(21, 22, 23);

      Assert.IsTrue(pixel1.IsSameColor(pixel2));
      Assert.IsFalse(pixel1.IsSameColor(pixel3));
    }

    [Test]
    public void GetBytes()
    {
      byte[] rgb = new byte[]{11, 12, 13};
      Pixel pixel = new Pixel(rgb);
      Assert.AreEqual(rgb, pixel.Bytes);
    }

    [Test]
    public void SetBytes()
    {
      byte[] rgb = new byte[]{11, 12, 13};
      Pixel pixel = new Pixel(3);
      pixel.Bytes = rgb;
      Assert.AreEqual(rgb, pixel.Bytes);
    }

    [Test]
    public void GetSetXY()
    {
      Pixel pixel = new Pixel(11, 12, 13);
      pixel.X = 123;
      pixel.Y = 321;
      Assert.AreEqual(123, pixel.X);
      Assert.AreEqual(321, pixel.Y);
    }

    [Test]
    public void GetSetRed()
    {
      Pixel pixel = new Pixel(3);
      pixel.Red = 127;
      Assert.AreEqual(127, pixel.Red);
    }

    [Test]
    public void GetSetGreen()
    {
      Pixel pixel = new Pixel(3);
      pixel.Green = 127;
      Assert.AreEqual(127, pixel.Green);
    }

    [Test]
    public void GetSetBlue()
    {
      Pixel pixel = new Pixel(3);
      pixel.Blue = 127;
      Assert.AreEqual(127, pixel.Blue);
    }

    [Test]
    public void HasAlpha()
    {
      Pixel pixel = new Pixel(1);
      Assert.IsFalse(pixel.HasAlpha);

      pixel = new Pixel(2);
      Assert.IsTrue(pixel.HasAlpha);

      pixel = new Pixel(3);
      Assert.IsFalse(pixel.HasAlpha);

      pixel = new Pixel(4);
      Assert.IsTrue(pixel.HasAlpha);
    }    

    [Test]
    public void AddOne()
    {
      Pixel pixel1 = new Pixel(11, 12, 13);
      Pixel pixel2 = new Pixel(1, 1, 1);
      pixel1.Add(pixel2);
      Assert.AreEqual(12, pixel1.Red);
      Assert.AreEqual(13, pixel1.Green);
      Assert.AreEqual(14, pixel1.Blue);
    }
  }
}
