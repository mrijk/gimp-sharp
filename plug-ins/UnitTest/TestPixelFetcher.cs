// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// TestPixelFetcher.cs
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
  public class TestPixelFetcher
  {
    int _width = 65;
    int _height = 129;
    Image _image;
    Drawable _drawable;

    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb);

      Layer layer = new Layer(_image, "test", _width, _height,
			      ImageType.Rgb, 100, 
			      LayerModeEffects.Normal);
      _image.AddLayer(layer, 0);

      _drawable = _image.ActiveDrawable;
    }

    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void PutGetPixel()
    {
      using (PixelFetcher pf = new PixelFetcher(_drawable, false))
	{
	  byte[] pixel = new byte[_drawable.Bpp];
	  byte[] expected = new byte[]{33, 66, 99};
	  
	  for (int y = 0; y < _height; y++)
	    {
	      for (int x = 0; x < _width; x++)
		{
		  pf.PutPixel(x, y, expected);
		  pf.GetPixel(x, y, pixel);
		  Assert.AreEqual(expected, pixel);
		}
	    }
	}
    }

    [Test]
    public void This()
    {
      using (PixelFetcher pf = new PixelFetcher(_drawable, false))
	{
	  byte[] pixel = new byte[_drawable.Bpp];
	  byte[] expected = new byte[]{33, 66, 99};
	  
	  for (int y = 0; y < _height; y++)
	    {
	      for (int x = 0; x < _width; x++)
		{
		  pf[x, y] = expected;
		  Assert.AreEqual(expected, pf[x, y]);
		}
	    }
	}
    }

    [Test]
    public void PutGetPixel2()
    {
      // Fill with some color
      RGB foreground = new RGB(22, 55, 77);
      Context.Push();
      Context.Foreground = foreground;
      _drawable.Fill(FillType.Foreground);
      Context.Pop();

      byte[] expected = new byte[]{33, 66, 99};

      // Fill with different color, using shadow
      using (PixelFetcher pf = new PixelFetcher(_drawable, true))
	{
	  for (int y = 0; y < _height; y++)
	    {
	      for (int x = 0; x < _width; x++)
		{
		  pf[x, y] = expected;
		}
	    }
	}

      // check that original hasn't changed
      using (PixelFetcher pf = new PixelFetcher(_drawable, false))
	{
	  for (int y = 0; y < _height; y++)
	    {
	      for (int x = 0; x < _width; x++)
		{
		  Assert.AreEqual(foreground.Bytes, pf[x, y]);
		}
	    }
	}

      _drawable.MergeShadow(true);

      // and now the orginal should be changed
      using (PixelFetcher pf = new PixelFetcher(_drawable, false))
	{
	  for (int y = 0; y < _height; y++)
	    {
	      for (int x = 0; x < _width; x++)
		{
		  Assert.AreEqual(expected, pf[x, y]);
		}
	    }
	}
    }   
  }
}
