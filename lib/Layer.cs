// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
//
// Layer.cs
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
    public class Layer : Drawable
    {    
      public Layer(Image image, string name, int width, int height, 
		   ImageType type, double opacity, LayerModeEffects mode) : 
	base(gimp_layer_new(image.ID, name, width, height, type, 
			    opacity, mode))
      {
      }
  
      public Layer(Layer layer) : base(gimp_layer_copy(layer.ID))
      {
      }

      public Layer(Drawable drawable, Image dest_image) :
	base(gimp_layer_new_from_drawable(drawable.ID, dest_image.ID))
      {
      }

      public Layer(Int32 layerID) : base(layerID)
      {
      }

      public void Scale(int new_width, int new_height,
			bool local_origin)
      {
	if (!gimp_layer_scale (_ID, new_width, new_height, local_origin))
	  {
	  throw new Exception();
	  }
      }

      public void Resize(int new_width, int new_height,
			 int offx, int offy)
      {
	if (!gimp_layer_resize (_ID, new_width, new_height, offx, offy))
	  {
	  throw new Exception();
	  }
      }

      public void ResizeToImageSize()
      {
	if (!gimp_layer_resize_to_image_size (_ID))
	  {
	  throw new Exception();
	  }
      }

      public void Translate(int offx, int offy)
      {
	if (!gimp_layer_translate (_ID, offx, offy))
	  {
	  throw new Exception();
	  }
      }

      public void AddAlpha()
      {
	if (!gimp_layer_add_alpha (_ID))
	  {
	  throw new Exception();
	  }
      }

      public void SetOffsets(int offx, int offy)
      {
	if (!gimp_layer_set_offsets (_ID, offx, offy))
	  {
	  throw new Exception();
	  }
      }

      public Mask CreateMask(AddMaskType mask_type)
      {
	return new Mask(gimp_layer_create_mask(_ID, mask_type));
      }

      public bool PreserveTrans
      {
	get {return gimp_layer_get_preserve_trans (_ID);}
	set 
	    {
	    if (!gimp_layer_set_preserve_trans (_ID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public bool ApplyMask
      {
	get {return gimp_layer_get_apply_mask (_ID);}
	set 
	    {
	    if (!gimp_layer_set_apply_mask (_ID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public bool ShowMask
      {
	get {return gimp_layer_get_show_mask (_ID);}
	set 
	    {
	    if (!gimp_layer_set_show_mask (_ID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public bool EditMask
      {
	get {return gimp_layer_get_edit_mask (_ID);}
	set 
	    {
	    if (!gimp_layer_set_edit_mask (_ID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public double Opacity
      {
	get {return gimp_layer_get_opacity (_ID);}
	set {
	if (!gimp_layer_set_opacity (_ID, value))
	  {
	  throw new Exception();
	  }
	}
      }
  
      public LayerModeEffects Mode
      {
	get {return gimp_layer_get_mode (_ID);}
	set 
	    {
	    if (!gimp_layer_set_mode (_ID, value))
	      {
	      throw new Exception();
	      }
	    }
      }

      public bool FloatingSel
      {
	get {return gimp_layer_is_floating_sel (_ID);}
      }

      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_layer_new (Int32 image_ID,
					  string name,
					  int width,
					  int height,
					  ImageType type,
					  double opacity,
					  LayerModeEffects mode);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_layer_copy (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_scale (Int32 layer_ID,
					   int new_width,
					   int new_height,
					   bool local_origin);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_resize (Int32 layer_ID,
					    int new_width,
					    int new_height,
					    int offx,
					    int offy);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_resize_to_image_size (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_translate (Int32 layer_ID, 
					       int offx, int offy);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_add_alpha (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_offsets (Int32 layer_ID,
						 int offx,
						 int offy);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_layer_create_mask (Int32 layer_ID,
						  AddMaskType mask_type);
      [DllImport("libgimp-2.0.so")]
      static extern Int32 gimp_layer_get_mask (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_add_mask (Int32 layer_ID,
					      Int32 mask_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_remove_mask (Int32 layer_ID,
						 MaskApplyMode mode);
      [DllImport("libgimp-2.0.so")]
      static extern int gimp_layer_new_from_drawable (Int32 drawable_ID,
						      Int32 dest_image_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_get_preserve_trans (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_preserve_trans (Int32 layer_ID,
							bool preserve_trans);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_get_apply_mask (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_apply_mask (Int32 layer_ID,
						    bool apply_mask);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_get_show_mask (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_show_mask (Int32 layer_ID,
						   bool show_mask);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_get_edit_mask (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_edit_mask (Int32 layer_ID,
						   bool edit_mask);
      [DllImport("libgimp-2.0.so")]
      static extern double gimp_layer_get_opacity (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_opacity (Int32 layer_ID,
						 double opacity);
      [DllImport("libgimp-2.0.so")]
      static extern LayerModeEffects gimp_layer_get_mode (Int32 layer_ID);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_set_mode (Int32 layer_ID,
					      LayerModeEffects mode);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_layer_is_floating_sel (Int32 layer_ID);
    }
  }
