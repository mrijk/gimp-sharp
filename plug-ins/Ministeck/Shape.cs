using System;
using System.Collections;

namespace Gimp.Ministeck
  {
  abstract public class Shape
  {
    ArrayList _set = new ArrayList(); 
    static protected Painter _painter;

    Random _random = new Random();
    byte[] _color = new byte[3]{3, 3, 3};

    public int _match = 0;

    public Shape()
    {
    }

    static public Painter Painter
    {
      set {_painter = value;}
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

    public bool Fits(bool[,] A, int x, int y)
    {
      int index = _random.Next(0, _set.Count);

      foreach (ShapeDescription shape in (ShapeSet) _set[index])
	{
	if (Fits(A, x, y, shape))
	  {
	  Fill(A, x, y, shape);
	  return true;
	  }
	}
      return false;
    }

    bool Fits(bool[,] A, int x, int y, ShapeDescription shape)
    {
      byte[] color = new byte[3];
      byte[] buf = new byte[3];
      _painter.GetPixel(x, y, color);

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

	_painter.GetPixel(cx, cy, buf);
	for (int b = 0; b < 3; b++)
	  {
	  if (color[b] != buf[b])
	    return false;
	  }
	}

      _match++;
      return true;
    }

    abstract protected void Fill(int x, int y, ShapeDescription shape) ;

    void Fill(bool[,] A, int x, int y, ShapeDescription shape)
    {
      Fill(x, y, shape);
      A[x, y] = true;
      foreach (Coordinate c in shape)
	{
	int cx = x + c.X;
	int cy = y + c.Y;
	A[cx, cy] = true;
	}
    }

    protected void LineStart(int x, int y)
    {
      _painter.LineStart(x, y);
    }

    protected void Rectangle(int x, int y, int w, int h)
    {
      _painter.Rectangle(x, y, w, h);
    }

    protected void HLine(int len)
    {
      _painter.HLine(len);
    }

    protected void VLine(int len)
    {
      _painter.VLine(len);
    }
  }
  }
