using System;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Palette
    {
      string _name;

      [DllImport("libgimp-2.0.so")]
      static extern string gimp_palette_new(string name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_delete(string name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_add_entry(string name,
						string entry_name,
						ref GimpRGB color,
						out int entry_num);


      public Palette(string name)
      {
	_name = gimp_palette_new(name);
      }

      public bool Delete()
      {
	return gimp_palette_delete(_name);
      }

      public bool AddEntry(string entry_name, RGB color, out int entry_num)
      {
	GimpRGB rgb = color.GimpRGB;
	return gimp_palette_add_entry(_name, entry_name, ref rgb,
				      out entry_num);
      }

      public bool AddEntry(string entry_name, RGB color)
      {
	int entry_num;
	return AddEntry(entry_name, color, out entry_num);
      }
    }
  }
