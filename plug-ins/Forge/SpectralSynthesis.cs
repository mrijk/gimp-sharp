// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
//
// SpectralSynthesis.cs
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
  class SpectralSynthesis
  {
    const uint meshsize = 256;	      	// FFT mesh size
    Random _random;

    public SpectralSynthesis(Random random)
    {
      _random = random;
    }

    // SPECTRALSYNTH  --  Spectrally synthesised fractal motion in two
    // dimensions. This algorithm is given under  the name   
    // SpectralSynthesisFM2D on page 108 of Peitgen & Saupe.

    public double[] Synthesize(int n, double h)
    {
      double arand = Math.Pow(2.0, 15.0) - 1.0;
      double rad, phase, rcos, rsin;

      GaussDistribution gauss = new GaussDistribution(_random);

      ComplexMatrix cm = new ComplexMatrix((int) n);

      for (int i = 0; i <= n / 2; i++) 
      {
        for (int j = 0; j <= n / 2; j++) 
        {
          phase = 2 * Math.PI * ((_random.Next() & 0x7FFF) / arand);
          if (i != 0 || j != 0) 
          {
            rad = Math.Pow((double) (i * i + j * j), -(h + 1) / 2) 
              * gauss.Value;
          } 
          else 
          {
            rad = 0;
          }
          rcos = rad * Math.Cos(phase);
          rsin = rad * Math.Sin(phase);

	  cm[i, j] = new Complex(rcos, rsin);

          int i0 = (i == 0) ? 0 : n - i;
          int j0 = (j == 0) ? 0 : n - j;

	  cm[i0, j0] = new Complex(rcos, -rsin);
        }
      }

      cm[n / 2, 0] = new Complex(cm[n / 2, 0].Real, 0);
      cm[0, n / 2] = new Complex(cm[0, n / 2].Real, 0);
      cm[n / 2, n / 2] = new Complex(cm[n / 2, n / 2].Real, 0);

      for (int i = 1; i <= n / 2 - 1; i++) 
      {
        for (int j = 1; j <= n / 2 - 1; j++) 
        {
          phase = 2 * Math.PI * ((_random.Next() & 0x7FFF) / arand);
          rad = Math.Pow((double) (i * i + j * j), -(h + 1) / 2) 
	    * gauss.Value;
          rcos = rad * Math.Cos(phase);
          rsin = rad * Math.Sin(phase);

	  cm[i, n - j] = new Complex(rcos, rsin);
	  cm[n - 1, j] = new Complex(rcos, -rsin);
        }
      }

      // Dimension of frequency domain array
      uint[] nsize = new uint[]{0, (uint) n, (uint) n};

      FourierTransform fourier = new FourierTransform();

      double[] a = cm.ToFlatArray();
      fourier.Transform(a, nsize, 2, -1); // Take inverse 2D Fourier transform

      return a;
    }
  }
}
