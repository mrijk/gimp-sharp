// The Forge plug-in
// Copyright (C) 2006-2011 Maurits Rijk
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
    readonly Random _random;
    readonly bool _hourspec, _inclspec;

    readonly double _starfraction;
    readonly double _starcolour;
    readonly double _icelevel;
    readonly double _glaciers;
    readonly double _hourangle, _inclangle;

    const double atSatFac = 1.0;
    const double atthick = 1.03;   /* Atmosphere thickness as a 
				      percentage of planet's diameter */
    const double planetAmbient = 0.05;
    const int meshsize = 256;	      	// FFT mesh size

    readonly StarFactory _starFactory;

    public Planet(Drawable drawable,
		  bool stars, double starfraction, double starcolour,
		  bool clouds, Random random, 
		  double icelevel, double glaciers,
		  double fracdim, 
		  bool hourspec, double hourangle,
		  bool inclspec, double inclangle,
		  double powscale, AspectPreview preview = null)
    {
      _starfraction = starfraction;
      _starcolour = starcolour;
      _icelevel = icelevel;
      _glaciers = glaciers;
      _random = random;
      _hourspec = hourspec;
      _inclspec = inclspec;
      _hourangle = hourangle;
      _inclangle = inclangle;

      _starFactory = new StarFactory(_random, _starfraction, _starcolour);

      if (stars)
	{
	  if (preview != null)
	    preview.Update((c) => _starFactory.Generate());
	  else
	    GenerateNightlySky(drawable);
	}
      else
	{
	  var spectrum = new SpectralSynthesis(random);
	  var a = spectrum.Synthesize(meshsize, 3.0 - fracdim);

	  var mesh = new Mesh(a, meshsize);

	  // Apply power law scaling if non-unity scale is requested.
	  if (powscale != 1.0) 
	    {
	      mesh.ApplyPowerLawScaling(powscale);
	    }

	  mesh.AutoScale();

	  var cp = CalculateIntensities(mesh, meshsize);
 
	  var dimensions = drawable.Dimensions;
	  var info = new RenderInfo(dimensions.Width, dimensions.Height, meshsize, 
				    cp);
	  if (clouds)
	    {
	      if (preview != null)
		preview.Update((c) => DoRenderClouds(c, info));
	      else
		{
		  var iter = new RgnIterator(drawable, "Forge...");
		  iter.IterateDest((c) => DoRenderClouds(c, info));
		}
	    }
	  else
	    {
	      int width = dimensions.Width;
	      int height = dimensions.Height;
	      var sunvec = IncidentLightDirectionVector();

	      if (preview != null)
		{
		  preview.Update((c) => DoRenderPlanet(c, width, height, info, 
						       sunvec));
		}
	      else
		{
		  var iter = new RgnIterator(drawable, "Forge...");
		  iter.IterateDest((c) => DoRenderPlanet(c, width, height, info, 
							 sunvec));
		}
	    }
	}
    }

    void GenerateNightlySky(Drawable drawable)
    {
      var iter = new RgnIterator(drawable, "Forge...");
      iter.IterateDest((c) => _starFactory.Generate());
    }

    Vector3 IncidentLightDirectionVector()
    {
      double shang = _hourspec ? _hourangle : Cast(0, 2 * Math.PI);
      double siang = _inclspec ? _inclangle : Cast(-Math.PI * 0.12, Math.PI * 0.12);
 
      var sunvec = new Vector3(Math.Sin(shang) * Math.Cos(siang), Math.Sin(siang),
			       Math.Cos(shang) * Math.Cos(siang));
      
      // Allow only 25% of random pictures to be crescents
      
      if (!_hourspec && ((_random.Next() % 100) < 75)) 
	{
	  sunvec.Z = Math.Abs(sunvec.Z);
	}

      return sunvec;
    }

    byte[] CalculateIntensities(Mesh mesh, int n)
    {
      var cp = new byte[n * n];
      int index = 0;
      
      for (int i = 0; i < n; i++) 
	{
	  for (int j = 0; j < n; j++) 
	    {
	      cp[index++] = (byte) (255.0 * (mesh[i, j] + 1.0) / 2.0);
	    }
	}
      return cp;
    }

    Pixel DoRenderClouds(IntCoordinate c, RenderInfo info)
    {
      const double rgbQuant = 255;

      info.InitializeRow(c.Y);
      double r = info.CalculateR(c.X);

      byte w = (byte)((r > 127.0) ? (rgbQuant * ((r - 127.0) / 128.0)) : 0);

      return new Pixel(w, w, (byte) rgbQuant);
    }

    Pixel DoRenderPlanet(IntCoordinate c, int width, int height, RenderInfo info, 
			 Vector3 sunvec)
    {
      const double starClose = 2;

      info.InitializeRow(c.Y);

      var sv = new Vector3(sunvec.X, sunvec.Y * info.dy, sunvec.Z * info.sqomdysq);

      double azimuth = Math.Asin(((double) c.Y / (height - 1)) * 2 - 1);
      int lcos = (int) ((height / 2) * Math.Abs(Math.Cos(azimuth)));

      if (c.X >= width / 2 - lcos && c.X <= width / 2 + lcos)
	{
	  double r = info.CalculateR(c.X);
	  	  
	  var rgb = (r >= 128) ? RenderLand(r) : RenderWater(r);
	  
	  double icet = Math.Pow(Math.Abs(Math.Sin(azimuth)), 1.0 / _icelevel) 
	    - 0.5;
	  RenderPolarIceCaps(r, icet, rgb);
 
	  return ApplyDarkening(width, height, c.X, sv, info.dysq, rgb);
	}
      else if (c.X < width / 2 - (lcos + starClose) ||
	       c.X > width / 2 + (lcos + starClose))
	{
	  return _starFactory.Generate();
	}
      else
	{
	  return new Pixel(3);
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
      return new RGB(r, r, (byte) 255);
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
    Pixel ApplyDarkening(int width, int height, int x, Vector3 sv, double dysq,
			 RGB rgb)
    {
      double dx = 2 * ((width / 2 - x) / ((double) height));
      double dxsq = dx * dx;

      double di = sv.X * dx + sv.Y + sv.Z * Math.Sqrt(1.0 - dxsq);
      di = Math.Min(1.0, Math.Max(0.0, di));

      // Calculate  atmospheric absorption  based on the
      // thickness of atmosphere traversed by  light  on
      // its way to the surface.
 
      double ds = Math.Min(1.0, Math.Sqrt(dxsq + dysq));
      double dsq = ds * ds;
      double athfac = Math.Sqrt(atthick * atthick - 1.0);
      double dsat = atSatFac * ((Math.Sqrt(atthick * atthick - dsq) -
				 Math.Sqrt(1.0 * 1.0 - dsq)) / athfac);

      double inx = planetAmbient + (1.0 - planetAmbient) * di;
      
      byte ir, ig, ib;
      rgb.GetUchar(out ir, out ig, out ib);

      ir = (byte) ((ir * (1.0 - dsat) + 127 * dsat) * inx);
      ig = (byte) ((ig * (1.0 - dsat) + 127 * dsat) * inx);
      ib = (byte) ((ib * (1.0 - dsat) + 255 * dsat) * inx);
      
      return new Pixel(ir, ig, ib);
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }
  }

  class RenderInfo
  {
    int _height, _n;

    byte[] _cp;
    double[] _u;
    double[] _u1;
    uint[] _bxf;
    uint[] _bxc;

    int _byf, _byc;
    double _t, _t1;

    public double dy {get; private set;}
    public double dysq {get; private set;}
    public double sqomdysq {get; private set;}

    public RenderInfo(int width, int height, int n, byte[] cp)
    {
      _height = height;
      _n = n;
      _cp = cp;

      _u = new double[width];
      _u1 = new double[width];
      _bxf = new uint[width];
      _bxc = new uint[width];

      /* Fill the screen from the computed  intensity  grid  by  mapping
	 screen  points onto the grid, then calculating each pixel value
	 by bilinear interpolation from  the surrounding  grid  points.
	 (N.b. the pictures would undoubtedly look better when generated
	 with small grids if a more well-behaved  interpolation  were
	 used.) */

      for (int j = 0; j < width; j++) 
	{
	  double bx = (n - 1) * (j / (width - 1.0));
	  
	  _bxf[j] = (uint) Math.Floor(bx);
	  _bxc[j] = _bxf[j] + 1;
	  _u[j] = bx - _bxf[j];
	  _u1[j] = 1 - _u[j];
	}
    }

    public void InitializeRow(int y)
    {
      double by = (_n - 1) * ((double) y / ((double) _height - 1.0));

      dy = 2 * (((_height / 2) - y) / ((double) _height));
      dysq = dy * dy;
      sqomdysq = Math.Sqrt(1.0 - dysq);

      _byf = (int) (Math.Floor(by) * _n);
      _byc = _byf + _n;
      _t = by - Math.Floor(by);
      _t1 = 1 - _t;
    }

    public double CalculateR(int x)
    {
      double r = 0;

      if (_byf + _bxf[x] < _cp.Length)
	r += _t1 * _u1[x] * _cp[_byf + _bxf[x]]; 
      if (_byc + _bxf[x] < _cp.Length)
	r += _t * _u1[x] * _cp[_byc + _bxf[x]]; 
      if (_byc + _bxc[x] < _cp.Length)
	r += _t * _u[x] * _cp[_byc + _bxc[x]]; 
      if (_byf + _bxc[x] < _cp.Length)
	r += _t1 * _u[x] * _cp[_byf + _bxc[x]]; 

      return r;
    }
  }
}
