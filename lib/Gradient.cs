// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Gradient.cs
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
  public sealed class Gradient
  {
    string _name;

    public Gradient(string name)
    {
      _name = gimp_gradient_new(name);
    }

    internal Gradient(string name, bool unused)
    {
      _name = name;
    }

    public Gradient(Gradient gradient)
    {
      _name = gimp_gradient_duplicate(gradient._name);
    }

    public string Rename(string new_name)
    {
      _name = gimp_gradient_rename(_name, new_name);
      return _name;
    }

    public string Name
    {
      get {return _name;}
      set {Rename(value);}
    }

    public void Delete()
    {
      if (!gimp_gradient_delete(_name))
        {
	  throw new Exception();
        }
    }

    public bool Editable
    {
      get {return gimp_gradient_is_editable(_name);}
    }

    public void SegmentSetLeftColor(int segment, RGB color, double opacity)
    {
      GimpRGB rgb = color.GimpRGB;
      if (!gimp_gradient_segment_set_left_color(_name, segment, ref rgb,
						opacity))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentSetRightColor(int segment, RGB color, double opacity)
    {
      GimpRGB rgb = color.GimpRGB;
      if (!gimp_gradient_segment_set_right_color(_name, segment, ref rgb,
						 opacity))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentSetLeftPosition(int segment, double pos)
    {
      double final_pos;
      if (!gimp_gradient_segment_set_left_pos(_name, segment, pos,
					      out final_pos))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeSplitMidpoint(int start_segment, int end_segment)
    {
      if (!gimp_gradient_segment_range_split_midpoint(_name, start_segment, 
						      end_segment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeSplitUniform(int start_segment, int end_segment, 
				    int split_parts)
    {
      if (!gimp_gradient_segment_range_split_uniform(_name, start_segment, 
						     end_segment, split_parts))
	{
	  throw new GimpSharpException();
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_new(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_duplicate(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static string gimp_gradient_rename(string name, string new_name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_delete(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_is_editable(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool 
    gimp_gradient_segment_get_left_color (string name,
					  int segment,
					  out GimpRGB color,
					  out double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_left_color(string name,
							    int segment,
							    ref GimpRGB color,
							    double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_right_color(string name,
							    int segment,
							    ref GimpRGB color,
							    double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_left_pos(string name,
							  int segment,
							  double pos,
							  out double final_pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool 
    gimp_gradient_segment_range_split_midpoint(string name,
					       int start_segment,
					       int end_segment);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool 
    gimp_gradient_segment_range_split_uniform(string name,
					      int start_segment,
					      int end_segment,
					      int split_parts);
  }
}
