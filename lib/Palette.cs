using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class Palette : IEnumerable
    {
      string _name;

      public Palette(string name)
      {
	_name = gimp_palette_new(name);
      }

      public Palette(Palette palette)
      {
	_name = gimp_palette_duplicate(palette._name);
      }

      public string Name
      {
	get {return _name;}
      }

      public virtual IEnumerator GetEnumerator()
      {
	return new PaletteEnumerator(this);
      }

      public string Rename(string new_name)
      {
	return gimp_palette_rename(_name, new_name);
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

      [DllImport("libgimp-2.0.so")]
      static extern string gimp_palette_new(string name);
      [DllImport("libgimp-2.0.so")]
      static extern string gimp_palette_duplicate(string name);
      [DllImport("libgimp-2.0.so")]
      static extern string gimp_palette_rename(string name,
					       string new_name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_delete(string name);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_get_info(string name,
					       out int num_colors);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_add_entry(string name,
						string entry_name,
						ref GimpRGB color,
						out int entry_num);
      [DllImport("libgimp-2.0.so")]
      static extern bool gimp_palette_delete_entry(string name, int entry_num);
    }
  }
