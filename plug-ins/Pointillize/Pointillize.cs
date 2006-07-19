// The Pointillize plug-in
// Copyright (C) 2006 Maurits Rijk
//
// Pointillize.cs
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
using Mono.Unix;

using Gtk;

namespace Gimp.Pointillize
{
  public class Pointillize : PluginWithPreview
  {
    [SaveAttribute("cell_size")]
    int _cellSize = 30;

    ColorCoordinateSet _coordinates;
    int _width, _height;

    static void Main(string[] args)
    {
      string localeDir = Gimp.LocaleDirectory;
      Catalog.Init("Pointillize", localeDir);
      new Pointillize(args);
    }

    public Pointillize(string[] args) : base(args)
    {
    }

    override protected ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();
      in_params.Add(new ParamDef("cell_size", 30, typeof(int),
				 "Cell size"));

      Procedure procedure = new Procedure("plug_in_pointillize",
					  Catalog.GetString("Create pointillist paintings"),
					  Catalog.GetString("Create pointillist paintings"),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  Catalog.GetString("Pointillize..."),
					  "RGB*, GRAY*",
					  in_params);
      procedure.MenuPath = "<Image>/Filters/Artistic";
      // procedure.IconFile = "Pointillize.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      Dialog dialog = DialogNew(Catalog.GetString("Pointillize"), 
        Catalog.GetString("Pointillize"), IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, Catalog.GetString("Pointillize"));

      GimpTable table = new GimpTable(1, 3, false);

      ScaleEntry entry = new ScaleEntry(table, 0, 1, 
          Catalog.GetString("Cell _Size:"), 150, 3,
					_cellSize, 3.0, 300.0, 1.0, 8.0, 0,
					true, 0, 0, null, null);
      entry.ValueChanged += delegate(object sender, EventArgs e)
	{
	  _cellSize = entry.ValueAsInt;
	  InvalidatePreview();
	};

      Vbox.PackStart(table, false, false, 0);
      
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void UpdatePreview(AspectPreview preview)
    {
      // move generic code from ncp to base class
      Initialize(_drawable);

      int width, height;
      preview.GetSize(out width, out height);

      byte[] buffer = new byte[width * height * 3];
      byte[] dest = new byte[3];
      for (int y = 0; y < height; y++)
	{
	  int y_orig = _height * y / height;
	  for (int x = 0; x < width; x++)
	    {
	      long index = 3 * (y * width + x);
	      int x_orig = _width * x / width;

	      dest = DoPointillize(x_orig, y_orig);
	      dest.CopyTo(buffer, index);
	    }
	}
      preview.DrawBuffer(buffer, width * 3);
    }

    override protected void Render(Drawable drawable)
    {
      Initialize(drawable);

      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(Catalog.GetString("Pointillize"));
      iter.IterateDest(DoPointillize);
			
      Display.DisplaysFlush();
    }

    void Initialize(Drawable drawable)
    {
      _coordinates = new ColorCoordinateSet(drawable, _cellSize);

      int x1, y1, x2, y2;
      drawable.MaskBounds(out x1, out y1, out x2, out y2);
      _width = x2 - x1;
      _height = y2 - y1;
    }

    byte[] DoPointillize(int x, int y)
    {
      return _coordinates.GetColor(x, y);
    }
  }
}
