// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
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
      ADD_WHITE_MASK,
      ADD_BLACK_MASK,
      ADD_ALPHA_MASK,
      ADD_ALPHA_TRANSFER_MASK,
      ADD_SELECTION_MASK,
      ADD_COPY_MASK
    }

  public enum BlendMode
    {
      FG_BG_RGB_MODE,
      FG_BG_HSV_MODE,
      FG_TRANSPARENT_MODE,
      CUSTOM_MODE
    }

  public enum BucketFillMode
    {
      FG_BUCKET_FILL,
      BG_BUCKET_FILL,
      PATTERN_BUCKET_FILL
    }

  public enum ChannelOps
    {
      ADD,
      SUBTRACT,
      REPLACE,
      INTERSECT
    }

  public enum ChannelType
    {
      RED,
      GREEN,
      BLUE,
      GRAY,
      INDEXED,
      ALPHA
    }

  public enum CheckSize
    {
      SMALL_CHECKS,
      MEDIUM_CHECKS,
      LARGE_CHECKS
    }

  public enum CheckType
    {
      LIGHT_CHECKS ,
      GRAY_CHECKS ,
      DARK_CHECKS ,
      WHITE_ONLY ,
      GRAY_ONLY ,
      BLACK_ONLY
    }

  public enum DesaturateMode
    {
      LIGHTNESS,
      LUMINOSITY,
      AVERAGE 
    }

  public enum DodgeBurnType
    {
      DODGE,
      BURN
    }

  public enum GradientType
    {
      LINEAR,
      BILINEAR,
      RADIAL,
      SQUARE,
      CONICAL_SYMMETRIC,
      CONICAL_ASYMMETRIC,
      SHAPEBURST_ANGULAR,
      SHAPEBURST_SPHERICAL,
      SHAPEBURST_DIMPLED,
      SPIRAL_CLOCKWISE,
      SPIRAL_ANTICLOCKWISE
    }

  public enum IconType
    {
      STOCK_ID,
      INLINE_PIXBUF,
      IMAGE_FILE
    }

  public enum ImageBaseType
    {
      RGB,
      GRAY,
      INDEXED
    }

  public enum ImageType
    {
      RGB,
      RGBA,
      GRAY,
      GRAYA,
      INDEXED,
      INDEXEDA
    }
  
  public enum InterpolationType
    {
      NONE,
      LINEAR,
      CUBIC,
      LANCZOS 
    }

  public enum MessageHandlerType
    {
      MESSAGE_BOX,
      CONSOLE,
      ERROR_CONSOLE
    }

  public enum PaintApplicationMode
    {
      CONSTANT,
      INCREMENTAL
    }

  public enum PDBArgType
    {
      INT32,
      INT16,
      INT8,
      FLOAT,
      STRING,
      INT32ARRAY,
      INT16ARRAY,
      INT8ARRAY,
      FLOATARRAY,
      STRINGARRAY,
      COLOR,
      REGION,
      DISPLAY,
      IMAGE,
      LAYER,
      CHANNEL,
      DRAWABLE,
      SELECTION,
      BOUNDARY,
      PATH,
      PARASITE,
      STATUS,
      END
    }

  public enum PDBProcType
    {
      INTERNAL,
      PLUGIN,
      EXTENSION,
      TEMPORARY
    }
 
  public enum PDBStatusType
    {
      EXECUTION_ERROR,
      CALLING_ERROR,
      PASS_THROUGH,
      SUCCESS,
      CANCEL
    }

  public enum ProgressCommand
    {
      START,
      END,
      SET_TEXT,
      SET_VALUE,
      PULSE
    }

  public enum RepeatMode
    {
      NONE,
      SAWTOOTH,
      TRIANGULAR
    }

  public enum SizeType
    {
      PIXELS,
      POINTS
    }
 
  public enum StackTraceMode
    {
      NEVER,
      QUERY,
      ALWAYS
    }

  public enum TransferMode
    {
      SHADOWS,
      MIDTONES,
      HIGHLIGHTS
    }

  public enum TransformDirection
    {
      FORWARD,
      BACKWARD
    }
  }
