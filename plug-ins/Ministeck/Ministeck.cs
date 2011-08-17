// The Ministeck plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Ministeck.cs
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

namespace Gimp.Ministeck
{
  class Ministeck : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<bool>("limit", 
			   _("Use real life ratio for number of pieces if true"), 
			   true),
	new Variable<int>("size", _("Default size"), 16),
	new Variable<RGB>("color", _("Color for the outline"), new RGB(0, 0, 0))
      };
      GimpMain<Ministeck>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_ministeck",
			   _("Generates Ministeck"),
			   _("Generates Ministeck"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2011",
			   _("Ministeck..."),
			   "RGB*, GRAY*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Artistic",
	  IconFile = "Ministeck.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("ministeck", true);
      return new Dialog(_image, _drawable, Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable, false);
    }
  }
}
