// The Raindrops plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Raindrop.cs
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

namespace Gimp.Raindrops
{
  public class Raindrop
  {
    readonly int _radius;
    readonly int _newSize;
    readonly double _newCoeff;
    readonly double _s;
    int X {get; set;}
    int Y {get; set;}

    public Raindrop(IntCoordinate coordinate, int newSize, double newCoeff)
    {
      _newSize = newSize;
      _radius = newSize / 2;		// Half of current raindrop
      _newCoeff = newCoeff;
      _s = _radius / Math.Log(newCoeff * _radius + 1);
      X = coordinate.X;
      Y = coordinate.Y;
    }

    public void Render(BoolMatrix boolMatrix, PixelFetcher pf, 
		       Drawable drawable)
    {
      var dimensions = drawable.Dimensions;
      RenderDrop(boolMatrix, pf, dimensions);
      // RenderShadow(pf, drawable, dimensions);
    }

    void RenderDrop(BoolMatrix boolMatrix, PixelFetcher pf, 
		    Dimensions dimensions)
    {
      int x0 = -_radius;
      int y0 = -_radius;
      int x1 = _newSize - _radius;
      int y1 = _newSize - _radius;
      var rectangle = new Rectangle(x0, y0, x1, y1);

      foreach (var c in new CoordinateGenerator(rectangle))
	{
	  double r = c.Radius;
	  double a = c.Angle;

	  if (r <= _radius)
	    {
	      double oldRadius = r;
	      r = (Math.Exp (r / _s) - 1) / _newCoeff;

	      int k = X + (int) (r * Math.Sin(a));
	      int l = Y + (int) (r * Math.Cos(a));

	      int m = X + c.X;
	      int n = Y + c.Y;

	      if (dimensions.IsInside(k, l) && dimensions.IsInside(m, n))
		{
#if false		  
		  boolMatrix[n, m] = true;

		  var newColor = pf[l, k] + GetBright(oldRadius, a);
		  newColor.Clamp0255();
		  pf[l, k] = newColor;
#endif
		}
	    }
	}
    }

    void RenderShadow(PixelFetcher pf, Drawable drawable, Dimensions dimensions)
    {      
      int blurRadius = _newSize / 25 + 1;

      int x0 = -_radius - blurRadius;
      int y0 = x0;
      int x1 = _newSize - _radius + blurRadius;
      int y1 = x1;
      var rectangle = new Rectangle(x0, y0, x1, y1);

      foreach (var c in new CoordinateGenerator(rectangle))
	{
	  if (c.Radius <= _radius * 1.1)
	    {
	      var average = drawable.CreatePixel();
	      int blurPixels = 0;
	      int m, n;

	      for (int k = -blurRadius; k < blurRadius + 1; k++)
		{
		  m = X + c.X + k;
		  for (int l = -blurRadius; l < blurRadius + 1; l++)
		    {
		      n = Y + c.Y + l;
		      
		      if (dimensions.IsInside(m, n))
			{
			  average += pf[n, m];
			  blurPixels++;
			}
		    }
		}

	      m = X + c.X;
	      n = Y + c.Y;
	      
	      if (dimensions.IsInside(m, n))
		{
		  pf[n, m] = average / blurPixels;
		}
	    }
	}
    }

    int GetBright(double OldRadius, double a)
    {
      int bright = 0;

      if (OldRadius >= 0.9 * _radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    bright = -80;
	  else if ((a <= -2.25) && (a > -2.5))
	    bright = -40;
	  else if ((a <= 0.25) && (a > 0))
	    bright = -40;
	}
      else if (OldRadius >= 0.8 * _radius)
	{
	  if ((a <= -0.75) && (a > -1.50))
	    bright = -40;
	  else if ((a <= 0.10) && (a > -0.75))
	    bright = -30;
	  else if ((a <= -1.50) && (a > -2.35))
	    bright = -30;
	}
      else if (OldRadius >= 0.7 * _radius)
	{
	  if ((a <= -0.10) && (a > -2.0))
	    bright = -20;
	  else if ((a <= 2.50) && (a > 1.90))
	    bright = 60;
	}
      else if (OldRadius >= 0.6 * _radius)
	{
	  if ((a <= -0.50) && (a > -1.75))
	    bright = -20;
	  else if ((a <= 0) && (a > -0.25))
	    bright = 20;
	  else if ((a <= -2.0) && (a > -2.25))
	    bright = 20;
	}
      else if (OldRadius >= 0.5 * _radius)
	{
	  if ((a <= -0.25) && (a > -0.50))
	    bright = 30;
	  else if ((a <= -1.75 ) && (a > -2.0))
	    bright = 30;
	}
      else if (OldRadius >= 0.4 * _radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    bright = 40;
	}
      else if (OldRadius >= 0.3 * _radius)
	{
	  if ((a <= 0) && (a > -2.25))
	    bright = 30;
	}
      else if (OldRadius >= 0.2 * _radius)
	{
	  if ((a <= -0.5) && (a > -1.75))
	    bright = 20;
	}
      return bright;
    }
  }
}
