using System;

using Gdk;

namespace Gimp.SliceTool
{
  public class HorizontalSlice : Slice
  {
    int _x1, _x2, _y;

    public HorizontalSlice(int x1, int x2, int y)
    {
      _x1 = x1;
      _x2 = x2;
      _y = y;
    }

    override public int CompareTo(object obj)
    {
      HorizontalSlice slice = obj as HorizontalSlice;
      return _y - slice._y;
    }

    override public void Draw(Preview preview, Gdk.GC gc)
    {
      preview.GdkWindow.DrawLine(gc, _x1, _y, _x2, _y);
    }

    override public bool IntersectsWith(Rectangle rectangle)
    {
      return _y > rectangle.Y1 && _y < rectangle.Y2
	&& _x1 <= rectangle.X1 && _x2 >= rectangle.X2;
    }

    override public Rectangle SliceRectangle(Rectangle rectangle)
    {
      Rectangle copy = new Rectangle(rectangle);
      rectangle.Bottom = this;
      copy.Top = this;
      return copy;
    }

    override public void SetPosition(int x, int y)
    {
      _y = y;
    }

    public int Y
    {
      get {return _y;}
      set {_y = value;}
    }

    override public void Dump()
    {
      Console.WriteLine("y: " + _y);
    }
  }
  }
