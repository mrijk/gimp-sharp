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
using System.Linq;
using Gtk;

namespace Gimp.Trim
{
  class Trim : Plugin
  {
    [SaveAttribute("top")]
    bool _top = true;
    [SaveAttribute("left")]
    bool _left = true;
    [SaveAttribute("bottom")]
    bool _bottom = true;
    [SaveAttribute("right")]
    bool _right = true;

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
	new ParamDef("top", 1, typeof(bool), _("Trim Top")),
	new ParamDef("left", 1, typeof(bool), _("Trim Left")),
	new ParamDef("bottom", 1, typeof(bool), _("Trim Bottom")),
	new ParamDef("right", 1, typeof(bool), _("Trim Right"))
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
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("Trim", true);

      var dialog = DialogNew("Trim", "Trim", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "Trim");
 
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      CreateBasedOnWidget(vbox);
      CreateTrimAwayWidget(vbox);
			
      return dialog;
    }

    void CreateBasedOnWidget(VBox parent)
    {
      var frame = new GimpFrame(_("Based On"));
      parent.PackStart(frame, true, true, 0);

      var vbox = new VBox(false, 12);
      frame.Add(vbox);
      
      var button = new RadioButton(_("Transparent Pixels"));
      vbox.PackStart(button, true, true, 0);
      button.Sensitive = _drawable.HasAlpha;

      button = new RadioButton(button, _("Top Left Pixel Color"));
      vbox.PackStart(button, false, false, 0);
      button.Active = true;

      button = new RadioButton(button, _("Bottom Right Pixel Color"));
      vbox.PackStart(button, false, false, 0);
    }

    void CreateTrimAwayWidget(VBox parent)
    {
      var frame = new GimpFrame(_("Trim Away"));
      parent.PackStart(frame, true, true, 0);

      var table = new GimpTable(2, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};      
      frame.Add(table);

      CreateTopWidget(table);
      CreateLeftWidget(table);
      CreateBottomWidget(table);
      CreateRightWidget(table);
    }

    void CreateTopWidget(GimpTable table)
    {
      var top = new CheckButton(_("_Top")) {Active = _top};
      top.Toggled += delegate {_top = top.Active;};
      table.Attach(top, 0, 1, 0, 1);
    }

    void CreateLeftWidget(GimpTable table)
    {
      var left = new CheckButton(_("_Left")) {Active = _left};
      left.Toggled += delegate {_left = left.Active;};
      table.Attach(left, 1, 2, 0, 1);
    }

    void CreateBottomWidget(GimpTable table)
    {
      var bottom = new CheckButton(_("_Bottom")) {Active = _bottom};
      bottom.Toggled += delegate {_bottom = bottom.Active;};
      table.Attach(bottom, 0, 1, 1, 2);
    }

    void CreateRightWidget(GimpTable table)
    {
      var right = new CheckButton(_("_Right")) {Active = _right};
      right.Toggled += delegate {_right = right.Active;};
      table.Attach(right, 1, 2, 1, 2);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var src = new PixelRgn(drawable, false, false);
      PixelRgn.Register(src);

      var upperLeft = src[0, 0];
      Console.WriteLine("Pixel: " + upperLeft);

      Predicate<bool> notTrue = (b) => {return !b;};

      int height = drawable.Height;
      var rows = new bool[height];
      int y = 0;
      src.ForEachRow(row => rows[y++] = AllEqual(row, upperLeft));

      int y1 = (_top) ? Array.FindIndex(rows, notTrue) : 0;
      int y2 = (_bottom) ? Array.FindLastIndex(rows, notTrue) : height;

      int width = drawable.Width;
      var cols = new bool[width];
      int x = 0;
      src.ForEachColumn(col => cols[x++] = AllEqual(col, upperLeft));

      int x1 = (_left) ? Array.FindIndex(cols, notTrue) : 0;
      int x2 = (_right) ? Array.FindLastIndex(cols, notTrue) : width;

      Crop(image, x1, y1, x2, y2);
    }

    void Crop(Image image, int x1, int y1, int x2, int y2)
    {
      image.Crop(x2 - x1, y2 - y1, x1, y1);
    }

    bool AllEqual(Pixel[] array, Pixel p)
    {
      return array.All(pixel => pixel.IsSameColor(p));
    }
  }
}
