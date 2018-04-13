// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// Pixel.cs
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
using System.Runtime.InteropServices;

using GLib;

namespace Gimp
{
  public class Pixel
  {
    static Random _random = new Random();

    PixelRgn _rgn;

    readonly int _bpp;
    readonly int _bppWithoutAlpha;
    readonly int[] _rgb;

    public int Bpp => _bpp;

    public Pixel(int bpp)
    {
      _bpp = bpp;
      _bppWithoutAlpha = (HasAlpha) ? bpp - 1 : bpp;
      _rgb = new int[bpp];
    }

    public Pixel(byte[] rgb) : this(rgb.Length)
    {
      Bytes = rgb;
    }

    public Pixel(int r, int g, int b) : this(3)
    {
      _rgb[0] = r;
      _rgb[1] = g;
      _rgb[2] = b;
    }

    public Pixel(int r, int g, int b, int a) : this(4)
    {
      _rgb[0] = r;
      _rgb[1] = g;
      _rgb[2] = b;
      _rgb[3] = a;
    }

    internal Pixel(Pixel p) : this(p._bpp)
    {
      Array.Copy(p._rgb, _rgb, Bpp);
    }

    internal Pixel(PixelRgn rgn, byte[] rgb) : this(rgb)
    {
      _rgn = rgn;
    }

    public void Fill(Func<int> func)
    {
      for (int i = 0; i < _bppWithoutAlpha; i++)
	{
	  _rgb[i] = func();
	}
    }

    public void Fill(Func<int, int> func)
    {
      for (int i = 0; i < _bppWithoutAlpha; i++)
	{
	  _rgb[i] = func(_rgb[i]);
	}
    }

    public void FillSame(Func<int> func)
    {
      int val = func();
      for (int i = 0; i < _bppWithoutAlpha; i++)
	{
	  _rgb[i] = val;
	}
    }

    public void Set(Pixel pixel)
    {
      _rgn[Y, X] = pixel;
    }

    public bool IsSameColor(Pixel pixel)
    {
      for (int i = 0; i < pixel._bpp; i++)
	{
	  if (_rgb[i] != pixel._rgb[i])
	    {
	      return false;
	    }
	}
      return true;
    }

    public RGB Color
    {
      get => new RGB((byte) Red, (byte) Green, (byte) Blue);

      set
	{
	  value.GetUchar(out byte r, out byte g, out byte b);
	  _rgb[0] = r;
	  _rgb[1] = g;
	  _rgb[2] = b;
	}
    }

    public byte[] Bytes
    {
      set
	{
	  Array.Copy(value, _rgb, _bpp);
	}

      get
	{
	  return Array.ConvertAll(_rgb, 
				  new Converter<int, byte>(ConvertToByte));
	}
    }

    static byte ConvertToByte(int value) => (byte) value;

    public void CopyTo(byte[] dest, long index)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  dest[index + i] = (byte) _rgb[i];
	}      
    }

    public void CopyFrom(byte[] src, long index)
    {
      Array.Copy(src, index, _rgb, 0, _bpp);
    }

    public int X {get; set;}
	
    public int Y {get; set;}

    public int this[int index]
    {
      get => _rgb[index];
      set {_rgb[index] = value;}
    }

    public int Red
    {
      get => _rgb[0];
      set {_rgb[0] = value;}
    }
	
    public int Green
    {
      get => _rgb[1];
      set {_rgb[1] = value;}
    }

    public int Blue
    {
      get => _rgb[2];
      set {_rgb[2] = value;}
    }

    public int Alpha
    {
      get => _rgb[(_bpp == 2) ? 1 : 3];
      set {_rgb[(_bpp == 2) ? 1 : 3] = value;}
    }

    public bool HasAlpha => Bpp == 2 || Bpp == 4;

    public void Clamp0255()
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] = Clamp(_rgb[i]);
	}
    }

    int Clamp(int rgb) => (rgb < 0) ? 0 : ((rgb > 255) ? 255 : rgb);

    public void AddNoise(int noise)
    {
      for (int i = 0; i < _bppWithoutAlpha; i++)
	{
	  _rgb[i] = Clamp(_rgb[i] + _random.Next(-noise, noise));
	}
    }

    public Pixel Add(Pixel p)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] += p._rgb[i];
	}
      return this;
    }

    public Pixel Add(int v)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] += v;
	}
      return this;
    }

    public Pixel Substract(Pixel p)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] -= p._rgb[i];
	}
      return this;
    }

    public Pixel Divide(int v)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] /= v;
	}
      return this;
    }

    public static Pixel operator / (Pixel p, int v) => 
      (new Pixel(p)).Divide(v);

    public static Pixel operator + (Pixel p1, Pixel p2) =>
      (new Pixel(p1)).Add(p2);

    public static Pixel operator - (Pixel p1, Pixel p2) =>
      (new Pixel(p1)).Substract(p2);

    public static Pixel operator + (Pixel p, int v) => 
      (new Pixel(p)).Add(v);

    public override string ToString() => $"({_rgb[0]} {_rgb[1]} {_rgb[2]})";

    static internal Pixel[,] ConvertToPixelArray(IntPtr src, 
						 Dimensions dimensions,
						 int bpp)
    {
      int width = dimensions.Width;
      int height = dimensions.Height;
      var dest = new byte[width * height * bpp];
      Marshal.Copy(src, dest, 0, width * height * bpp);

      var thumbnail = new Pixel[height, width];
      
      int index = 0;
      for (int y = 0; y < height; y++)
	{
	  for (int x = 0; x < width; x++)
	    {
	      var pixel = new Pixel(bpp);
	      pixel.CopyFrom(dest, index);
	      index += bpp;
	      thumbnail[y, x] = pixel;
	    }
	}
      return thumbnail;
    }
  }
}
