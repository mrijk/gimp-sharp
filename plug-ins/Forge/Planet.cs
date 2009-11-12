// The Forge plug-in
// Copyright (C) 2006-2009 Maurits Rijk
//
// Planet.cs
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
  class Planet
  {
    readonly bool _stars;
    readonly bool _clouds;
    readonly Random _random;
    readonly bool _hourspec, _inclspec;

    readonly double _starfraction;
    readonly double _starcolour;
    readonly double _icelevel;
    readonly double _glaciers;
    readonly double _hourangle, _inclangle;

    readonly Progress _progress;

    const double planetAmbient = 0.05;
    const int meshsize = 256;	      	// FFT mesh size

    byte [,] pgnd = new byte[99,3] {
      {206, 205, 0}, {208, 207, 0}, {211, 208, 0},
      {214, 208, 0}, {217, 208, 0}, {220, 208, 0},
      {222, 207, 0}, {225, 205, 0}, {227, 204, 0},
      {229, 202, 0}, {231, 199, 0}, {232, 197, 0},
      {233, 194, 0}, {234, 191, 0}, {234, 188, 0},
      {233, 185, 0}, {232, 183, 0}, {231, 180, 0},
      {229, 178, 0}, {227, 176, 0}, {225, 174, 0},
      {223, 172, 0}, {221, 170, 0}, {219, 168, 0},
      {216, 166, 0}, {214, 164, 0}, {212, 162, 0},
      {210, 161, 0}, {207, 159, 0}, {205, 157, 0},
      {203, 156, 0}, {200, 154, 0}, {198, 152, 0},
      {195, 151, 0}, {193, 149, 0}, {190, 148, 0},
      {188, 147, 0}, {185, 145, 0}, {183, 144, 0},
      {180, 143, 0}, {177, 141, 0}, {175, 140, 0},
      {172, 139, 0}, {169, 138, 0}, {167, 137, 0},
      {164, 136, 0}, {161, 135, 0}, {158, 134, 0},
      {156, 133, 0}, {153, 132, 0}, {150, 132, 0},
      {147, 131, 0}, {145, 130, 0}, {142, 130, 0},
      {139, 129, 0}, {136, 128, 0}, {133, 128, 0},
      {130, 127, 0}, {127, 127, 0}, {125, 127, 0},
      {122, 127, 0}, {119, 127, 0}, {116, 127, 0},
      {113, 127, 0}, {110, 128, 0}, {107, 128, 0},
      {104, 128, 0}, {102, 127, 0}, { 99, 126, 0},
      { 97, 124, 0}, { 95, 122, 0}, { 93, 120, 0},
      { 92, 117, 0}, { 92, 114, 0}, { 92, 111, 0},
      { 93, 108, 0}, { 94, 106, 0}, { 96, 104, 0},
      { 98, 102, 0}, {100, 100, 0}, {103,  99, 0},
      {106,  99, 0}, {109,  99, 0}, {111, 100, 0},
      {114, 101, 0}, {117, 102, 0}, {120, 103, 0},
      {123, 102, 0}, {125, 102, 0}, {128, 100, 0},
      {130,  98, 0}, {132,  96, 0}, {133,  94, 0},
      {134,  91, 0}, {134,  88, 0}, {134,  85, 0},
      {133,  82, 0}, {131,  80, 0}, {129,  78, 0}
    };

    public Planet(Drawable drawable, byte[] pixelArray, Dimensions dimensions,
		  bool stars, double starfraction, double starcolour,
		  bool clouds, Random random, 
		  double icelevel, double glaciers,
		  double fracdim, 
		  bool hourspec, double hourangle,
		  bool inclspec, double inclangle,
		  double powscale, Progress progress)
    {
      _stars = stars;
      _starfraction = starfraction;
      _starcolour = starcolour;
      _clouds = clouds;
      _icelevel = icelevel;
      _glaciers = glaciers;
      _random = random;
      _hourspec = hourspec;
      _inclspec = inclspec;
      _hourangle = hourangle;
      _inclangle = inclangle;
      _progress = progress;

      double[] a = null;

      if (!stars)
	{
	  var spectrum = new SpectralSynthesis(random);
	  a = spectrum.Synthesize(meshsize, 3.0 - fracdim);

	  // Apply power law scaling if non-unity scale is requested.
	  if (powscale != 1.0) 
	    {
	      for (int i = 0; i < meshsize; i++) 
		{
		  for (int j = 0; j < meshsize; j++) 
		    {
		      //        double r = Real(a, i, j);
		      double r = a[1 + (i * meshsize + j) * 2];

		      if (r > 0) 
			{
			  //      Real(a, i, j) = Math.Pow(r, powscale);
			  a[1 + (i * meshsize + j) * 2] = Math.Pow(r, powscale);
			}
		    }
		}
	    }

	  // Compute extrema for autoscaling.

	  double rmin = double.MaxValue;
	  double rmax = double.MinValue;

	  for (int i = 0; i < meshsize; i++) 
	    {
	      for (int j = 0; j < meshsize; j++) 
		{
		  //	    double r = Real(a, i, j);
		  double r = a[1 + (i * meshsize + j) * 2];

		  rmin = Math.Min(rmin, r);
		  rmax = Math.Max(rmax, r);
		}
	    }

	  double rmean = (rmin + rmax) / 2;
	  double rrange = (rmax - rmin) / 2;

	  for (int i = 0; i < meshsize; i++) 
	    {
	      for (int j = 0; j < meshsize; j++) 
		{
		  //	    Real(a, i, j) = (Real(a, i, j) - rmean) / rrange;
		  a[1 + (i * meshsize + j) * 2] = 
		    (a[1 + (i * meshsize + j) * 2] - rmean) / rrange;
		}
	    }
	}

      GenPlanet(drawable, pixelArray, dimensions, a, meshsize);
    }

    void GenPlanet(Drawable drawable, byte[] pixelArray, 
		   Dimensions dimensions, double[] a, uint n)
    {
      const double rgbQuant = 255; 
      const double atthick = 1.03;   /* Atmosphere thickness as a 
                                        percentage of planet's diameter */
      byte[] cp = null;
      byte[] ap = null;
      double[] u = null;
      double[] u1 = null;
      uint[] bxf = null;
      uint[] bxc = null;
      uint ap_index = 0;

      uint  rpix_offset = 0; // Offset to simulate the pointer for rpix
      double athfac = Math.Sqrt(atthick * atthick - 1.0);
      Vector3 sunvec = new Vector3();
      const double starClose = 2;
      const double atSatFac = 1.0;

      double r;
      double t = 0;
      double t1 = 0;
      double by, dy;
      double dysq = 0;
      double sqomdysq, icet;
      int lcos = 0;
      double dx; 
      double dxsq;
      double ds, di, inx;
      double dsq, dsat;

      int width = dimensions.Width;
      int height = dimensions.Height;

      PixelFetcher pf = null; 

      if (drawable != null)
        pf = new PixelFetcher(drawable, false);

      if (!_stars) 
	{
	  u = new double[width];
	  u1 = new double[width];
	  bxf = new uint[width];
	  bxc = new uint[width];

	  // Compute incident light direction vector.

	  double shang = _hourspec ? _hourangle : Cast(0, 2 * Math.PI);
	  double siang = _inclspec ? _inclangle 
	    : Cast(-Math.PI * 0.12, Math.PI * 0.12);

	  sunvec.X = Math.Sin(shang) * Math.Cos(siang);
	  sunvec.Y = Math.Sin(siang);
	  sunvec.Z = Math.Cos(shang) * Math.Cos(siang);

	  // Allow only 25% of random pictures to be crescents

	  if (!_hourspec && ((_random.Next() % 100) < 75)) 
	    {
	      sunvec.Z = Math.Abs(sunvec.Z);
	    }

	  // Prescale the grid points into intensities.

	  cp = new byte[n * n];

	  ap = cp;
	  for (int i = 0; i < n; i++) 
	    {
	      for (int j = 0; j < n; j++) 
		{
		  ap[ap_index++] = (byte)
		    (255.0 * (a[1 + (i * meshsize + j) * 2] + 1.0) / 2.0);
		}
	    }

	  /* Fill the screen from the computed  intensity  grid  by  mapping
	     screen  points onto the grid, then calculating each pixel value
	     by bilinear interpolation from  the surrounding  grid  points.
	     (N.b. the pictures would undoubtedly look better when generated
	     with small grids if a more well-behaved  interpolation  were
	     used.)

	     Before  we get started, precompute the line-level interpolation
	     parameters and store them in an array so we don't  have  to  do
	     this every time around the inner loop. */

	  //#define UPRJ(a,size) ((a)/((size)-1.0))

	  for (int j = 0; j < width; j++) 
	    {
	      //          double bx = (n - 1) * UPRJ(j, screenxsize);
	      double bx = (n - 1) * (j/(width-1.0));

	      bxf[j] = (uint) Math.Floor(bx);
	      bxc[j] = bxf[j] + 1;
	      u[j] = bx - bxf[j];
	      u1[j] = 1 - u[j];
	    }
	}

      StarFactory starFactory = new StarFactory(_random, _starfraction,
						_starcolour);
      Pixel[] pixels = new Pixel[width];

      for (int i = 0; i < height; i++) 
      {
        t = 0;
        t1 = 0;
        dysq = 0;
        double svx = 0;
        double svy = 0; 
        double svz = 0; 
        int byf = 0;
        int byc = 0;
        lcos = 0;

        if (!_stars) 
	  {	 // Skip all this setup if just stars
	    //#define UPRJ(a,size) ((a)/((size)-1.0))
	    //          by = (n - 1) * UPRJ(i, screenysize);
	    by = (n - 1) * ((double)i / ((double)height-1.0));
	    dy = 2 * (((height / 2) - i) / ((double) height));
	    dysq = dy * dy;
	    sqomdysq = Math.Sqrt(1.0 - dysq);
	    svx = sunvec.X;
	    svy = sunvec.Y * dy;
	    svz = sunvec.Z * sqomdysq;
	    byf = (int)(Math.Floor(by) * n);
	    byc = byf + (int)n;
	    t = by - Math.Floor(by);
	    t1 = 1 - t;
	  }

        if (_clouds) 
        {

          // Render the FFT output as clouds.

          for (int j = 0; j < width; j++) 
          {
            r = 0;
            if((byf + bxf[j]) < cp.Length)
              r += t1 * u1[j] * cp[byf + bxf[j]]; 
            if((byc + bxf[j]) < cp.Length)
              r += t * u1[j] * cp[byc + bxf[j]]; 
            if((byc + bxc[j]) < cp.Length)
              r += t * u[j] * cp[byc + bxc[j]]; 
            if((byf + bxc[j]) < cp.Length)
              r += t1 * u[j] * cp[byf + bxc[j]]; 
            byte w = (byte)((r > 127.0) ? (rgbQuant * ((r - 127.0) / 128.0)) : 0);

            pixels[j] = new Pixel(w, w, (byte)rgbQuant);
          }
        } 
        else if (_stars) 
        {

          /* Generate a starry sky.  Note  that no FFT is performed;
             the output is  generated  directly  from  a  power  law
             mapping	of  a  pseudorandom sequence into intensities. */

          for (int j = 0; j < pixels.Length; j++) 
          {
            pixels[j] = starFactory.Generate();
          }
        } 
        else 
        {
          for (int j = 0; j < width; j++) 
            pixels[j] = new Pixel(3);

          double azimuth = Math.Asin((((double) i / (height - 1)) * 2) - 1);
          icet = Math.Pow(Math.Abs(Math.Sin(azimuth)), 1.0 / _icelevel) 
            - 0.5;
          lcos = (int)((height / 2) * Math.Abs(Math.Cos(azimuth)));
          rpix_offset = (uint)(width/2 - lcos);

          for (int j = (int)((width / 2) - lcos); 
	       j <= (int)((width / 2) + lcos); 
	       j++) 
          {
            r = 0.0;
            byte ir = 0;
            byte ig = 0;
            byte ib = 0;

            r = 0;
            if((byf + bxf[j]) < cp.Length)
              r += t1 * u1[j] * cp[byf + bxf[j]]; 
            if((byc + bxf[j]) < cp.Length)
              r += t * u1[j] * cp[byc + bxf[j]]; 
            if((byc + bxc[j]) < cp.Length)
              r += t * u[j] * cp[byc + bxc[j]]; 
            if((byf + bxc[j]) < cp.Length)
              r += t1 * u[j] * cp[byf + bxc[j]]; 

            double ice;

            if (r >= 128) 
            {
              //#define ELEMENTS(array) (sizeof(array)/sizeof((array)[0]))
              //int ix = ((r - 128) * (ELEMENTS(pgnd) - 1)) / 127;
              int ix = (int)((r - 128) * (99 - 1)) / 127;

              /* Land area.  Look up colour based on elevation from
                 precomputed colour map table. */
              ir = pgnd[ix, 0];
              ig = pgnd[ix, 1];
              ib = pgnd[ix, 2];
            } 
            else 
            {

              /* Water.  Generate clouds above water based on
                 elevation.  */

              ir = (byte)((r > 64) ? (r - 64) * 4 : 0);
              ig = ir;
              ib = 255;
            }

            /* Generate polar ice caps. */

            ice = Math.Max(0.0, icet + _glaciers * Math.Max(-0.5, 
                  (r - 128) / 128.0));
            if  (ice > 0.125) 
            {
              ir = ig = ib = 255;
            }

            /* Apply limb darkening by cosine rule. */

            {   
              dx = 2 * (((width / 2) - j) / ((double) height));
              dxsq = dx * dx;
              di = svx * dx + svy + svz * Math.Sqrt(1.0 - dxsq);
              //#define 	    PlanetAmbient  0.05
              if (di < 0)
                di = 0;
              di = Math.Min(1.0, di);

              ds = Math.Sqrt(dxsq + dysq);
              ds = Math.Min(1.0, ds);
              /* Calculate  atmospheric absorption  based on the
                 thickness of atmosphere traversed by  light  on
                 its way to the surface. */

              //#define 	    AtSatFac 1.0
              dsq = ds * ds;
              dsat = atSatFac * ((Math.Sqrt(atthick * atthick - dsq) -
                    Math.Sqrt(1.0 * 1.0 - dsq)) / athfac);

              ir = (byte) (ir * (1.0 - dsat) + 127 * dsat);
              ig = (byte) (ig * (1.0 - dsat) + 127 * dsat);
              ib = (byte) (ib * (1.0 - dsat) + 255 * dsat);

              inx = planetAmbient + (1.0 - planetAmbient) * di;
              ir = (byte)(ir * inx);
              ig = (byte)(ig * inx);
              ib = (byte)(ib * inx);
            }

            pixels[rpix_offset++].Bytes = new byte[]{ir, ig, ib};
          }

          /* Left stars */
          //#define StarClose	2
          for (int j = 0; j < width / 2 - (lcos + starClose); j++) 
	    {
	      pixels[j] = starFactory.Generate();
	    }

          /* Right stars */
          for (int j = (int) (width / 2 + (lcos + starClose)); j < width; 
	       j++) 
	    {
	      pixels[j] = starFactory.Generate();
          }
        }
	
        if (drawable != null)
	  {
	    for (int x = 0; x < pixels.Length; x++) 
	      {
		pf.PutPixel(x, i, pixels[x]);
	      }
	    
	    _progress.Update((double)i/height);
	  }
        else
	  {
	    for (int x = 0; x < pixels.Length; x++) 
	      {
		pixels[x].CopyTo(pixelArray, 3 * (i * width + x));
	      }
	  }
      }

      if (drawable != null)
      {
        pf.Dispose();

        drawable.Flush();
        drawable.Update();
      }
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }
  }
}
