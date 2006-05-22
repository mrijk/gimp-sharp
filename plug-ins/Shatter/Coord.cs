// The Shatter plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Coord.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

namespace Gimp.Shatter
{
  public class Coord
  {
    double _x, _y;
    
    public Coord(double x, double y)
    {
      _x = x;
      _y = y;
    }
    
    static public Coord PointInBetween(Coord c1, Coord c2, double p)
    {
      double x = c1.X + p * (c2.X - c1.X);
      double y = c1.Y + p * (c2.Y - c1.Y);
      return new Coord(x, y);
    } 
    
    public double X
    {
      get {return _x;}
    }
    
    public double Y
    {
      get {return _y;}
    }
  }
}
