// The Raindrops plug-in
// Copyright (C) 2004-2009 Maurits Rijk, Massimo Perga
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
using System.Collections.Generic;

using Gtk;

namespace Gimp.Raindrops
{
  class Raindrops : Plugin
  {
    DrawablePreview _preview;

    [SaveAttribute("drop_size")]
    int _dropSize = 80;
    [SaveAttribute("number")]
    int _number = 80;
    [SaveAttribute("fish_eye")]
    int _fishEye = 30;

    static void Main(string[] args)
    {
      new Raindrops(args);
    }

    Raindrops(string[] args) : base(args, "Raindrops")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList()
	{
	  new ParamDef("drop_size", 80, typeof(int), _("Size of raindrops")),
	  new ParamDef("number", 80, typeof(int), _("Number of raindrops")),
	  new ParamDef("fish_eye", 30, typeof(int), _("Fisheye effect"))
	};

      yield return new Procedure("plug_in_raindrops",
				 _("Generates raindrops"),
				 _("Generates raindrops"),
				 "Massimo Perga",
				 "(C) Massimo Perga",
				 "2006-2009",
				 _("Raindrops..."),
				 "RGB*, GRAY*",
				 inParams)
	{
	  MenuPath = "<Image>/Filters/" + _("Light and Shadow") + "/" + 
	    _("Glass"),
	  IconFile = "Raindrops.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Raindrops", true);

      GimpDialog dialog = DialogNew(_("Raindrops 0.1"), _("Raindrops"),
				    IntPtr.Zero, 0, Gimp.StandardHelpFunc,
				    _("Raindrops"));

      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      _preview = new DrawablePreview(_drawable, false);
      _preview.Invalidated += UpdatePreview;
      vbox.PackStart(_preview, true, true, 0);

      GimpTable table = new GimpTable(2, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      vbox.PackStart(table, false, false, 0);

      CreateDropSizeEntry(table);
      CreateNumberEntry(table);
      CreateFishEyeEntry(table);

      return dialog;
    }

    void CreateDropSizeEntry(Table table)
    {
      var _dropSizeEntry = new ScaleEntry(table, 0, 1,
					  _("_Drop size:"),
					  150, 3, _dropSize, 1.0,
					  256.0, 1.0, 8.0, 0);
      _dropSizeEntry.ValueChanged += delegate
	{
	  _dropSize = _dropSizeEntry.ValueAsInt;
	  _preview.Invalidate();
	};
    }

    void CreateNumberEntry(Table table)
    {
      var _numberEntry = new ScaleEntry(table, 0, 2,
					_("_Number:"),
					150, 3, _number, 1.0,
					256.0, 1.0, 8.0, 0);
      _numberEntry.ValueChanged += delegate
	{
	  _number = _numberEntry.ValueAsInt;
	  _preview.Invalidate();
	};
    }

    void CreateFishEyeEntry(Table table)
    {
      var _fishEyeEntry = new ScaleEntry(table, 0, 3,
					 _("_Fish eye:"),
					 150, 3, _fishEye, 1.0,
					 256.0, 1.0, 8.0, 0);
      _fishEyeEntry.ValueChanged += delegate
	{
	  _fishEye = _fishEyeEntry.ValueAsInt;
	  _preview.Invalidate();
	};
    }

    void UpdatePreview(object sender, EventArgs e)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!
      Image clone = new Image(_image);
      clone.Crop(_preview.Bounds);

      Drawable drawable = clone.ActiveDrawable;
      RenderRaindrops(clone, drawable, true);
      _preview.Redraw(drawable);
      clone.Delete();
    }

    override protected void Reset()
    {
      Console.WriteLine("Reset!");
    }

    override protected void Render(Image image, Drawable drawable)
    {
      RenderRaindrops(image, drawable, false);
    }

    void RenderRaindrops(Image image, Drawable drawable, bool isPreview)
    {
      var dimensions = image.Dimensions;
      var progress = (isPreview) ? null : new Progress(_("Raindrops..."));

      Tile.CacheDefault(drawable);
      var pf = new PixelFetcher(drawable, false);

      var iter = new RgnIterator(drawable, RunMode.Interactive);
      iter.IterateSrcDest(src => src);

      var factory = new RaindropFactory(_dropSize, _fishEye, dimensions);

      for (int numBlurs = 0; numBlurs <= _number; numBlurs++)
	{
	  var raindrop = factory.Create();
	  if (raindrop == null)
	    {
	      if (!isPreview)
		progress.Update(1.0);
	      break;
	    }
	  raindrop.Render(factory.BoolMatrix, pf, drawable);

	  if (!isPreview)
	    progress.Update((double) numBlurs / _number);
	}

      pf.Dispose();

      drawable.Flush();
      // drawable.MergeShadow(true);
      drawable.Update();
    }
  }
}

