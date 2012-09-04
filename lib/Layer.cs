// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
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
    readonly Func<Image, Layer> _delay;

    public Layer(Image image, string name, int width, int height, 
		 ImageType type, double opacity = 100,
		 LayerModeEffects mode = LayerModeEffects.Normal) : 
      base(gimp_layer_new(image.ID, name, width, height, type, 
			  opacity, mode))
    {
    }

    public Layer(Image image, string name, ImageType type, 
		 double opacity = 100, 
		 LayerModeEffects mode = LayerModeEffects.Normal) : 
      this(image, name, image.Width, image.Height, type, opacity, mode)
    {
    }

    public Layer(string name, ImageType type)
    {
      _delay = (image) => {return new Layer(image, name, type);};
    }

    internal Layer DelayedConstruct(Image image)
    {
      return _delay(image);
    }
  
    public Layer(Layer layer) : base(gimp_layer_copy(layer.ID))
    {
    }

    public Layer(Drawable drawable, Image destImage) :
      base(gimp_layer_new_from_drawable(drawable.ID, destImage.ID))
    {
    }

    public Layer(Image image, Image destImage, string name) :
      base(gimp_layer_new_from_visible(image.ID, destImage.ID, name))
    {
    }

    // Fix me: can this one be avoided?
    public Layer(Drawable drawable) : base(drawable.ID)
    {
    }

    internal Layer(Int32 layerID) : base(layerID)
    {
    }

    public Layer()
    {
    }

    public void Scale(int newWidth, int newHeight, bool localOrigin)
    {
      if (!gimp_layer_scale(_ID, newWidth, newHeight, localOrigin))
	{
	  throw new GimpSharpException();
	}
    }

    public void Scale(Dimensions dimensions, bool localOrigin)
    {
      Scale(dimensions.Width, dimensions.Height, localOrigin);
    }

    public void Scale(int newWidth, int newHeight, bool localOrigin, 
                      InterpolationType interpolation)
    {
      if (!gimp_layer_scale_full(_ID, newWidth, newHeight, localOrigin, 
				 interpolation))
	{
	  throw new GimpSharpException();
	}
    }

    public void Scale(Dimensions dimensions, bool localOrigin,
                      InterpolationType interpolation)
    {
      Scale(dimensions.Width, dimensions.Height, localOrigin, interpolation);
    }

    public void Resize(int newWidth, int newHeight, int offx, int offy)
    {
      if (!gimp_layer_resize(_ID, newWidth, newHeight, offx, offy))
	{
	  throw new GimpSharpException();
	}
    }

    public void Resize(Dimensions dimensions, Offset offset)
    {
      Resize(dimensions.Width, dimensions.Height, offset.X, offset.Y);
    }

    public void ResizeToImageSize()
    {
      if (!gimp_layer_resize_to_image_size(_ID))
	{
	  throw new GimpSharpException();
	}
    }

    public void Translate(int offx, int offy)
    {
      if (!gimp_layer_translate(_ID, offx, offy))
	{
	  throw new GimpSharpException();
	}
    }

    public void Translate(Offset offset)
    {
      Translate(offset.X, offset.Y);
    }

    public void AddAlpha()
    {
      if (!gimp_layer_add_alpha(_ID))
	{
	  throw new GimpSharpException();
	}
      RecalculateBpp();
    }

    public void Flatten()
    {
      if (!gimp_layer_flatten(_ID))
	{
	  throw new GimpSharpException();
	}
    }

    public new Offset Offsets
    {
      get {return base.Offsets;}
      set
	{
	  if (!gimp_layer_set_offsets(_ID, value.X, value.Y))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public Mask CreateMask(AddMaskType mask_type)
    {
      return new Mask(gimp_layer_create_mask(_ID, mask_type));
    }

    public Mask Mask
    {
      get
	{
	  Int32 maskID = gimp_layer_get_mask(_ID);
	  return (maskID == -1) ? null : new Mask(maskID);
	}
      set
	{
	  if (!gimp_layer_add_mask(_ID, value.ID))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public void RemoveMask(MaskApplyMode mode)
    {
      if (!gimp_layer_remove_mask(_ID, mode))
	{
	  throw new GimpSharpException();
	}
    }

    public bool LockAlpha
    {
      get {return gimp_layer_get_lock_alpha(_ID);}
      set 
	{
	  if (!gimp_layer_set_lock_alpha(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool ApplyMask
    {
      get {return gimp_layer_get_apply_mask(_ID);}
      set 
	{
	  if (!gimp_layer_set_apply_mask(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool ShowMask
    {
      get {return gimp_layer_get_show_mask(_ID);}
      set 
	{
	  if (!gimp_layer_set_show_mask(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool EditMask
    {
      get {return gimp_layer_get_edit_mask(_ID);}
      set 
	{
	  if (!gimp_layer_set_edit_mask(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public double Opacity
    {
      get {return gimp_layer_get_opacity(_ID);}
      set {
	if (!gimp_layer_set_opacity(_ID, value))
	  {
	    throw new GimpSharpException();
	  }
      }
    }
  
    public LayerModeEffects Mode
    {
      get {return gimp_layer_get_mode(_ID);}
      set 
	{
	  if (!gimp_layer_set_mode(_ID, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public bool IsFloatingSelection
    {
      get {return gimp_layer_is_floating_sel(_ID);}
    }

    public void SetBuffer(byte[] buffer)
    {
      var rgn = new PixelRgn(this, true, false);
      rgn.SetRect(buffer, Bounds);
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_layer_new (Int32 image_ID,
					string name,
					int width,
					int height,
					ImageType type,
					double opacity,
					LayerModeEffects mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_layer_copy (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_scale (Int32 layer_ID,
					 int new_width,
					 int new_height,
					 bool local_origin);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_scale_full (Int32 layer_ID,
                                              int new_width,
                                              int new_height,
                                              bool local_origin,
                                              InterpolationType interpolation);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_resize (Int32 layer_ID,
					  int new_width,
					  int new_height,
					  int offx,
					  int offy);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_resize_to_image_size(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_translate(Int32 layer_ID, 
					    int offx, int offy);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_add_alpha(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_flatten(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_offsets(Int32 layer_ID,
					      int offx, int offy);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_layer_create_mask(Int32 layer_ID,
					       AddMaskType mask_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_layer_get_mask(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_add_mask(Int32 layer_ID, Int32 mask_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_remove_mask (Int32 layer_ID,
					       MaskApplyMode mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_layer_new_from_drawable (Int32 drawable_ID,
						    Int32 dest_image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_layer_new_from_visible (Int32 image_ID,
                                                   Int32 dest_image_ID,
                                                   string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_get_lock_alpha(Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_lock_alpha(Int32 layer_ID,
						 bool lock_alpha);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_get_apply_mask (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_apply_mask (Int32 layer_ID,
						  bool apply_mask);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_get_show_mask (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_show_mask (Int32 layer_ID,
						 bool show_mask);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_get_edit_mask (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_edit_mask (Int32 layer_ID,
						 bool edit_mask);
    [DllImport("libgimp-2.0-0.dll")]
    static extern double gimp_layer_get_opacity (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_opacity (Int32 layer_ID,
					       double opacity);
    [DllImport("libgimp-2.0-0.dll")]
    static extern LayerModeEffects gimp_layer_get_mode (Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_set_mode (Int32 layer_ID,
					    LayerModeEffects mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_layer_is_floating_sel (Int32 layer_ID);
  }
}
