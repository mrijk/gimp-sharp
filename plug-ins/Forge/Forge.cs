// The Forge plug-in
// Copyright (C) 2006-2011 Massimo Perga (massimo.perga@gmail.com)
//
// Forge.cs
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

namespace Gimp.Forge
{
  class Forge : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet {
	new Variable<bool>("clouds", _("Clouds (true), Planet or Stars (false)"), 
			   false),
	new Variable<bool>("stars", _("Stars (true), Planet or Clouds (false)"), 
			   false),
	new Variable<double>("dimension", _("Fractal dimension factor"), 2.4),
	new Variable<double>("power", _("Power factor"), 1.0),
	new Variable<double>("glaciers", _("Glaciers factor"), 0.75),
	new Variable<double>("ice_level", _("Ice factor"), 0.4),
	new Variable<double>("hour", _("Hour factor"), 0.0),
	new Variable<double>("inclination", _("Inclination factor"), 0.0),
	new Variable<double>("stars_fraction", _("Stars factor"), 100.0),
	new Variable<double>("saturation", _("Saturation factor"), 100.0),
	new Variable<UInt32>("seed", _("Random generated seed"), 0),
	new Variable<bool>("random_seed", _("Random seed enabled"), true)
      };
      GimpMain<Forge>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_forge",
			   _("Creates an artificial world."),
			   _("Creates an artificial world."),
			   "Massimo Perga, Maurits Rijk",
			   "(C) Massimo Perga, Maurits Rijk",
			   "2006-2011",
			   _("Forge..."),
			   "RGB*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "Forge.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Forge", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable);
    }
  }
}
