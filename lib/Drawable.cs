// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2017 Maurits Rijk
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
  public class Drawable : Item
  {
    protected IntPtr _drawable;
    protected int _bpp;

    public bool HasAlpha => gimp_drawable_has_alpha(ID);
    public ImageType TypeWithAlpha => gimp_drawable_type_with_alpha(ID);
    public ImageType Type => gimp_drawable_type(ID);

    public bool IsRGB => gimp_drawable_is_rgb(ID);
    public bool IsGray => gimp_drawable_is_gray(ID);
    public bool IsIndexed => gimp_drawable_is_indexed(ID);

    public int Bpp => _bpp;
    public int Width => gimp_drawable_width(ID);
    public int Height => gimp_drawable_height(ID);

    public Drawable(Int32 drawableID) : base(drawableID)
    {
      _drawable = gimp_drawable_get(drawableID);
      RecalculateBpp();
    }

    protected void RecalculateBpp()
    {
      _bpp = gimp_drawable_bpp(ID);	// Cache for performance
    }

    internal Drawable()
    {
    }

    static internal Drawable Create(Int32 drawableID) => new Drawable(drawableID);

    public void Detach()
    {
      gimp_drawable_detach(_drawable);
    }

    public void Flush()
    {
      gimp_drawable_flush(_drawable);
    }

    public Tile GetTile(bool shadow, int row, int col) =>
      new Tile(gimp_drawable_get_tile(ID, shadow, row, col));

    public Tile GetTile2(bool shadow, int x, int y) =>
      new Tile(gimp_drawable_get_tile2(ID, shadow, x, y));

    public Pixel[,] GetThumbnailData(Dimensions dimensions)
    {
      var (width, height) = dimensions;

      IntPtr src = gimp_drawable_get_thumbnail_data(ID, ref width,
                                                    ref height, out int bpp);
      var thumbnail = 
	Pixel.ConvertToPixelArray(src, new Dimensions(width, height), bpp);
      Marshaller.Free(src);

      return thumbnail;
    }

    public Pixel[,] GetThumbnailData(Rectangle rectangle,
				     Dimensions dimensions)
    {
      var (width, height) = dimensions;

      IntPtr src = gimp_drawable_get_sub_thumbnail_data(ID, rectangle.X1,
							rectangle.Y1,
							rectangle.Width,
							rectangle.Height,
							ref width, ref height, 
							out int bpp);
      var thumbnail = 
	Pixel.ConvertToPixelArray(src, new Dimensions(width, height), bpp);
      Marshaller.Free(src);

      return thumbnail;
    }

    public void FreeShadow()
    {
      if (!gimp_drawable_free_shadow(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void Fill(FillType fill_type)
    {
      if (!gimp_drawable_fill(ID, fill_type))
        {
	  throw new GimpSharpException();
        }
    }

    public void Update(int x, int y, int width, int height)
    {
      if (!gimp_drawable_update(ID, x, y, width, height))
        {
	  throw new GimpSharpException();
        }
    }

    public void Update(Rectangle rectangle)
    {
      Update(rectangle.X1, rectangle.Y1, rectangle.Width, rectangle.Height);
    }

    public void Update() => Update(Bounds);

    public Rectangle Bounds => new Rectangle(0, 0, Width, Height);

    public Dimensions Dimensions => new Dimensions(Width, Height);

    public Rectangle MaskBounds
    {
      get 
	{
	  gimp_drawable_mask_bounds(ID, out int x1, out int y1, out int x2, out int y2);
	  return new Rectangle(x1, y1, x2, y2);
	}
    }

    public Rectangle MaskIntersect
    {
      get 
	{
	  if (gimp_drawable_mask_intersect(ID, out int x1, out int y1, 
					   out int x2, out int y2))
	    {
	      return new Rectangle(x1, y1, x2, y2);
	    }
	  else
	    {
	      return null;	// selection is empty
	    }
	}
    }

    public byte[] GetColorUchar(RGB color)
    {
      var colorUchar = new byte[_bpp];
      var rgb = color.GimpRGB;
      gimp_drawable_get_color_uchar(ID, ref rgb, colorUchar);
      return colorUchar;
    }

    public bool MergeShadow(bool undo) => gimp_drawable_merge_shadow(ID, undo);

    public virtual Offset Offsets
    {
      get
	{
	  if (!(gimp_drawable_offsets(ID, out int offX, out int offY)))
	    {
	      throw new GimpSharpException();
	    }
	  return new Offset(offX, offY);
	}
    }

    public void Offset(bool wrapAround, OffsetType fillType,
                       int offsetX, int offsetY)
    {
      if (!gimp_drawable_offset(ID, wrapAround, fillType, offsetX, 
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
      if (!gimp_drawable_foreground_extract(ID, mode, mask.ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void FuzzySelect(double x, double y, int threshold, 
                            ChannelOps operation, bool antialias,
                            bool feather, double feather_radius,
                            bool sample_merged)
    {
      if (!gimp_fuzzy_select(ID, x, y, threshold, operation,
                             antialias, feather, feather_radius, 
                             sample_merged))
        {
	  throw new GimpSharpException();
        }
    }

    // GimpEdit

    public void EditCut()
    {
      if (!gimp_edit_cut(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditCopy()
    {
      if (!gimp_edit_copy(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public FloatingSelection EditPaste(bool pasteInto) =>
      new FloatingSelection(gimp_edit_paste(ID, pasteInto));

    static public FloatingSelection EditPasteAsNew() =>
      new FloatingSelection(gimp_edit_paste_as_new());

    public Buffer EditNamedCut(string bufferName) =>
      CreateNewBuffer(gimp_edit_named_cut(ID, bufferName));

    public Buffer EditNamedCopy(string bufferName) =>
      CreateNewBuffer(gimp_edit_named_copy(ID, bufferName));

    public Buffer EditNamedCopyVisible(string bufferName) =>
      CreateNewBuffer(gimp_edit_named_copy_visible(ID, bufferName));

    Buffer CreateNewBuffer(string name) =>
      (name == null) ? null : new Buffer(name);

    public FloatingSelection EditNamedPaste(string bufferName, bool pasteInto) =>
      new FloatingSelection(gimp_edit_named_paste(ID, bufferName, pasteInto));

    static public FloatingSelection EditNamedPasteAsNew(string bufferName) =>
      new FloatingSelection(gimp_edit_named_paste_as_new(bufferName));

    public void EditClear()
    {
      if (!gimp_edit_clear(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditFill(FillType fill_type)
    {
      if (!gimp_edit_fill(ID, fill_type))
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
      if (!gimp_edit_bucket_fill(ID, fillMode, paintMode, opacity, 
				 threshold, sampleMerged, x, y))
	{
	  throw new GimpSharpException();
	}
    }
 
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
      if (!gimp_edit_bucket_fill_full(ID, fillMode, paintMode, opacity, 
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
      if (!gimp_edit_blend(ID, blend_mode, paint_mode, gradient_type,
			   opacity, offset, repeat, reverse, supersample,
			   max_depth, threshold, dither, x1, y1, x2, y2))
        {
	  throw new GimpSharpException();
        }
    }

    public void EditStroke()
    {
      if (!gimp_edit_stroke(ID))
        {
	  throw new GimpSharpException();
        }
    }

    // GimpColor

    public void BrightnessContrast(int brightness, int contrast)
    {
      if (!gimp_brightness_contrast(ID, brightness, contrast))
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
      if (!gimp_levels(ID, channel, low_input, high_input,
                       gamma, low_output, high_output))
        {
	  throw new GimpSharpException();
        }
    }

    public void LevelsStretch()
    {
      if (!gimp_levels_stretch(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void Posterize(int levels)
    {
      if (!gimp_posterize(ID, levels))
        {
	  throw new GimpSharpException();
        }
    }

    public void Desaturate()
    {
      if (!gimp_desaturate(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void Desaturate(DesaturateMode desaturate_mode)
    {
      if (!gimp_desaturate_full(ID, desaturate_mode))
        {
	  throw new GimpSharpException();
        }
    }

    public void Equalize(bool mask_only)
    {
      if (!gimp_equalize(ID, mask_only))
        {
	  throw new GimpSharpException();
        }
    }

    public void Invert()
    {
      if (!gimp_invert(ID))
        {
	  throw new GimpSharpException();
        }
    }

    public void CurvesSpline(HistogramChannel channel, CoordinateList<byte>
			     controlPoints)
    {
      byte[] array = controlPoints.ToArray();

      if (!gimp_curves_spline(ID, channel, array.Length, array))
        {
	  throw new GimpSharpException();
        }
    }

    public void CurvesExplicit(HistogramChannel channel, 
			       CoordinateList<byte> controlPoints)
    {
      byte[] array = controlPoints.ToArray();
      if (!gimp_curves_explicit(ID, channel, array.Length, array))
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
      if (!gimp_color_balance(ID, transfer_mode, preserve_lum,
                              cyan_red, magenta_green, yellow_blue))
        {
	  throw new GimpSharpException();
        }
    }

    public void Colorize(double hue, double saturation, double lightness)
    {
      if (!gimp_colorize(ID, hue, saturation, lightness))
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
      if (!gimp_histogram(ID, channel, start_range, end_range,
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
      if (!gimp_hue_saturation(ID, hue_range, hue_offset,
                               lightness, saturation))
        {
	  throw new GimpSharpException();
        }
    }

    public void Threshold(int lowThreshold,
                          int highThreshold)
    {
      if (!gimp_threshold(ID, lowThreshold,
                          highThreshold))
        {
	  throw new GimpSharpException();
        }
    }

    // gimpdrawabletransform

    public Drawable TransformFlipSimple(OrientationType flip_type,
					bool auto_center, double axis,
					bool clip_result) =>
      new Drawable(gimp_drawable_transform_flip_simple(ID, flip_type, auto_center,
						       axis, clip_result));

    public Drawable TransformFlip(double x0,
				  double y0,
				  double x1,
				  double y1,
				  TransformDirection transform_direction,
				  InterpolationType interpolation,
				  bool supersample,
				  int recursion_level,
				  bool clip_result) =>
      new Drawable(gimp_drawable_transform_flip(ID, x0, y0, x1, y1, 
						transform_direction, interpolation, 
						supersample, recursion_level,
						clip_result));

    public Drawable TransformFlip(double x0,
				  double y0,
				  double x1,
				  double y1,
				  bool interpolate,
				  bool clip_result) =>
      new Drawable(gimp_drawable_transform_flip_default(ID, x0, y0, x1, y1, 
							interpolate, clip_result));

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
			  (ID, x0, y0, x1, y1, x2, y2, x3, y3,
			   transform_direction, interpolation, supersample,
			   recursion_level, clip_result));
    }

    public Drawable TransformPerspective(double x0,
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
			  (ID, x0, y0, x1, y1, x2, y2, x3, y3,
			   interpolate, clip_result));
    }

    public Drawable TransformRotateSimple(RotationType rotate_type, 
					  bool auto_center,
					  int center_x, int center_y,
					  bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_rotate_simple
			  (ID, rotate_type, auto_center, center_x, center_y,
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
			  (ID, angle, auto_center, center_x, center_y,
			   transform_direction, interpolation, supersample,
			   recursion_level, clip_result));
    }

    public Drawable TransformRotate(double angle,
				    bool auto_center,
				    int center_x, int center_y,
				    bool interpolate,
				    bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_rotate_default
			  (ID, angle, auto_center, center_x, center_y,
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
			  (ID, x0, y0, x1, y1, transform_direction, 
			   interpolation, supersample, recursion_level, 
			   clip_result));
    }

    public Drawable TransformScale(double x0, double y0,
				   double x1, double y1,
				   bool interpolate,
				   bool clip_result)
    {
      return new Drawable(gimp_drawable_transform_scale_default
			  (ID, x0, y0, x1, y1, interpolate, clip_result));
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
			  (ID, shear_type, magnitude, transform_direction,
			   interpolation, supersample, recursion_level,
			   clip_result));
    }

    public Drawable TransformShear(OrientationType shear_type,
				   double magnitude,
				   bool interpolate,
				   TransformResize clip_result)
    {
      return new Drawable(gimp_drawable_transform_shear_default
			  (ID, shear_type, magnitude, interpolate,
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
			  (ID, coeff_0_0, coeff_0_1, coeff_0_2, coeff_1_0,
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
			  (ID, coeff_0_0, coeff_0_1, coeff_0_2, coeff_1_0,
			   coeff_1_1, coeff_1_2, coeff_2_0, coeff_2_1,
			   coeff_2_2, interpolate, clip_result));
    }

    // Convenience routines

    public Pixel CreatePixel() => new Pixel(Bpp);

    public void Save(string filename)
    {
      if (!Image.Save(this, filename ))
	{
	  throw new GimpSharpException();
	}
    }

    new internal Int32 ID
    {
      get => base.ID;
      set 
	{
	  _ID = value;
	  _drawable = gimp_drawable_get(ID);
	  RecalculateBpp();
	}
    }

    internal IntPtr Ptr => _drawable;

    public override string ToString() => "Drawable: " + ID;

    [DllImport("libgimp-2.0-0.dll")]
    static extern IntPtr gimp_drawable_get(Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_drawable_detach(IntPtr drawable);
    [DllImport("libgimp-2.0-0.dll")]
    static extern void gimp_drawable_flush(IntPtr drawable);
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
						     ref GimpRGB color,
						     byte[] color_uchar);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_merge_shadow(Int32 drawable_ID,
                                                  bool undo);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_drawable_free_shadow(Int32 drawable_ID);
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
    static extern bool gimp_curves_spline(Int32 drawable_ID,
					  HistogramChannel channel,
					  int num_points,
					  byte[] control_pts);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_curves_explicit(Int32 drawable_ID,
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
					   TransformResize clip_result);

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
