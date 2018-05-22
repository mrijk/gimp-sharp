// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// Vectors.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class Vectors : Item
  {
    public Vectors(Image image, string name) : 
    	base(gimp_vectors_new(image.ID, name))
    {
    }
	
    public Vectors(Image image, TextLayer layer) :
	base(gimp_vectors_new_from_text_layer(image.ID, layer.ID))
    {
    }

    public Vectors(Vectors vectors) : base(gimp_vectors_copy(vectors.ID))
    {
    }

    internal Vectors(Int32 ID) : base(ID)
    {
    }

    public List<Stroke> Strokes
    {
      get
	{
	  var list = new List<Stroke>();
	  IntPtr ptr = gimp_vectors_get_strokes(ID, out int numStrokes);
	  if (numStrokes > 0)
	    {
	      var dest = new int[numStrokes];
	      Marshal.Copy(ptr, dest, 0, numStrokes);
	      Array.ForEach(dest, 
			    strokeID => list.Add(new Stroke(ID, strokeID)));
	    }
	  return list;
	}
    }

    public void RemoveStroke(Stroke stroke)
    {
      if (!gimp_vectors_remove_stroke(ID, stroke.ID))
	{
	  throw new GimpSharpException();
	}
    }

    // Fix me: deprecated
    public void ToSelection(ChannelOps operation, bool antialias, bool feather,
			    double featherRadiusX, double featherRadiusY)
    {
      if (!gimp_vectors_to_selection(ID, operation, antialias, feather,
				     featherRadiusX, featherRadiusY))
        {
	  throw new GimpSharpException();
        }
    }

    public void ToSelection(ChannelOps operation, bool antialias)
    {
      ToSelection(operation, antialias, false, 0, 0);
    }    

    public void ExportToFile(string filename)
    {
      if (!gimp_vectors_export_to_file(Image.ID, filename, ID))
        {
	  throw new GimpSharpException();
        }
    }

    public string ExportToString()
    {
      return gimp_vectors_export_to_string(Image.ID, ID);
    }

    public Stroke NewFromPoints(VectorsStrokeType type, 
				CoordinateList<double> controlpoints,
				bool closed)
    {
      var tmp = controlpoints.ToArray();
      int strokeID = gimp_vectors_stroke_new_from_points(ID, type, tmp.Length,
							 tmp, closed);
      return new Stroke(ID, strokeID);
    }

    public Stroke BezierStrokeNewEllipse(Coordinate<double> c, 
					 double radiusX, double radiusY,
					 double angle)
    {
      return 
	new Stroke(ID, gimp_vectors_bezier_stroke_new_ellipse(ID, c.X, c.Y,
							      radiusX,
							      radiusY,
							      angle));
    }

    public Stroke BezierStrokeNewMoveto(Coordinate<double> c) =>
      new Stroke(ID, gimp_vectors_bezier_stroke_new_moveto(ID, c.X, c.Y));

    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_new(Int32 image_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_new_from_text_layer(Int32 image_ID,
							 Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_copy(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static IntPtr gimp_vectors_get_strokes(Int32 vectors_ID,
						  out int num_strokes);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_remove_stroke(Int32 vectors_ID,
						  int stroke_id);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_to_selection(Int32 vectors_ID,
						 ChannelOps operation,
						 bool antialias,
						 bool feather,
						 double feather_radius_x,
						 double feather_radius_y);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_export_to_file(Int32 image_ID,
						   string filename,
						   Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_vectors_export_to_string(Int32 image_ID,
						       Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_vectors_stroke_new_from_points(Int32 drawable_ID,
						     VectorsStrokeType type,
						     int num_points,
						     double[] controlpoints,
						     bool closed);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_vectors_bezier_stroke_new_ellipse(Int32 vectors_ID,
							     double x0,
							     double y0,
							     double radius_x,
							     double radius_y,
							     double angle);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_vectors_bezier_stroke_new_moveto(Int32 vectors_ID,
							    double x0,
							    double y0);
  }
}
