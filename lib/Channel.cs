using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Channel
    {

      Int32 _channelID;

      public Channel(Image image, string name, int width, int height,
		     double opacity, RGB color)
      {
	GimpRGB rgb = color.GimpRGB;
	_channelID = gimp_channel_new (image.ID, name, width, height,
				       opacity, ref rgb);
      }

      public Channel(Channel channel)
      {
	_channelID = gimp_channel_copy(channel.ID);
      }
     
      public Channel(Int32 channelID)
      {
	_channelID = channelID;
      }

      public bool ShowMasked
      {
	get {return gimp_channel_get_show_masked (_channelID);}
	set {gimp_channel_set_show_masked (_channelID, value);}
      }

      public double Opacity
      {
	get {return gimp_channel_get_opacity (_channelID);}
	set {gimp_channel_set_opacity (_channelID, value);}
      }

      public RGB Color
      {
	get 
	    {
	    GimpRGB rgb = new GimpRGB();
	    gimp_channel_get_color (_channelID, ref rgb);
	    return new RGB(rgb);
	    }
	set 
	    {
	    GimpRGB rgb = value.GimpRGB;
	    gimp_channel_set_color (_channelID, ref rgb);
	    }
      }

      public bool CombineMasks (Channel channel, ChannelOps operation,
				int offx, int offy)
      {
	return gimp_channel_combine_masks (_channelID, channel.ID, operation,
					   offx, offy);
      }

      public Int32 ID
      {
	get {return _channelID;}
      }

      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_channel_new (Int32 image_ID, string name,
					    int width, int height,
					    double opacity, 
					    ref GimpRGB color);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_channel_copy (Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_get_show_masked (Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_set_show_masked (Int32 channel_ID,
						       bool show_masked);
      [DllImport("libgimp-2.0.so")]
      static extern double gimp_channel_get_opacity (Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_set_opacity (Int32 channel_ID,
						   double opacity);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_get_color (Int32 channel_ID,
						 ref GimpRGB color);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_set_color (Int32 channel_ID,
						 ref GimpRGB color);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_channel_combine_masks (Int32 channel1_ID,
						     Int32 channel2_ID,
						     ChannelOps operation,
						     int offx,
						     int offy);
    }
  }
