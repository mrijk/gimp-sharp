using System;

namespace Gimp
  {
    public class Layout
    {
      RectangleSet _rectangles = new RectangleSet();

      public Layout()
      {
      }

      public void Add(Rectangle rectangle)
      {
	_rectangles.Add(rectangle);
      }

      public Rectangle Find(int x, int y)
      {
	return _rectangles.Find(x, y);
      }

      public void Draw(Gdk.GC gc)
      {
	_rectangles.Draw(gc);
      }
    }
  }
