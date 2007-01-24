// The Forge plug-in
// Copyright (C) 2006-2007 Massimo Perga (massimo.perga@gmail.com)
//
// StarFactory.cs
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
  class StarFactory
  {
    readonly Random _random;

    readonly double _starFraction;
    readonly double _starColour;

    public StarFactory(Random random, double starFraction, double starColour)
    {
      _random = random;
      _starFraction = starFraction;
      _starColour = starColour;
    }

    public Pixel Generate()
    {
      const double starQuality = 0.5;	    // Brightness distribution exponent
      const double starIntensity = 8;	    // Brightness scale factor
      const double starTintExp = 0.5;	    // Tint distribution exponent

      if ((_random.Next() % 1000) < _starFraction) 
	{
	  double v = starIntensity * Math.Pow(1 / (1 - Cast(0, 0.9999)), 
					      starQuality);
	  if (v > 255) v = 255;
	  
	  /* We make a special case for star colour  of zero in order to
	     prevent  floating  point  roundoff  which  would  otherwise
	     result  in  more  than  256 star colours.  We can guarantee
	     that if you specify no star colour, you never get more than
	     256 shades in the image. */

	  if (_starColour == 0) 
	    {
	      return new Pixel((byte)v, (byte)v, (byte)v);
	    } 
	  else 
	    {
	      double temp = 5500 + _starColour *
		Math.Pow(1 / (1 - Cast(0, 0.9999)), starTintExp) *
		(((_random.Next() & 7) != 0) ? -1 : 1);
	      /* Constrain temperature to a reasonable value: >= 2600K
		 (S Cephei/R Andromedae), <= 28,000 (Spica). */
	      temp = Math.Max(2600, Math.Min(28000, temp));
	      RGB rgb = TempRGB(temp);
	      
	      rgb.Multiply(v);
	      rgb.Add(new RGB(0.499, 0.499, 0.499));
	      rgb.Divide(255);
	      
	      return new Pixel(rgb.Bytes);
	    }
	} 
      else 
	{
	  return new Pixel(3);
	}
    }

    double Planck(double temperature, double lambda)  
    {
      const double c1 = 3.7403e10;
      const double c2 = 14384.0;
      double ret_val = c1 * Math.Pow(lambda, -5.0);
      return ret_val / (Math.Exp(c2 / (lambda * temperature)) - 1);
    }

    /*  TEMPRGB  --  Calculate the relative R, G, and B components for  a
        black	body  emitting	light  at a given temperature.
        The Planck radiation equation is solved directly  for
        the R, G, and B wavelengths defined for the CIE  1931
        Standard    Colorimetric    Observer.	  The	colour
        temperature is specified in degrees Kelvin. */

    RGB TempRGB(double temp)
    {
      // Lambda is the wavelength in microns: 5500 angstroms is 0.55 microns.
      
      double r = Planck(temp, 0.7000);
      double g = Planck(temp, 0.5461);
      double b = Planck(temp, 0.4358);

      RGB rgb = new RGB(r, g, b);
      rgb.Divide(rgb.Max);
      return rgb;
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }
  }
}
