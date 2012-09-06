 // GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// Context.cs
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

using GLib;

namespace Gimp
{
  public static class Context
  {
    static public void Push()
    {
      if (!gimp_context_push())
	{
	  throw new GimpSharpException();
	}
    }

    static public void Pop()
    {
      if (!gimp_context_pop())
	{
	  throw new GimpSharpException();
	}
    }

    static public void SetDefaults()
    {
      if (!gimp_context_set_defaults())
	{
	  throw new GimpSharpException();
	}
    }

    static public RGB Foreground
    {
      get
	{
	  var rgb = new GimpRGB();
	  if (!gimp_context_get_foreground(out rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  var rgb = value.GimpRGB;
	  if (!gimp_context_set_foreground(ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    static public RGB Background
    {
      get
	{
	  var rgb = new GimpRGB();
	  if (!gimp_context_get_background(out rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  var rgb = value.GimpRGB;
	  if (!gimp_context_set_background(ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    static public void SetDefaultColors()
    {
      if (!gimp_context_set_default_colors())
	{
	  throw new GimpSharpException();
	}
    }

    static public void SwapColors()
    {
      if (!gimp_context_swap_colors())
	{
	  throw new GimpSharpException();
	}
    }

    public static double Opacity
    {
      get {return gimp_context_get_opacity();}
      set
	{
	  if (!gimp_context_set_opacity(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static LayerModeEffects PaintMode
    {
      get {return gimp_context_get_paint_mode();}
      set
	{
	  if (!gimp_context_set_paint_mode(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static Brush Brush
    {
      get {return new Brush(gimp_context_get_brush(), false);}
      set
	{
	  if (!gimp_context_set_brush(value.Name))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static double BrushSize
    {
      get {return gimp_context_get_brush_size();}
      set
	{
	  if (!gimp_context_set_brush_size(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static void SetBrushDefaultSize()
    {
      if (!gimp_context_set_brush_default_size())
	{
	  throw new GimpSharpException();
	}
    }

    public static double BrushAspectRatio
    {
      get {return gimp_context_get_brush_aspect_ratio();}
      set
	{
	  if (!gimp_context_set_brush_aspect_ratio(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static double BrushAngle
    {
      get {return gimp_context_get_brush_angle();}
      set
	{
	  if (!gimp_context_set_brush_angle(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static string Dynamics
    {
      get {return gimp_context_get_dynamics();}
      set
	{
	  if (!gimp_context_set_dynamics(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static Pattern Pattern
    {
      get {return new Pattern(gimp_context_get_pattern(), false);}
      set
	{
	  if (!gimp_context_set_pattern(value.Name))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static Gradient Gradient
    {
      get {return new Gradient(gimp_context_get_gradient(), false);}
      set
	{
	  if (!gimp_context_set_gradient(value.Name))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static Palette Palette
    {
      get {return new Palette(gimp_context_get_palette(), false);}
      set
	{
	  if (!gimp_context_set_palette(value.Name))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static string Font
    {
      get {return gimp_context_get_font();}
      set
	{
	  if (!gimp_context_set_font(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static bool Antialias
    {
      get {return gimp_context_get_antialias();}
      set
	{
	  if (!gimp_context_set_antialias(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static bool Feather
    {
      get {return gimp_context_get_feather();}
      set
	{
	  if (!gimp_context_set_feather(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static Coordinate<double> FeatherRadius
    {
      get 
	{
	  double x, y;
	  if (!gimp_context_get_feather_radius(out x, out y))
	    {
	      throw new GimpSharpException();
	    }
	  return new Coordinate<double>(x, y);
	}
      set
	{
	  if (!gimp_context_set_feather_radius(value.X, value.Y))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static string PaintMethod
    {
      get {return gimp_context_get_paint_method();}
      set
	{
	  if (!gimp_context_set_paint_method(value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public static List<string> PaintMethods
    {
      get 
	{
	  int numPaintMethods;
	  IntPtr paintMethods;
	  if (!gimp_context_list_paint_methods(out numPaintMethods, 
					       out paintMethods)) 
	    {
	      throw new GimpSharpException();
	    }

	  var methods = Util.ToStringList(paintMethods, numPaintMethods);
	  // TODO: find out if next line is needed
	  Marshaller.Free(paintMethods);
	  return methods;
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_push();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_pop();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_defaults();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_get_foreground(out GimpRGB foreground);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_foreground(ref GimpRGB foreground);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_get_background(out GimpRGB background);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_background(ref GimpRGB background);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_default_colors();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_swap_colors();
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_context_get_opacity();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_opacity(double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    static extern LayerModeEffects gimp_context_get_paint_mode();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_paint_mode(LayerModeEffects paint_mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_brush();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_brush(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_context_get_brush_size();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_brush_size(double size);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_brush_default_size();
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_context_get_brush_aspect_ratio();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_brush_aspect_ratio(double aspect);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_context_get_brush_angle();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_brush_angle(double angle);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_dynamics();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_dynamics(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_pattern();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_pattern(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_gradient();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_gradient(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_palette();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_palette(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_font();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_font(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_get_antialias();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_antialias(bool antialias);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_get_feather();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_feather(bool feather);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_get_feather_radius(out double feather_radius_x,
						       out double feather_radius_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_feather_radius(double feather_radius_x,
						       double feather_radius_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_context_get_paint_method();
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_set_paint_method(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_context_list_paint_methods(
				      out int num_paint_methods,
				      out IntPtr paint_methods);
  }
}
