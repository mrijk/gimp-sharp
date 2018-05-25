// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
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

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Gimp
{
  public sealed class Palette : DataObject
  {
    public Palette(string name) : base(gimp_palette_new(name))
    {
    }

    internal Palette(string name, bool unused) : base(name)
    {
    }

    public Palette(Palette palette) : 
      base(gimp_palette_duplicate(palette.Name))
    {
    }

    public IEnumerator<PaletteEntry> GetEnumerator()
    {
      int numColors = NumberOfColors;
      for (int i = 0; i < numColors; i++)
	{
	  yield return this[i];
	}
    }

    public override bool Equals(object o)
    {
      if (o is Palette)
	{
	  return (o as Palette).Name == Name;
	}
      return false;
    }

    public override int GetHashCode() => Name.GetHashCode();

    protected override string TryRename(string newName) => 
      gimp_palette_rename(Name, newName);

    public void Delete()
    {
      if (!gimp_palette_delete(Name))
        {
	  throw new GimpSharpException();
        }
    }

    public void GetInfo(out int numColors)
    {
      if (!gimp_palette_get_info(Name, out numColors))
        {
	  throw new GimpSharpException();
        }
    }

    public int Columns
    {
      get => gimp_palette_get_columns(Name);
      set
	{
	  if (!gimp_palette_set_columns(Name, value))
	    {
	      throw new GimpSharpException();
	    }
	}
    }
    
    public int NumberOfColors
    {
      get 
	{
	  GetInfo(out int num_colors);
	  return num_colors;
	}
    }

    public PaletteEntry AddEntry(string entryName, RGB color)
    {
      GimpRGB rgb = color.GimpRGB;
      int entryNum;
      if (!gimp_palette_add_entry(Name, entryName, ref rgb,
                                  out entryNum))
        {
	  throw new GimpSharpException();
        }
      return new PaletteEntry(this, entryNum);
    }

    public void DeleteEntry(PaletteEntry entry)
    {
      if (!gimp_palette_delete_entry(Name, entry.Index))
        {
	  throw new GimpSharpException();
        }
    }

    public bool IsEditable => gimp_palette_is_editable(Name);

    public PaletteEntry this[int index] => new PaletteEntry(this, index);

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
    static extern int gimp_palette_get_columns(string name);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_set_columns(string name, int columns);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_add_entry(string name,
                                              string entry_name,
                                              ref GimpRGB color,
                                              out int entry_num);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_delete_entry(string name, int entry_num);
    [DllImport("libgimp-2.0-0.dll")]
    static extern bool gimp_palette_is_editable(string name);
  }
}
