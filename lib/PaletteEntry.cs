using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class PaletteEntry
    {
      string _paletteName;
      int _index;

      public PaletteEntry(Palette palette, int index)
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

      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_entry_get_name(string name, 
						     int entry_num,
						     out string entry_name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_entry_set_name(string name, 
						     int entry_num,
						     string entry_name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_entry_get_color(string name, 
						     int entry_num,
						     out GimpRGB color);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_entry_set_color(string name, 
						     int entry_num,
						     GimpRGB color);
    }
  }
