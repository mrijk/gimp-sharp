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

namespace Gimp
  {
    public class ParamDef
    {
      string _name;
      string _description;
      object _value;
      Type   _type;

      GimpParamDef _paramDef = new GimpParamDef();

      public ParamDef(string name, object value, Type type, string description)
      {
        _name = name;
	_value = value;
	_type = type;
	_description = description;
      }

      public PDBArgType GetGimpType()
      {
	  if (_type == typeof(int))
	    return PDBArgType.INT32;
	  else
	    return PDBArgType.END;
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

      public GimpParamDef GimpParamDef
      {
	get {return _paramDef;}
      }
    }
  }
