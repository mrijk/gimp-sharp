using System;
using System.Collections;

using Gimp;

namespace Ministeck
  {
  abstract public class Shape
  {
    protected ShapeSet[] _set;
    Random _random = new Random();
    byte[] _color = new byte[3]{3, 3, 3};

    public int _match = 0;

    public Shape()
    {
    }

    public bool Fits(PixelFetcher PR, bool[,] A, int x, int y)
    {
      int index = _random.Next(0, _set.Length);

      foreach (ShapeDescription shape in _set[index])
	{
	if (Fits(PR, A, x, y, shape))
	  {
	  Fill(PR, A, x, y, shape);
	  return true;
	  }
	}
      return false;
    }

    bool Fits(PixelFetcher PR, bool[,] A, int x, int y, ShapeDescription shape)
    {
      byte[] color = new byte[3];
      byte[] buf = new byte[3];
      PR.GetPixel(x * 16, y * 16, color);

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
	cx *= 16;
	cy *= 16;

	PR.GetPixel(cx, cy, buf);
	for (int b = 0; b < 3; b++)
	  {
	  if (color[b] != buf[b])
	    return false;
	  }
	}

      _match++;
      return true;
    }

    abstract protected void Fill(PixelFetcher PR, int x, int y,
				 ShapeDescription shape) ;

    void Fill(PixelFetcher PR, bool[,] A, int x, int y, ShapeDescription shape)
    {
      Fill(PR, x, y, shape);
      A[x, y] = true;
      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	A[cx, cy] = true;
	}
    }

    PixelFetcher _PR;
    int _x, _y;

    protected void LineStart(PixelFetcher PR, int x, int y)
    {
      _PR = PR;
      _x = x * 16;
      _y = y * 16;
    }

    protected void Rectangle(PixelFetcher PR, int x, int y, int w, int h)
    {
      w *= 16;
      h *= 16;

      LineStart(PR, x, y);
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
	_PR.PutPixel(_x, _y, _color);
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
	_PR.PutPixel(_x, _y, _color);
	_y += dy;
	}
      _y -= dy;
    }
  }
  }
