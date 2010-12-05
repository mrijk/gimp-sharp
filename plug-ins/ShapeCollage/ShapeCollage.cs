// The ShapeCollage plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// ShapeCollage.cs
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
using Gtk;

namespace Gimp.ncp
{
  class ShapeCollage : Plugin
  {
    static void Main(string[] args)
    {
      new ShapeCollage(args);
    }

    ShapeCollage(string[] args) : base(args, "ShapeCollage")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList() {
      };

      yield return new Procedure("plug_in_shape_collage",
				 _("Generates collage"),
				 _("Generates collage"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2010",
				 "ShapeCollage...",
				 "RGB*, GRAY*",
				 inParams)
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "ShapeCollage.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("ShapeCollage", true);

      var dialog = DialogNew("ShapeCollage", "ShapeCollage", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "ShapeCollage");

      var table = new GimpTable(4, 3)
	{
	  ColumnSpacing = 6, 
	  RowSpacing = 6
	};
      // Vbox.PackStart(table, false, false, 0);

      return dialog;
    }

    override protected void Render(Drawable drawable)
    {
    }
  }
}
