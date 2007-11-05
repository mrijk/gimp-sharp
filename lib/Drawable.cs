// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
//
// Drawable.cs
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
  public class Drawable
  {
    readonly IntPtr _drawable;
    readonly protected Int32 _ID;
    readonly int _bpp;

    public Drawable(Int32 drawableID)
    {
      _ID = drawableID;
      _drawable = gimp_drawable_get(drawableID);
      _bpp = gimp_drawable_bpp(_ID);	// Cache for performance
    }

    public void Detach()
    {
      gimp_drawable_detach(_drawable);
    }

    public void Flush()
    {
      gimp_drawable_flush(_drawable);
    }

    public bool Delete()
    {
      return gimp_drawable_delete(_ID);
    }

    public bool IsValid
    {
      get {return gimp_drawable_is_valid(_ID);}
    }

    public string Name
    {
      get {return gimp_drawable_get_name(_ID);}
      set {gimp_drawable_set_name(_ID, value);}
    }

    public bool Visible
    {
      get {return gimp_drawable_get_visible(_ID);}
      set {gimp_drawable_set_visible(_ID, value);}
    }

    public bool Linked
    {
      get {return gimp_drawable_get_linked(_ID);}
      set {gimp_drawable_set_linked(_ID, value);}
    }

    public Tattoo Tattoo
    {
      get {return new Tattoo(gimp_drawable_get_tattoo(_ID));}
      set {gimp_drawable_set_tattoo(_ID, value.ID);}
    }

    public Tile GetTile(bool shadow, int row, int col)
    {
      return new Tile(gimp_drawable_get_tile(_ID, shadow, row, col));
    }

    public Tile GetTile2(bool shadow, int x, int y)
    {
      return new Tile(gimp_drawable_get_tile2(_ID, shadow, x, y));
    }

    public Pixel[,] GetThumbnailData(Dimensions dimensions)
    {
      int width = dimensions.Width;
      int height = dimensions.Height;
      int bpp;

      IntPtr src = gimp_drawable_get_thumbnail_data(_ID, ref width,
                                                    ref height, out bpp);
      Pixel[,] thumbnail = 
	Pixel.ConvertToPixelArray(src, new Dimensions(width, height), bpp);
      Marshaller.Free(src);

      return thumbnail;
    }

    public Pixel[,] GetThumbnailData(Rectangle rectangle,
				     Dimensions dimensions)
    {
      int width = dimensions.Width;
      int height = dimensions.Height;
      int bpp;

      IntPtr src = gimp_drawable_get_sub_thumbnail_data(_ID, rectangle.X1,
							rectangle.Y1,
							rectangle.Width,
							rectangle.Height,
							ref width, ref height, 
							out bpp);
      Pixel[,] thumbnail = 
	Pixel.ConvertToPixelArray(src, new Dimensions(width, height), bpp);
      Marshaller.Free(src);

      return thumbnail;
    }

    public void Fill(FillType fill_type)
    {
      if (!gimp_drawable_fill(_ID, fill_type))
        {
	  throw new GimpSharpException();
        }
    }

    public void Update(int x, int y, int width, int height)
    {
      if (!gimp_drawable_update(_ID, x, y, width, height))
        {
	  throw new GimpSharpException();
        }
    }

    public void Update(Rectangle rectangle)
    {
      Update(rectangle.X1, rectangle.Y1, rectangle.Width, rectangle.Height);
    }

    public void Update()
    {
      Update(Bounds);
    }

    public Rectangle Bounds
    {
      get
	{
	  return new Rectangle(0, 0, Width, Height);
	}
    }

    public Dimensions Dimensions
    {
      get {return new Dimensions(Width, Height);}
    }

    public Rectangle MaskBounds
    {
      get 
	{
	  int x1, y1, x2, y2;
	  gimp_drawable_mask_bounds(_ID, out x1, out y1, out x2, out y2);
	  return new Rectangle(x1, y1, x2, y2);
	}
    }

    public Rectangle MaskIntersect
    {
      get 
	{
	  int x1, y1, x2, y2;
	  if (gimp_drawable_mask_intersect(_ID, out x1, out y1, 
					   out x2, out y2))
	    {
	      return new Rectangle(x1, y1, x2, y2);
	    }
	  else
	    {
	      return null;	// selection is empty
	    }
	}
    }

    public Image Image
    {
      get {return new Image(gimp_drawable_get_image(_ID));}
    }

    public bool MergeShadow(bool undo)
    {
      return gimp_drawable_merge_shadow(_ID, undo);
    }

    public bool HasAlpha
    {
      get {return gimp_drawable_has_alpha(_ID);}
    }

    public ImageType TypeWithAlpha
    {
      get {return gimp_drawable_type_with_alpha(_ID);}
    }

    public ImageType Type
    {
      get {return gimp_drawable_type(_ID);}
    }

    public bool IsRGB
    {
      get {return gimp_drawable_is_rgb(_ID);}
    }

    public bool IsGray
    {
      get {return gimp_drawable_is_gray(_ID);}
    }

    public bool IsIndexed
    {
      get {return gimp_drawable_is_indexed(_ID);}
    }

    public int Bpp
    {
      get {return _bpp;}
    }

    public int Width
    {
      get {return gimp_drawable_width(_ID);}
    }

    public int Height
    {
      get {return gimp_drawable_height(_ID);}
    }

    public virtual Offset Offsets
    {
      get
	{
	  int offX, offY;
	  if (!(gimp_drawable_offsets(_ID, out offX, out offY)))
	    {
	      throw new GimpSharpException();
	    }
	  return new Offset(offX, offY);
	}
    }

    public bool IsLayer()
    {
      return gimp_drawable_is_layer(_ID);
    }

    public bool IsLayerMask()
    {
      return gimp_drawable_is_layer_mask(_ID);
    }

    public bool IsChannel()
    {
      return gimp_drawable_is_channel(_ID);
    }

    public void Offset(bool wrapAround, OffsetType fillType,
                       int offsetX, int offsetY)
    {
      if (!gimp_drawable_offset(_ID, wrapAround, fillType, offsetX, 
				offsetY))
        {
	  throw new GimpSharpException();
        }
    }

    public void Offset(bool wrapAround, OffsetType fillType, Offset offset)
    {
      Offset(wrapAround, fillType, offset.X, offset.Y);
    }

    public void ForegroundExtract(ForegroundExtractMode mode, Mask mask)
    {
      if (!gimp_drawable_foreground_extract(_ID, mode, mask.ID))
        {
	  throw new GimpSharpException();
        }
    }

    public Parasite ParasiteFind(string name)
    {
      return new Parasite(gimp_drawable_parasite_find(_ID, name));
    }

    // TODO: make ParasiteList iso List<Parasite>
    // TOOD: implement this!
    public List<Parasite> ParasiteList
    {
      get {return null;}
    }

    public void ParasiteAttach(Parasite parasite)
    {
      if (!gimp_drawable_parasite_attach(_ID, parasite.Ptr))
        {
	  throw new GimpSharpException();
        }
    }

    public void ParasiteDetach(string name)
    {
      if (!gimp_drawable_parasite_detach(_ID, name))
        {
	  throw new GimpSharpException();
        }
    }

    public void AttachNewParasite(string name, int flags, int size, 
                                  object data)
    {
      if (!gimp_drawable_attach_new_parasite(_ID, name, flags, size, data))
        {
	  throw new GimpSharpException();
        }
    }

    public void FuzzySelect(double x, double y, int threshold, 
                            ChannelOps operation, bool antialias,
                            bool feather, double feather_radius,
                            bool sample_merged)
    {
      if (!gimp_fuzzy_select(_ID, x, y, threshold, operation,
                             antialias, feather, feather_radius, 
                             sample_merged))
        {
	  throw new GimpSharpException();
        }
    }

    // GimpEdit

    public void EditCut()
    {
      if (!gimp_edit_cut(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditCopy()
    {
      if (!gimp_edit_copy(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public FloatingSelection EditPaste(bool pasteInto)
    {
      return new FloatingSelection(gimp_edit_paste(_ID, pasteInto));
    }

    static public FloatingSelection EditPasteAsNew()
    {
      return new FloatingSelection(gimp_edit_paste_as_new());
    }

    public Buffer EditNamedCut(string bufferName)
    {
      string name = gimp_edit_named_cut(_ID, bufferName);
      return (name == null) ? null : new Buffer(name);
    }

    public Buffer EditNamedCopy(string bufferName)
    {
      string name = gimp_edit_named_copy(_ID, bufferName);
      return (name == null) ? null : new Buffer(name);
    }

    public Buffer EditNamedCopyVisible(string bufferName)
    {
      string name = gimp_edit_named_copy_visible(_ID, bufferName);
      return (name == null) ? null : new Buffer(name);
    }

    public FloatingSelection EditNamedPaste(string bufferName, bool pasteInto)
    {
      return new FloatingSelection(gimp_edit_named_paste(_ID, bufferName, 
							 pasteInto));
    }

    static public FloatingSelection EditNamedPasteAsNew(string bufferName)
    {
      return new FloatingSelection(gimp_edit_named_paste_as_new(bufferName));
    }

    public void EditClear()
    {
      if (!gimp_edit_clear(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditFill(FillType fill_type)
    {
      if (!gimp_edit_fill(_ID, fill_type))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditBucketFill(BucketFillMode fillMode,
			       LayerModeEffects paintMode,
			       double opacity,
			       double threshold,
			       bool sampleMerged,
			       double x,
			       double y)
    {
      if (!gimp_edit_bucket_fill(_ID, fillMode, paintMode, opacity, 
				 threshold, sampleMerged, x, y))
	{
	  throw new GimpSharpException();
	}
    }

    // Only available in GIMP 2.4.x!
    public void EditBucketFill(BucketFillMode fillMode,
			       LayerModeEffects paintMode,
			       double opacity,
			       double threshold,
			       bool sampleMerged,
			       bool fillTransparent,
			       SelectCriterion selectCriterion,
			       double x,
			       double y)
    {
      if (!gimp_edit_bucket_fill_full(_ID, fillMode, paintMode, opacity, 
				      threshold, sampleMerged, fillTransparent,
				      selectCriterion, x, y))
	{
	  throw new GimpSharpException();
	}
    }

    public void EditBlend(BlendMode blend_mode,
			  LayerModeEffects paint_mode,
			  GradientType gradient_type,
			  double opacity,
			  double offset,
			  RepeatMode repeat,
			  bool reverse,
			  bool supersample,
			  int max_depth,
			  double threshold,
			  bool dither,
			  double x1,
			  double y1,
			  double x2,
			  double y2)
    {
      if (!gimp_edit_blend(_ID, blend_mode, paint_mode, gradient_type,
			   opacity, offset, repeat, reverse, supersample,
			   max_depth, threshold, dither, x1, y1, x2, y2))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditStroke()
    {
      if (!gimp_edit_stroke(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    // GimpColor

    public void BrightnessContrast(int brightness, int contrast)
    {
      if (!gimp_brightness_contrast(_ID, brightness, contrast))
        {
	  throw new GimpSharpException();
        }
    }

    public void Levels(HistogramChannel channel,
                       int low_input, int high_input,
                       double gamma,
                       int low_output,
                       int high_output)
    {
      if (!gimp_levels(_ID, channel, low_input, high_input,
                       gamma, low_output, high_output))
        {
	  throw new GimpSharpException();
        }
    }

    public void LevelsStretch()
    {
      if (!gimp_levels_stretch(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void Posterize(int levels)
    {
      if (!gimp_posterize(_ID, levels))
        {
	  throw new GimpSharpException();
        }
    }

    public void Desaturate()
    {
      if (!gimp_desaturate(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void Desaturate(DesaturateMode desaturate_mode)
    {
      if (!gimp_desaturate_full(_ID, desaturate_mode))
        {
	  throw new GimpSharpException();
        }
    }

    public void Equalize(bool mask_only)
    {
      if (!gimp_equalize(_ID, mask_only))
        {
	  throw new GimpSharpException();
        }
    }

    public void Invert()
    {
      if (!gimp_invert(_ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void CurvesSpline(HistogramChannel channel, CoordinateList<byte>
			     controlPoints)
    {
      byte[] array = controlPoints.ToArray();

      if (!gimp_curves_spline(_ID, channel, array.Length, array))
        {
	  throw new GimpSharpException();
        }
    }

    public void CurvesSplineExplicit(HistogramChannel channel, 
				     CoordinateList<byte> controlPoints)
    {
      byte[] array = controlPoints.ToArray();
      if (!gimp_curves_spline_explicit(_ID, channel, array.Length, array))
        {
	  throw new GimpSharpException();
        }
    }

    public void ColorBalance(TransferMode transfer_mode,
                             bool preserve_lum,
                             double cyan_red,
                             double magenta_green,
                             double yellow_blue)
    {
      if (!gimp_color_balance(_ID, transfer_mode, preserve_lum,
                              cyan_red, magenta_green, yellow_blue))
        {
	  throw new GimpSharpException();
        }
    }

    public void Colorize(double hue, double saturation, double lightness)
    {
      if (!gimp_colorize(_ID, hue, saturation, lightness))
        {
	  throw new GimpSharpException();
        }
    }

    public void Histogram(HistogramChannel channel,
                          int start_range,
                          int end_range,
                          out double mean,
                          out double std_dev,
                          out double median,
                          out double pixels,
                          out double count,
                          out double percentile)
    {
      if (!gimp_histogram(_ID, channel, start_range, end_range,
                          out mean, out std_dev, out median, out pixels, 
                          out count, out percentile))
        {
	  throw new GimpSharpException();
        }
    }

    public void HueSaturation(HueRange hue_range,
                              double hue_offset,
                              double lightness,
                              double saturation)
    {
      if (!gimp_hue_saturation(_ID, hue_range, hue_offset,
                               lightness, saturation))
        {
	  throw new GimpSharpException();
        }
    }

    public void Threshold(int low_threshold,
                          int high_threshold)
    {
      if (!gimp_threshold(_ID, low_threshold,
                          high_threshold))
        {
	  throw new GimpSharpException();
        }
    }

    // gimpdrawabletransform

    public Drawable TransformFlipSimple(OrientationType flip_type,
					bool auto_center, double axis,
					bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_flip_simple(_ID, flip_type,
							      auto_center,
							      axis,
							      clip_result));
    }

    public Drawable TransformFlip(double x0,
				  double y0,
				  double x1,
				  double y1,
				  TransformDirection transform_direction,
				  InterpolationType interpolation,
				  bool supersample,
				  int recursion_level,
				  bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_flip
			  (_ID, x0, y0, x1, y1, transform_direction,
			   interpolation, supersample, recursion_level,
			   clip_result));
    }

    public Drawable TransformFlipDefault(double x0,
					 double y0,
					 double x1,
					 double y1,
					 bool interpolate,
					 bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_flip_default
			  (_ID, x0, y0, x1, y1, interpolate, clip_result));
    }

    public Drawable TransformPerspective(double x0,
					 double y0,
					 double x1,
					 double y1,
					 double x2,
					 double y2,
					 double x3,
					 double y3,
					 TransformDirection transform_direction,
					 InterpolationType interpolation,
					 bool supersample,
					 int recursion_level,
					 bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_perspective
			  (_ID, x0, y0, x1, y1, x2, y2, x3, y3,
			   transform_direction, interpolation, supersample,
			   recursion_level, clip_result));
    }

    public Drawable TransformPerspectiveDefault(double x0,
						double y0,
						double x1,
						double y1,
						double x2,
						double y2,
						double x3,
						double y3,
						bool interpolate,
						bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_perspective_default
			  (_ID, x0, y0, x1, y1, x2, y2, x3, y3,
			   interpolate, clip_result));
    }

    public Drawable TransformRotateSimple(RotationType rotate_type, 
					  bool auto_center,
					  int center_x, int center_y,
					  bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_rotate_simple
			  (_ID, rotate_type, auto_center, center_x, center_y,
			   clip_result));
    }

    public Drawable TransformRotate(double angle,
				    bool auto_center,
				    int center_x, int center_y,
				    TransformDirection transform_direction,
				    InterpolationType interpolation,
				    bool supersample,
				    int recursion_level,
				    bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_rotate
			  (_ID, angle, auto_center, center_x, center_y,
			   transform_direction, interpolation, supersample,
			   recursion_level, clip_result));
    }

    public Drawable TransformRotateDefault(double angle,
					   bool auto_center,
					   int center_x, int center_y,
					   bool interpolate,
					   bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_rotate_default
			  (_ID, angle, auto_center, center_x, center_y,
			   interpolate, clip_result));
    }

    public Drawable TransformScale(double x0, double y0,
				   double x1, double y1,
				   TransformDirection transform_direction,
				   InterpolationType interpolation,
				   bool supersample,
				   int recursion_level,
				   bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_scale
			  (_ID, x0, y0, x1, y1, transform_direction, 
			   interpolation, supersample, recursion_level, 
			   clip_result));
    }

    public Drawable TransformScaleDefault(double x0, double y0,
					  double x1, double y1,
					  bool interpolate,
					  bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_scale_default
			  (_ID, x0, y0, x1, y1, interpolate, clip_result));
    }

    public Drawable TransformShear(OrientationType shear_type,
				   double magnitude,
				   TransformDirection transform_direction,
				   InterpolationType interpolation,
				   bool supersample,
				   int recursion_level,
				   bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_shear
			  (_ID, shear_type, magnitude, transform_direction,
			   interpolation, supersample, recursion_level,
			   clip_result));
    }

    public Drawable TransformShearDefault(OrientationType shear_type,
					  double magnitude,
					  bool interpolate,
					  bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_shear_default
			  (_ID, shear_type, magnitude, interpolate,
			   clip_result));
    }

    public Drawable Transform2d(double source_x, double source_y,
				double scale_x, double scale_y,
				double angle, double dest_x, double dest_y,
				TransformDirection transform_direction,
				InterpolationType interpolation,
				bool supersample, int recursion_level,
				bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_2d
			  (ID, source_x, source_y, scale_x, scale_y,
			   angle, dest_x, dest_y, transform_direction,
			   interpolation, supersample, recursion_level,
			   clip_result));
    }

    public Drawable Transform2dDefault(double source_x, double source_y,
				       double scale_x, double scale_y,
				       double angle, 
				       double dest_x, double dest_y,
				       bool interpolate, bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_2d_default
			  (ID, source_x, source_y, scale_x, scale_y,
			   angle, dest_x, dest_y, interpolate, clip_result));
    }

    public Drawable TransformMatrix(double coeff_0_0, double coeff_0_1,
				    double coeff_0_2, double coeff_1_0,
				    double coeff_1_1, double coeff_1_2,
				    double coeff_2_0, double coeff_2_1,
				    double coeff_2_2,
				    TransformDirection transform_direction,
				    InterpolationType interpolation,
				    bool supersample,
				    int recursion_level,
				    bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_matrix
			  (_ID, coeff_0_0, coeff_0_1, coeff_0_2, coeff_1_0,
			   coeff_1_1, coeff_1_2, coeff_2_0, coeff_2_1,
			   coeff_2_2, transform_direction, interpolation,
			   supersample, recursion_level, clip_result));
    }

    public Drawable TransformMatrixDefault(double coeff_0_0, double coeff_0_1,
					   double coeff_0_2, double coeff_1_0,
					   double coeff_1_1, double coeff_1_2,
					   double coeff_2_0, double coeff_2_1,
					   double coeff_2_2,
					   bool interpolate, bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_matrix_default
			  (_ID, coeff_0_0, coeff_0_1, coeff_0_2, coeff_1_0,
			   coeff_1_1, coeff_1_2, coeff_2_0, coeff_2_1,
			   coeff_2_2, interpolate, clip_result));
    }

    // Convenience routines

    public Pixel CreatePixel()
    {
      return new Pixel(Bpp);
    }

    // Misc routines

    public override bool Equals(object o)
    {
      if (o is Drawable)
	{
	  return (o as Drawable).ID == ID;
	}
      return false;
    }

    public override int GetHashCode()
    {
      return ID;
    }

    internal Int32 ID
    {
      get {return _ID;}
    }

    internal IntPtr Ptr
    {
      get {return _drawable;}
    }

    public override string ToString()
    {
      return "Drawable: " + _ID;
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_drawable_detach(IntPtr drawable);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_drawable_flush(IntPtr drawable);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_delete(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_valid(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_drawable_get_name(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_set_name(Int32 drawable_ID, 
                                              string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_get_visible(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_set_visible(Int32 drawable_ID,
                                                 bool visible);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_get_linked(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_set_linked(Int32 drawable_ID,
                                                bool linked);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_drawable_get_tattoo(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_set_tattoo(Int32 drawable_ID, 
                                                int tattoo);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get_tile(Int32 drawable_ID,
                                                bool shadow, int row, int col);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get_tile2(Int32 drawable_ID,
                                                 bool shadow, int x, int y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get_thumbnail_data(Int32 drawable_ID,
                                                          ref int width, 
                                                          ref int height, 
                                                          out int bpp);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get_sub_thumbnail_data(
							  Int32 drawable_ID,
							  int src_x,
							  int src_y,
							  int src_width,
							  int src_height,
                                                          ref int dest_width, 
                                                          ref int dest_height, 
                                                          out int bpp);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_drawable_get_color_uchar(Int32 drawable_ID,
						     GimpRGB color,
						     byte[] color_uchar);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_merge_shadow(Int32 drawable_ID,
                                                  bool undo);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_fill(Int32 drawable_ID,
                                          FillType fill_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_update(Int32 drawable_ID,
                                            int x,
                                            int y,
                                            int width,
                                            int height);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_mask_bounds(Int32 drawable_ID,
                                                 out int x1,
                                                 out int y1,
                                                 out int x2,
                                                 out int y2);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_mask_intersect(Int32 drawable_ID,
                                                    out int x1,
                                                    out int y1,
                                                    out int x2,
                                                    out int y2);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_drawable_get_image(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_has_alpha (Int32 drawable_ID);      
    [DllImport("libgimp-2.0-0.dll")]
    static extern ImageType gimp_drawable_type_with_alpha(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern ImageType gimp_drawable_type(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_rgb(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_gray (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_indexed (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_drawable_bpp (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_drawable_width(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern int gimp_drawable_height(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_offsets(Int32 drawable_ID,
                                             out int offset_x,
                                             out int offset_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_layer(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_layer_mask(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_is_channel(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_offset(Int32 drawable_ID,
                                            bool wrap_around,
                                            OffsetType fill_type,
                                            int offset_x,
                                            int offset_y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_foreground_extract(Int32 drawable_ID,
						   ForegroundExtractMode mode,
							Int32 mask_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_parasite_find(Int32 drawable_ID,
                                                     string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_parasite_list(Int32 drawable_ID,
						   out int num_parasites,
						   out IntPtr parasites);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_parasite_attach(Int32 drawable_ID,
                                                     IntPtr parasite);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_parasite_detach(Int32 drawable_ID,
                                                     string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_attach_new_parasite(Int32 drawable_ID,
                                                         string name, 
                                                         int flags,
                                                         int size,
                                                         object data);


    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_brightness_contrast (Int32 drawable_ID,
                                                 int brightness, int contrast);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_levels (Int32 drawable_ID,
                                    HistogramChannel channel,
                                    int low_input,
                                    int high_input,
                                    double gamma,
                                    int low_output,
                                    int high_output);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_levels_stretch (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_posterize (Int32 drawable_ID, int levels);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_desaturate (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_desaturate_full (Int32 drawable_ID,
                                             DesaturateMode desaturate_mode);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_equalize (Int32 drawable_ID,
                                      bool mask_only);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_invert (Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_curves_spline (Int32 drawable_ID,
                                           HistogramChannel channel,
                                           int num_points,
                                           byte[] control_pts);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_curves_spline_explicit (Int32 drawable_ID,
                                                    HistogramChannel channel,
                                                    int num_points,
                                                    byte[] control_pts);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_color_balance (Int32 drawable_ID,
                                           TransferMode transfer_mode,
                                           bool preserve_lum,
                                           double cyan_red,
                                           double magenta_green,
                                           double yellow_blue);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_colorize (Int32 drawable_ID,
                                      double hue,
                                      double saturation,
                                      double lightness);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_histogram (Int32 drawable_ID,
                                       HistogramChannel channel,
                                       int start_range,
                                       int end_range,
                                       out double mean,
                                       out double std_dev,
                                       out double median,
                                       out double pixels,
                                       out double count,
                                       out double percentile);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_hue_saturation (Int32 drawable_ID,
                                            HueRange hue_range,
                                            double hue_offset,
                                            double lightness,
                                            double saturation);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_threshold (Int32 drawable_ID,
                                       int low_threshold,
                                       int high_threshold);

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_fuzzy_select (Int32 drawable_ID,
                                          double x,
                                          double y,
                                          int threshold,
                                          ChannelOps operation,
                                          bool antialias,
                                          bool feather,
                                          double feather_radius,
                                          bool sample_merged);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_flip_simple (Int32 drawable_ID,
					 OrientationType flip_type,
					 bool auto_center,
					 double axis,
					 bool clip_result);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_flip (Int32 drawable_ID,
				  double x0,
				  double y0,
				  double x1,
				  double y1,
				  TransformDirection transform_direction,
				  InterpolationType interpolation,
				  bool supersample,
				  int recursion_level,
				  bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_flip_default (Int32 drawable_ID,
					  double x0,
					  double y0,
					  double x1,
					  double y1,
					  bool interpolate,
					  bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_perspective (Int32 drawable_ID,
					 double x0,
					 double y0,
					 double x1,
					 double y1,
					 double x2,
					 double y2,
					 double x3,
					 double y3,
					 TransformDirection transform_direction,
					 InterpolationType interpolation,
					 bool supersample,
					 int recursion_level,
					 bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_perspective_default (Int32 drawable_ID,
						 double x0,
						 double y0,
						 double x1,
						 double y1,
						 double x2,
						 double y2,
						 double x3,
						 double y3,
						 bool interpolate,
						 bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_rotate_simple (Int32 drawable_ID,
					   RotationType rotate_type,
					   bool auto_center,
					   int center_x,
					   int center_y,
					   bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_rotate (Int32 drawable_ID,
				    double angle,
				    bool auto_center,
				    int center_x,
				    int center_y,
				    TransformDirection transform_direction,
				    InterpolationType interpolation,
				    bool supersample,
				    int recursion_level,
				    bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_rotate_default (Int32 drawable_ID,
					    double angle,
					    bool auto_center,
					    int center_x,
					    int center_y,
					    bool interpolate,
					    bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_scale (Int32 drawable_ID,
				   double x0,
				   double y0,
				   double x1,
				   double y1,
				   TransformDirection transform_direction,
				   InterpolationType interpolation,
				   bool supersample,
				   int recursion_level,
				   bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_scale_default (Int32 drawable_ID,
					   double x0,
					   double y0,
					   double x1,
					   double y1,
					   bool interpolate,
					   bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 
    gimp_drawable_transform_shear (Int32 drawable_ID,
				   OrientationType shear_type,
				   double magnitude,
				   TransformDirection transform_direction,
				   InterpolationType interpolation,
				   bool supersample,
				   int recursion_level,
				   bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 
    gimp_drawable_transform_shear_default (Int32 drawable_ID,
					   OrientationType shear_type,
					   double magnitude,
					   bool interpolate,
					   bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_2d (Int32 drawable_ID,
				double source_x,
				double source_y,
				double scale_x,
				double scale_y,
				double angle,
				double dest_x,
				double dest_y,
				TransformDirection transform_direction,
				InterpolationType interpolation,
				bool supersample,
				int recursion_level,
				bool clip_result);
    
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_2d_default (Int32 drawable_ID,
					double source_x,
					double source_y,
					double scale_x,
					double scale_y,
					double angle,
					double dest_x,
					double dest_y,
					bool interpolate,
					bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_matrix (Int32 drawable_ID,
				    double coeff_0_0,
				    double coeff_0_1,
				    double coeff_0_2,
				    double coeff_1_0,
				    double coeff_1_1,
				    double coeff_1_2,
				    double coeff_2_0,
				    double coeff_2_1,
				    double coeff_2_2,
				    TransformDirection transform_direction,
				    InterpolationType interpolation,
				    bool supersample,
				    int recursion_level,
				    bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32
    gimp_drawable_transform_matrix_default (Int32 drawable_ID,
					    double coeff_0_0,
					    double coeff_0_1,
					    double coeff_0_2,
					    double coeff_1_0,
					    double coeff_1_1,
					    double coeff_1_2,
					    double coeff_2_0,
					    double coeff_2_1,
					    double coeff_2_2,
					    bool interpolate,
					    bool clip_result);

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_cut(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_copy(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_edit_paste(Int32 drawable_ID, bool paste_into);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_edit_paste_as_new();
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_edit_named_cut(Int32 drawable_ID, 
					     string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_edit_named_copy(Int32 drawable_ID, 
					      string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_edit_named_copy_visible(Int32 drawable_ID, 
						      string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_edit_named_paste(Int32 drawable_ID, 
					      string buffer_name,
					      bool past_into);
    [DllImport("libgimp-2.0-0.dll")]
    static extern Int32 gimp_edit_named_paste_as_new(string buffer_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_clear(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_fill(Int32 drawable_ID,
				      FillType fill_type);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_bucket_fill(Int32 drawable_ID,
                                             BucketFillMode fill_mode,
                                             LayerModeEffects paint_mode,
                                             double opacity,
                                             double threshold,
                                             bool sample_merged,
                                             double x,
                                             double y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_bucket_fill_full(Int32 drawable_ID,
						  BucketFillMode fill_mode,
						  LayerModeEffects paint_mode,
						  double opacity,
						  double threshold,
						  bool sample_merged,
						  bool fill_transparent,
						  SelectCriterion select_criterion,
						  double x,
						  double y);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_blend(Int32 drawable_ID,
				       BlendMode blend_mode,
				       LayerModeEffects paint_mode,
				       GradientType gradient_type,
				       double opacity,
				       double offset,
				       RepeatMode repeat,
				       bool reverse,
				       bool supersample,
				       int max_depth,
				       double threshold,
				       bool dither,
				       double x1,
				       double y1,
				       double x2,
				       double y2);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_edit_stroke(Int32 drawable_ID);
  }
}
