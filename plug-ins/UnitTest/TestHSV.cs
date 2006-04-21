// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestHSV.cs
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
  public class TestHSV
  {
    [Test]
    public void Constructor()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;

      HSV hsv = new HSV(hue, saturation, value);
      Assert.AreEqual(hue, hsv.Hue);
      Assert.AreEqual(saturation, hsv.Saturation);
      Assert.AreEqual(value, hsv.Value);
      Assert.AreEqual(0.0, hsv.Alpha);
    }

    [Test]
    public void EqualsTrue()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;
      HSV hsv1 = new HSV(hue, saturation, value);
      HSV hsv2 = new HSV(hue, saturation, value);
      Assert.IsTrue(hsv1.Equals(hsv2));
    }

    [Test]
    public void EqualsFalse()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;
      HSV hsv1 = new HSV(hue, saturation, value);
      HSV hsv2 = new HSV(hue + 0.1, saturation, value);
      Assert.IsFalse(hsv1.Equals(hsv2));
    }

    [Test]
    public void EqualsOperator()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;
      HSV hsv1 = new HSV(hue, saturation, value);
      HSV hsv2 = new HSV(hue, saturation, value);
      Assert.IsTrue(hsv1 == hsv2);
    }

    [Test]
    public void NotEqualsOperator()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;
      HSV hsv1 = new HSV(hue, saturation, value);
      HSV hsv2 = new HSV(hue + 0.1, saturation, value);
      Assert.IsTrue(hsv1 != hsv2);
    }

    [Test]
    public void ClampNothing()
    {
      double hue = 0.13;
      double saturation = 0.24;
      double value = 0.35;

      HSV hsv = new HSV(hue, saturation, value);
      hsv.Clamp();
      Assert.AreEqual(new HSV(hue, saturation, value), hsv);
    }

    [Test]
    public void ClampUpper()
    {
      double hue = 0.13;
      double saturation = 1.24;
      double value = 0.35;

      HSV hsv = new HSV(hue, saturation, value);
      hsv.Clamp();
      Assert.AreEqual(new HSV(hue, 1.0, value), hsv);
    }

    [Test]
    public void ClampLower()
    {
      double hue = 0.13;
      double saturation = -0.24;
      double value = 0.35;

      HSV hsv = new HSV(hue, saturation, value);
      hsv.Clamp();
      Assert.AreEqual(new HSV(hue, 0.0, value), hsv);
    }
  }
}
