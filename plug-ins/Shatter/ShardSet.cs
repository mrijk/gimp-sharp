// The Shatter plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ShardSet.cs
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
using System.Collections.Generic;

namespace Gimp.Shatter
{
  public class ShardSet
  {
    List<Shard> _set = new List<Shard>();

    public ShardSet(Coord ul, Coord lr, int n)
    {
      Coord ur = new Coord(ul.X, lr.Y);
      Coord ll = new Coord(lr.X, ul.Y);
      _set = Split(new Shard(ul, ur, ll, lr), n);
      Console.WriteLine("n: " + n);
      Console.WriteLine("#shards: " + _set.Count);
    }
    
    public IEnumerator<Shard> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    List<Shard> Split(Shard shard, int n)
    {
      if (n == 0)
	{
	  return null;
	}
      else if (n == 1)
	{
	  List<Shard> set = new List<Shard>();
	  set.Add(shard);
	  return set;
	}
      else
	{
	  Shard shard1, shard2;
	  shard.Split(out shard1, out shard2);

	  int m = n / 2;

	  List<Shard> set1 = Split(shard1, m);
	  List<Shard> set2 = Split(shard2, n - m);
	  set1.AddRange(set2);
	  return set1;
	}
    }
  }
}
