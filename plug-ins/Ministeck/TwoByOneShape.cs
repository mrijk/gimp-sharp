using System;

namespace Gimp.Ministeck
{
  public class TwoByOneShape : Shape
  {
    ShapeDescription _shape1 = new ShapeDescription();
    ShapeDescription _shape2 = new ShapeDescription();

    public TwoByOneShape()
    {
      _shape1.Add(0, 1);
      _shape2.Add(1, 0);

      Combine(_shape1, _shape2);
    }

    protected override void Fill(int x, int y, ShapeDescription shape)
    {
      if (shape == _shape1)	// Vertical
	{
	Rectangle(x, y, 1, 2);
	}
      else			// Horizontal
	{
	Rectangle(x, y, 2, 1);
	}
    }
  }
  }
