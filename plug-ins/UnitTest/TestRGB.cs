// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
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
using System.Collections;
using System.Collections.Generic;

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
    public void ConstructorWithHSV()
    {
      HSV hsv = new HSV(0, 0, 0);
      RGB rgb = new RGB(hsv);
      Assert.AreEqual(new RGB(0, 0, 0), rgb);
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
    public void ParseName()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseName("blue");
      Assert.AreEqual(new RGB(0, 0, 255), rgb);
    }    

    [Test]
    [ExpectedException(typeof(GimpSharpException))]
    public void ParseInvalidName()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseName("nonsense");
      Assert.AreEqual(new RGB(0, 0, 255), rgb);
    }

    [Test]
    public void ParseHex()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseHex("#0a1b2c");
      Assert.AreEqual(new RGB(10, 27, 44), rgb);
    }    

    [Test]
    [ExpectedException(typeof(GimpSharpException))]
    public void ParseInvalidHex()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseHex("#nonsense");
    }

    [Test]
    public void ParseCss()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseCss("rgb(10, 27, 44)");
      Assert.AreEqual(new RGB(10, 27, 44), rgb);
    }    

    [Test]
    public void ParseCssHex()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseCss("#0a1b2c");
      Assert.AreEqual(new RGB(10, 27, 44), rgb);
    }    

    [Test]
    [ExpectedException(typeof(GimpSharpException))]
    public void ParseInvalidCss()
    {
      RGB rgb = new RGB(0, 0, 0);
      rgb.ParseCss("#nonsense");
    }

    // Fix me: next test segfaults on RGB.ListNames. However if I call this 
    // function in a plug-in everything seems to be fine
#if false
    [Test]
    public void ListNames()
    {
      List<string> names;

      var colors = RGB.ListNames(out names);

      Assert.IsTrue(names.Count == colors.Count);
      Assert.IsTrue(names.Count > 0);
      Assert.IsTrue(names.Contains("blue"));
    }
#endif
    [Test]
    public void Add()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      rgb.Add(new RGB(0, 0, 0));
      Assert.AreEqual(new RGB(red, green, blue), rgb);
    }

    [Test]
    public void Subtract()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      RGB tmp = new RGB(red, green, blue);
      rgb.Subtract(new RGB(0, 0, 0));
      Assert.AreEqual(tmp, rgb);
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
    public void AddOperator()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      RGB result = rgb + new RGB(0, 0, 0);
      Assert.AreEqual(rgb, result);
    }

    [Test]
    public void SubstractOperator()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb = new RGB(red, green, blue);
      RGB result = rgb - new RGB(0, 0, 0);
      Assert.AreEqual(rgb, result);
    }

    [Test]
    public void EqualsTrue()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb1 = new RGB(red, green, blue);
      RGB rgb2 = new RGB(red, green, blue);
      Assert.IsTrue(rgb1.Equals(rgb2));
    }

    [Test]
    public void EqualsFalse()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb1 = new RGB(red, green, blue);
      RGB rgb2 = new RGB(red + 0.1, green, blue);
      Assert.IsFalse(rgb1.Equals(rgb2));
    }

    [Test]
    public void EqualsOperator()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb1 = new RGB(red, green, blue);
      RGB rgb2 = new RGB(red, green, blue);
      Assert.IsTrue(rgb1 == rgb2);
    }

    [Test]
    public void NotEqualsOperator()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb1 = new RGB(red, green, blue);
      RGB rgb2 = new RGB(red + 0.1, green, blue);
      Assert.IsTrue(rgb1 != rgb2);
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
    public void DistanceGreen()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;
      RGB rgb1 = new RGB(red, green, blue);
      RGB rgb2 = new RGB(red, green + 0.11, blue);

      Assert.AreEqual(0.11, rgb1.Distance(rgb2), 0.001);
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

    [Test]
    public void ClampNothing()
    {
      double red = 0.13;
      double green = 0.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      rgb.Clamp();
      Assert.AreEqual(new RGB(red, green, blue), rgb);
    }

    [Test]
    public void ClampUpper()
    {
      double red = 0.13;
      double green = 1.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      rgb.Clamp();
      Assert.AreEqual(new RGB(red, 1.0, blue), rgb);
    }

    [Test]
    public void ClampLower()
    {
      double red = 0.13;
      double green = -0.24;
      double blue = 0.35;

      RGB rgb = new RGB(red, green, blue);
      rgb.Clamp();
      Assert.AreEqual(new RGB(red, 0.0, blue), rgb);
    }
  }
}
