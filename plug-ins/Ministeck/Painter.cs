using System;

namespace Gimp.Ministeck
  {
    public class Painter
    {
      PixelFetcher _pf;
      int _size;
      int _x, _y;
      byte[] _color = new byte[3]{3, 3, 3};

      public Painter(Drawable drawable, int size)
      {
	_pf = new PixelFetcher(drawable, false);
	_size = size;
      }

      public void Destroy()
      {
	_pf.Destroy();
      }

      public int Size
      {
	get {return _size;}
      }

      public void GetPixel(int x, int y, byte[] pixel)
      {
	x *= _size;
	y *= _size;
	_pf.GetPixel(x, y, pixel);
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
	  _pf.PutPixel(_x, _y, _color);
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
	  _pf.PutPixel(_x, _y, _color);
	  _y += dy;
	  }
	_y -= dy;
      }
    }
  }
