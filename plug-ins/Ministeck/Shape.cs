using System;
using System.Collections;

namespace Gimp.Ministeck
  {
  abstract public class Shape
  {
    public ArrayList _set = new ArrayList(); 
    protected int _size;

    Random _random = new Random();
    byte[] _color = new byte[3]{3, 3, 3};

    public int _match = 0;

    public Shape(int size)
    {
      _size = size;
    }

    protected void Combine(params ShapeDescription[] list)
    {
      ShapeSet empty = new ShapeSet();
      _set.Add(empty);

      foreach (ShapeDescription val in list)
	{
	ArrayList copy = new ArrayList();
	foreach (ShapeSet ele in _set)
	  {
	  for (int i = 0; i <= ele.Count; i++)
	    {
	    ShapeSet tmp = new ShapeSet(ele);
	    tmp.Insert(i, val);
	    copy.Add(tmp);
	    }
	  }
	_set = copy;
	}
    }

    public bool Fits(PixelFetcher pf, bool[,] A, int x, int y)
    {
      int index = _random.Next(0, _set.Count);

      foreach (ShapeDescription shape in (ShapeSet) _set[index])
	{
	if (Fits(pf, A, x, y, shape))
	  {
	  Fill(pf, A, x, y, shape);
	  return true;
	  }
	}
      return false;
    }

    bool Fits(PixelFetcher pf, bool[,] A, int x, int y, ShapeDescription shape)
    {
      byte[] color = new byte[3];
      byte[] buf = new byte[3];
      pf.GetPixel(x * _size, y * _size, color);

      int width = A.GetLength(0);
      int height = A.GetLength(1);

      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	if (cx < 0 || cx >= width || cy < 0 || cy >= height || A[cx, cy])
	  {
	  return false;
	  }
	cx *= _size;
	cy *= _size;

	pf.GetPixel(cx, cy, buf);
	for (int b = 0; b < 3; b++)
	  {
	  if (color[b] != buf[b])
	    return false;
	  }
	}

      _match++;
      return true;
    }

    abstract protected void Fill(PixelFetcher pf, int x, int y,
				 ShapeDescription shape) ;

    void Fill(PixelFetcher pf, bool[,] A, int x, int y, ShapeDescription shape)
    {
      Fill(pf, x, y, shape);
      A[x, y] = true;
      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	A[cx, cy] = true;
	}
    }

    PixelFetcher _pf;
    int _x, _y;

    protected void LineStart(PixelFetcher pf, int x, int y)
    {
      _pf = pf;
      _x = x * _size;
      _y = y * _size;
    }

    protected void Rectangle(PixelFetcher pf, int x, int y, int w, int h)
    {
      w *= _size;
      h *= _size;

      LineStart(pf, x, y);
      HLine(w);
      VLine(h);
      HLine(-w);
      VLine(-h);
    }

    protected void HLine(int len)
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

    protected void VLine(int len)
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
