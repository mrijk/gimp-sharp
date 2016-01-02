// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2016 Maurits Rijk
//
// GimpBaseEnums.cs
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
  public enum AddMaskType
  {
    White,
    Black,
    Alpha,
    AlphaTransfer,
    Selection,
    Copy
  }

  public enum BlendMode
  {
    FgBgRgb,
    FgBgHsv,
    FgTransparent,
    Custom
  }

  public enum BucketFillMode
  {
    Foreground,
    Background,
    Pattern,
  }

  public enum ChannelOps
  {
    Add,
    Subtract,
    Replace,
    Intersect
  }

  public enum ChannelType
  {
    Red,
    Green,
    Blue,
    Gray,
    Indexed,
    Alpha
  }

  public enum CheckSize
  {
    Small,
    Medium,
    Large
  }

  public enum CheckType
  {
    Light,
    Gray,
    Dark,
    WhiteOnly,
    GrayOnly,
    BlackOnly
  }

  public enum DesaturateMode
  {
    Lightness,
    Luminosity,
    Average 
  }

  public enum DodgeBurnType
  {
    Dodge,
    Burn
  }

  public enum ForegroundExtractMode
  {
    ExtractSiox
  }

  public enum GradientType
  {
    Linear,
    Bilinear,
    Radial,
    Square,
    ConicalSymmetric,
    ConicalAsymmetric,
    ShapeburstAngular,
    ShapeburstSpherical,
    ShapeburstDimpled,
    SpiralClockwise,
    SpiralAnticlockwise
  }

  public enum GridStyle
  {
    Dots,
    Intersections,
    OnOffDash,
    DoubleDash,
    Solid
  }

  public enum IconType
  {
    StockId,
    InlinePixbuf,
    ImageFile
  }

  public enum ImageBaseType
  {
    Rgb,
    Gray,
    Indexed
  }

  public enum ImageType
  {
    Rgb,
    Rgba,
    Gray,
    Graya,
    Indexed,
    Indexeda
  }
  
  public enum InterpolationType
  {
    None,
    Linear,
    Cubic,
    Lanczos 
  }

  public enum MessageHandlerType
  {
    MessageBox,
    Console,
    ErrorConsole
  }

  public enum PaintApplicationMode
  {
    Constant,
    Incremental
  }

  public enum PDBArgType
  {
    Int32,
    Int16,
    Int8,
    Float,
    String,
    Int32array,
    Int16array,
    Int8array,
    Floatarray,
    Stringarray,
    Color,
    Region,
    Display,
    Image,
    Layer,
    Channel,
    Drawable,
    Selection,
    ColorArray,
    Vectors,
    Parasite,
    Status,
    End,

    Path = Vectors, // deprecated
    Boundary = ColorArray // deprecated
  }

  public enum PDBProcType
  {
    Internal,
    Plugin,
    Extension,
    Temporary
  }
 
  public enum PDBStatusType
  {
    ExecutionError,
    CallingError,
    PassThrough,
    Success,
    Cancel
  }

  public enum ProgressCommand
  {
    Start,
    End,
    SetText,
    SetValue,
    Pulse
  }

  public enum RepeatMode
  {
    None,
    Sawtooth,
    Triangular
  }

  public enum SizeType
  {
    Pixels,
    Points
  }
 
  public enum StackTraceMode
  {
    Never,
    Query,
    Always
  }

  public enum TextDirection
  {
    Ltr,
    Rtl
  }

  public enum TextHintStyle
  {
    None,
    Slight,
    Medium,
    Full
  }

  public enum TextJustification
  {
    Left,
    Right,
    Center,
    Fill
  }

  public enum TransferMode
  {
    Shadows,
    Midtones,
    Highlights
  }

  public enum TransformDirection
  {
    Forward,
    Backward
  }

  public enum TransformResize
  {
    Adjust = 0,
    Clip = 1,
    Crop,                
    CropWithAspect 
  }

  public enum UserDirectory
  {
    Desktop,
    Documents,
    Download,
    Music,
    Pictures,
    PublicShare,
    Templates,
    Videos
  }

  public enum VectorsStrokeType
  {
    Bezier
  }
}
