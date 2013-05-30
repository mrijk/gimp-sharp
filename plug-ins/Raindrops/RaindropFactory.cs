// The Raindrops plug-in
// Copyright (C) 2004-2013 Maurits Rijk
//
// RaindropFactory.cs
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

namespace Gimp.Raindrops
{
  public class RaindropFactory
  {
    readonly Random _random = new Random();
    public BoolMatrix BoolMatrix {get; private set;}
    readonly int _dropSize;
    readonly double _newCoeff;

    public RaindropFactory(int dropSize, int fishEye, Dimensions dimensions)
    {
      _dropSize = dropSize;
      _newCoeff = Clamp(fishEye, 1, 100) * 0.01;
      BoolMatrix = new BoolMatrix(dimensions.Width, dimensions.Height);
    }

    public Raindrop Create()
    {
      int size = _random.Next(_dropSize);
      int radius = size / 2;
      var center = BoolMatrix.Generate(radius);

      return (center == null) ? null : new Raindrop(center, size, _newCoeff);
    }

    int Clamp(int x, int l, int u)
    {
      return (x < l) ? l : ((x > u) ? u : x);
    }
  }
}
