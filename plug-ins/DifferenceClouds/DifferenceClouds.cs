// The Difference Clouds plug-in
// Copyright (C) 2006-2011 Massimo Perga (massimo.perga@gmail.com)
//
// DifferenceClouds.cs
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

namespace Gimp.DifferenceClouds
{
  class DifferenceClouds : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<UInt32>("seed", _("Random seed"), 0),
	new Variable<bool>("random_seed", _("Random seed enabled"), false),
	new Variable<double>("turbulence", _("Turbulence of the cloud"), 0)
      };
      GimpMain<DifferenceClouds>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_difference_clouds",
			   _("Creates difference clouds."),
			   _("Creates difference clouds."),
			   "Massimo Perga",
			   "(C) Massimo Perga",
			   "2006-2011",
			   _("Difference Clouds..."),
			   "RGB*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render/Clouds",
	  IconFile = "DifferenceClouds.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Difference Clouds", true);
      return new Dialog(Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable);
    }
  }
}

