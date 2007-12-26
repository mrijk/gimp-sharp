// The Fragment plug-in
// Copyright (C) 2004-2007 Maurits Rijk
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
using System.Collections.Generic;

namespace Gimp.Fragment
{
  class Fragment : Plugin
  {
    static void Main(string[] args)
    {
      new Fragment(args);
    }

    Fragment(string[] args) : base(args, "Fragment")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_fragment",
				 _("Fragments the picture"),
				 _("Creates four copies of the pixels in the selection, averages them, and offsets them from each other."),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2006-2007",
				 _("Fragment"),
				 "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Distorts"
	};
    }

    override protected void Render(Drawable drawable)
    {
      Tile.CacheDefault(drawable);

      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(_("Fragment"));

      using (PixelFetcher pf = new PixelFetcher(drawable) 
	{EdgeMode = EdgeMode.Black})
	{
	  iter.IterateDest((x, y) => 
	    {
	      return (pf[y - 4, x - 4] +
		      pf[y - 4, x + 4] +
		      pf[y + 4, x - 4] +
		      pf[y + 4, x + 4]) / 4;
	    });
	}
    }
  }
}
