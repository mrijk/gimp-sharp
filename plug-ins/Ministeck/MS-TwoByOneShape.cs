using System;

namespace Ministeck
{
  public class TwoByOneShape : Shape
  {
    public TwoByOneShape()
    {
      ShapeDescription shape1 = new ShapeDescription();
      shape1.Add(0, 1);

      ShapeDescription shape2 = new ShapeDescription();
      shape2.Add(1, 0);

      _set = new ShapeSet[2];
      _set[0] = new ShapeSet(shape1, shape2);
      _set[1] = new ShapeSet(shape2, shape1);
    }
  }
  }
