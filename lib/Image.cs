// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Image.cs
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

using Gdk;

namespace Gimp
{
  public sealed class Image
  {
    internal Int32 _imageID;
     
    internal Image(Int32 imageID)
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
      return new Image(this);
    }

    public static Image Load(RunMode run_mode, string filename, 
                             string raw_filename)
    {
      Int32 imageID = gimp_file_load(run_mode, filename, raw_filename);
      return (imageID >= 0) ? new Image(imageID) : null;
    }

    public bool Save(RunMode run_mode, string filename, string raw_filename)
    {
      Int32 drawableID = ActiveDrawable.ID;	// Check this!
      return gimp_file_save(run_mode, _imageID, drawableID, filename,
                            raw_filename);
    }

    public void Delete()
    {
      if (!gimp_image_delete(_imageID))
        {
	  throw new Exception();
        }
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
       
    public void FreeShadow()
    {
      if (!gimp_image_free_shadow(_imageID))
        {
	  throw new Exception();
        }
    }

    public void Flip(OrientationType flip_type)
    {
      if (!gimp_image_flip(_imageID, flip_type))
        {
	  throw new Exception();
        }
    }

    public void Rotate(RotationType rotate_type)
    {
      if (!gimp_image_rotate(_imageID, rotate_type))
        {
	  throw new Exception();
        }
    }

    public void Resize(int new_width, int new_height, int offx, int offy)
    {
      if (!gimp_image_resize (_imageID, new_width, new_height, offx, offy))
        {
	  throw new Exception();
        }
    }

    public void ResizeToLayers()
    {
      if (!gimp_image_resize_to_layers (_imageID))
        {
	  throw new Exception();
        }
    }
       
    public void Scale(int new_width, int new_height)
    {
      if (!gimp_image_scale(_imageID, new_width, new_height))
        {
	  throw new Exception();
        }
    }
       
    public void Crop(int new_width, int new_height, int offx, int offy)
    {
      if (!gimp_image_crop(_imageID, new_width, new_height, offx, offy))
        {
	  throw new Exception();
        }
    }
      
    public LayerList Layers
    {
      get {return new LayerList(this);}
    }

    public ChannelList Channels
    {
      get {return new ChannelList(this);}
    }

    public Drawable ActiveDrawable
    {
      get {return new Drawable(gimp_image_get_active_drawable (_imageID));}
    }

    public FloatingSelection FloatingSelection
    {
      get
	{
          Int32 layerID = gimp_image_get_floating_sel(_imageID);
          return (layerID == -1) ? null : new FloatingSelection(layerID);
	}
    }

    public Drawable FloatingSelectionAttachedTo
    {
      get
	{
          Int32 drawableID = gimp_image_floating_sel_attached_to(_imageID);
          return (drawableID == -1) ? null : new Drawable(drawableID);
	}
    }

    public RGB PickColor(Drawable drawable, double x, double y,
                         bool sample_merged, bool sample_average, 
			 double average_radius)
    {
      GimpRGB color;
      if (!gimp_image_pick_color(_imageID, drawable.ID, x, y, sample_merged,
                                 sample_average, average_radius, out color))
        {
	  return null;
        }
      return new RGB(color);
    }

    public Layer PickCorrelateLayer(int x, int y)
    {
      Int32 layerID = gimp_image_pick_correlate_layer(_imageID, x, y);
      return (layerID == -1) ? null : new Layer(layerID);
    }

    public void AddLayer(Layer layer, int position)
    {
      if (!gimp_image_add_layer(_imageID, layer.ID, position))
        {
	  throw new Exception();
        }
    }

    public void RemoveLayer(Layer layer)
    {
      if (!gimp_image_remove_layer(_imageID, layer.ID))
        {
	  throw new Exception();
        }
    }

    public void RaiseLayer(Layer layer)
    {
      if (!gimp_image_raise_layer(_imageID, layer.ID))
        {
	  throw new Exception();
        }
    }
       
    public void LowerLayer(Layer layer)
    {
      if (!gimp_image_lower_layer(_imageID, layer.ID))
        {
	  throw new Exception();
        }
    }

    public void RaiseLayerToTop(Layer layer)
    {
      if (!gimp_image_raise_layer_to_top(_imageID, layer.ID))
        {
	  throw new Exception();
        }
    }
       
    public void LowerLayerToBottom(Layer layer)
    {
      if (!gimp_image_lower_layer_to_bottom(_imageID, layer.ID))
        {
	  throw new Exception();
        }
    }

    public void AddChannel(Channel channel, int position)
    {
      if (!gimp_image_add_channel(_imageID, channel.ID, position))
        {
	  throw new Exception();			  
        }
    }

    public void RemoveChannel(Channel channel)
    {
      if (!gimp_image_remove_channel(_imageID, channel.ID))
        {
	  throw new Exception();			  
        }
    }

    public void RaiseChannel(Channel channel)
    {
      if (!gimp_image_raise_channel(_imageID, channel.ID))
        {
	  throw new Exception();			  
        }
    }
       
    public void LowerChannel(Channel channel)
    {
      if (!gimp_image_lower_channel(_imageID, channel.ID))
        {
	  throw new Exception();			  
        }
    }

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

    public Selection Selection
    {
      get {return new Selection(gimp_image_get_selection(_imageID));}
    }

    public bool GetComponentActive(ChannelType component)
    {
      return gimp_image_get_component_active(_imageID, component);
    }

    public void SetComponentActive(ChannelType component, bool active)
    {
      if (!gimp_image_set_component_active(_imageID, component, active))
        {
	  throw new Exception();
        }
    }

    public bool GetComponentVisible(ChannelType component)
    {
      return gimp_image_get_component_visible(_imageID, component);
    }

    public void SetComponentVisible(ChannelType component, bool visible)
    {
      if (!gimp_image_set_component_visible(_imageID, component, visible))
        {
	  throw new Exception();
        }
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

    public string Name
    {
      get {return gimp_image_get_name (_imageID);}
    }

    public void GetResolution(out double xresolution, out double yresolution)
    {
      if (!gimp_image_get_resolution(_imageID, out xresolution, 
				     out yresolution))
        {
	  throw new Exception();
        }
    }

    public void SetResolution(double xresolution, double yresolution)
    {
      if (!gimp_image_set_resolution(_imageID, xresolution, yresolution))
        {
	  throw new Exception();
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

    public int TattooState
    {
      get {return gimp_image_get_tattoo_state(_imageID);}
      set 
	{
          if (!gimp_image_set_tattoo_state(_imageID, value))
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
      return new Channel(gimp_image_get_channel_by_tattoo(_imageID, 
                                                          tattoo.ID));
    }

    public byte[] Colormap
    {
      get
	{
          int num_colors;
          IntPtr cmap = gimp_image_get_colormap(_imageID, out num_colors);
          byte[] colormap = new byte[num_colors];
          Marshal.Copy(cmap, colormap, 0, num_colors);
          // Free cmap!
          return colormap;
	}

      set
	{
          if (!gimp_image_set_colormap(_imageID, value, value.Length / 3))
            {
	      throw new Exception();
            }
	}
    }

    public Parasite ParasiteFind(string name)
    {
      return new Parasite(gimp_image_parasite_find(_imageID, name));
    }

    public void ParasiteAttach(Parasite parasite)
    {
      if (!gimp_image_parasite_attach(_imageID, parasite.Ptr))
        {
	  throw new Exception();
        }
    }

    public void ParasiteDetach(string name)
    {
      if (!gimp_image_parasite_detach(_imageID, name))
        {
	  throw new Exception();
        }
    }

    public void AttachNewParasite(string name, int flags, int size, object data)
    {
      if (!gimp_image_attach_new_parasite(_imageID, name, flags, size, data))
        {
	  throw new Exception();
        }
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

    [DllImport("libgimpui-2.0-0.dll")]
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
       

    public void UndoGroupStart()
    {
      if (!gimp_image_undo_group_start (_imageID))
        {
	  throw new Exception();
        }
    }

    public bool Enabled
    {
      get {return gimp_image_undo_is_enabled(_imageID);}
    }

    public void UndoGroupEnd()
    {
      if (!gimp_image_undo_group_end (_imageID))
        {
	  throw new Exception();
        }
    }

    public void UndoDisable()
    {
      if (!gimp_image_undo_disable (_imageID))
        {
	  throw new Exception();
        }
    }

    public void UndoEnable()
    {
      if (!gimp_image_undo_enable (_imageID))
        {
	  throw new Exception();
        }
    }

    public void UndoFreeze()
    {
      if (!gimp_image_undo_freeze (_imageID))
        {
	  throw new Exception();
        }
    }

    public void UndoThaw()
    {
      if (!gimp_image_undo_thaw (_imageID))
        {
	  throw new Exception();
        }
    }

    // Misc functions

    // Fix me: this should become internal
    public Int32 ID
    {
      get {return _imageID;}
    }
       
    public GuideCollection Guides
    {
      get {return new GuideCollection(this);}
    }

    // All the dll imports

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_new (int width, int height, 
                                        ImageBaseType type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_duplicate (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_delete (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern ImageBaseType gimp_image_base_type (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_image_width (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_image_height (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_free_shadow (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_flip (Int32 image_ID,
                                        OrientationType flip_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_rotate (Int32 image_ID,
                                          RotationType rotate_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_resize (Int32 image_ID,
                                          int new_width,
                                          int new_height,
                                          int offx,
                                          int offy);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_resize_to_layers (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_scale (Int32 image_ID,
                                         int new_width,
                                         int new_height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_crop (Int32 image_ID,
                                        int new_width, int new_height,
                                        int offx, int offy);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_active_drawable (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_floating_sel (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_floating_sel_attached_to (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool  gimp_image_pick_color (Int32 image_ID,
                                               Int32 drawable_ID,
                                               double x, double y,
                                               bool sample_merged, 
					       bool sample_average,
                                               double average_radius,
                                               out GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_pick_correlate_layer (Int32 image_ID,
                                                         int x, int y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_add_layer (Int32 image_ID,
                                             Int32 layer_ID,
                                             int position);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_remove_layer (Int32 image_ID,
                                                Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_raise_layer (Int32 image_ID,
                                               Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_lower_layer (Int32 image_ID,
                                               Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_raise_layer_to_top (Int32 image_ID,
                                                      Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_lower_layer_to_bottom (Int32 image_ID,
                                                         Int32 layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_add_channel (Int32 image_ID,
                                               Int32 channel_ID,
                                               int position);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_remove_channel (Int32 image_ID,
                                                  Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_raise_channel (Int32 image_ID,
                                                 Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_lower_channel (Int32 image_ID,
                                                 Int32 channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_flatten (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_merge_visible_layers(Int32 image_ID,
                                                        MergeType merge_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_merge_down(Int32 image_ID,
                                              Int32 merge_layer_ID,
                                              MergeType merge_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_clean_all (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_is_dirty (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_active_layer (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_active_layer (Int32 image_ID,
                                                    Int32 active_layer_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_active_channel (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_active_channel (Int32 image_ID,
                                                      Int32 active_channel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_selection(Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_get_component_active (Int32 image_ID,
                                                        ChannelType component);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_component_active (Int32 image_ID,
                                                        ChannelType component,
							bool active);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_get_component_visible (Int32 image_ID,
                                                         ChannelType component);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_component_visible (Int32 image_ID,
                                                         ChannelType component,
							 bool visible);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_image_get_filename (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_filename (Int32 image_ID, 
                                                string filename);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_image_get_name (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_get_resolution (Int32 image_ID,
                                                  out double xresolution, 
						  out double yresolution);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_resolution (Int32 image_ID, 
                                                  double xresolution, 
						  double yresolution);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Unit gimp_image_get_unit (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_unit (Int32 image_ID, Unit unit);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_tattoo_state(Int32 image_ID, 
						   int tattoo_state);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_image_get_tattoo_state(Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_layer_by_tattoo (Int32 image_ID, 
                                                        int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_image_get_channel_by_tattoo (Int32 image_ID, 
                                                          int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_get_colormap(Int32 image_ID, 
                                                 out int num_colors);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_set_colormap(Int32 image_ID, 
					       byte[] colormap, 
                                               int num_colors);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_image_parasite_find(Int32 drawable_ID,
                                                  string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_parasite_attach(Int32 drawable_ID,
                                                  IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_parasite_detach(Int32 drawable_ID,
                                                  string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_attach_new_parasite(Int32 drawable_ID,
                                                      string name, 
                                                      int flags,
                                                      int size,
                                                      object data);

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_convert_rgb (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_convert_grayscale (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_convert_indexed 
    (Int32 image_ID,
     ConvertDitherType dither_type,
     ConvertPaletteType palette_type,
     int num_cols,
     bool alpha_dither,
     bool remove_unused,
     string palette);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_file_load(RunMode run_mode, string filename,
                                       string raw_filename);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_file_save(RunMode run_mode, Int32 image_ID,
                                      Int32 drawable_ID, string filename,
                                      string raw_filename);

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_group_start (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_group_end (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_is_enabled (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_disable (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_enable (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_freeze (Int32 image_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_image_undo_thaw (Int32 image_ID);
  }
}
