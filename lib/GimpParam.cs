// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// GimpParam.cs
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
  public class GimpParam
  {
    dynamic _param;

    public GimpParam()
    {
      _param = CreateGimpParam();
    }

    public GimpParam(IntPtr ptr)
    {
      Type type = (arch64) ? typeof(GimpParam64) : typeof(GimpParam32);
      _param = Marshal.PtrToStructure(ptr, type);
    }

    public GimpParam(PDBArgType type, object value) : this()
    {
      _param.type = type;

      switch (type)
	{
	case PDBArgType.Int32:
	  _param.data.d_int32 = (Int32) value;
	  break;
	case PDBArgType.Float:
	  _param.data.d_float = (double) value;
	  break;
	case PDBArgType.String:
	  _param.data.d_string = Marshal.StringToHGlobalAuto((string) value);
	  break;
	case PDBArgType.Drawable:
	  _param.data.d_drawable = (value as Drawable).ID;
	  break;
	case PDBArgType.Image:
	  _param.data.d_image = (value as Image).ID;
	  break;
	case PDBArgType.Status:
	  _param.data.d_status = (PDBStatusType) value;
	  break;
	default:
	  Console.WriteLine("GetGimpParam: couldn't create");
	  break;
	}
    }

    static dynamic CreateGimpParam()
    {
      dynamic param;
      if (arch64)
	param = new GimpParam64();
      else 
	param = new GimpParam32();
      return param;
    }

    public static int Size
    {
      get {
	dynamic foo = CreateGimpParam();
	return Marshal.SizeOf(foo);
      }
    }

    public PDBArgType type
    {
      get {return _param.type;}
      set {_param.type = value;}
    }

    public ParamData data
    {
      get {return _param.data;}
      set {_param.data = value;}
    }

    public static GimpParam GetIntParam(Object obj)
    {
      var param = new GimpParam() {type = PDBArgType.Int32};
      if (obj is bool)
	{
	  Int32 val = ((bool) obj) ? 1 : 0;
	  param._param.data.d_int32 = val;
	}
      else
	{
	  param._param.data.d_int32 = (Int32) obj;
	}
      return param;
    }

    public static GimpParam GetFloatParam(Object obj)
    {
      var param = new GimpParam() {type = PDBArgType.Float};
      if (obj is int)
	{
	  param._param.data.d_float = (double) (int) obj;
	}
      else
	{
	  param._param.data.d_float = (double) obj;
	}
      return param;
    }

    public static GimpParam GetStringParam(Object obj)
    {
      var param = new GimpParam() {type = PDBArgType.String};
      param._param.data.d_string = Marshal.StringToHGlobalAuto(obj as string);
      return param;
    }

    public void Fill(IntPtr ptr)
    {
      Marshal.StructureToPtr(_param, ptr, false);
    }

    static bool arch64
    {
      get {return IntPtr.Size == 8;}
    }
  }
}
