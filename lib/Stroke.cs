// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Stroke.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class Stroke
  {
    readonly Int32 _vectorsID;
    readonly int _strokeID;

    internal Stroke(Int32 vectorsID, int strokeID)
    {
      _vectorsID = vectorsID;
      _strokeID = strokeID;
    }

    internal int ID
    {
      get {return _strokeID;}
    }

    public void Close()
    {
      if (!gimp_vectors_stroke_close(_vectorsID, _strokeID))
	{
	  throw new GimpSharpException();
	}
    }

    public double GetLength(double precision)
    {
      return gimp_vectors_stroke_get_length(_vectorsID, _strokeID, precision);
    }

    public double GetPointAtDist(double dist, double precision,
				 out double yPoint, out double slope, 
				 out bool valid)
    {
      return gimp_vectors_stroke_get_point_at_dist(_vectorsID, _strokeID,
						   dist, precision, out yPoint,
						   out slope, out valid);
    }

    public void Scale(int scaleX, int scaleY)
    {
      if (!gimp_vectors_stroke_scale(_vectorsID, _strokeID, scaleX, scaleY))
	{
	  throw new GimpSharpException();
	}
    }

    public void Translate(int offX, int offY)
    {
      if (!gimp_vectors_stroke_translate(_vectorsID, _strokeID, offX, offY))
	{
	  throw new GimpSharpException();
	}
    }

    public void Conicto(Coordinate<double> c0, Coordinate<double> c1)
    {
      if (!gimp_vectors_stroke_conicto(_vectorsID, _strokeID, 
				       c0.X, c0.Y, c1.X, c1.Y))
	{
	  throw new GimpSharpException();
	}
    }

    public void Cubicto(Coordinate<double> c0, Coordinate<double> c1,
			Coordinate<double> c2)
    {
      if (!gimp_vectors_stroke_cubicto(_vectorsID, _strokeID, 
				       c0.X, c0.Y, c1.X, c1.Y, c2.X, c2.Y))
	{
	  throw new GimpSharpException();
	}
    }

    public void Lineto(Coordinate<double> c)
    {
      if (!gimp_vectors_stroke_lineto(_vectorsID, _strokeID, c.X, c.Y))
	{
	  throw new GimpSharpException();
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_close(Int32 vectors_ID,
						 int stroke_id);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_vectors_stroke_get_length(Int32 vectors_ID,
							int stroke_id,
							double precision);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_vectors_stroke_get_point_at_dist
    (Int32 vectors_ID, int stroke_id, double dist, double precision,
     out double y_point, out double slope, out bool valid);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_scale(Int32 vectors_ID,
						 int stroke_id,
						 int scale_x, int scale_y);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_translate(Int32 vectors_ID,
						     int stroke_id,
						     int off_x, int off_y);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_conicto(Int32 vectors_ID,
						   int stroke_id,
						   double x0, double y0,
						   double x1, double y1);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_cubicto(Int32 vectors_ID,
						   int stroke_id,
						   double x0, double y0,
						   double x1, double y1,
						   double x2, double y2);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_stroke_lineto(Int32 vectors_ID,
						  int stroke_id,
						  double x0, double y0);
  }
}
