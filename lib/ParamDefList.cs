// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
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
using System.Linq;

using GLib;

namespace Gimp
{
  public class ParamDefList : IEnumerable<ParamDef>
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

    public IEnumerator<ParamDef> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public ParamDef this[int index]
    {
      get {return _set[index];}
    }

    public void Marshall(IntPtr paramPtr, int n_params)
    {
Console.WriteLine("Parameters: " + n_params);
      for (int i = 0; i < n_params; i++)
	{
#if false
	  GimpParamCust paramCust = (GimpParamCust) 
	    Marshal.PtrToStructure(paramPtr, typeof(GimpParamCust));
		
	  GimpParam param = new GimpParam();
	  param.type = (PDBArgType) paramCust.cust.ToInt32();
	  param.data = paramCust.data;
#else
	  GimpParam param = (GimpParam) 
	    Marshal.PtrToStructure(paramPtr, typeof(GimpParam));
#endif 
          Console.WriteLine("param.Size: " + Marshal.SizeOf(param));
          Console.WriteLine("param.type.Size: " + sizeof(PDBArgType));
	  ParamData foo = param.data;
          Console.WriteLine("param.type.Data: " + Marshal.SizeOf(foo));
          
          Console.WriteLine("{0} {1}", i, param.type);

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

//	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(paramCust));
	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(param));
	}
    }

    public void Marshall(out IntPtr return_vals, out int n_return_vals)
    {
      GimpParam foo = new GimpParam();
#if false
      GimpParamCust fooCust = new GimpParamCust();
#endif
      n_return_vals = _set.Count;

#if false
      return_vals = Marshal.AllocCoTaskMem(n_return_vals * 
					   Marshal.SizeOf(fooCust));
#else
      return_vals = Marshal.AllocCoTaskMem(n_return_vals * 
					   Marshal.SizeOf(foo));
#endif

      IntPtr paramPtr = return_vals;

      for (int i = 0; i < n_return_vals; i++)
	{
	  foo = this[i].GetGimpParam();
#if false
	  fooCust = new GimpParamCust();
	  fooCust.cust = (IntPtr)foo.type;
	  fooCust.data = foo.data;
	  Marshal.StructureToPtr(fooCust, paramPtr, false);
	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(fooCust));
#else
	  Marshal.StructureToPtr(foo, paramPtr, false);
	  paramPtr = (IntPtr)((int)paramPtr + Marshal.SizeOf(foo));
#endif
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
      var p = Lookup(name);
      return (p == null) ? null : p.Value;
    }
    
    public void SetValue(string name, object value)
    {
      var p = Lookup(name);
      if (p != null)
	{
	  p.Value = value;
	}
    }

    public GimpParamDef[] GetGimpParamDef()
    {
      return _set.Select(def => def.GimpParamDef).ToArray();
    }
  }
}
