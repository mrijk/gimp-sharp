// The Mezzotint plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// Mezzotint.cs
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

namespace Gimp.Mezzotint
{
  class Mezzotint : Plugin
  {
    DrawablePreview _preview;

    static void Main(string[] args)
    {
      new Mezzotint(args);
    }

    Mezzotint(string[] args) : base(args, "Mezzotint")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      Procedure procedure = new Procedure("plug_in_mezzotint",
					  _("Mezzotint"),
					  _("Mezzotint"),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2007",
					  _("Mezzotint..."),
					  "RGB*");
      procedure.MenuPath = "<Image>/Filters/Noise"; 
      procedure.IconFile = "Mezzotint.png";

      yield return procedure;
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Mezzotint", true);

      GimpDialog dialog = DialogNew("Mezzotint", "Mezzotint", IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, "Mezzotint");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      ComboBox type = ComboBox.NewText();
      type.AppendText("Fine dots");
      type.AppendText("Medium dots");
      type.AppendText("Grainy dots");
      type.AppendText("Coarse dots");
      type.AppendText("Short lines");
      type.AppendText("Medium lines");
      type.AppendText("Long lines");
      type.AppendText("Short strokes");
      type.AppendText("Medium strokes");
      type.AppendText("Long strokes");
      type.Active = 0;

      vbox.PackStart(type, false, false, 0);

      return dialog;
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      Rectangle rectangle = _preview.Bounds;
      Console.WriteLine("UpdatePreview: " + rectangle);

      int rowStride = rectangle.Width * 3;
      byte[] buffer = new byte[rectangle.Area * 3];	// Fix me!

      PixelRgn srcPR = new PixelRgn(_drawable, rectangle, false, false);
      for (IntPtr pr = PixelRgn.Register(srcPR); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	  for (int y = srcPR.Y; y < srcPR.Y + srcPR.H; y++)
	    {
	      for (int x = srcPR.X; x < srcPR.X + srcPR.W; x++)
		{
		  Pixel pixel = srcPR[y, x];
		  pixel.X = x;
		  pixel.Y = y;
		  pixel = DoMezzotint(pixel);
		  int index = (y - rectangle.Y1) * rowStride +
		    (x - rectangle.X1) * 3;
		  pixel.CopyTo(buffer, index);
		}
	    }
	}
      _preview.DrawBuffer(buffer, rowStride);
    }

    override protected void Render(Drawable drawable)
    {
      RgnIterator iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.Progress = new Progress(_("Mezzotint"));

      iter.IterateSrcDest(delegate (Pixel pixel)
      {
	pixel.Red = (pixel.Red > 127) ? 255 : 0;
	pixel.Green = (pixel.Green > 127) ? 255 : 0;
	pixel.Blue = (pixel.Blue > 127) ? 255 : 0;
	return pixel;
      });

      Display.DisplaysFlush();
    }

    Pixel DoMezzotint(Pixel pixel)
    {
      pixel.Red = (pixel.Red > 127) ? 255 : 0;
      pixel.Green = (pixel.Green > 127) ? 255 : 0;
      pixel.Blue = (pixel.Blue > 127) ? 255 : 0;
      return pixel;
    }
  }
}
