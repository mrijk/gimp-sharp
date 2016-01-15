// The Forge plug-in
// Copyright (C) 2006-2016 Massimo Perga (massimo.perga@gmail.com)
//
// GaussDistribution.cs
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

namespace Gimp.Forge
{
  class GaussDistribution
  {
    const int _nRand = 4; // Gauss() sample count
    readonly double _arand, _gaussAdd, _gaussFac;
    readonly Random _random;

    public GaussDistribution(Random random)
    {
      _random = random;
      _arand = Math.Pow(2.0, 15.0) - 1.0;
      _gaussAdd = Math.Sqrt(3.0 * _nRand);
      _gaussFac = 2 * _gaussAdd / (_nRand * _arand);
    }
    
    public double Value
    {
      get
	{
	  double sum = 0.0;
	  
	  for (int i = 0; i < _nRand; i++) 
	    {
	      sum += (_random.Next() & 0x7FFF);
	    }
	  
	  return _gaussFac * sum - _gaussAdd;
	}
    }
  }
}
