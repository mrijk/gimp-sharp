using System;

using Gimp;

namespace Ministeck
{
  public class TwoByOneShape : Shape
  {
    ShapeDescription _shape1 = new ShapeDescription();
    ShapeDescription _shape2 = new ShapeDescription();

    public TwoByOneShape()
    {
      _shape1.Add(0, 1);
      _shape2.Add(1, 0);

      _set = new ShapeSet[2];
      _set[0] = new ShapeSet(_shape1, _shape2);
      _set[1] = new ShapeSet(_shape2, _shape1);
    }

    protected override void Fill(PixelFetcher PR, int x, int y,
				 ShapeDescription shape)
    {
	if (shape == _shape1)	// Vertical
	  {
	  Rectangle(PR, x, y, 1, 2);
	  }
	else			// Horizontal
	  {
	  Rectangle(PR, x, y, 2, 1);
	  }
    }
  }
  }
