using System;

namespace Ministeck
  {
    public class OneByOneShape : Shape
    {
      public OneByOneShape()
      {
	ShapeDescription shape = new ShapeDescription();

	_set = new ShapeSet[1];
	_set[0] = new ShapeSet(shape);
      }
    }	
  }
