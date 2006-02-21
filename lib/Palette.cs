// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// Palette.cs
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
  public sealed class Palette : IEnumerable<PaletteEntry>
  {
    string _name;

    public Palette(string name)
    {
      _name = gimp_palette_new(name);
    }

    internal Palette(string name, bool unused)
    {
      _name = name;
    }

    public Palette(Palette palette)
    {
      _name = gimp_palette_duplicate(palette._name);
    }

    public string Name
    {
      get {return _name;}
    }
 
    IEnumerator IEnumerable.GetEnumerator()
    {
      int numColors = NumberOfColors;
      for (int i = 0; i < numColors; i++)
	{
	  yield return this[i];
	}
    }
 
    IEnumerator<PaletteEntry> IEnumerable<PaletteEntry>.GetEnumerator()
    {
      int numColors = NumberOfColors;
      for (int i = 0; i < numColors; i++)
	{
	  yield return this[i];
	}
    }

    public string Rename(string new_name)
    {
      _name = gimp_palette_rename(_name, new_name);
      return _name;
    }

    public void Delete()
    {
      if (!gimp_palette_delete(_name))
        {
	  throw new Exception();
        }
    }

    public void GetInfo(out int num_colors)
    {
      if (!gimp_palette_get_info(_name, out num_colors))
        {
	  throw new Exception();
        }
    }

    public int NumberOfColors
    {
      get 
	{
	  int num_colors;
	  GetInfo(out num_colors);
	  return num_colors;
	}
    }

    public void AddEntry(string entry_name, RGB color, out int entry_num)
    {
      GimpRGB rgb = color.GimpRGB;
      if (!gimp_palette_add_entry(_name, entry_name, ref rgb,
                                  out entry_num))
        {
	  throw new Exception();
        }
    }

    public void AddEntry(string entry_name, RGB color)
    {
      int entry_num;
      AddEntry(entry_name, color, out entry_num);
    }

    public void DeleteEntry(int entry_num)
    {
      if (!gimp_palette_delete_entry(_name, entry_num))
        {
	  throw new Exception();
        }
    }

    public PaletteEntry this[int index]
    {
      get {return new PaletteEntry(this, index);}
    }

    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_palette_new(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_palette_duplicate(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern string gimp_palette_rename(string name,
                                             string new_name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_delete(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_get_info(string name,
                                             out int num_colors);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_add_entry(string name,
                                              string entry_name,
                                              ref GimpRGB color,
                                              out int entry_num);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_delete_entry(string name, int entry_num);
  }
}
