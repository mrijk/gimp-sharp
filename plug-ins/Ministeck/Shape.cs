using System;
using System.Collections;

using Gimp;

namespace Ministeck
  {
  abstract public class Shape
  {
    protected ShapeSet[] _set;
    Random _random = new Random();
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
	  return true;
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
	if (cx < 0 || cx >= width || cy < 0 || cy >= height)
	  return false;

	cx *= 16;
	cy *= 16;

	PR.GetPixel(buf, cx, cy);
	for (int b = 0; b < 3; b++)
	  {
	  if (color[b] != buf[b])
	    return false;
	  }
	}

      Fill(PR, A, x, y, shape);
      _match++;
      return true;
    }

    void Fill(PixelRgn PR, bool[,] A, int x, int y, ShapeDescription shape)
    {
      byte[] color = new byte[3]{3, 3, 3};

      A[x, y] = true;
      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	A[cx, cy] = true;
	PR.SetPixel(color, cx * 16, cy * 16);
	}
    }
  }
  }
