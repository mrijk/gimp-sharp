// The Shatter plug-in
// Copyright (C) 2006-2011 Maurits Rijk
//
// Shatter.cs
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

namespace Gimp.Shatter
{
  public class Shatter : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet {
	new Variable<int>("pieces", _("Number of shards"), 4)
      };
      GimpMain<Shatter>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_shatter",
			   _("Shatter an image"),
			   _("Shatter an image"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Shatter..."),
			   "RGB*, GRAY*",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Distorts",
	  IconFile = "Shatter.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Shatter", true);
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable);
    }
  }
}
