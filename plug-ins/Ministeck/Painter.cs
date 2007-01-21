// The Ministeck plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// Painter.cs
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

namespace Gimp.Ministeck
{
  public class Painter : IDisposable
  {
    readonly PixelFetcher _pf;
    readonly Pixel _color;
    readonly int _size;
    int _x, _y;

    public Painter(Drawable drawable, int size, RGB color)
    {
      _color = new Pixel(color.Bytes);
      _pf = new PixelFetcher(drawable);
      _size = size;
    }

    public void Dispose()
    {
      _pf.Dispose();
      GC.SuppressFinalize(this);
    }

    public int Size
    {
      get {return _size;}
    }

    public Pixel GetPixel(int x, int y)
    {
      return _pf[y * _size, x * _size];
    }

    public void LineStart(int x, int y)
    {
      _x = x * _size;
      _y = y * _size;
    }
      
    public void Rectangle(int x, int y, int w, int h)
    {
      w *= _size;
      h *= _size;
	
      LineStart(x, y);
      HLine(w);
      VLine(h);
      HLine(-w);
      VLine(-h);
    }

    public void HLine(int len)
    {
      int dx = 1;
      if (len < 0)
	{
	  len = -len;
	  dx = -1;
	}
	
      for (int i = 0; i < len; i++)
	{
	  _pf[_y, _x] = _color;
	  _x += dx;
	}
      _x -= dx;
    }
      
    public void VLine(int len)
    {
      int dy = 1;
      if (len < 0)
	{
	  len = -len;
	  dy = -1;
	}
	
      for (int i = 0; i < len; i++)
	{
	  _pf[_y, _x] = _color;
	  _y += dy;
	}
      _y -= dy;
    }
  }
}
