using System;
using System.Collections;

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

    public bool Fits(int[,] A, int x, int y)
    {
      int index = _random.Next(0, _set.Length);

      foreach (ShapeDescription shape in _set[index])
	{
	if (Fits(A, x, y, shape))
	  return true;
	}
      return false;
    }

    bool Fits(int[,] A, int x, int y, ShapeDescription shape)
    {
      int color = A[x, y];
      int width = A.GetLength(0);
      int height = A.GetLength(1);
      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	if (cx < 0 || cx >= width || cy < 0 || cy >= height ||
	    A[cx, cy] != color)
	  {
	  return false;
	  }
	}

      Fill(A, x, y, shape);
      _match++;
      return true;
    }

    void Fill(int[,] A, int x, int y, ShapeDescription shape)
    {
      int color = A[x, y];
      A[x, y] = -color;
      foreach (Coordinate c in shape)
	{
	A[x + c.X, y + c.Y] = -color;
	}

    }
  }
  }
