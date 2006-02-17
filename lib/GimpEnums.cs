// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// GimpEnums.cs
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

namespace Gimp
{

  public enum BrushApplicationMode
  {
    HARD,
    SOFT
  }

  public enum ConvertDitherType
  {
    NO,
    FS,
    FSLOWBLEED,
    FIXED
  }

  public enum ConvertPaletteType
  {
    MAKE,
    REUSE,
    WEB,
    MONO,
    CUSTOM
  }

  public enum ConvolutionType
  {
    NORMAL,
    ABSOLUTE,
    NEGATIVE
  }

  public enum ConvolveType
  {
    BLUR,
    SHARPEN
  }

  public enum FillType
  {
    FOREGROUND,
    BACKGROUND,
    WHITE,
    TRANSPARENT,
    PATTERN
  }

  public enum GradientSegmentColor
  {
    RGB,
    HSV_CCW,
    HSV_CW
  }

  public enum GradientSegmentType
  {
    LINEAR,
    CURVED,
    SINE,
    SPHERE_INCREASING,
    SPHERE_DECREASING
  }

  public enum HistogramChannel
  {
    VALUE,
    RED,
    GREEN,
    BLUE,
    ALPHA
  }

  public enum HueRange
  {
      
    ALL,
    RED,
    YELLOW,
    GREEN,
    CYAN,
    BLUE,
    MAGENTA
  }

  public enum LayerModeEffects
  {
    NORMAL,
    DISSOLVE,
    BEHIND,
    MULTIPLY,
    SCREEN,
    OVERLAY,
    DIFFERENCE,
    ADDITION,
    SUBTRACT,
    DARKEN_ONLY,
    LIGHTEN_ONLY,
    HUE,
    SATURATION,
    COLOR,
    VALUE,
    DIVIDE,
    DODGE,
    BURN,
    HARDLIGHT,
    SOFTLIGHT,
    GRAIN_EXTRACT,
    GRAIN_MERGE,
    COLOR_ERASE
  }

  public enum MaskApplyMode
  {
    APPLY,
    DISCARD
  }

  public enum MergeType
  {
    EXPAND_AS_NECESSARY,
    CLIP_TO_IMAGE,
    CLIP_TO_BOTTOM_LAYER,
    FLATTEN_IMAGE
  }

  public enum OffsetType
  {
    BACKGROUND,
    TRANSPARENT
  }

  public enum OrientationType
  {
    HORIZONTAL,
    VERTICAL,
    UNKNOWN
  }

  public enum RotationType
  {
    ROTATE_90,
    ROTATE_180,
    ROTATE_270
  }

  public enum RunMode
  {
    INTERACTIVE,
    NONINTERACTIVE,
    WITH_LAST_VALS
  }

  public enum Transparency
  {
    KEEP_ALPHA,
    SMALL_CHECKS,
    LARGE_CHECKS
  }

  public enum Unit
  {
    PIXEL   = 0,
    INCH    = 1,
    MM      = 2,
    POINT   = 3,
    PICA    = 4,
    END     = 5,

    PERCENT = 65536 
  }
}
