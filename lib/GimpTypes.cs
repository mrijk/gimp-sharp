using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
  [StructLayout(LayoutKind.Explicit)]
    public struct ParamData
    {
      [FieldOffset(0)]
      public Int32	d_int32;
      [FieldOffset(0)]
      public Int16	d_int16;
      [FieldOffset(0)]
      public byte	d_int8;
      [FieldOffset(0)]
      public double 	d_float;
      [FieldOffset(0)]
      public IntPtr	d_string;
      [FieldOffset(0)]
      public GimpRGB  	d_color;
#if _FIXME_
      [FieldOffset(0)]
      public ParamRegion d_region;
#endif
      [FieldOffset(0)]
      public Int32    	d_image;
      [FieldOffset(0)]
      public Int32    	d_drawable;
#if _FIXME_
      [FieldOffset(0)]
      GimpParasite    	d_parasite;
#endif
      [FieldOffset(0)]
      public PDBStatusType	d_status;
    };

  [StructLayout(LayoutKind.Sequential)]
    public struct GimpParamDef
    {
      public PDBArgType type;
      public string name;
      public string description;
    };
  

  [StructLayout(LayoutKind.Sequential)]
    public struct GimpParam
    {
      public PDBArgType type;
      public ParamData  data;
    };
  
  [StructLayout(LayoutKind.Sequential)]
    public struct GimpRGB
    {
      public double r, g, b, a;
    }

  [StructLayout(LayoutKind.Sequential)]
    public struct ParamRegion
    {
      public Int32 x;
      public Int32 y;
      public Int32 width;
      public Int32 height;
    };

  [StructLayout(LayoutKind.Sequential)]
    public struct GimpParasite
    {
      string  name;  
      Int32   flags; 
      Int32   size;  
      IntPtr  data;  
    };
  }
