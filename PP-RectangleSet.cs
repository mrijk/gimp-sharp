using System;
using System.Collections;

namespace Gimp
  {
    public class RectangleSet
    {
      ArrayList _set = new ArrayList();

      public RectangleSet()
      {
      }

      public void Add(Rectangle rectangle)
      {
	_set.Add(rectangle);
      }

      public Rectangle Find(int x, int y)
      {
	foreach (Rectangle rectangle in _set)
	  {
	  }
	return null;
      }

      public void Draw(Gdk.GC gc)
      {
	foreach (Rectangle rectangle in _set)
	  {
	  rectangle.Draw(gc);
	  }
      }
    }
  }
