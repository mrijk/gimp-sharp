// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
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
    public string Name {get; private set;}
    public string Description {get; private set;}
    public Type Type {get; private set;}
    public object Value {get; set;}

    public ParamDef(string name, object value, Type type, 
		    string description)
    {
      Name = name;
      Value = value;
      Type = type;
      Description = description;
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
      if (Type == typeof(int))
	return PDBArgType.Int32;
      else if (Type == typeof(uint))
	return PDBArgType.Int32;
      else if (Type == typeof(double))
	return PDBArgType.Float;
      else if (Type == typeof(bool))
	return PDBArgType.Int32;
      else if (Type == typeof(string))
	return PDBArgType.String;
      else if (Type == typeof(RGB))
	return PDBArgType.Color;
      else if (Type == typeof(Drawable))
	return PDBArgType.Drawable;
      else if (Type == typeof(Image))
	return PDBArgType.Image;
      else if (Type == typeof(PDBStatusType))
	return PDBArgType.Status;
      else if (Type == typeof(FileName))
	return PDBArgType.String;
      else
	return PDBArgType.End;
    }

    // Can this be done by a casting overload?
    internal GimpParam GetGimpParam()
    {
      var param = new GimpParam();

      param.type = GetGimpType();

      switch (param.type)
	{
	case PDBArgType.Int32:
	  param.data.d_int32 = (Int32) Value;
	  break;
	case PDBArgType.Float:
	  param.data.d_float = (double) Value;
	  break;
	case PDBArgType.String:
	  param.data.d_string = Marshal.StringToHGlobalAuto((string) Value);
	  break;
	case PDBArgType.Image:
	  param.data.d_image = (Value as Image).ID;
	  break;
	case PDBArgType.Status:
	  param.data.d_status = (PDBStatusType) Value;
	  break;
	default:
	  Console.WriteLine("GetGimpParam: couldn't create");
	  break;
	}

      return param;
    }

    public GimpParamDef GimpParamDef
    {
      get 
	{
	  return new GimpParamDef() {
	    type = GetGimpType(),
	      name = Name,
	      description = Description};
	}
    }
  }

  internal class FileName
  {
  }
}
