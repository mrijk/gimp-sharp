using System;

namespace Gimp.Ministeck
  {
    public class OneByOneShape : Shape
    {
      public OneByOneShape(int size) : base(size)
      {
	ShapeDescription shape = new ShapeDescription();

	Combine(shape);
      }

      protected override void Fill(PixelFetcher PR, int x, int y,
				   ShapeDescription shape)
      {
	Rectangle(PR, x, y, 1, 1);
      }
    }	
  }
