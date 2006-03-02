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
    DrawablePreview _preview;

    [SaveAttribute]
    int _dropSize = 80;
    [SaveAttribute]
    int _number = 80;
    [SaveAttribute]
    int _fishEye = 30;

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

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      vbox.PackStart(table, false, false, 0);

      ScaleEntry entry = new ScaleEntry(table, 0, 1, "_Drop size:", 150, 3,
					_dropSize, 1.0, 256.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);

      entry = new ScaleEntry(table, 0, 2, "_Number:", 150, 3,
			     _number, 1.0, 256.0, 1.0, 8.0, 0,
			     true, 0, 0, null, null);


      entry = new ScaleEntry(table, 0, 3, "_Fish eye:", 150, 3,
			     _fishEye, 1.0, 256.0, 1.0, 8.0, 0,
			     true, 0, 0, null, null);

      // entry.ValueChanged += PointsUpdate;
			
      dialog.ShowAll();
      return DialogRun();
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      int x, y, width, height;
 	
      _preview.GetPosition(out x, out y);
      _preview.GetSize(out width, out height);
      Image clone = new Image(_image);
      clone.Crop(width, height, x, y);

      PixelRgn rgn = new PixelRgn(clone.ActiveDrawable, 0, 0, width, height, 
				  false, false);
      _preview.DrawRegion(rgn);
	
      clone.Delete();
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Image image, Drawable drawable)
    {
      Display.DisplaysFlush();
    }
  }
}
