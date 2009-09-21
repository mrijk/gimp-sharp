// The Sky plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Octave.cs
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
  public class Octave
  {
    readonly double[] _data = new double[32768];
    readonly double _frequency;

    public Octave(int seed, double frequency, double amplitude)
    {
      var random = new TRandom(seed);

      for (int i = 0; i < 32768; i++)
	{
	  _data[i] = random.Rnd1() * amplitude;
	}
      _frequency = frequency;
    }

    double Interpolate(double a, double b, double weight)
    {
      weight = 0.5 + Math.Cos(weight * Math.PI) * 0.5;
      
      return a * weight + b * (1.0 - weight);
    }

    public double Get(double x, double y, double z)
    {
      x *= _frequency;
      y *= _frequency;
      
      int intX = (int) x;
      int intY = (int) y;
      int intZ = (int) z;
      
      double fracX = x - intX;
      double fracY = y - intY;
      double fracZ = z - intZ;
      
      intX &= 31;
      intY &= 31;
      intZ &= 31;
      
      double point_1 = _data[intY * 1024 + intX * 32 + intZ];
      double point_2 = _data[intY * 1024 + ((intX + 1) & 31) * 32 + intZ];
      double point_3 = _data[((intY + 1) & 31) * 1024 + intX * 32 + intZ];
      double point_4 = _data[((intY + 1) & 31) * 1024 + 
			     ((intX + 1) & 31) * 32 + intZ];
      
      double point_a = _data[intY * 1024 + intX * 32 + ((intZ + 1) & 31)];
      double point_b = _data[intY * 1024 + ((intX + 1) & 31) * 32 + 
			     ((intZ + 1) & 31)];
      double point_c = _data[((intY + 1) & 31) * 1024 + intX * 32 + 
			     ((intZ + 1) & 31)];
      double point_d = _data[((intY + 1) & 31) * 1024 + 
			     ((intX + 1) & 31) * 32 + ((intZ + 1) & 31)];
      
      double point_w = Interpolate(point_1, point_2, fracX);
      double pointX = Interpolate(point_3, point_4, fracX);
      
      double pointY = Interpolate(point_a, point_b, fracX);
      double pointZ = Interpolate(point_c, point_d, fracX);
      
      double point_A = Interpolate(point_w, pointX, fracY);
      double point_B = Interpolate(pointY, pointZ, fracY);
      
      return Interpolate(point_A, point_B, fracZ);
    }
  }
}
