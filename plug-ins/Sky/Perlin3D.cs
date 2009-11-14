// The Sky plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Perlin3D.cs
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

namespace Gimp.Sky
{
  public class Perlin3D
  {
    readonly List<Octave> _octaves = new List<Octave>();

    public Perlin3D(int count, double frequency, double persistence,
		    int seed)
    {
      double max = 0.0;
      double amplitude = 1.0;

      for (int i = 0; i < count; i++)
	{
	  max += amplitude;
	  amplitude *= persistence;
	}
      amplitude = 1.0 / max;

      for (int i = 0; i < count; i++)
	{
	  _octaves.Add(new Octave(seed + i, frequency, amplitude));
	  
	  frequency *= 2.0;
	  amplitude *= persistence;
	}
    }

    public Perlin3D(int count, double frequency, double[] amplitudes,
		    int seed)
    {
      double max = 0.0;
      Array.ForEach(amplitudes, amplitude => max += amplitude);

      for (int i = 0; i < count; i++)
	{
	  _octaves.Add(new Octave(seed + i, frequency, amplitudes[i] / max));
	  
	  frequency *= 2.0;
	}
    }

    public double Get(double x, double y, double z)
    {
#if true
      double sum = 0.0;
      _octaves.ForEach(octave => sum += octave.Get(x, y, z));
      return sum;
#else
      // next line works, but is somehow a lot slower!
      return _octaves.Sum(octave => octave.Get(x, y, z));
#endif
    }
  }
}
