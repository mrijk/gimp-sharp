// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestRGB.cs
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
  public class TestRGB
  {
    [Test]
    public void Constructor()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      Assert.AreEqual(red, rgb.R);
      Assert.AreEqual(green, rgb.G);
      Assert.AreEqual(blue, rgb.B);
      Assert.AreEqual(0.0, rgb.Alpha);
    }

    [Test]
    public void Alpha()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      rgb.Alpha = 0.5;
      Assert.AreEqual(0.5, rgb.Alpha);
    }

    [Test]
    public void Bytes()
    {
      byte red = 13;
      byte green = 24;
      byte blue = 35;
      RGB rgb = new RGB(red, green, blue);

      byte[] bytes = rgb.Bytes;
      Assert.AreEqual(new byte[]{red, green, blue}, bytes);
    }

    [Test]
    public void GetUChar()
    {
      byte red = 13;
      byte green = 24;
      byte blue = 35;
      RGB rgb = new RGB(red, green, blue);

      byte r, g, b;
      rgb.GetUchar(out r, out g, out b);
      Assert.AreEqual(red, r);
      Assert.AreEqual(green, g);
      Assert.AreEqual(blue, b);
    }

    [Test]
    public void SetUChar()
    {
      RGB rgb = new RGB(0, 0, 0);

      byte red = 13;
      byte green = 24;
      byte blue = 35;

      rgb.SetUchar(red, green, blue);

      byte r, g, b;
      rgb.GetUchar(out r, out g, out b);
      Assert.AreEqual(red, r);
      Assert.AreEqual(green, g);
      Assert.AreEqual(blue, b);
    }

    [Test]
    public void ParseHex()
    {
      // Fix me: create test
    }    

    [Test]
    public void Multiply()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      double factor = 2.1;

      RGB rgb = new RGB(red, green, blue);
      rgb.Multiply(factor);

      Assert.AreEqual(factor * red, rgb.R);
      Assert.AreEqual(factor * green, rgb.G);
      Assert.AreEqual(factor * blue, rgb.B);      
    }

    [Test]
    public void Distance()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      
      Assert.AreEqual(0.0, rgb.Distance(rgb));
    }

    [Test]
    public void Max()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      Assert.AreEqual(blue, rgb.Max);
    }

    [Test]
    public void Min()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      Assert.AreEqual(red, rgb.Min);
    }
  }
}
