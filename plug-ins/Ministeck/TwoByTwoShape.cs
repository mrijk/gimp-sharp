using System;

namespace Gimp.Ministeck
  {
    public class TwoByTwoShape : Shape
    {
      public TwoByTwoShape()
      {
	ShapeDescription shape = new ShapeDescription();
	shape.Add(0, 1);
	shape.Add(1, 0);
	shape.Add(1, 1);

	Combine(shape);
      }

      protected override void Fill(int x, int y, ShapeDescription shape)
      {
	Rectangle(x, y, 2, 2);
      }
    }
  }
