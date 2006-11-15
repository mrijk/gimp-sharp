// The Colorize plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Colorize.cs
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

namespace Gimp.Colorize
{
  public class Colorize : Plugin
  {
    static void Main(string[] args)
    {
      new Colorize(args);
    }

    public Colorize(string[] args) : base(args, "Colorize")
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("points", 12, typeof(int),
				 _("Number of points")));

      Procedure procedure = new Procedure("plug_in_colorize",
          _("Fix me!"),
          _("Fix me!"),
          "Maurits Rijk",
          "(C) Maurits Rijk",
          "2004-2006",
          "COLORIZE...",
          "RGB*, GRAY*",
          in_params);
      procedure.MenuPath = "<Image>/Filters/Generic";
      procedure.IconFile = "Colorize.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      Dialog dialog = DialogNew("Colorize", "Colorize", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "Colorize");

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
