using System;

namespace Ministeck
  {
    public class TwoByTwoShape : Shape
    {
      public TwoByTwoShape()
      {
	ShapeDescription shape = new ShapeDescription();
	shape.Add(0, 1);
	shape.Add(1, 0);
	shape.Add(1, 1);

	_set = new ShapeSet[1];
	_set[0] = new ShapeSet(shape);
      }
    }
  }
