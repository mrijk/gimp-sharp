using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Image
    {

      Int32 _imageID;
     
      public Image(Int32 imageID)
      {
	_imageID = imageID;
      }
       
      public Image Duplicate()
      {
	return new Image(gimp_image_duplicate(_imageID));
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
       
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_rgb (Int32 image_ID);
       
      public bool ConvertRgb()
      {
	return gimp_image_convert_rgb(_imageID);
      }
       
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_grayscale (Int32 image_ID);
       
      public bool ConvertGrayscale()
      {
	return gimp_image_convert_grayscale(_imageID);
      }
       
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_image_convert_indexed 
      (Int32 image_ID,
       ConvertDitherType dither_type,
       ConvertPaletteType palette_type,
       int num_cols,
       bool alpha_dither,
       bool remove_unused,
       string palette);
       
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
    }
  }
