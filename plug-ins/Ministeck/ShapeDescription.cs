using System;
using System.Collections;

namespace Gimp.Ministeck
  {
    public class ShapeDescription : IEnumerable 
    {
      ArrayList _shape = new ArrayList();

      public ShapeDescription()
      {
      }

      public void Add(int x, int y)
      {
	_shape.Add(new Coordinate(x, y));
      }

      IEnumerator IEnumerable.GetEnumerator() 
      {
	return _shape.GetEnumerator();
      }
    }
  }
