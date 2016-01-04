// The Raindrops plug-in
// Copyright (C) 2004-2016 Maurits Rijk, Massimo Perga
//
// Raindrops.cs
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

namespace Gimp.Raindrops
{
  class Raindrops : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<int>("drop_size", _("Size of raindrops"), 80),
	new Variable<int>("number", _("Number of raindrops"), 80),
	new Variable<int>("fish_eye", _("Fisheye effect"), 30)
      };
      GimpMain<Raindrops>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_raindrops",
			   _("Generates raindrops"),
			   _("Generates raindrops"),
			   "Massimo Perga",
			   "(C) Massimo Perga",
			   "2006-2016",
			   _("Raindrops..."),
			   "RGB*, GRAY*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/" + _("Light and Shadow") + "/" + 
	    _("Glass"),
	  IconFile = "Raindrops.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Raindrops", true);
      return new Dialog(_image, _drawable, Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable, new Progress(_("Raindrops...")));
    }
  }
}

