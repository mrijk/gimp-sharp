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
	      if (rectangle.Inside(x, y))
		  return rectangle;
	  }
	return null;
      }

      public void Draw(Preview preview, double zoom)
      {
	foreach (Rectangle rectangle in _set)
	  {
	  rectangle.Draw(preview, zoom);
	  }
      }
    }
  }
