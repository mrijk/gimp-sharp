// The Pointillize plug-in
// Copyright (C) 2006-2018 Maurits Rijk
//
// Pointillize.cs
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

namespace Gimp.Pointillize
{
  class Pointillize : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<int>("cell_size", "Cell size", 30)
      };
      GimpMain<Pointillize>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_pointillize",
			   _("Create pointillist paintings"),
			   _("Create pointillist paintings"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2018",
			   _("Pointillize..."),
			   "RGB*, GRAY*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Artistic",
	  IconFile = "Pointillize.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Pointillize", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(drawable);
    }
  }
}
