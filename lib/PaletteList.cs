using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gimp
  {
    public class PaletteList : IEnumerable
    {
      ArrayList _list = new ArrayList();

      public PaletteList(string filter)
      {
	int num_palettes;
	IntPtr names = gimp_palettes_get_list(filter, out num_palettes);

	for (int i = 0; i < num_palettes; i++)
	  {
	  // _list.Add(new Palette(name, false));
	  }
      }

      public PaletteList()
      {
      }

      public virtual IEnumerator GetEnumerator()
      {
	return _list.GetEnumerator();
      }

      static public void Refresh()
      {
	gimp_palettes_refresh();
      }

      [DllImport("libgimp-2.0.so")]
      static extern void gimp_palettes_refresh();
      [DllImport("libgimp-2.0.so")]
      static extern IntPtr gimp_palettes_get_list (string filter,
						   out int num_palettes);
    }
  }
