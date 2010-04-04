// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// TestPixelRgn.cs
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
  public class TestPixelRgn
  {
    int _width = 65;
    int _height = 129;
    Image _image;
    Drawable _drawable;
  
    [SetUp]
    public void Init()
    {
      _image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};    
      _drawable = _image.ActiveDrawable;
    }
  
    [TearDown]
    public void Exit()
    {
      _image.Delete();
    }

    [Test]
    public void GetSetPixel()
    {
      var rgn = new PixelRgn(_drawable, false, false);
      var pixel = new byte[]{13, 24, 35};

      rgn.SetPixel(pixel, 13, 14);

      var result = new byte[_drawable.Bpp];
      rgn.GetPixel(result, 13, 14);

      Assert.AreEqual(pixel, result);
    }

    [Test]
    public void Rowstride()
    {
      var rgn = new PixelRgn(_drawable, false, false);
      PixelRgn.Register(rgn);
      Assert.AreEqual(_drawable.Bpp * Gimp.TileWidth, rgn.Rowstride);
    }

    [Test]
    public void GetSetRow()
    {
      var rgn = new PixelRgn(_drawable, false, false);
      var row = new Pixel[_width];

      for (int i = 0; i < _width; i++)
	{
	  row[i] = new Pixel(13, 13, 13);
	}

      rgn.SetRow(row, 0, 13);

      var result = rgn.GetRow(0, 13, _width);

      for (int i = 0; i < _width; i++)
	{
	  Assert.IsTrue(row[i].IsSameColor(result[i]));
	}
    }

    [Test]
    public void GetSetRect()
    {
      var rgn = new PixelRgn(_drawable, true, false);
      var rect = new byte[_drawable.Bpp * _width * _height];
      for (int i = 0; i < _drawable.Bpp * _width * _height; i++)
	{
	  rect[i] = 13;
	}

      rgn.SetRect(rect, 0, 0, _width, _height);

      var result = rgn.GetRect(0, 0, _width, _height);

      Assert.AreEqual(rect, result);
    }

    [Test]
    public void CountTiles()
    {
      var rgn = new PixelRgn(_drawable, false, false);

      int tw = (int) Gimp.TileWidth;
      int th = (int) Gimp.TileHeight;
      int htiles = (_width + tw - 1) / tw;
      int vtiles = (_height + th - 1) / th;

      int count = 0;
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  count++;
	}
      Assert.AreEqual(htiles * vtiles, count);
    }

    [Test]
    public void CountPixels()
    {
      var rgn = new PixelRgn(_drawable, false, false);

      int count = 0;
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = rgn.Y; y < rgn.Y + rgn.H; y++)
	    {
	      for (int x = rgn.X; x < rgn.X + rgn.W; x++)
		{
		  count++;
		}
	    }
	}
      Assert.AreEqual(_width * _height, count);
    }

    [Test]
    public void DirectAccessRgb()
    {
      var rgn = new PixelRgn(_drawable, true, false);
      var pixel = new Pixel(13, 24, 35);

      FillDrawable(_drawable, pixel);

      rgn = new PixelRgn(_drawable, false, false);
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = rgn.Y; y < rgn.Y + rgn.H; y++)
	    {
	      for (int x = rgn.X; x < rgn.X + rgn.W; x++)
		{
		  Assert.AreEqual(pixel.Color, rgn[y, x].Color);
		}
	    }
	}
    }

    [Test]
    public void DirectAccessRgba()
    {
      var image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgba), 0}};
    
      var drawable = image.ActiveDrawable;
      var pixel = new Pixel(13, 24, 35, 128);
      FillDrawable(drawable, pixel);

      var rgn = new PixelRgn(drawable, false, false);
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = rgn.Y; y < rgn.Y + rgn.H; y++)
	    {
	      for (int x = rgn.X; x < rgn.X + rgn.W; x++)
		{
		  Assert.AreEqual(pixel.Color, rgn[y, x].Color);
		}
	    }
	}
      image.Delete();
    }
     
    [Test]
    public void CopyRgb2Rgb()
    {
      // Fill src region

      var pixel = new Pixel(13, 24, 35);
      FillDrawable(_drawable, pixel);

      // Copy to dest region
      var image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgb), 0}};
      var drawable = image.ActiveDrawable;

      var srcRgn = new PixelRgn(_drawable, false, false);
      var destRgn = new PixelRgn(drawable, true, false);

      for (IntPtr pr = PixelRgn.Register(srcRgn, destRgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcRgn.Y; y < srcRgn.Y + srcRgn.H; y++)
	    {
	      for (int x = srcRgn.X; x < srcRgn.X + srcRgn.W; x++)
		{
		  destRgn[y, x] = srcRgn[y, x];
		}
	    }
	}

      // Check results

      srcRgn = new PixelRgn(_drawable, false, false);
      destRgn = new PixelRgn(drawable, false, false);
      for (IntPtr pr = PixelRgn.Register(srcRgn, destRgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcRgn.Y; y < srcRgn.Y + srcRgn.H; y++)
	    {
	      for (int x = srcRgn.X; x < srcRgn.X + srcRgn.W; x++)
		{
		  Assert.AreEqual(pixel.Color, srcRgn[y, x].Color);
		  Assert.AreEqual(pixel.Color, destRgn[y, x].Color);
		  Assert.AreEqual(srcRgn[y, x].Color, destRgn[y, x].Color);
		}
	    }
	}

      image.Delete();
    }

    [Test]
    public void CopyRgb2Rgba()
    {
      // Fill src region

      var pixel = new Pixel(13, 24, 35);
      FillDrawable(_drawable, pixel);

      // Copy to dest region
      var image = new Image(_width, _height, ImageBaseType.Rgb) {
	{new Layer("test", ImageType.Rgba), 0}};
      var drawable = image.ActiveDrawable;

      var srcRgn = new PixelRgn(_drawable, false, false);
      var destRgn = new PixelRgn(drawable, true, false);

      for (IntPtr pr = PixelRgn.Register(srcRgn, destRgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcRgn.Y; y < srcRgn.Y + srcRgn.H; y++)
	    {
	      for (int x = srcRgn.X; x < srcRgn.X + srcRgn.W; x++)
		{
		  var tmp = srcRgn[y, x];
		  destRgn[y, x] = new Pixel(tmp.Red, tmp.Green, tmp.Blue, 255);
		}
	    }
	}

      // Check results

      var pixel2 = new Pixel(13, 24, 35, 255);

      srcRgn = new PixelRgn(_drawable, false, false);
      destRgn = new PixelRgn(drawable, false, false);
      for (IntPtr pr = PixelRgn.Register(srcRgn, destRgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcRgn.Y; y < srcRgn.Y + srcRgn.H; y++)
	    {
	      for (int x = srcRgn.X; x < srcRgn.X + srcRgn.W; x++)
		{
		  Assert.AreEqual(pixel.Color, srcRgn[y, x].Color);
		  Assert.AreEqual(pixel2.Color, destRgn[y, x].Color);
		}
	    }
	}
      image.Delete();
    }

    void FillDrawable(Drawable drawable, Pixel pixel)
    {
      var rgn = new PixelRgn(drawable, true, false);

      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = rgn.Y; y < rgn.Y + rgn.H; y++)
	    {
	      for (int x = rgn.X; x < rgn.X + rgn.W; x++)
		{
		  rgn[y, x] = pixel;
		}
	    }
	}
    }
  }
}
