using System;

using Gimp;

namespace Ministeck
  {
    public class OneByOneShape : Shape
    {
      public OneByOneShape(int size) : base(size)
      {
	ShapeDescription shape = new ShapeDescription();

	_set = new ShapeSet[1];
	_set[0] = new ShapeSet(shape);
      }

      protected override void Fill(PixelFetcher PR, int x, int y,
				   ShapeDescription shape)
      {
	Rectangle(PR, x, y, 1, 1);
      }
    }	
  }
