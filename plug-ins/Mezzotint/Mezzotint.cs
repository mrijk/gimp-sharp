// The Mezzotint plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// Mezzotint.cs
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

namespace Gimp.Mezzotint
{
  class Mezzotint : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<int>("type", _("Stroke or dots type"), 0)
      };
      GimpMain<Mezzotint>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_mezzotint",
			   _("Mezzotint"),
			   _("Mezzotint"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2007-2016",
			   _("Mezzotint..."),
			   "RGB*")
	{
	  MenuPath = "<Image>/Filters/Noise",
	  IconFile = "Mezzotint.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Mezzotint", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(drawable);
    }
  }
}
