// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// Grid.cs
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
  public sealed class Grid
  {
    readonly Int32 _imageID;

    internal Grid(Int32 imageID)
    {
      _imageID = imageID;
    }

    public Spacing Spacing
    {
      get
	{
	  double xspacing, yspacing; 
	  if (!gimp_image_grid_get_spacing(_imageID, out xspacing, out yspacing))
	    {
	      throw new GimpSharpException();
	    }
	  return new Spacing(xspacing, yspacing);
	}
      set
	{
	  if (!gimp_image_grid_set_spacing(_imageID, value.X, value.Y))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public DoubleOffset Offset
    {
      get
	{
	  double xoffset, yoffset; 
	  if (!gimp_image_grid_get_offset(_imageID, out xoffset, out yoffset))
	    {
	      throw new GimpSharpException();
	    }
	  return new DoubleOffset(xoffset, yoffset);
	}
      set
	{
	  if (!gimp_image_grid_set_offset(_imageID, value.X, value.Y))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public RGB ForegroundColor
    {
      get
	{
	  var rgb = new GimpRGB();
	  if (!gimp_image_grid_get_foreground_color(_imageID, out rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  var rgb = value.GimpRGB;
	  if (!gimp_image_grid_set_foreground_color(_imageID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public RGB BackgroundColor
    {
      get
	{
	  var rgb = new GimpRGB();
	  if (!gimp_image_grid_get_background_color(_imageID, out rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  var rgb = value.GimpRGB;
	  if (!gimp_image_grid_set_background_color(_imageID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public GridStyle Style
    {
      get 
	{
	  return gimp_image_grid_get_style(_imageID);
	}
      set
	{
	  if (!gimp_image_grid_set_style(_imageID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_get_spacing(Int32 image_ID,
						   out double xspacing,
						   out double yspacing);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_set_spacing(Int32 image_ID,
						   double xspacing,
						   double yspacing);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_get_offset(Int32 image_ID,
						  out double xoffset,
						  out double yoffset);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_set_offset(Int32 image_ID,
						  double xoffset,
						  double yoffset);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_get_foreground_color(Int32 image_ID,
							    out GimpRGB fgcolor);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_set_foreground_color(Int32 image_ID,
							    ref GimpRGB fgcolor);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_get_background_color(Int32 image_ID,
							    out GimpRGB bgcolor);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_set_background_color(Int32 image_ID,
							    ref GimpRGB bgcolor);
    [DllImport("libgimp-2.0-0.dll")]
    static extern GridStyle gimp_image_grid_get_style(Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_grid_set_style(Int32 image_ID, GridStyle style);
  }
}
