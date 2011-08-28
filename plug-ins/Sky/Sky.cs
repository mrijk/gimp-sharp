// The Sky plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Code ported from Physically Modeled Media Plug-In for The GIMP
//                  Copyright (c) 2000-2001 David A. Bartold
//
// Sky.cs
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

namespace Gimp.Sky
{
  class Sky : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<double>("tilt", _("Camera tilt angle (0.0 - 90.0)"), 0.0),
	new Variable<double>("rotation", 
			     _("Camera rotation angle (0.0 - 90.0)"), 0.0),
	new Variable<UInt32>("seed", _("Random seed, -1 to use current time"),
			     0),
	new Variable<bool>("sun_show", _("Show sun? (bool)"), true),
	new Variable<double>("sun_x", _("Sun's x coordinate (0.0 - 1.0)"), 
			     0.2),
	new Variable<double>("sun_y", _("Sun's y coordinate (0.0 - 1.0)"), 
			     0.2),
	new Variable<double>("time", _("Time in hours (0.0 - 24.0)"), 0.0),
	new Variable<RGB>("horizon_color", _("Horizon color"), 
			  new RGB(0.31, 0.35, 0.40)),
	new Variable<RGB>("sky_color", _("Color at highest point in the sky"), 
			  new RGB(0.01, 0.04, 0.18)),
	new Variable<RGB>("sun_color", _("Sun color"), 
			  new RGB(0.995, 0.90, 0.83)),
	new Variable<RGB>("cloud_color", _("Cloud color"), 
			  new RGB(1.0, 1.0, 1.0)),
	new Variable<RGB>("shadow_color", _("Cloud shadow color"), 
			  new RGB(0, 0, 0)),
	new Variable<bool>("random_seed", _(""), false)
      };
      GimpMain<Sky>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_sky",
			   _("Sky"),
			   _("Sky"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2007-2011",
			   _("Sky..."),
			   "RGB*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render/",
	  IconFile = "Sky.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Sky", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Drawable drawable)
    {
      var renderer = new Renderer(Variables, drawable);
      renderer.Render();
    }
  }
}
