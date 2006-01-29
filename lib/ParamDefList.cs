// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// ParamDefList.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public class ParamDefList
  {
    List<ParamDef> _set;

    public ParamDefList(params ParamDef[] list)
    {
      _set = new List<ParamDef>(list);
    }

    //
    // Used for marshalling
    //

    public void Fill(IntPtr paramPtr, int n_params)
    {
      for (int i = 0; i < n_params; i++)
	{
	  GimpParam param = (GimpParam) 
	    Marshal.PtrToStructure(paramPtr, typeof(GimpParam));

	  switch (param.type)
	    {
	    case PDBArgType.INT32:
	      
	      break;
	    default:
	      break;
	    }

	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(param));
	}
    }

    public void Add(ParamDef p)
    {
      _set.Add(p);
    }

    public ParamDef Lookup(string name)
    {
      foreach (ParamDef p in _set)
	{
	  if (p.Name == name)
	    {
	      return p;
	    }
	}
      return null;
    }

    public object GetValue(string name)
    {
      ParamDef p = Lookup(name);
      return (p == null) ? null : p.Value;
    }
    
    public void SetValue(string name, object value)
    {
      ParamDef p = Lookup(name);
      if (p != null)
	{
	  p.Value = value;
	}
    }

    public GimpParamDef[] GetGimpParamDef(bool uses_image,
					  bool uses_drawable)
    {
      int len = _set.Count;
      GimpParamDef[] args = new GimpParamDef[3 + len];
	
      args[0].type = PDBArgType.INT32;
      args[0].name = "run_mode";
      args[0].description = "Interactive, non-interactive";
	
      args[1].type = PDBArgType.IMAGE;
      args[1].name = "image";
      args[1].description = "Input image" + 
	((uses_image) ?  "" : " (unused)");
	
      args[2].type = PDBArgType.DRAWABLE;
      args[2].name = "drawable";
      args[2].description = "Input drawable" + 
	((uses_drawable) ?  "" : " (unused)");

      int i = 3;
      foreach (ParamDef def in _set)
	{
	  args[i].type = def.GetGimpType();
	  args[i].name = def.Name;
	  args[i].description = def.Description;
	  Console.WriteLine("Add: " + args[i].type);
	  i++;
	}
      
      return args;
    }
  }
}
