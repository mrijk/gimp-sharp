// The Shatter plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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

using System;

using Gtk;

namespace Gimp.Shatter
{
  public class Shatter : PluginWithPreview
  {
    [STAThread]
    static void Main(string[] args)
    {
      new Shatter(args);
    }

    public Shatter(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_shatter",
					  "Shatter an image",
					  "Shatter an image",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Shatter...",
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Distorts";
      procedure.IconFile = "Shatter.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      Dialog dialog = DialogNew("Shatter", "Shatter", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Shatter");

      GimpTable table = new GimpTable(4, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      Vbox.PackStart(table, false, false, 0);
			
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void Render(Drawable drawable)
    {
      Display.DisplaysFlush();
    }
  }
}
