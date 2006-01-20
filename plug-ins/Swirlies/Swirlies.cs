// The Swirlies plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// Swirlies.cs
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
using System.Collections;

using Gtk;

namespace Gimp.Swirlies
{
  public class Swirlies : Plugin
  {
    AspectPreview _preview;

    [STAThread]
    static void Main(string[] args)
    {
      new Swirlies(args);
    }

    public Swirlies(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      GimpParamDef[] args = new GimpParamDef[1];

      InstallProcedure("plug_in_swirlies",
		       "Generates 2D textures",
		       "Generates 2D textures",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2006",
		       "Swirlies...",
		       "RGB*, GRAY*",
		       args);

      MenuRegister("<Image>/Filters/Render");
      // IconRegister("Swirlies.png");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("Swirlies", true);

      Dialog dialog = DialogNew("Swirlies", "swirlies", IntPtr.Zero, 0, null, 
				"swirlies");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new AspectPreview(_drawable, false);
      // _preview.Invalidated += new EventHandler(UpdatePreview);
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(4, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);
			
      dialog.ShowAll();
      return DialogRun();
    }
  /*
    void UpdatePreview(object sender, EventArgs e)
    {
      Initialize(_drawable);

      int width, height;
      _preview.GetSize(out width, out height);

      byte[] buffer = new byte[width * height * 3];
      byte[] dest = new byte[3];
      for (int y = 0; y < height; y++)
	{
	int y_orig = _height * y / height;
	for (int x = 0; x < width; x++)
	  {
	  long index = 3 * (y * width + x);
	  int x_orig = _width * x / width;

	  dest = DoSwirlies(x_orig, y_orig);
	  dest.CopyTo(buffer, index);
	  }
	}
      _preview.DrawBuffer(buffer, width * 3);
    }
  */

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void DoSomething(Drawable drawable)
    {
    }
    }
}
