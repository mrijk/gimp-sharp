// The Trim plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// Trim.cs
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

namespace Gimp.Trim
{
  class Trim : Plugin
  {
    [SaveAttribute("top")]
    bool _top = true;

    static void Main(string[] args)
    {
      new Trim(args);
    }

    Trim(string[] args) : base(args, "Trim")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList() {
	new ParamDef("top", 1, typeof(bool), _("Color (true), B&W (false)"))
      };

      yield return new Procedure("plug_in_trim",
				 _("Trim"),
				 _("Trim"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2010",
				 "Trim...",
				 "RGB*, GRAY*",
				 inParams)
	{
	  MenuPath = "<Image>/Image",
	  IconFile = "Trim.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Trim", true);

      var dialog = DialogNew("Trim", "Trim", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "Trim");
 
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var table = new GimpTable(4, 3, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, true, true, 0);

      CreateTopWidget(table);
			
      return dialog;
    }

    void CreateTopWidget(GimpTable table)
    {
      var top = new CheckButton(_("_Use top")) {Active = _top};
      top.Toggled += delegate
	{
	  _top = top.Active;
	};
      table.Attach(top, 0, 1, 3, 4);
    }

    override protected void Render(Drawable drawable)
    {
    }
  }
}
