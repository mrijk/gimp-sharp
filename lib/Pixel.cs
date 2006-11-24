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

    public Pixel(int r, int g, int b)
    {
      _rgb[0] = r;
      _rgb[1] = g;
      _rgb[2] = b;
    }

    public byte[] ToByte()
    {
      byte[] rgb = new byte[_bpp];

      for (int i = 0; i < _bpp; i++)
	{
	  rgb[i] = (byte) _rgb[i];
	}

      return rgb;
    }

    public int X
    {
      set {_x = value;}
    }
	
    public int Y
    {
      set {_y = value;}
    }

    public int Red
    {
      set {_rgb[0] = value;}
    }
	
    public int Green
    {
      set {_rgb[1] = value;}
    }

    public int Blue
    {
      set {_rgb[2] = value;}
    }

    public static Pixel operator / (Pixel p, int v)
    {
      for (int i = 0; i < p._bpp; i++)
	{
	  p._rgb[i] /= v;
	}
      return p;
    }
	
    public static Pixel operator + (Pixel p1, Pixel p2)
    {
      for (int i = 0; i < p1._bpp; i++)
	{
	  p1._rgb[i] += p2._rgb[i]; 
	}
      return p1;
    }

    public override string ToString()
    {
      return string.Format("({0} {1} {2})", _rgb[0], _rgb[1], _rgb[2]);
    }
  }
}
