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

    const double atSatFac = 1.0;
    const double atthick = 1.03;   /* Atmosphere thickness as a 
				      percentage of planet's diameter */
    const double planetAmbient = 0.05;
    const int meshsize = 256;	      	// FFT mesh size

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

      Mesh mesh = null;

      if (!stars)
	{
	  var spectrum = new SpectralSynthesis(random);
	  var a = spectrum.Synthesize(meshsize, 3.0 - fracdim);

	  mesh = new Mesh(a, meshsize);

	  // Apply power law scaling if non-unity scale is requested.
	  if (powscale != 1.0) 
	    {
	      mesh.ApplyPowerLawScaling(powscale);
	    }

	  mesh.AutoScale();
	}

      GenPlanet(drawable, pixelArray, dimensions, mesh, meshsize);
    }

    void GenPlanet(Drawable drawable, byte[] pixelArray, 
		   Dimensions dimensions, Mesh mesh, uint n)
    {
      byte[] cp = null;
      byte[] ap = null;
      double[] u = null;
      double[] u1 = null;
      uint[] bxf = null;
      uint[] bxc = null;
      uint ap_index = 0;

      Vector3 sunvec = new Vector3();

      double t = 0;
      double t1 = 0;
      double by, dy;
      double dysq = 0;
      double sqomdysq;

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
		    (255.0 * (mesh.GetReal(i, j) + 1.0) / 2.0);
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
	  RenderClouds(width, t, t1, byf, byc, bxf, bxc, cp, 
		       u1, u, pixels);
        } 
        else if (_stars) 
        {
	  RenderStars(starFactory, pixels);
        } 
        else 
        {
	  RenderPlanet(width, t, t1, byf, byc, bxf, bxc, cp, 
		       u1, u, pixels, 
		       starFactory,
		       svx, svy, svz,
		       height, i, dysq);
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

    void RenderClouds(int width, double t, double t1, int byf, int byc, 
		      uint[] bxf, uint[] bxc, byte[] cp, double[] u1,
		      double[] u, Pixel[] pixels)
    {
      const double rgbQuant = 255; 

      for (int j = 0; j < width; j++) 
	{
	  double r = 0;

	  if ((byf + bxf[j]) < cp.Length)
	    r += t1 * u1[j] * cp[byf + bxf[j]]; 
	  if ((byc + bxf[j]) < cp.Length)
	    r += t * u1[j] * cp[byc + bxf[j]]; 
	  if ((byc + bxc[j]) < cp.Length)
	    r += t * u[j] * cp[byc + bxc[j]]; 
	  if ((byf + bxc[j]) < cp.Length)
	    r += t1 * u[j] * cp[byf + bxc[j]]; 

	  byte w = (byte)((r > 127.0) ? (rgbQuant * ((r - 127.0) / 128.0)) : 0);

	  pixels[j] = new Pixel(w, w, (byte) rgbQuant);
	}
    }

    /* Generate a starry sky.  Note  that no FFT is performed;
       the output is  generated  directly  from  a  power  law
       mapping  of  a  pseudorandom sequence into intensities. */
    
    void RenderStars(StarFactory starFactory, Pixel[] pixels)
    {
      for (int j = 0; j < pixels.Length; j++) 
	{
	  pixels[j] = starFactory.Generate();
	}
    }

    void RenderPlanet(int width, double t, double t1, int byf, int byc, 
		      uint[] bxf, uint[] bxc, byte[] cp, double[] u1,
		      double[] u, Pixel[] pixels, 
		      StarFactory starFactory, 
		      double svx, double svy, double svz,
		      int height, int i, double dysq)
    {
      double athfac = Math.Sqrt(atthick * atthick - 1.0);

      for (int j = 0; j < width; j++) 
	pixels[j] = new Pixel(3);
      
      double azimuth = Math.Asin((((double) i / (height - 1)) * 2) - 1);
      double icet = Math.Pow(Math.Abs(Math.Sin(azimuth)), 1.0 / _icelevel) 
	- 0.5;
      int lcos = (int)((height / 2) * Math.Abs(Math.Cos(azimuth)));
      uint rpix_offset = (uint)(width/2 - lcos);
      
      for (int j = (int)((width / 2) - lcos); 
	   j <= (int)((width / 2) + lcos); 
	   j++) 
	{
	  double r = 0;
	  if ((byf + bxf[j]) < cp.Length)
	    r += t1 * u1[j] * cp[byf + bxf[j]]; 
	  if ((byc + bxf[j]) < cp.Length)
	    r += t * u1[j] * cp[byc + bxf[j]]; 
	  if ((byc + bxc[j]) < cp.Length)
	    r += t * u[j] * cp[byc + bxc[j]]; 
	  if ((byf + bxc[j]) < cp.Length)
	    r += t1 * u[j] * cp[byf + bxc[j]]; 
	  	  
	  RGB rgb = (r >= 128) ? RenderLand(r) : RenderWater(r);
	  
	  RenderPolarIceCaps(r, icet, rgb);
 
	  byte[] bytes = ApplyDarkening(width, height, j, svx, svy, svz,
					dysq, athfac, rgb);
	  pixels[rpix_offset++].Bytes = bytes;
	}
      
      AddStars(width, pixels, starFactory, lcos);
    }

    void AddStars(int width, Pixel[] pixels, StarFactory starFactory, int lcos)
    {
      const double starClose = 2;

      /* Left stars */
      for (int j = 0; j < width / 2 - (lcos + starClose); j++) 
	{
	  pixels[j] = starFactory.Generate();
	}

      /* Right stars */
      for (int j = (int) (width / 2 + (lcos + starClose)); j < width; j++) 
	{
	  pixels[j] = starFactory.Generate();
	}
    }

    RGB RenderLand(double val)
    {
      return Land.GetLand(val);
    }

    // Water. Generate clouds above water based on elevation.
	      
    RGB RenderWater(double val)
    {
      byte r = (byte)((val > 64) ? (val - 64) * 4 : 0);
      return new RGB(r, r, 255);
    }

    void RenderPolarIceCaps(double val, double icet, RGB rgb)
    {
      double ice = Math.Max(0.0, icet + 
			    _glaciers * Math.Max(-0.5, (val - 128) / 128.0));
      if  (ice > 0.125) 
	{
	  rgb.R = rgb.G = rgb.B = 255;
	}
    }

    /* Apply limb darkening by cosine rule. */
    byte[] ApplyDarkening(int width, int height, int j,
			  double svx, double svy, double svz, double dysq,
			  double athfac, RGB rgb)
    {
      double dx = 2 * (((width / 2) - j) / ((double) height));
      double dxsq = dx * dx;
      double di = svx * dx + svy + svz * Math.Sqrt(1.0 - dxsq);

      byte ir, ig, ib;

      rgb.GetUchar(out ir, out ig, out ib);

      if (di < 0)
	di = 0;
      di = Math.Min(1.0, di);
      
      double ds = Math.Sqrt(dxsq + dysq);
      ds = Math.Min(1.0, ds);
      /* Calculate  atmospheric absorption  based on the
	 thickness of atmosphere traversed by  light  on
	 its way to the surface. */
      
      double dsq = ds * ds;
      double dsat = atSatFac * ((Math.Sqrt(atthick * atthick - dsq) -
				 Math.Sqrt(1.0 * 1.0 - dsq)) / athfac);
      
      ir = (byte) (ir * (1.0 - dsat) + 127 * dsat);
      ig = (byte) (ig * (1.0 - dsat) + 127 * dsat);
      ib = (byte) (ib * (1.0 - dsat) + 255 * dsat);
      
      double inx = planetAmbient + (1.0 - planetAmbient) * di;
      ir = (byte)(ir * inx);
      ig = (byte)(ig * inx);
      ib = (byte)(ib * inx);

      return new byte[]{ir, ig, ib};
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }
  }
}
