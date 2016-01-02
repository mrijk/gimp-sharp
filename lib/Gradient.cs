// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class Gradient : DataObject
  {
    public bool Editable => gimp_gradient_is_editable(Name);
    public int NumberOfSegments => gimp_gradient_get_number_of_segments(Name);

    public Gradient(string name) : base(gimp_gradient_new(name))
    {
    }

    internal Gradient(string name, bool unused) : base(name)
    {
    }

    public Gradient(Gradient gradient) : 
      base(gimp_gradient_duplicate(gradient.Name))
    {
    }

    public IEnumerator<GradientSegment> GetEnumerator()
    {
      for (int i = 0; i < NumberOfSegments; i++)
	{
	  yield return new GradientSegment(this, i);
	}
    }

    public void ForEach(Action<GradientSegment> action)
    {
      foreach (var segment in this)
	{
	  action(segment);
	}
    }

    public override bool Equals(object o)
    {
      if (o is Gradient)
	{
	  return (o as Gradient).Name == Name;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }

    protected override string TryRename(string newName)
    {
      return gimp_gradient_rename(Name, newName);
    }

    public void Delete()
    {
      if (!gimp_gradient_delete(Name))
        {
	  throw new GimpSharpException();
        }
    }

    public RGB SegmentGetLeftColor(int segment, out double opacity)
    {
      var rgb = new GimpRGB();
      if (!gimp_gradient_segment_get_left_color(Name, segment, out rgb,
						out opacity))
	{
	  throw new GimpSharpException();
	}
      return new RGB(rgb);
    }

    public void SegmentSetLeftColor(int segment, RGB color, double opacity)
    {
      var rgb = color.GimpRGB;
      if (!gimp_gradient_segment_set_left_color(Name, segment, ref rgb,
						opacity))
	{
	  throw new GimpSharpException();
	}
    }

    public RGB SegmentGetRightColor(int segment, out double opacity)
    {
      var rgb = new GimpRGB();
      if (!gimp_gradient_segment_get_right_color(Name, segment, out rgb,
						 out opacity))
	{
	  throw new GimpSharpException();
	}
      return new RGB(rgb);
    }

    public void SegmentSetRightColor(int segment, RGB color, double opacity)
    {
      var rgb = color.GimpRGB;
      if (!gimp_gradient_segment_set_right_color(Name, segment, ref rgb,
						 opacity))
	{
	  throw new GimpSharpException();
	}
    }

    public double SegmentGetLeftPosition(int segment)
    {
      double position;
      if (!gimp_gradient_segment_get_left_pos(Name, segment, out position))
	{
	  throw new GimpSharpException();
	}
      return position;
    }

    public double SegmentSetLeftPosition(int segment, double pos)
    {
      double finalPos;
      if (!gimp_gradient_segment_set_left_pos(Name, segment, pos,
					      out finalPos))
	{
	  throw new GimpSharpException();
	}
      return finalPos;
    }

    public double SegmentGetMiddlePosition(int segment)
    {
      double position;
      if (!gimp_gradient_segment_get_middle_pos(Name, segment, out position))
	{
	  throw new GimpSharpException();
	}
      return position;
    }

    public double SegmentSetMiddlePosition(int segment, double pos)
    {
      double finalPos;
      if (!gimp_gradient_segment_set_middle_pos(Name, segment, pos,
						out finalPos))
	{
	  throw new GimpSharpException();
	}
      return finalPos;
    }

    public double SegmentGetRightPosition(int segment)
    {
      double position;
      if (!gimp_gradient_segment_get_right_pos(Name, segment, out position))
	{
	  throw new GimpSharpException();
	}
      return position;
    }

    public double SegmentSetRightPosition(int segment, double pos)
    {
      double finalPos;
      if (!gimp_gradient_segment_set_right_pos(Name, segment, pos,
					       out finalPos))
	{
	  throw new GimpSharpException();
	}
      return finalPos;
    }

    public GradientSegmentType SegmentGetBlendingFunction(int segment)
    {
      GradientSegmentType blendFunc;
      if (!gimp_gradient_segment_get_blending_function(Name,
						       segment,
						       out blendFunc))
      {
	throw new GimpSharpException();
      }
      return blendFunc;
    }

    public GradientSegmentColor SegmentGetBlendingColoringType(int segment)
    {
      GradientSegmentColor coloringType;
      if (!gimp_gradient_segment_get_coloring_type(Name,
						   segment,
						   out coloringType))
	{
	  throw new GimpSharpException();
	}
      return coloringType;
    }

    public void 
    SegmentRangeSetBlendingFunction(int startSegment, 
				    int endSegment,
				    GradientSegmentType blendingFunction)
    {
      if (!gimp_gradient_segment_range_set_blending_function(Name,
							     startSegment,
							     endSegment,
							     blendingFunction))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeSetColoringType(int startSegment, 
					    int endSegment,
					    GradientSegmentColor coloringType)
    {
      if (!gimp_gradient_segment_range_set_coloring_type(Name, startSegment,
							 endSegment,
							 coloringType))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeFlip(int startSegment, int endSegment)
    {
      if (!gimp_gradient_segment_range_flip(Name, startSegment, endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeReplicate(int startSegment, int endSegment,
				      int replicateTimes)
    {
      if (!gimp_gradient_segment_range_replicate(Name, startSegment, 
						 endSegment, replicateTimes))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeSplitMidpoint(int startSegment, int endSegment)
    {
      if (!gimp_gradient_segment_range_split_midpoint(Name, startSegment, 
						      endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeSplitUniform(int startSegment, int endSegment, 
				    int splitParts)
    {
      if (!gimp_gradient_segment_range_split_uniform(Name, startSegment, 
						     endSegment, splitParts))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeDelete(int startSegment, int endSegment)
    {
      if (!gimp_gradient_segment_range_delete(Name, startSegment, endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeRedistributeHandles(int startSegment, 
						int endSegment)
    {
      if (!gimp_gradient_segment_range_redistribute_handles(Name, startSegment,
							    endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeBlendColors(int startSegment, int endSegment)
    {
      if (!gimp_gradient_segment_range_blend_colors(Name, startSegment,
						    endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public void SegmentRangeBlendOpacity(int startSegment, int endSegment)
    {
      if (!gimp_gradient_segment_range_blend_opacity(Name, startSegment,
						     endSegment))
	{
	  throw new GimpSharpException();
	}
    }

    public double SegmentRangeMove(int startSegment, int endSegment,
				   double delta, bool controlCompress)
    {
      return gimp_gradient_segment_range_move(Name, startSegment, endSegment, 
					      delta, controlCompress);
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
    extern static int gimp_gradient_get_number_of_segments(string name);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_left_color(string name,
							    int segment,
							    ref GimpRGB color,
							    double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_left_color(string name,
							    int segment,
							    out GimpRGB color,
							    out double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_right_color(string name,
							    int segment,
							    ref GimpRGB color,
							    double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_right_color(string name,
							    int segment,
							    out GimpRGB color,
							    out double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_left_pos(string name,
							  int segment,
							  out double pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_left_pos(string name,
							  int segment,
							  double pos,
							  out double final_pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_middle_pos(string name,
							    int segment,
							    out double pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_middle_pos(string name,
							  int segment,
							  double pos,
							  out double final_pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_right_pos(string name,
							  int segment,
							  out double pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_set_right_pos(string name,
							  int segment,
							  double pos,
							  out double final_pos);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_blending_function(string name,
								   int segment,
								   out GradientSegmentType blend_func);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_get_coloring_type(string name,
							       int segment,
							       out GradientSegmentColor coloring_type);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_set_blending_function
                                                        (string name,
                                                         int start_segment,
                                                         int end_segment,
                                                         GradientSegmentType blending_function);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_set_coloring_type
                                                        (string name,
                                                         int start_segment,
                                                         int end_segment,
                                                         GradientSegmentColor coloring_type);
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_flip(string name,
							int start_segment,
							int end_segment);
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_replicate(string name,
							     int start_segment,
							     int end_segment,
							 int replicate_times);
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
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_delete(string name,
							  int start_segment,
							  int end_segment);
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_redistribute_handles
                                                        (string name,
                                                         int start_segment,
                                                         int end_segment);
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_blend_colors
                                                        (string name,
                                                         int start_segment,
                                                         int end_segment);
   [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_gradient_segment_range_blend_opacity
                                                        (string name,
                                                         int start_segment,
                                                         int end_segment);
    [DllImport("libgimp-2.0-0.dll")]
    extern static double gimp_gradient_segment_range_move(string name,
							  int start_segment,
							  int end_segment,
							  double delta,
							  bool control_compress);
  }
}
