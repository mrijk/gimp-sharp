// The Fragment plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Fragment.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using Mono.Unix;

namespace Gimp.Fragment
{
  public class Fragment : Plugin
  {
    static void Main(string[] args)
    {
      string localeDir = Gimp.LocaleDirectory;
      Catalog.Init("Fragment", localeDir);
      new Fragment(args);
    }

    public Fragment(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      Procedure procedure = new Procedure("plug_in_fragment",
					  Catalog.GetString("Fragments the picture"),
					  Catalog.GetString("Creates four copies of the pixels in the selection, averages them, and offsets them from each other."),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  Catalog.GetString("Fragment"),
					  "RGB*, GRAY*");
      procedure.MenuPath = "<Image>/Filters/Distorts";

      set.Add(procedure);

      return set;
    }

    override protected void Render(Drawable drawable)
    {
      int bpp = drawable.Bpp;

      Tile.CacheNtiles((ulong) (2 * (drawable.Width / Gimp.TileWidth + 1))); 

      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(Catalog.GetString("Fragment"));

      PixelFetcher pf = new PixelFetcher(drawable, false);
      pf.EdgeMode = EdgeMode.Black;

      byte[] ul = new byte[bpp];
      byte[] ur = new byte[bpp];
      byte[] ll = new byte[bpp];
      byte[] lr = new byte[bpp];
      byte[] average = new byte[bpp];

      iter.IterateDest(delegate (int x, int y) 
      {
      	pf.GetPixel(x - 4, y - 4, ul);
      	pf.GetPixel(x + 4, y - 4, ur);
      	pf.GetPixel(x - 4, y + 4, ll);
      	pf.GetPixel(x + 4, y + 4, lr);
      	
      	for (int b = 0; b < bpp; b++)
      	{
	  average[b] = (byte) ((ul[b] + ur[b] + ll[b] + lr[b]) / 4);
      	}
      	return average;
      });

      pf.Dispose();

      Display.DisplaysFlush();
    }
  }
}
