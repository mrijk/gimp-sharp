using System;
using System.Collections;

namespace Gimp
  {
    public class Rectangle
    {
      int _x, _y, _w, _h;

      public Rectangle(int x, int y, int w, int h)
      {
	_x = x;
	_y = y;
	_w = w;
	_h = h;
      }

      public void Draw(Gdk.GC gc)
      {
      }

      public bool Inside(int x, int y)
      {
	return x >= _x && x <= _x + _w
	  && y >= _y && y <= _y + _h;
      }
    }
  }
