// The Forge plug-in
// Copyright (C) 2004-2006 Massimo Perga
//
// Pixel.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.Forge
{
  public class Pixel
  {
    public const int MONO = 1;
    public const int RGB  = 3;
    public const int RGBA = 4;

    public enum ColorName {
      Red,
        Green,
        Blue,
        Alpha,
        MaxColorName,
        Grey = 0
    };

    private byte _grey, _red, _green, _blue, _alpha;
    private int _bpp;

    public Pixel() : this(0, 0, 0)
    {
    }

    public Pixel(byte grey)
    {
      _bpp = MONO;
      _grey = grey;
    }

    public Pixel(byte red, byte green, byte blue)
    {
      _bpp = RGB;
      _red = red;
      _green = green;
      _blue = blue;
    }

    public Pixel(byte red, byte green, byte blue, byte alpha) : this(red,green,blue)
    {
      _bpp = RGBA;
      _alpha = alpha;
    }

    public int Bpp 
    {
      get { return _bpp; }
      set { _bpp = value;}
    }

    public byte[] ToByte()
    {
      byte[] retArray = new byte[_bpp];
      switch(_bpp)
      {
        case MONO:
          retArray[0] = _grey;
          break;
        case RGBA:
          retArray[0] = _red;
          retArray[1] = _green;
          retArray[2] = _blue;
          retArray[3] = _alpha;
          break;
        case RGB:
          retArray[0] = _red;
          retArray[1] = _green;
          retArray[2] = _blue;
          break;
      }
      return retArray;
    }

    public void SetColor(ColorName colorIndex, byte colorValue)
    {
      if(_bpp == RGB || _bpp == RGBA)
      {
        switch(colorIndex)
        {
          case ColorName.Red:
            _red = colorValue;
            break;
          case ColorName.Green:
            _green = colorValue;
            break;
          case ColorName.Blue:
            _blue = colorValue;
            break;
          case ColorName.Alpha:
            _alpha = colorValue;
            break;
        }
      }
      else if(_bpp == MONO && colorIndex == ColorName.Grey)
        _grey = colorValue;
    }

    public byte Red
    {
      get { return _red; }
      set { _red = value; }
    }

    public byte Green
    {
      get { return _green; }
      set { _green = value; }
    }

    public byte Blue
    {
      get { return _blue; }
      set { _blue = value; }
    }

    public byte Alpha
    {
      get { return _alpha; }
      set { _alpha = value; }
    }

    public byte Grey
    {
      get { return _grey; }
      set { _grey = value; }
    }

    public void SetColor(byte grey)
    {
      _grey = grey;
    }

    public void SetColor(byte red, byte green, byte blue)
    {
      _red = red;
      _green = green;
      _blue = blue;
    }

    public void SetColor(byte red, byte green, byte blue, byte alpha)
    {
      SetColor(red, green, blue);
      _alpha = alpha;
    }
  }
}
