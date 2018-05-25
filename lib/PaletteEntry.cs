// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// PaletteEntry.cs
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
  public sealed class PaletteEntry
  {
    readonly string _paletteName;
    internal int Index {get; set;}

    internal PaletteEntry(Palette palette, int index)
    {
      _paletteName = palette.Name;
      Index = index;
    }

    public string Name
    {
      get
	{
	  if (!gimp_palette_entry_get_name(_paletteName, Index, 
					   out var entry_name))
	    {
	      throw new GimpSharpException();
	    }
	  return entry_name;
	}
      set
	{
	  if (!gimp_palette_entry_set_name(_paletteName, Index, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    public RGB Color
    {
      get
	{
	  if (!gimp_palette_entry_get_color(_paletteName, Index, out var rgb))
	    {
	      throw new GimpSharpException();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  if (!gimp_palette_entry_set_color(_paletteName, Index,
					    value.GimpRGB))
	    {
	      throw new GimpSharpException();
	    }
	}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_entry_get_name(string name, 
						   int entry_num,
						   out string entry_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_entry_set_name(string name, 
						   int entry_num,
						   string entry_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_entry_get_color(string name, 
						    int entry_num,
						    out GimpRGB color);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_entry_set_color(string name, 
						    int entry_num,
						    GimpRGB color);
  }
}
