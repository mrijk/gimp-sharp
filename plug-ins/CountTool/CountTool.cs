// The CountTool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// CountTool.cs
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

namespace Gimp.CountTool
{
  class CountTool : Plugin
  {
    static void Main(string[] args)
    {
      GimpMain<CountTool>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_count_tool",
			   _("Count Tool"),
			   _("Count Tool"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Count Tool..."),
			   "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Generic"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("CountTool", true);
      return new Dialog(_drawable);
    }

    override protected void Render(Drawable drawable)
    {
    }
  }
}
