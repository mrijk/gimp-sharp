using System;

namespace Gimp.Ministeck
  {
    public class CornerShape : Shape
    {
      ShapeDescription _shape1 = new ShapeDescription();
      ShapeDescription _shape2 = new ShapeDescription();
      ShapeDescription _shape3 = new ShapeDescription();
      ShapeDescription _shape4 = new ShapeDescription();

      public CornerShape(int size) : base(size) 
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

	Combine(_shape1, _shape2, _shape3, _shape4);
      }

      protected override void Fill(PixelFetcher pf, int x, int y,
				   ShapeDescription shape)
      {
	LineStart(pf, x, y);
	if (shape == _shape1)
	  {
	  HLine(2 * _size);
	  VLine(1 * _size);
	  HLine(-_size - 1);
	  VLine(_size + 1);
	  HLine(-_size);
	  VLine(-2 * _size);
	  }
	else if (shape == _shape2)
	  {
	  HLine(2 * _size);
	  VLine(2 * _size);
	  HLine(-_size);
	  VLine(-_size - 1);
	  HLine(-_size - 1);
	  VLine(-_size);
	  }
	else if (shape == _shape3)
	  {
	  HLine(_size);
	  VLine(_size + 1);
	  HLine(_size + 1);
	  VLine(_size);
	  HLine(-2 * _size);
	  VLine(-2 * _size);
	  }
	else
	  {
	  HLine(_size);
	  VLine(2 * _size);
	  HLine(-2 * _size);
	  VLine(-_size);
	  HLine(_size + 1);
	  VLine(-_size - 1);
	  }
      }
    }
  }
