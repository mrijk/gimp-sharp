using System;

using Gimp;

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

      protected override void Fill(PixelFetcher PR, int x, int y,
				   ShapeDescription shape)
      {
	Rectangle(PR, x, y, 2, 2);
      }
    }
  }
