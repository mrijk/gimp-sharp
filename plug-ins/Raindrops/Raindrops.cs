// The Raindrops plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Raindrops.cs
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

namespace Gimp.Raindrops
{
  public class Raindrops : Plugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new Raindrops(args);
    }

    public Raindrops(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      /*
      in_params.Add(new ParamDef("points", 12, typeof(int),
				 "Number of points"));
      */
      Procedure procedure = new Procedure("plug_in_raindrops",
					  "Generates raindrops",
					  "Generates raindrops",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Raindrops...",
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Light and Shadow/Glass";
      procedure.IconFile = "Raindrops.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Raindrops", true);

      Dialog dialog = DialogNew("Raindrops", "Raindrops", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Raindrops");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);
			
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Drawable drawable)
    {
      Display.DisplaysFlush();
    }
  }
}
