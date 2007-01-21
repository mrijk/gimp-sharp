// The Ministeck plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// ShapeSet.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Gimp.Ministeck
{
  public class ShapeSet
  {
    readonly List<Shape> _set = new List<Shape>();
    readonly Random _random = new Random();
    long _combinations = 1;

    public void Add(Shape shape)
    {
      _set.Add(shape);
      _combinations *= _set.Count;
    }

    public void Add(int nr, Shape shape)
    {
      for (; nr > 0; nr--)
	{
	  Add(shape);
	}
    }

    public IEnumerator<Shape> GetEnumerator()
    {
      long index = (long) (_random.NextDouble() * _combinations);
      foreach (Shape shape in GeneratePermutation(index))
	yield return shape;
    }

    List<Shape> GeneratePermutation(long index)
    {
      List<Shape> permutation = _set;
      int len = _set.Count;
      long[] fac = new long[len];
      int[] idn = new int[len];

      fac[len - 1] = 1;
      idn[len - 1] = len - 1;
      for (int j = len - 2; j >= 0; j--)
	{
	  fac[j] = fac[j + 1] * (len - 1 - j);
	  idn[j] = j;
	}
      
      for (int j = 0; j < len; j++)
	{
	  int idx = (int) (index / fac[j]);
	  
	  int tmp = idn[j];
	  idn[j] = idn[j + idx];
	  idn[j + idx] = tmp;

	  Shape tmp1 = permutation[j];
	  permutation[j] = permutation[j + idx];
	  permutation[j + idx] = tmp1;
	  
	  index -= (idx * fac[j]);
	}
      return permutation; 
    }
  }
}
