using System;
using System.Runtime.InteropServices;

using Gdk;

namespace Gimp
  {
    public class Image
    {
      Int32 _imageID;
     
      public Image(Int32 imageID)
      {
	_imageID = imageID;
      }
       
      public Image(int width, int height, ImageBaseType type)
      {
	_imageID = gimp_image_new (width, height, type);
      }

      public Image(Image image)
      {
	_imageID = gimp_image_duplicate(image._imageID);
      }

      public Image Duplicate()
      {
	return new Image(gimp_image_duplicate(_imageID));
      }

      public static Image Load(RunMode run_mode, string filename, 
			       string raw_filename)
      {
	Int32 imageID = gimp_file_load(run_mode, filename, raw_filename);
	return (imageID >= 0) ? new Image(imageID) : null;
      }

      public bool Delete()
      {
	return gimp_image_delete(_imageID);
      }
       
      public ImageBaseType BaseType
      {
	get {return gimp_image_base_type (_imageID);}
      }

      public int Width
      {
	get {return gimp_image_width(_imageID);}
      }
       
      public int Height
      {
	get {return gimp_image_height(_imageID);}
      }
       
      public bool FreeShadow()
      {
	return gimp_image_free_shadow(_imageID);
      }

      public bool Flip(OrientationType flip_type)
      {
	return gimp_image_flip(_imageID, flip_type);
      }

      public bool Rotate(RotationType rotate_type)
      {
	return gimp_image_rotate(_imageID, rotate_type);
      }

      public bool Resize(int new_width, int new_height, int offx, int offy)
      {
	return gimp_image_resize (_imageID, new_width, new_height, offx, offy);
      }

      public bool ResizeToLayers()
      {
	return gimp_image_resize_to_layers (_imageID);
      }
       
      public bool Scale(int new_width, int new_height)
      {
	return gimp_image_scale(_imageID, new_width, new_height);
      }
       
      public bool Crop(int new_width, int new_height,
		       int offx, int offy)
      {
	return gimp_image_crop(_imageID, new_width, new_height, offx, offy);
      }
              
      public Drawable ActiveDrawable
      {
	get {return new Drawable(gimp_image_get_active_drawable (_imageID));}
      }

      public bool AddLayer(Layer layer, int position)
      {
	return gimp_image_add_layer(_imageID, layer.ID, position);
      }

      public bool RemoveLayer(Layer layer)
      {
	return gimp_image_remove_layer(_imageID, layer.ID);
      }

      public bool RaiseLayer(Layer layer)
      {
	return gimp_image_raise_layer(_imageID, layer.ID);
      }
       
      public bool LowerLayer(Layer layer)
      {
	return gimp_image_lower_layer(_imageID, layer.ID);
      }

      public bool RaiseLayerToTop(Layer layer)
      {
	return gimp_image_raise_layer(_imageID, layer.ID);
      }
       
      public bool LowerLayerToBottom(Layer layer)
      {
	return gimp_image_lower_layer(_imageID, layer.ID);
      }
#if false
      public bool AddChannel(Channel channel, int position)
      {
	return gimp_image_add_channel(_imageID, channel.ID, position);
      }

      public bool RemoveChannel(Channel channel)
      {
	return gimp_image_remove_channel(_imageID, channel.ID);
      }

      public bool RaiseChannel(Channel channel)
      {
	return gimp_image_raise_channel(_imageID, channel.ID);
      }
       
      public bool LowerChannel(Channel channel)
      {
	return gimp_image_lower_channel(_imageID, channel.ID);
      }
#endif
      public Layer Flatten()
      {
	return new Layer(gimp_image_flatten (_imageID));
      }

      public Layer MergeVisibleLayers(MergeType merge_type)
      {
	return new Layer(gimp_image_merge_visible_layers (_imageID,
							  merge_type));
      }

      public Layer MergeDown(Layer layer, MergeType merge_type)
      {
	return new Layer(gimp_image_merge_down (_imageID,
						layer.ID,
						merge_type));
      }

      public void CleanAll()
      {
	if (!gimp_image_clean_all (_imageID))
	  {
	  throw new Exception();
	  }
      }

      public bool IsDirty
      {
	get {return gimp_image_is_dirty (_imageID);}
      }

      public Layer ActiveLayer
      {
	get {return new Layer(gimp_image_get_active_layer (_imageID));}
	set 
	    {
	    if (!gimp_image_set_active_layer (_imageID, value.ID))
	      {
	      throw new Exception();
	      }
	    }
      }

      public Channel ActiveChannel
      {
	get {return new Channel(gimp_image_get_active_channel (_imageID));}
	set 
	    {
	    if (!gimp_image_set_active_channel (_imageID, value.ID))
	      {
	      throw new Exception();
	      }
	    }
      }

      public string Name
      {
	get {return gimp_image_get_name (_imageID);}
      }

      public string Filename
      {
	get {return gimp_image_get_filename(_imageID);}
	set
	    {
	    if (!gimp_image_set_filename(_imageID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public Unit Unit
      {
	get {return gimp_image_get_unit (_imageID);}
	set 
	    {
	    if (!gimp_image_set_unit (_imageID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public Layer GetLayerByTattoo(Tattoo tattoo)
      {
	return new Layer(gimp_image_get_layer_by_tattoo(_imageID, tattoo.ID));
      }

      public Channel GetChannelByTattoo(Tattoo tattoo)
      {
	return new Channel(gimp_image_get_channel_by_tattoo(_imageID, tattoo.ID));
      }

      public bool ConvertRgb()
      {
	return gimp_image_convert_rgb(_imageID);
      }
       
      public bool ConvertGrayscale()
      {
	return gimp_image_convert_grayscale(_imageID);
      }
       
       
      public bool ConvertIndexed(ConvertDitherType dither_type,
				 ConvertPaletteType palette_type,
				 int num_cols,
				 bool alpha_dither,
				 bool remove_unused,
				 string palette)
      {
	return gimp_image_convert_indexed(_imageID,
					  dither_type,
					  palette_type,
					  num_cols,
					  alpha_dither,
					  remove_unused,
					  palette);
      }

      // Implementation of ...

      [DllImport("libgimpui-2.0.so")]
      static extern IntPtr gimp_image_get_thumbnail (Int32 image_ID,
						     int width,
						     int height,
						     Transparency alpha);

      public Pixbuf GetThumbnail (int width, int height, Transparency alpha)
      {
	return new Pixbuf (gimp_image_get_thumbnail (_imageID, width, height,
						     alpha));
      }

      // Implementation of gimpundo_pdb.h
       
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_undo_group_start (Int32 image_ID);

      public bool UndoGroupStart()
      {
	return gimp_image_undo_group_start (_imageID);
      }

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_undo_group_end (Int32 image_ID);

      public bool UndoGroupEnd()
      {
	return gimp_image_undo_group_end (_imageID);
      }

      // Misc functions

      public Int32 ID
      {
	get {return _imageID;}
      }
       
      public GuideCollection Guides
      {
	get {return new GuideCollection(this);}
      }

      // All the dll imports

      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_new (int width, int height, 
					  ImageBaseType type);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_duplicate (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_delete (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern ImageBaseType gimp_image_base_type (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern int gimp_image_width (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern int gimp_image_height (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_free_shadow (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_flip (Int32 image_ID,
					  OrientationType flip_type);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_rotate (Int32 image_ID,
					    RotationType rotate_type);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_resize (Int32 image_ID,
					    int new_width,
					    int new_height,
					    int offx,
					    int offy);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_resize_to_layers (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_scale (Int32 image_ID,
					   int new_width,
					   int new_height);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_crop (Int32 image_ID,
					  int new_width, int new_height,
					  int offx, int offy);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_get_active_drawable (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_add_layer (Int32 image_ID,
					       Int32 layer_ID,
					       int position);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_remove_layer (Int32 image_ID,
						  Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_raise_layer (Int32 image_ID,
						 Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_lower_layer (Int32 image_ID,
						 Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_raise_layer_to_top (Int32 image_ID,
							Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_raise_layer_to_bottom (Int32 image_ID,
							   Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_add_channel (Int32 image_ID,
						 Int32 channel_ID,
						 int position);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_remove_channel (Int32 image_ID,
						    Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_raise_channel (Int32 image_ID,
						   Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_lower_channel (Int32 image_ID,
						   Int32 channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_flatten (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_merge_visible_layers(Int32 image_ID,
							  MergeType merge_type);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_merge_down(Int32 image_ID,
						Int32 merge_layer_ID,
						MergeType merge_type);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_clean_all (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_is_dirty (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_get_active_layer (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_set_active_layer (Int32 image_ID,
						      Int32 active_layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_get_active_channel (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_set_active_channel (Int32 image_ID,
							Int32 active_channel_ID);
      [DllImport("libgimp-2.0.so")]
      static extern string gimp_image_get_filename (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_set_filename (Int32 image_ID, 
						  string filename);
      [DllImport("libgimp-2.0.so")]
      static extern string gimp_image_get_name (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern Unit gimp_image_get_unit (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_set_unit (Int32 image_ID, Unit unit);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_get_layer_by_tattoo (Int32 image_ID, 
							  int tattoo);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_image_get_channel_by_tattoo (Int32 image_ID, 
							    int tattoo);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_rgb (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_grayscale (Int32 image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_indexed 
      (Int32 image_ID,
       ConvertDitherType dither_type,
       ConvertPaletteType palette_type,
       int num_cols,
       bool alpha_dither,
       bool remove_unused,
       string palette);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_file_load(RunMode run_mode, string filename,
					 string raw_filename);
    }
  }
