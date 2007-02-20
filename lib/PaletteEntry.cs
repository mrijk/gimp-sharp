// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2007 Maurits Rijk
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
    readonly int _index;

    internal PaletteEntry(Palette palette, int index)
    {
      _paletteName = palette.Name;
      _index = index;
    }

    public string Name
    {
      get
	{
	  string entry_name;
	  if (!gimp_palette_entry_get_name(_paletteName, _index, 
					   out entry_name))
	    {
	      throw new Exception();
	    }
	  return entry_name;
	}
      set
	{
	  if (!gimp_palette_entry_set_name(_paletteName, _index, value))
	    {
	      throw new Exception();
	    }
	}
    }

    public RGB Color
    {
      get
	{
	  GimpRGB rgb = new GimpRGB();
	  if (!gimp_palette_entry_get_color(_paletteName, _index, out rgb))
	    {
	      throw new Exception();
	    }
	  return new RGB(rgb);
	}
      set
	{
	  if (!gimp_palette_entry_set_color(_paletteName, _index,
					    value.GimpRGB))
	    {
	      throw new Exception();
	    }
	}
    }

    internal int Index
    {
      get {return _index;}
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
