using System;

using Gdk;

namespace Gimp.SliceTool
{
  public class VerticalSlice : Slice
  {
    int _x, _y1, _y2;

    public VerticalSlice(int x, int y1, int y2)
    {
      _x = x;
      _y1 = y1;
      _y2 = y2;
    }

    override public int CompareTo(object obj)
    {
      VerticalSlice slice = obj as VerticalSlice;
      return _x - slice._x;
    }

    override public void Draw(Preview preview, Gdk.GC gc)
    {
      preview.GdkWindow.DrawLine(gc, _x, _y1, _x, _y2);
    }

    override public bool IntersectsWith(Rectangle rectangle)
    {
      return _x > rectangle.X1 && _x < rectangle.X2
	&& _y1 <= rectangle.Y1 && _y2 >= rectangle.Y2;
    }

    override public Rectangle SliceRectangle(Rectangle rectangle)
    {
      Rectangle copy = new Rectangle(rectangle);
      rectangle.Right = this;
      copy.Left = this;
      return copy;		
    }

    override public void SetPosition(int x, int y)
    {
      _x = x;
    }

    public int X
    {
      get {return _x;}
      set {_x = value;}
    }

    override public void Dump()
    {
      Console.WriteLine("x: " + _x);
    }
  }
  }
