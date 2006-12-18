// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// ParamDef.cs
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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class ParamDef
  {
    readonly string _name;
    readonly string _description;
    readonly Type   _type;
    object _value;

    GimpParamDef _paramDef = new GimpParamDef();

    public ParamDef(string name, object value, Type type, string description)
    {
      _name = name;
      _value = value;
      _type = type;
      _description = description;
    }

    public ParamDef(string name, Type type, string description) : 
      this(name, null, type, description)
    {
    }

    public ParamDef(object value, Type type) :
      this(null, value, type, null)
    {
    }

    public PDBArgType GetGimpType()
    {
      if (_type == typeof(int))
	return PDBArgType.Int32;
      else if (_type == typeof(uint))
	return PDBArgType.Int32;
      else if (_type == typeof(double))
	return PDBArgType.Float;
      else if (_type == typeof(bool))
	return PDBArgType.Int32;
      else if (_type == typeof(string))
	return PDBArgType.String;
      else if (_type == typeof(RGB))
	return PDBArgType.Color;
      else if (_type == typeof(Drawable))
	return PDBArgType.Drawable;
      else if (_type == typeof(Image))
	return PDBArgType.Image;
      else if (_type == typeof(PDBStatusType))
	return PDBArgType.Status;
      else if (_type == typeof(FileName))
	return PDBArgType.String;
      else
	return PDBArgType.End;
    }

    // Can this be done by a casting overload?
    internal GimpParam GetGimpParam()
    {
      GimpParam param = new GimpParam();

      param.type = GetGimpType();

      switch (param.type)
	{
	case PDBArgType.Int32:
	  param.data.d_int32 = (Int32) _value;
	  break;
	case PDBArgType.Float:
	  param.data.d_float = (double) _value;
	  break;
	case PDBArgType.String:
	  param.data.d_string = Marshal.StringToHGlobalAuto((string) _value);
	  break;
	case PDBArgType.Image:
	  param.data.d_image = ((Image) _value).ID;
	  break;
	case PDBArgType.Status:
	  param.data.d_status = (PDBStatusType) _value;
	  break;
	default:
	  Console.WriteLine("GetGimpParam: couldn't create");
	  break;
	}

      return param;
    }

    public string Name
    {
      get {return _name;}
    }

    public object Value
    {
      get {return _value;}
      set {_value = value;}
    }

    public string Description
    {
      get {return _description;}
    }

    public Type Type
    {
      get {return _type;}
    }

    public GimpParamDef GimpParamDef
    {
      get {return _paramDef;}
    }
  }

  internal class FileName
  {
  }
}
