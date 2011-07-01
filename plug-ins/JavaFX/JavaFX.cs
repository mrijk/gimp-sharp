// The JavaFX plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// JavaFX.cs
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
using Gtk;

namespace Gimp.JavaFX
{
  class JavaFX : Plugin
  {
    static void Main(string[] args)
    {
      GimpMain<JavaFX>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_javafx",
			   _("JavaFX"),
			   _("JavaFX"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2011",
			   _("Save As JavaFX"),
			   "RGB*, GRAY*")
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "JavaFX.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("JavaFX", true);

      var dialog = DialogNew("JavaFX", "JavaFX", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "JavaFX");

      var table = new GimpTable(4, 3) {
	ColumnSpacing = 6, RowSpacing = 6};
      dialog.VBox.PackStart(table, false, false, 0);

      return dialog;
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var writer = new ZipWriter(image);
      writer.CreateFxz();
    }
  }
}
