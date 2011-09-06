// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// TestGradient.cs
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
  public class TestGradient
  {
    [Test]
    public void Constructor()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);
      var gradients = new GradientList(tmpName);
      Assert.AreEqual(1, gradients.Count);
      gradient.Delete();
      Assert.AreEqual(0, (new GradientList(tmpName)).Count);
    }

    [Test]
    public void Duplicate()
    {
      string tmpName = "DummyGradient";
      var original = new Gradient(tmpName);
      var copy = new Gradient(original);
      Assert.AreNotEqual(copy.Name, original.Name);
      copy.Delete();
      original.Delete();
    }

    [Test]
    public void Rename()
    {
      string tmpName = "DummyGradient";
      string newName = "FooGradient";
      var gradient = new Gradient(tmpName);
      
      Assert.AreEqual(newName, gradient.Rename(newName));

      var copy = new Gradient(gradient);
      Assert.AreNotEqual(newName, copy.Rename(newName));

      copy.Delete();
      gradient.Delete();
    }

    [Test]
    public void Editable()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);
      Assert.IsTrue(gradient.Editable);
      gradient.Delete();
     }

    [Test]
    public void ForEach()
    {
      var gradient = new Gradient("DummyGradient");
      int count = 0;
      gradient.ForEach((segment) => count++);
      Assert.AreEqual(gradient.NumberOfSegments, count);
      gradient.Delete();      
    }

    [Test]
    public void NumberOfSegments()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);
      Assert.AreEqual(1, gradient.NumberOfSegments);
      gradient.Delete();
     }

    [Test]
    public void SegmentGetLeftRightColor()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);

      double opacity;
      var rgb = gradient.SegmentGetLeftColor(0, out opacity);

      Assert.AreEqual(0.0, rgb.R);
      Assert.AreEqual(0.0, rgb.G);
      Assert.AreEqual(0.0, rgb.B);

      rgb = gradient.SegmentGetRightColor(0, out opacity);
      Assert.AreEqual(1.0, rgb.R);
      Assert.AreEqual(1.0, rgb.G);
      Assert.AreEqual(1.0, rgb.B);

      gradient.Delete();
    }

    [Test]
    public void GetLeftPosition()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);

      double position = gradient.SegmentGetLeftPosition(0);
      Assert.AreEqual(0.0, position);

      gradient.Delete();
    }

    [Test]
    public void GetRightPosition()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);

      double position = gradient.SegmentGetRightPosition(0);
      Assert.AreEqual(1.0, position);

      gradient.Delete();
    }

    [Test]
    public void GetMiddlePosition()
    {
      string tmpName = "DummyGradient";
      var gradient = new Gradient(tmpName);

      double position = gradient.SegmentGetMiddlePosition(0);
      Assert.AreEqual(0.5, position);

      gradient.Delete();
    }
  }
}
