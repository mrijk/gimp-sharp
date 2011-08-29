// The Swirlies plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Swirlies.cs
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

namespace Gimp.Swirlies
{
  class Swirlies : PluginWithPreview<AspectPreview>
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet {
	new Variable<UInt32>("seed", _("Value for random seed"), 0),
	new Variable<bool>("random_seed", _("Use specified random seed"), false),
	new Variable<int>("points", _("Fix me"), 3)
      };
      GimpMain<Swirlies>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_swirlies",
			   _("Generates 2D textures"),
			   _("Generates 2D textures"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Swirlies..."),
			   "RGB",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "Swirlies.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Swirlies", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Drawable drawable)
    {
      var renderer = new Renderer(Variables, drawable);
      renderer.Render(drawable);
    }
  }
}
