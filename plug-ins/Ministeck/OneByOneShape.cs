using System;

namespace Gimp.Ministeck
  {
    public class OneByOneShape : Shape
    {
      public OneByOneShape()
      {
	Combine(new ShapeDescription());
      }

      protected override void Fill(int x, int y, ShapeDescription shape)
      {
	Rectangle(x, y, 1, 1);
      }
    }	
  }
