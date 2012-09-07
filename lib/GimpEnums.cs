// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
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
    Hard,
    Soft
  }

  public enum BrushGeneratedShape
  {
    Circle,
    Square,
    Diamond
  }

  public enum ConvertDitherType
  {
    No,
    Fs,
    FslowBleed,
    Fixed
  }

  public enum ConvertPaletteType
  {
    Make,
    Reuse,
    Web,
    Mono,
    Custom
  }

  public enum ConvolutionType
  {
    Normal,
    Absolute,
    Negative
  }

  public enum ConvolveType
  {
    Blur,
    Sharpen
  }

  public enum FillType
  {
    Foreground,
    Background,
    White,
    Transparent,
    Pattern
  }

  public enum GradientSegmentColor
  {
    Rgb,
    HsvCcw,
    HsvCw
  }

  public enum GradientSegmentType
  {
    Linear,
    Curved,
    Sine,
    SphereIncreasing,
    SphereDecreasing
  }

  public enum HistogramChannel
  {
    Value,
    Red,
    Green,
    Blue,
    Alpha
  }

  public enum HueRange
  {
    All,
    Red,
    Yellow,
    Green,
    Cyan,
    Blue,
    Magenta
  }

  public enum InkBlobType
  {
    Circle,
    Square,
    Diamond
  } 

  public enum LayerModeEffects
  {
    Normal,
    Dissolve,
    Behind,
    Multiply,
    Screen,
    Overlay,
    Difference,
    Addition,
    Subtract,
    DarkenOnly,
    LightenOnly,
    Hue,
    Saturation,
    Color,
    Value,
    Divide,
    Dodge,
    Burn,
    Hardlight,
    Softlight,
    GrainExtract,
    GrainMerge,
    ColorErase
  }

  public enum MaskApplyMode
  {
    Apply,
    Discard
  }

  public enum MergeType
  {
    ExpandAsNecessary,
    ClipToImage,
    ClipToBottomLayer,
    FlattenImage
  }

  public enum OffsetType
  {
    Background,
    Transparent
  }

  public enum OrientationType
  {
    Horizontal,
    Vertical,
    Unknown
  }

  public enum RotationType
  {
    Rotate90,
    Rotate180,
    Rotate270
  }

  public enum SelectCriterion
  {
    Composite,
    R,
    G,
    B,
    H,
    S,
    V
  }

  public enum RunMode
  {
    Interactive,
    Noninteractive,
    WithLastVals
  }

  public enum Transparency
  {
    KeepAlpha,
    SmallChecks,
    LargeChecks
  }

  public enum Unit
  {
    Pixel   = 0,
    Inch    = 1,
    Mm      = 2,
    Point   = 3,
    Pica    = 4,
    End     = 5,

    Percent = 65536 
  }
}
