using System;

namespace Ministeck
  {
    public class CornerShape : Shape
    {
      public CornerShape() 
      {
	ShapeDescription shape1 = new ShapeDescription();
	shape1.Add(0, 1);
	shape1.Add(1, 0);

	ShapeDescription shape2 = new ShapeDescription();
	shape2.Add(1, 0);
	shape2.Add(1, 1);

	ShapeDescription shape3 = new ShapeDescription();
	shape3.Add(0, 1);
	shape3.Add(1, 1);

	ShapeDescription shape4 = new ShapeDescription();
	shape4.Add(0, 1);
	shape4.Add(-1, 1);

	_set = new ShapeSet[24];
	_set[0] = new ShapeSet(shape1, shape2, shape3, shape4);
	_set[1] = new ShapeSet(shape1, shape2, shape4, shape3);
	_set[2] = new ShapeSet(shape1, shape3, shape2, shape4);
	_set[3] = new ShapeSet(shape1, shape3, shape4, shape2);
	_set[4] = new ShapeSet(shape1, shape4, shape3, shape2);
	_set[5] = new ShapeSet(shape1, shape4, shape2, shape3);
	_set[6] = new ShapeSet(shape2, shape1, shape3, shape4);
	_set[7] = new ShapeSet(shape2, shape1, shape4, shape3);
	_set[8] = new ShapeSet(shape2, shape3, shape1, shape4);
	_set[9] = new ShapeSet(shape2, shape3, shape4, shape1);
	_set[10] = new ShapeSet(shape2, shape4, shape1, shape3);
	_set[11] = new ShapeSet(shape2, shape4, shape3, shape1);
	_set[12] = new ShapeSet(shape3, shape1, shape2, shape4);
	_set[13] = new ShapeSet(shape3, shape1, shape4, shape2);
	_set[14] = new ShapeSet(shape3, shape2, shape1, shape4);
	_set[15] = new ShapeSet(shape3, shape2, shape4, shape1);
	_set[16] = new ShapeSet(shape3, shape4, shape1, shape2);
	_set[17] = new ShapeSet(shape3, shape4, shape2, shape1);
	_set[18] = new ShapeSet(shape4, shape1, shape2, shape3);
	_set[19] = new ShapeSet(shape4, shape1, shape3, shape2);
	_set[20] = new ShapeSet(shape4, shape2, shape1, shape3);
	_set[21] = new ShapeSet(shape4, shape2, shape3, shape1);
	_set[22] = new ShapeSet(shape4, shape3, shape1, shape2);
	_set[23] = new ShapeSet(shape4, shape3, shape2, shape1);
      }
    }
  }
