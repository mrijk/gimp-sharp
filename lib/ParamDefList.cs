// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
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

    public int Count => _set.Count;

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

    public ParamDefList(params IVariable[] variables) : this()
    {
      Array.ForEach(variables, v => Add(new ParamDef(v.Identifier, v.Type, 
						     v.Description)));
    }

    public ParamDefList(VariableSet variables) : this()
    {
      variables.ForEach(v => Add(new ParamDef(v.Identifier, v.Type, v.Description)));
    }

    public IEnumerator<ParamDef> GetEnumerator() => _set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public ParamDef this[int index] => _set[index];

    public void Marshall(IntPtr paramPtr, int n_params)
    {
      var parameters = new GimpParamSet(paramPtr, n_params);
      int i = 0;
      foreach (var param in parameters) 
	{
	  FillValue(i++, param);
	}
    }

    void FillValue(int i, GimpParam param)
    {
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
    }

    public void Marshall(out IntPtr return_vals, out int n_return_vals)
    {
      n_return_vals = _set.Count;
      int size = GimpParam.Size;

      return_vals = Marshal.AllocCoTaskMem(n_return_vals * size);

      var seq = new IntPtrSeq(return_vals, n_return_vals, size);
      seq.ForEach((i, ptr) => this[i].GetGimpParam().Fill(ptr));
    }

    public void Add(ParamDef p) => _set.Add(p);

    public ParamDef Lookup(string name) => _set.Find(p => p.Name == name);

    public object GetValue(string name) => Lookup(name)?.Value;
    
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
      return (Count == 0) 
	? null 
	: _set.Select(def => def.GimpParamDef).ToArray();
    }
  }
}
