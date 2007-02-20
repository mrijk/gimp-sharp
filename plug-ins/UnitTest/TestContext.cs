// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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

    [Test]
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
  }
}
