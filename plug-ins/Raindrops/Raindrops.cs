// The Raindrops plug-in
// Copyright (C) 2004-2011 Maurits Rijk, Massimo Perga
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
  class Raindrops : PluginWithPreview<DrawablePreview>
  {
    Variable<int> _dropSize = 
    new Variable<int>("drop_size", _("Size of raindrops"), 80);
    Variable<int> _number = 
    new Variable<int>("number", _("Number of raindrops"), 80);
    Variable<int> _fishEye = 
    new Variable<int>("fish_eye", _("Fisheye effect"), 30);

    static void Main(string[] args)
    {
      GimpMain<Raindrops>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_raindrops",
			   _("Generates raindrops"),
			   _("Generates raindrops"),
			   "Massimo Perga",
			   "(C) Massimo Perga",
			   "2006-2011",
			   _("Raindrops..."),
			   "RGB*, GRAY*",
			   new ParamDefList(_dropSize, _number, _fishEye))
	{
	  MenuPath = "<Image>/Filters/" + _("Light and Shadow") + "/" + 
	    _("Glass"),
	  IconFile = "Raindrops.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      var dialog = DialogNew(_("Raindrops 0.2"), _("Raindrops"),
			     IntPtr.Zero, 0, Gimp.StandardHelpFunc,
			     _("Raindrops"));

      var table = new GimpTable(2, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);

      CreateDropSizeEntry(table);
      CreateNumberEntry(table);
      CreateFishEyeEntry(table);

      _dropSize.ValueChanged += delegate {InvalidatePreview();};
      _number.ValueChanged += delegate {InvalidatePreview();};
      _fishEye.ValueChanged += delegate {InvalidatePreview();};

      return dialog;
    }

    void CreateDropSizeEntry(Table table)
    {
      new ScaleEntry(table, 0, 1, _("_Drop size:"), 150, 3, _dropSize, 1.0,
		     256.0, 1.0, 8.0, 0);
    }

    void CreateNumberEntry(Table table)
    {
      new ScaleEntry(table, 0, 2, _("_Number:"), 150, 3, _number, 1.0,
		     256.0, 1.0, 8.0, 0);
    }

    void CreateFishEyeEntry(Table table)
    {
      new ScaleEntry(table, 0, 3, _("_Fish eye:"), 150, 3, _fishEye, 1.0,
		     256.0, 1.0, 8.0, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      // Fix me: it's probably better to just create a new Drawable iso
      // a completely new image!
      var clone = new Image(_image);
      clone.Crop(preview.Bounds);

      var drawable = clone.ActiveDrawable;
      RenderRaindrops(clone, drawable, true);
      preview.Redraw(drawable);
      clone.Delete();
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

      var factory = new RaindropFactory(_dropSize.Value, _fishEye.Value, 
					dimensions);
      for (int numBlurs = 0; numBlurs <= _number.Value; numBlurs++)
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
	    progress.Update((double) numBlurs / _number.Value);
	}

      pf.Dispose();

      drawable.Flush();
      drawable.Update();
    }
  }
}

