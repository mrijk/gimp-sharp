using System;

namespace Ministeck
  {
    public class ThreeByOneShape : Shape
    {
      public ThreeByOneShape()
      {
	ShapeDescription shape1 = new ShapeDescription();
	shape1.Add(0, 1);
	shape1.Add(0, 2);

	ShapeDescription shape2 = new ShapeDescription();
	shape2.Add(1, 0);
	shape2.Add(2, 0);

	_set = new ShapeSet[2];
	_set[0] = new ShapeSet(shape1, shape2);
	_set[1] = new ShapeSet(shape2, shape1);
      }
    }
  }
