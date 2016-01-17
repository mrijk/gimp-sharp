// The ncp plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// ncp.cs
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

namespace Gimp.ncp
{
  class ncp : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<UInt32>("seed", _("Random seed"), 0),
	new Variable<bool>("random_seed", _("Random seed enabled"), false),
	new Variable<int>("points", _("Number of points"), 12),
	new Variable<int>("closest", _("Closest point"), 1),
	new Variable<bool>("color", _("Color (true), B&W (false)"), true)
      };
      GimpMain<ncp>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_ncp",
			   _("Generates 2D textures"),
			   _("Generates 2D textures"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2016",
			   "NCP...",
			   "RGB*, GRAY*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "ncp.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("ncp", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Drawable drawable)
    {
      var renderer = new Renderer(Variables, drawable);
      renderer.Render();
    }
  }
}
