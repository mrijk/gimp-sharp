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

    public bool Fits(PixelRgn PR, bool[,] A, int x, int y)
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

    bool Fits(PixelRgn PR, bool[,] A, int x, int y, ShapeDescription shape)
    {
      byte[] color = new byte[3];
      byte[] buf = new byte[3];
      PR.GetPixel(color, x * 16, y * 16);

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

	PR.GetPixel(buf, cx, cy);
	for (int b = 0; b < 3; b++)
	  {
	  if (color[b] != buf[b])
	    return false;
	  }
	}

      _match++;
      return true;
    }

    abstract protected void Fill(PixelRgn PR, int x, int y,
				 ShapeDescription shape) ;

    void Fill(PixelRgn PR, bool[,] A, int x, int y, ShapeDescription shape)
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

    PixelRgn _PR;
    int _x, _y;

    protected void LineStart(PixelRgn PR, int x, int y)
    {
      _PR = PR;
      _x = x * 16;
      _y = y * 16;
    }

    protected void Rectangle(PixelRgn PR, int x, int y, int w, int h)
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
	_PR.SetPixel(_color, _x, _y);
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
	_PR.SetPixel(_color, _x, _y);
	_y += dy;
	}
      _y -= dy;
    }
  }
  }
