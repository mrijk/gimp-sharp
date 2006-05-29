// The Shatter plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Shard.cs
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
  public class Shard
  {
    static Random _random = new Random();
    Coord _ul, _ur, _ll, _lr;

    public Shard(Coord ul, Coord ur, Coord ll, Coord lr)
    {
      _ul = ul;
      _ur = ur;
      _ll = ll;
      _lr = lr;
    }
    
    public void Split(out Shard s1, out Shard s2)
    {
      bool horizontal = (_random.Next() & 1) == 1;
      
      Console.WriteLine(horizontal);

      if (horizontal)
	{
	  Coord c1 = Coord.PointInBetween(_ul, _ll, _random.NextDouble());
	  Coord c2 = Coord.PointInBetween(_ur, _lr, _random.NextDouble());
	  s1 = new Shard(_ul, _ur, c1, c2);
	  s2 = new Shard(c1, c2, _ll, _lr);
	}
      else
	{
	  Coord c1 = Coord.PointInBetween(_ul, _ur, _random.NextDouble());
	  Coord c2 = Coord.PointInBetween(_ll, _lr, _random.NextDouble());
	  s1 = new Shard(_ul, c1, _ll, c2);
	  s2 = new Shard(c1, _ur, c2, _lr);
	}		
    }

    public double[] GetValues()
    {
      return new double[] {_ul.X, _ul.Y, _ur.X, _ur.Y, _lr.X, _lr.Y,
			   _ll.X, _ll.Y};
    }
  }
}
