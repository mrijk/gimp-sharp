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
