using System;

using Gimp;

namespace Ministeck
  {
    public class CornerShape : Shape
    {
      ShapeDescription _shape1 = new ShapeDescription();
      ShapeDescription _shape2 = new ShapeDescription();
      ShapeDescription _shape3 = new ShapeDescription();
      ShapeDescription _shape4 = new ShapeDescription();

      public CornerShape() 
      {
	_shape1.Add(0, 1);
	_shape1.Add(1, 0);

	_shape2.Add(1, 0);
	_shape2.Add(1, 1);

	_shape3.Add(0, 1);
	_shape3.Add(1, 1);

	_shape4.Add(0, 1);
	_shape4.Add(-1, 1);

	_set = new ShapeSet[24];
	_set[0] = new ShapeSet(_shape1, _shape2, _shape3, _shape4);
	_set[1] = new ShapeSet(_shape1, _shape2, _shape4, _shape3);
	_set[2] = new ShapeSet(_shape1, _shape3, _shape2, _shape4);
	_set[3] = new ShapeSet(_shape1, _shape3, _shape4, _shape2);
	_set[4] = new ShapeSet(_shape1, _shape4, _shape3, _shape2);
	_set[5] = new ShapeSet(_shape1, _shape4, _shape2, _shape3);
	_set[6] = new ShapeSet(_shape2, _shape1, _shape3, _shape4);
	_set[7] = new ShapeSet(_shape2, _shape1, _shape4, _shape3);
	_set[8] = new ShapeSet(_shape2, _shape3, _shape1, _shape4);
	_set[9] = new ShapeSet(_shape2, _shape3, _shape4, _shape1);
	_set[10] = new ShapeSet(_shape2, _shape4, _shape1, _shape3);
	_set[11] = new ShapeSet(_shape2, _shape4, _shape3, _shape1);
	_set[12] = new ShapeSet(_shape3, _shape1, _shape2, _shape4);
	_set[13] = new ShapeSet(_shape3, _shape1, _shape4, _shape2);
	_set[14] = new ShapeSet(_shape3, _shape2, _shape1, _shape4);
	_set[15] = new ShapeSet(_shape3, _shape2, _shape4, _shape1);
	_set[16] = new ShapeSet(_shape3, _shape4, _shape1, _shape2);
	_set[17] = new ShapeSet(_shape3, _shape4, _shape2, _shape1);
	_set[18] = new ShapeSet(_shape4, _shape1, _shape2, _shape3);
	_set[19] = new ShapeSet(_shape4, _shape1, _shape3, _shape2);
	_set[20] = new ShapeSet(_shape4, _shape2, _shape1, _shape3);
	_set[21] = new ShapeSet(_shape4, _shape2, _shape3, _shape1);
	_set[22] = new ShapeSet(_shape4, _shape3, _shape1, _shape2);
	_set[23] = new ShapeSet(_shape4, _shape3, _shape2, _shape1);
      }

      protected override void Fill(PixelFetcher PR, int x, int y,
				   ShapeDescription shape)
      {
	LineStart(PR, x, y);
	if (shape == _shape1)
	  {
	  HLine(2 * 16);
	  VLine(1 * 16);
	  HLine(-1 * 16 - 1);
	  VLine(1 * 16 + 1);
	  HLine(-1 * 16);
	  VLine(-2 * 16);
	  }
	else if (shape == _shape2)
	  {
	  HLine(2 * 16);
	  VLine(2 * 16);
	  HLine(-1 * 16);
	  VLine(-1 * 16 - 1);
	  HLine(-1 * 16 - 1);
	  VLine(-1 * 16);
	  }
	else if (shape == _shape3)
	  {
	  HLine(1 * 16);
	  VLine(1 * 16 + 1);
	  HLine(1 * 16 + 1);
	  VLine(1 * 16);
	  HLine(-2 * 16);
	  VLine(-2 * 16);
	  }
	else
	  {
	  HLine(1 * 16);
	  VLine(2 * 16);
	  HLine(-2 * 16);
	  VLine(-1 * 16);
	  HLine(1 * 16 + 1);
	  VLine(-1 * 16 - 1);
	  }
      }
    }
  }
