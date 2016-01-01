// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// Brush.cs
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
  public sealed class Brush : DataObject
  {
    public Brush(string name) : base(gimp_brush_new(name))
    {
    }

    internal Brush(string name, bool unused) : base(name)
    {
    }

    public Brush(Brush brush) : base(gimp_brush_duplicate(brush.Name))
    {
    }

    protected override string TryRename(string newName)
    {
      return gimp_brush_rename(Name, newName);
    }

    public void Delete()
    {
      if (!gimp_brush_delete(Name))
        {
	  throw new GimpSharpException();
        }
    }

    public BrushInfo Info
    {
      get
	{
	  int width, height, maskBpp, colorBpp;
	  if (!gimp_brush_get_info(Name, out width, out height, out maskBpp,
				   out colorBpp))
	    {
	      throw new GimpSharpException();
	    }
	  return new BrushInfo(width, height, maskBpp, colorBpp);
	}
    }

    public int Spacing
    {
      get
	{
          int spacing;
          if (!gimp_brush_get_spacing(Name, out spacing))
            {
	      throw new GimpSharpException();
            }
          return spacing;
	}
      set
	{
          if (!gimp_brush_set_spacing(Name, value))
            {
	      throw new GimpSharpException();
            }
	}
    }
	
    public BrushGeneratedShape Shape
    {
      get {return gimp_brush_get_shape(Name);}
      set {gimp_brush_set_shape(Name, value);}
    }

    public int Spikes
    {
      get {return gimp_brush_get_spikes(Name);}
      set {gimp_brush_set_spikes(Name, value);}
    }

    public double Angle
    {
      get {return gimp_brush_get_angle(Name);}
      set {gimp_brush_set_angle(Name, value);}
    }

    public double Radius
    {
      get {return gimp_brush_get_radius(Name);}
      set {gimp_brush_set_radius(Name, value);}
    }

    public double AspectRatio
    {
      get {return gimp_brush_get_aspect_ratio(Name);}
      set {gimp_brush_set_aspect_ratio(Name, value);}
    }

    public double Hardness
    {
      get {return gimp_brush_get_hardness(Name);}
      set {gimp_brush_set_hardness(Name, value);}
    }

    public bool Generated => gimp_brush_is_generated(Name);

    public bool Editable => gimp_brush_is_editable(Name);

    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_brush_new(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_brush_duplicate(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_brush_rename(string name, string new_name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_delete(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_get_info(string name, out int width,
                                           out int height, out int mask_bpp, 
					   out int color_bpp);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_get_spacing(string name, out int spacing);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_set_spacing(string name, int spacing);
    [DllImport("libgimp-2.0-0.dll")]
    extern static BrushGeneratedShape gimp_brush_get_shape(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_brush_set_shape(string name, 
					   BrushGeneratedShape shape_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_brush_get_spikes(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static int gimp_brush_set_spikes(string name, int spikes_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_get_angle(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_set_angle(string name, double angle_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_get_radius(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_set_radius(string name, double radius_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_get_aspect_ratio(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_set_aspect_ratio(string name, 
						     double aspect_ratio_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_get_hardness(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_brush_set_hardness(string name, 
						 double hardness_in);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_is_generated(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_brush_is_editable(string name);
  }
}
