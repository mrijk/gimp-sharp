// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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

namespace Gimp
{
  public class Pixel
  {
    readonly int _bpp;
    readonly int[] _rgb;
    int _x;
    int _y;
	
    public Pixel(int bpp)
    {
      _bpp = bpp;
      _rgb = new int[_bpp];
    }

    public Pixel(byte[] rgb)
    {
      _bpp = rgb.Length;
      _rgb = new int[_bpp];

      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] = rgb[i];
	}
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
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] = p[i];
	}
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

    public byte[] Bytes
    {
      set
	{
	  for (int i = 0; i < _bpp; i++)
	    {
	      _rgb[i] = value[i];
	    }
	}

      get
	{
	  byte[] rgb = new byte[_bpp];

	  for (int i = 0; i < _bpp; i++)
	    {
	      rgb[i] = (byte) _rgb[i];
	    }
	  
	  return rgb;
	}
    }

    public void CopyTo(byte[] dest, long index)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  dest[index + i] = (byte) _rgb[i];
	}
    }

    public void CopyFrom(byte[] src, long index)
    {
      for (int i = 0; i < _bpp; i++)
	{
	  _rgb[i] = src[index + i];
	}
    }

    public int X
    {
      set {_x = value;}
    }
	
    public int Y
    {
      set {_y = value;}
    }

    public int this[int index]
    {
      get {return _rgb[index];}
      set {_rgb[index] = value;}
    }

    public int Red
    {
      get {return _rgb[0];}
      set {_rgb[0] = value;}
    }
	
    public int Green
    {
      get {return _rgb[1];}
      set {_rgb[1] = value;}
    }

    public int Blue
    {
      get {return _rgb[2];}
      set {_rgb[2] = value;}
    }

    public int Alpha
    {
      get {return _rgb[(_bpp == 2) ? 1 : 3];}
      set {_rgb[(_bpp == 2) ? 1 : 3] = value;}
    }

    public bool HasAlpha
    {
      get {return _bpp == 2 || _bpp == 4;}
    }

    public void Clamp0255()
    {
      for (int i = 0; i < _bpp; i++)
	{
	  if (_rgb[i] < 0)
	    {
	      _rgb[i] = 0;
	    }
	  else if (_rgb[i] > 255)
	    {
	      _rgb[i] = 255;
	    }
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

    public static Pixel operator / (Pixel p, int v)
    {
      return (new Pixel(p)).Divide(v);
    }

    public static Pixel operator + (Pixel p1, Pixel p2)
    {
      return (new Pixel(p1)).Add(p2);
    }

    public static Pixel operator - (Pixel p1, Pixel p2)
    {
      return (new Pixel(p1)).Substract(p2);
    }

    public static Pixel operator + (Pixel p, int v)
    {
      return (new Pixel(p)).Add(v);
    }

    public override string ToString()
    {
      return string.Format("({0} {1} {2})", _rgb[0], _rgb[1], _rgb[2]);
    }
  }
}
