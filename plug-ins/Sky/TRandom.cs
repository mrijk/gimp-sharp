// The Sky plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// TRandom.cs
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

namespace Gimp.Sky
{
  public class TRandom
  {
    double[] _u = new double[98];
    double _c, _cd, _cm;
    int _i97, _j97;

    public TRandom(int seed)
    {
      if (seed == -1)
	{
	  DateTime startTime = new DateTime(1970, 1, 1);
	  UInt32 time_t = 
	    Convert.ToUInt32((DateTime.Now - startTime).TotalSeconds);
	  Init((int) time_t);
	}
      else
	{
	  Init(seed);
	}
    }

    void Init(int seed)
    {
      int i = Math.Abs(97 * seed) % 31329;
      int j = Math.Abs(33 * seed) % 30082;

      InitSequence(i, j);
    }

    void InitSequence(int ij, int kl)
    {
      int i = (ij / 177) % 177 + 2;
      int j = ij % 177 + 2;
      int k = (kl / 169) % 178 + 1;
      int l = kl % 169;
        
      for (int ii = 1; ii <= 97; ii++) 
	{
	  double s = 0.0;
	  double t = 0.5;
	  for (int jj = 1; jj <= 24; jj++) 
	    {
	      int m = (((i * j) % 179) * k) % 179;
	      i = j;
	      j = k;
	      k = m;
	      l = (53 * l + 1) % 169;
	      if ((l * m) % 64 >= 32) s += t;
	      t *= 0.5;
	    }
	  _u[ii] = s;
	}
        
      _c = 362436.0 / 16777216.0;
      _cd = 7654321.0 / 16777216.0;
      _cm = 16777213.0 / 16777216.0;
      
      _i97 = 97;
      _j97 = 33;
    }

    public double Rnd1()
    {
      // difference between two [0..1] #s
      double uni = _u[_i97] - _u[_j97];
      if (uni < 0.0)
	uni += 1.0;
      _u[_i97] = uni;

      // i97 ptr decrements and wraps around
      _i97--;
      if (_i97 == 0) 
	_i97 = 97;

      // j97 ptr decrements likewise
      _j97--;
      if (_j97 == 0) 
	_j97 = 97;
      
      // finally, condition with c-decrement
      _c -= _cd;
      if (_c < 0.0) 
	_c += _cm; // cm > cd we hope! (only way c always >0)

      uni -= _c;
      if (uni < 0.0) 
	uni += 1.0;
      return uni;
    }
  }
}
