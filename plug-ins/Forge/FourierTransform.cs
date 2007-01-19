// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
//
// FourierTransform.cs
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
  class FourierTransform
  {
    /*	FOURN  --  Multi-dimensional fast Fourier transform
	
        Called with arguments:
	
        data       A  one-dimensional  array  of  floats  (NOTE!!!	NOT
        DOUBLES!!), indexed from one (NOTE!!!   NOT  ZERO!!),
        containing  pairs of numbers representing the complex
        valued samples.  The Fourier transformed results	are
        returned in the same array.

        nn	      An  array specifying the edge size in each dimension.
        THIS ARRAY IS INDEXED FROM  ONE,	AND  ALL  THE  EDGE
        SIZES MUST BE POWERS OF TWO!!!

        ndim       Number of dimensions of FFT to perform.  Set to 2 for
        two dimensional FFT.

        isign      If 1, a Fourier transform is done; if -1 the  inverse
        transformation is performed.

        This  function  is essentially as given in Press et al., "Numerical
        Recipes In C", Section 12.11, pp.  467-470.
        */

    public void Transform(double[] data, uint[] nn, uint ndim, int isign)
    {
      uint i1, i2, i3;
      uint i2rev, i3rev;
      uint ip1, ip2, ip3;
      uint ifp1, ifp2;
      uint ibit, idim, k1, k2;
      uint nprev;
      uint nrem;
      uint n;
      uint ntot;
      double tempi, tempr;
      double theta, wi, wpi, wpr, wr, wtemp;

      ntot = 1;
      for (idim = 1; idim <= ndim; idim++)
	{
	  ntot *= nn[idim];
	}

      nprev = 1;
      for (idim = ndim; idim >= 1; idim--) 
	{
	  n = nn[idim];
	  nrem = ntot / (n * nprev);
	  ip1 = nprev << 1;
	  ip2 = ip1 * n;
	  ip3 = ip2 * nrem;
	  i2rev = 1;
	  for (i2 = 1; i2 <= ip2; i2 += ip1) 
	    {
	      if (i2 < i2rev) 
		{
		  for (i1 = i2; i1 <= i2 + ip1 - 2; i1 += 2) 
		    {
		      for (i3 = i1; i3 <= ip3; i3 += ip2) 
			{
			  i3rev = i2rev + i3 - i2;
			  Swap(ref data[i3], ref data[i3rev]);
			  Swap(ref data[i3+1], ref data[i3rev+1]);
			}
		    }
		}
	      ibit = ip2 >> 1;
	      while (ibit >= ip1 && i2rev > ibit) 
		{
		  i2rev -= ibit;
		  ibit >>= 1;
		}
	      i2rev += ibit;
	    }
	  ifp1 = ip1;
	  while (ifp1 < ip2) 
	    {
	      ifp2 = ifp1 << 1;
	      theta = isign * (Math.PI * 2) / (ifp2 / ip1);
	      wtemp = Math.Sin(0.5 * theta);
	      wpr = -2.0 * wtemp * wtemp;
	      wpi = Math.Sin(theta);
	      wr = 1.0;
	      wi = 0.0;
	      for (i3 = 1; i3 <= ifp1; i3 += ip1) 
		{
		  for (i1 = i3; i1 <= i3 + ip1 - 2; i1 += 2) 
		    {
		      for (i2 = i1; i2 <= ip3; i2 += ifp2) 
			{
			  k1 = i2;
			  k2 = k1 + ifp1;
			  tempr = wr * data[k2] - wi * data[k2 + 1];
			  tempi = wr * data[k2 + 1] + wi * data[k2];
			  data[k2] = data[k1] - tempr;
			  data[k2 + 1] = data[k1 + 1] - tempi;
			  data[k1] += tempr;
			  data[k1 + 1] += tempi;
			}
		    }
		  wr = (wtemp = wr) * wpr - wi * wpi + wr;
		  wi = wi * wpr + wtemp * wpi + wi;
		}
	      ifp1 = ifp2;
	    }
	  nprev *= n;
	}
    }

    void Swap(ref double a, ref double b)
    {
      double tempSwap = a;
      a = b;
      b = tempSwap;
    }
  }
}
