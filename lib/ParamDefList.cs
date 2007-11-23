// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk, Massimo Perga
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

using GLib;

namespace Gimp
{
  public class ParamDefList
  {
    readonly List<ParamDef> _set;

    public ParamDefList(bool usesImage, bool usesDrawable)
    {
      _set = new List<ParamDef>()
	{
	  new ParamDef("run_mode", typeof(Int32),
		       "Interactive, non-interactive"),
	  new ParamDef("image", typeof(Image), "Input image"),
	  new ParamDef("drawable", typeof(Drawable), "Input drawable")
	};
    }

    public ParamDefList() : this(true, true)
    {
    }

    public ParamDefList(bool foo)
    {
      _set = new List<ParamDef>();
    }

    public ParamDefList(params ParamDef[] list)
    {
      _set = new List<ParamDef>(list);
    }

    IEnumerator<ParamDef> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public ParamDef this[int index]
    {
      get {return _set[index];}
    }

    public void Marshall(IntPtr paramPtr, int n_params)
    {
      for (int i = 0; i < n_params; i++)
	{
	  GimpParamCust paramCust = (GimpParamCust) 
	    Marshal.PtrToStructure(paramPtr, typeof(GimpParamCust));
		
	  GimpParam param = new GimpParam();
	  param.type = (PDBArgType) paramCust.cust.ToInt32();
	  param.data = paramCust.data;
	  
	  Type type = this[i].Type;

	  switch (param.type)
	    {
	    case PDBArgType.Int32:
	      if (type == typeof(int) || type == typeof(uint))
		this[i].Value = (Int32) param.data.d_int32;
	      else if (type == typeof(bool))
		this[i].Value = ((Int32) param.data.d_int32 == 0) 
		  ? false 
		  : true;
	      break;
	    case PDBArgType.Float:
	      this[i].Value = param.data.d_float;
	      break;
	    case PDBArgType.Color:
	      this[i].Value = new RGB(param.data.d_color);
	      break;
	    case PDBArgType.Image:
	      this[i].Value = new Image((Int32) param.data.d_image);
	      break;
	    case PDBArgType.String:
	      if (type == typeof(string))
		this[i].Value = Marshal.PtrToStringAuto(param.data.d_string);
	      else
		this[i].Value = Marshaller.FilenamePtrToString
		  (param.data.d_string);
	      break;
	    case PDBArgType.Drawable:
	      this[i].Value = new Drawable((Int32) param.data.d_drawable);
	      break;
	    default:
	      Console.WriteLine("Fill: parameter " + param.type + 
				" not supported yet!");
	      break;
	    }

	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(paramCust));
	}
    }

    public void Marshall(out IntPtr return_vals, out int n_return_vals)
    {
      GimpParam foo = new GimpParam();
      GimpParamCust fooCust = new GimpParamCust();

      n_return_vals = _set.Count;

      return_vals = Marshal.AllocCoTaskMem(n_return_vals * 
					   Marshal.SizeOf(fooCust));

      IntPtr paramPtr = return_vals;

      for (int i = 0; i < n_return_vals; i++)
	{
	  foo = this[i].GetGimpParam();
	  fooCust = new GimpParamCust();
	  fooCust.cust = (IntPtr)foo.type;
	  fooCust.data = foo.data;
	  Marshal.StructureToPtr(fooCust, paramPtr, false);
	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(fooCust));
	}
    }

    public void Add(ParamDef p)
    {
      _set.Add(p);
    }

    public ParamDef Lookup(string name)
    {
      return _set.Find(p => p.Name == name);
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

    public GimpParamDef[] GetGimpParamDef(bool usesImage, bool usesDrawable)
    {
      GimpParamDef[] args = new GimpParamDef[_set.Count];
      int i = 0;

      foreach (ParamDef def in _set)
	{
	  args[i].type = def.GetGimpType();
	  args[i].name = def.Name;
	  args[i].description = def.Description;
	  i++;
	}
      
      return args;
    }
  }
}
