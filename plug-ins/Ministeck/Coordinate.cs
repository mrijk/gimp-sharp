using System;

namespace Ministeck
  {
    public class Coordinate
    {
      int _x, _y;
      public Coordinate(int x, int y)
      {
	_x = x;
	_y = y;
      }

      public int X
      {
	get {return _x;}
      }

      public int Y
      {
	get {return _y;}
      }
    }
  }
