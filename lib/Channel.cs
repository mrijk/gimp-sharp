// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2008 Maurits Rijk
//
// Channel.cs
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
  public class Channel : Drawable
  {
    public Channel(Image image, string name, int width, int height,
                   double opacity, RGB color) :
      this(image, name, width, height, opacity, color.GimpRGB)
    {
    }

    public Channel(Image image, string name, RGB color) :
      this(image, name, image.Width, image.Height, 100, color.GimpRGB)
    {
    }

    // Private (!) constructor so we can pass rgb by reference
    Channel(Image image, string name, int width, int height,
	    double opacity, GimpRGB rgb) :
      base(gimp_channel_new(image.ID, name, width, height, opacity, ref rgb))
    {
    }

    // GIMP 2.4
    public Channel(Image image, ChannelType component, string name) : 
      base(gimp_channel_new_from_component(image.ID, component, name))
    {
    }

    public Channel(Channel channel) : base(gimp_channel_copy(channel.ID))
    {
    }
     
    internal Channel(Int32 channelID) : base(channelID)
    {
    }

    internal Channel()
    {
    }

    public bool ShowMasked
    {
      get {return gimp_channel_get_show_masked(_ID);}
      set {gimp_channel_set_show_masked(_ID, value);}
    }

    public double Opacity
    {
      get {return gimp_channel_get_opacity(_ID);}
      set {gimp_channel_set_opacity(_ID, value);}
    }

    public RGB Color
    {
      get 
	{
          GimpRGB rgb = new GimpRGB();
          if (!gimp_channel_get_color(_ID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set 
	{
          GimpRGB rgb = value.GimpRGB;
          if (!gimp_channel_set_color(_ID, ref rgb))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void CombineMasks(Channel channel, ChannelOps operation,
			     int offx, int offy)
    {
      if (!gimp_channel_combine_masks(_ID, channel.ID, operation,
				      offx, offy))
	{
	  throw new GimpSharpException();
	}
    }

    public void CombineMasks(Channel channel, ChannelOps operation,
			     Offset offset)
    {
      CombineMasks(channel, operation, offset.X, offset.Y);
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_channel_new(Int32 image_ID, string name,
					 int width, int height,
					 double opacity, 
					 ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_channel_new_from_component(Int32 image_ID, 
							ChannelType component,
							string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_channel_copy(Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_get_show_masked(Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_set_show_masked(Int32 channel_ID,
						    bool show_masked);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_channel_get_opacity(Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_set_opacity(Int32 channel_ID,
						double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_get_color(Int32 channel_ID,
					      ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_set_color(Int32 channel_ID,
                                               ref GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_channel_combine_masks(Int32 channel1_ID,
						  Int32 channel2_ID,
						  ChannelOps operation,
						  int offx,
						  int offy);
  }
}
