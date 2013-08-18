// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2013 Maurits Rijk
//
// TestContext.cs
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
  public class TestContext
  {
    [SetUp]
    public void Init()
    {
      Context.Push();
    }

    [TearDown]
    public void Exit()
    {
      Context.Pop();
    }

    [Test]
    public void PushPop()
    {
      RGB previous = Context.Foreground;
      Context.Push();
      Context.Foreground = new RGB(11, 12, 13);
      Context.Pop();
      Assert.AreEqual(previous, Context.Foreground);
    }

    [Test]
    public void SetDefaults()
    {
      RGB previous = Context.Foreground;
      Context.Foreground = new RGB(11, 12, 13);
      Context.SetDefaults();
      Assert.AreEqual(previous, Context.Foreground);
    }

    [Test]
    public void Foreground()
    {
      RGB foreground = new RGB(11, 12, 13);
      Context.Foreground = foreground;
      RGB result = Context.Foreground;
      Assert.AreEqual(foreground.R, result.R);
      Assert.AreEqual(foreground.G, result.G);
      Assert.AreEqual(foreground.B, result.B);
      // Fix me: why doesn't Assert.AreEqual(foreground, result) just work?
      // Answer: because of the undefined alpha channel
    }

    /*
     * This test does work, but pops up a very annoying GIMP message.
     * 

    // [Test]
    [ExpectedException(typeof(Exception))]
    public void ForegroundTwo()
    {
      RGB foreground = new RGB(11, 12, 13.44);
      Context.Foreground = foreground;
    }
    */

    [Test]
    public void Background()
    {
      RGB background = new RGB(11, 12, 13);
      Context.Background = background;
      RGB result = Context.Background;
      Assert.AreEqual(background.R, result.R);
      Assert.AreEqual(background.G, result.G);
      Assert.AreEqual(background.B, result.B);
      // Fix me: why doesn't Assert.AreEqual(background, result) just work?
    }

    [Test]
    public void Opacity()
    {
      double opacity = 0.13;
      Context.Opacity = opacity;
      Assert.AreEqual(opacity, Context.Opacity);
    }

    [Test]
    public void PaintMode()
    {
      Context.PaintMode = LayerModeEffects.Multiply;
      Assert.AreEqual(LayerModeEffects.Multiply, Context.PaintMode);
    }

    // [Test]
    public void Brush()
    {
      foreach (var brush in new BrushList(null))
	{
	  Context.Brush = brush;
	  Assert.AreEqual(brush, Context.Brush);
	}
    }

    [Test]
    public void GetSetBrushSize()
    {
      double size = 3.14;
      Context.BrushSize = size;
      Assert.AreEqual(size, Context.BrushSize);
    }

    [Test]
    public void SetBrushDefaultSize()
    {
      Context.SetBrushDefaultSize();
      double defaultSize = Context.BrushSize;
      Context.BrushSize = 3.14;
      Context.SetBrushDefaultSize();
      Assert.AreEqual(defaultSize, Context.BrushSize);
    }

    [Test]
    public void BrushAspectRatio()
    {
      double ratio = 2.0;
      Context.BrushAspectRatio = ratio;
      Assert.AreEqual(ratio, Context.BrushAspectRatio);
    }

    [Test]
    public void GetSetBrushAngle()
    {
      double angle = 3.14;
      Context.BrushAngle = angle;
      Assert.AreEqual(angle, Context.BrushAngle);
    }

    [Test]
    public void Dynamics()
    {
      Assert.IsTrue(false);
    }

    // [Test]
    public void Pattern()
    {
      foreach (var pattern in new PatternList(null))
	{
	  Context.Pattern = pattern;
	  Assert.AreEqual(pattern, Context.Pattern);
	}
    }

    // [Test]
    public void Gradient()
    {
      foreach (var gradient in new GradientList(null))
	{
	  Context.Gradient = gradient;
	  Assert.AreEqual(gradient, Context.Gradient);
	}
    }

    // [Test]
    public void Palette()
    {
      foreach (var palette in new PaletteList(null))
	{
	  Context.Palette = palette;
	  Assert.AreEqual(palette, Context.Palette);
	}
    }

    [Test]
    public void GetSetFont()
    {
      var font = "Serif";
      Context.Font = font;
      Assert.AreEqual(font, Context.Font);
    }

    [Test]
    public void GetSetAntialias()
    {
      bool current = Context.Antialias;
      Context.Antialias = !current;
      Assert.AreEqual(!current, Context.Antialias);
    }

    [Test]
    public void GetSetFeather()
    {
      bool current = Context.Feather;
      Context.Feather = !current;
      Assert.AreEqual(!current, Context.Feather);
    }

    [Test]
    public void GetSetFeatherRadius()
    {
      var radius = new Coordinate<double>(1.1, 2.2);
      Context.FeatherRadius = radius;
      Assert.AreEqual(radius, Context.FeatherRadius);
    }

    [Test]
    public void GetSetSampleMerged()
    {
      bool current = Context.SampleMerged;
      Context.SampleMerged = !current;
      Assert.AreEqual(!current, Context.SampleMerged);
    }

    [Test]
    public void GetSetSampleCriterion()
    {
      Context.SampleCriterion = SelectCriterion.H;
      Assert.AreEqual(SelectCriterion.H, Context.SampleCriterion);
    }

    [Test]
    public void GetSetSampleThreshold()
    {
      double threshold = 0.314;
      Context.SampleThreshold = threshold;
      Assert.AreEqual(threshold, Context.SampleThreshold);
    }

    [Test]
    public void GetSetSampleThresholdInt()
    {
      int threshold = 5;
      Context.SampleThresholdInt = threshold;
      Assert.AreEqual(threshold, Context.SampleThresholdInt); 
    }

    [Test]
    public void GetSetSampleTransparent()
    {
      bool current = Context.SampleTransparent;
      Context.SampleTransparent = !current;
      Assert.AreEqual(!current, Context.SampleTransparent);
    }

    [Test]
    public void GetSetInterpolation()
    {
      Context.Interpolation = InterpolationType.Lanczos;
      Assert.AreEqual(InterpolationType.Lanczos, Context.Interpolation);
    }

    [Test]
    public void GetSetTransformDirection()
    {
      Context.TransformDirection = TransformDirection.Forward;
      Assert.AreEqual(TransformDirection.Forward, Context.TransformDirection);
    }

    [Test]
    public void GetSetTransformResize()
    {
      Context.TransformResize = TransformResize.Crop;
      Assert.AreEqual(TransformResize.Crop, Context.TransformResize);
    }

    [Test]
    public void GetSetTransformRecursion()
    {
      int recursion = 5;
      Context.TransformRecursion = recursion;
      Assert.AreEqual(recursion, Context.TransformRecursion);
    }

    [Test]
    public void GetSetInkSize()
    {
      double size = 3.14;
      Context.InkSize = size;
      Assert.AreEqual(size, Context.InkSize);
    }

    [Test]
    public void GetSetInkAngle()
    {
      double angle = 3.14;
      Context.InkAngle = angle;
      Assert.AreEqual(angle, Context.InkAngle);
    }

    [Test]
    public void GetSetInkSizeSensitivity()
    {
      double sensitivity = 0.5;
      Context.InkSizeSensitivity = sensitivity;
      Assert.AreEqual(sensitivity, Context.InkSizeSensitivity);
    }

    [Test]
    public void GetSetInkTiltSensitivity()
    {
      double sensitivity = 0.5;
      Context.InkTiltSensitivity = sensitivity;
      Assert.AreEqual(sensitivity, Context.InkTiltSensitivity);
    }

    [Test]
    public void GetSetInkSpeedSensitivity()
    {
      double sensitivity = 0.5;
      Context.InkSpeedSensitivity = sensitivity;
      Assert.AreEqual(sensitivity, Context.InkSpeedSensitivity);
    }

    [Test]
    public void GetSetInkBlobType()
    {
      Context.InkBlobType = InkBlobType.Square;
      Assert.AreEqual(InkBlobType.Square, Context.InkBlobType);
    }

    [Test]
    public void GetSetInkBlobAspectRatio()
    {
      double ratio = 2.0;
      Context.InkBlobAspectRatio = ratio;
      Assert.AreEqual(ratio, Context.InkBlobAspectRatio);
    }

    [Test]
    public void GetSetInkBlobAngle()
    {
      double angle = 3.14;
      Context.InkBlobAngle = angle;
      Assert.AreEqual(angle, Context.InkBlobAngle);
    }

    // [Test]
    public void PaintMethod()
    {
      foreach (var method in Context.PaintMethods)
	{
	  Context.PaintMethod = method;
	  Assert.AreEqual(method, Context.PaintMethod);
	}
    }

    // [Test]
    public void PaintMethods()
    {
      Assert.IsTrue(Context.PaintMethods.Count > 0);
    }
  }
}
