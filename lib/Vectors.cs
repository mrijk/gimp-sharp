// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2008 Maurits Rijk
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
  public class Vectors
  {
    readonly protected Int32 _ID;

    public Vectors(Image image, string name)
    {
      _ID = gimp_vectors_new(image.ID, name);
    }

    public Vectors(Vectors vectors)
    {
      _ID = gimp_vectors_copy(vectors.ID);
    }

    internal Vectors(Int32 ID)
    {
      _ID = ID;
    }

    internal Int32 ID
    {
      get {return _ID;}
    }

    public bool IsValid
    {
      get {return gimp_vectors_is_valid(_ID);}
    }

    public List<Stroke> Strokes
    {
      get
	{
	  List<Stroke> list = new List<Stroke>();
	  int numStrokes;
	  IntPtr ptr = gimp_vectors_get_strokes(_ID, out numStrokes);
	  if (numStrokes > 0)
	    {
	      int[] dest = new int[numStrokes];
	      Marshal.Copy(ptr, dest, 0, numStrokes);
	      
	      foreach (int strokeID in dest)
		{
		  list.Add(new Stroke(_ID, strokeID));
		}
	    }
	  return list;
	}
    }

    public Image Image
    {
      get {return new Image(gimp_vectors_get_image(_ID));}
    }

    public int Position
    {
      get {return Image.GetVectorsPosition(this);}
    }

    public bool Linked
    {
      get {return gimp_vectors_get_linked(_ID);}
      set 
	{
	  if (!gimp_vectors_set_linked(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public string Name
    {
      get {return gimp_vectors_get_name(_ID);}
      set
	{
	  if (!gimp_vectors_set_name(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public Tattoo Tattoo
    {
      get {return new Tattoo(gimp_vectors_get_tattoo(_ID));}
      set
	{
	  if (!gimp_vectors_set_tattoo(_ID, value.ID))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool Visible
    {
      get {return gimp_vectors_get_visible(_ID);}
      set
	{
	  if (!gimp_vectors_set_visible(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void RemoveStroke(Stroke stroke)
    {
      if (!gimp_vectors_remove_stroke(_ID, stroke.ID))
	{
	  throw new GimpSharpException();
	}
    }

    public void ToSelection(ChannelOps operation, bool antialias, bool feather,
			    double featherRadiusX, double featherRadiusY)
    {
      if (!gimp_vectors_to_selection(_ID, operation, antialias, feather,
				     featherRadiusX, featherRadiusY))
        {
	  throw new GimpSharpException();
        }
    }

    public void ParasiteAttach(Parasite parasite)
    {
      if (!gimp_vectors_parasite_attach(_ID, parasite.Ptr))
        {
	  throw new GimpSharpException();
        }
    }

    public void ParasiteDetach(Parasite parasite)
    {
      if (!gimp_vectors_parasite_detach(_ID, parasite.Name))
        {
	  throw new GimpSharpException();
        }
    }

    public Parasite ParasiteFind(string name)
    {
      IntPtr found = gimp_vectors_parasite_find(_ID, name);
      return (found == IntPtr.Zero) ? null : new Parasite(found);
    }

    public ParasiteList ParasiteList
    {
      get
	{
	  ParasiteList list = new ParasiteList();
	  int numParasites;
	  IntPtr parasites;
	  if (gimp_vectors_parasite_list(_ID, out numParasites,
					  out parasites))
	    {
	      for (int i = 0; i < numParasites; i++)
		{
		  IntPtr tmp = (IntPtr) Marshal.PtrToStructure(parasites, 
							       typeof(IntPtr));
		  string name = Marshal.PtrToStringAnsi(tmp);
		  Parasite parasite = ParasiteFind(name);
		  list.Add(parasite);
		  parasites = (IntPtr)((int) parasites + Marshal.SizeOf(tmp));
		}
	    }
	  else
	    {
	      throw new GimpSharpException();
	    }
	  return list;
	}
    }

    public Stroke NewFromPoints(VectorsStrokeType type, 
				CoordinateList<double> controlpoints,
				bool closed)
    {
      double[] tmp = controlpoints.ToArray();
      int strokeID = gimp_vectors_stroke_new_from_points(_ID, type, tmp.Length,
							 tmp, closed);
      return new Stroke(_ID, strokeID);
    }

    public Stroke BezierStrokeNewEllipse(Coordinate<double> c, 
					 double radiusX, double radiusY,
					 double angle)
    {
      return 
	new Stroke(_ID, gimp_vectors_bezier_stroke_new_ellipse(_ID, c.X, c.Y,
							       radiusX,
							       radiusY,
							       angle));
    }

    public Stroke BezierStrokeNewMoveto(Coordinate<double> c)
    {
      return new Stroke(_ID, 
			gimp_vectors_bezier_stroke_new_moveto(_ID, c.X, c.Y));
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_new(Int32 image_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_copy(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_is_valid(Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static IntPtr gimp_vectors_get_strokes(Int32 vectors_ID,
						  out int num_strokes);
    [DllImport("libgimp-2.0-0.dll")]
    extern static Int32 gimp_vectors_get_image(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_get_linked(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_vectors_get_name(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_vectors_get_tattoo(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_get_visible(Int32 vectors_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_linked(Int32 vectors_ID, bool linked);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_name(Int32 vectors_ID, string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_tattoo(Int32 vectors_ID, int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_set_visible(Int32 vectors_ID, 
						bool visible);
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
    extern static bool gimp_vectors_parasite_attach(Int32 vectors_ID,
						    IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_vectors_parasite_detach(Int32 vectors_ID,
						    string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_vectors_parasite_find(Int32 drawable_ID,
						    string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_vectors_parasite_list(Int32 drawable_ID,
						  out int num_parasites,
						  out IntPtr parasites);
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
