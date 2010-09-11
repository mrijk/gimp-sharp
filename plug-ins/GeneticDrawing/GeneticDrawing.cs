// The GeneticDrawing plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// GeneticDrawing.cs
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
using System.Linq;
using Gtk;

namespace Gimp.GeneticDrawing
{
  class GeneticDrawing : Plugin
  {
    static void Main(string[] args)
    {
      new GeneticDrawing(args);
    }

    GeneticDrawing(string[] args) : base(args, "GeneticDrawing")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList();

      yield return new Procedure("plug_in_genetic_drawing",
				 _("GeneticDrawing"),
				 _("GeneticDrawing"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2010",
				 "GeneticDrawing...",
				 "RGB*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/Render",
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("GeneticDrawing", true);

      var dialog = DialogNew("GeneticDrawing", "GeneticDrawing", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "GeneticDrawing");
 
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      return dialog;
    }

    override protected void Render(Image image, Drawable drawable)
    {
    }
  }
}
