// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2008 Maurits Rijk, Massimo Perga
//
// GimpTypes.cs
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
  [StructLayout(LayoutKind.Explicit)]
  public struct ParamData
  {
    [FieldOffset(0)]
    public Int32	d_int32;
    [FieldOffset(0)]
    public Int16	d_int16;
    [FieldOffset(0)]
    public byte		d_int8;
    [FieldOffset(0)]
    public double 	d_float;
    [FieldOffset(0)]
    public IntPtr	d_string;
    [FieldOffset(0)]
    internal GimpRGB  	d_color;
    [FieldOffset(0)]
    internal ParamRegion d_region;
    [FieldOffset(0)]
    public Int32    	d_image;
    [FieldOffset(0)]
    public Int32    	d_drawable;
    [FieldOffset(0)]
    internal GimpParasite    	d_parasite;
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

  [StructLayout(LayoutKind.Sequential, Pack=1)]
  public struct GimpParam
  {
    public PDBArgType type;
    public ParamData data;
  }
  
  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpParamCust
  {
    public IntPtr	cust; 
    public ParamData	data;
  }
  
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpRGB
  {
    public double r, g, b, a;
  }

  [StructLayout(LayoutKind.Sequential)]
  internal struct ParamRegion
  {
    public Int32 x;
    public Int32 y;
    public Int32 width;
    public Int32 height;
  };

  [StructLayout(LayoutKind.Sequential)]
  internal struct GimpParasite
  {
    // string  name;  
    public IntPtr  name;
    public Int32   flags; 
    public Int32   size;  
    public IntPtr  data;  
  };
}
