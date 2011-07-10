// The Trim plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.Linq;
using Gtk;

namespace Gimp.Trim
{
  class Trim : Plugin
  {
    Variable<int> _basedOn = new Variable<int>
    ("based-on", _("Based On"), 1);
    Variable<bool> _top = new Variable<bool>
    ("top", _("Trim Top"), true);
    Variable<bool> _left = new Variable<bool>
    ("left", _("Trim Left"), true);
    Variable<bool> _bottom = new Variable<bool>
    ("bottom", _("Trim Bottom"), true);
    Variable<bool> _right = new Variable<bool>
    ("right", _("Trim Right"), true);

    static void Main(string[] args)
    {
      GimpMain<Trim>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_trim",
			   _("Trim"),
			   _("Trim"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2011",
			   "Trim...",
			   "RGB*, GRAY*",
			   new ParamDefList(_basedOn, _top, _left, _bottom,
					    _right))
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
      
      var button = AddBasedOnButton(vbox, null, 0, _("Transparent Pixels"));
      button.Sensitive = _drawable.HasAlpha;
      button = AddBasedOnButton(vbox, button, 1, _("Top Left Pixel Color"));
      AddBasedOnButton(vbox, button, 2, _("Bottom Right Pixel Color"));
    }

    RadioButton AddBasedOnButton(VBox vbox, RadioButton previous, int type, 
				 string description)
    {
      var button = new RadioButton(previous, description);
      vbox.Add(button);
      if (_basedOn.Value == type) {
	button.Active = true;
      }
      button.Clicked += delegate {
	if (button.Active) {
	  _basedOn.Value = type;
	}
      };
      return button;
    }

    void CreateTrimAwayWidget(VBox parent)
    {
      var frame = new GimpFrame(_("Trim Away"));
      parent.PackStart(frame, true, true, 0);

      var table = new GimpTable(2, 2)
	{ColumnSpacing = 6, RowSpacing = 6};      
      frame.Add(table);

      CreateTopWidget(table);
      CreateLeftWidget(table);
      CreateBottomWidget(table);
      CreateRightWidget(table);
    }

    void CreateTopWidget(GimpTable table)
    {
      var top = new GimpCheckButton(_("_Top"), _top);
      table.Attach(top, 0, 1, 0, 1);
    }

    void CreateLeftWidget(GimpTable table)
    {
      var left = new GimpCheckButton(_("_Left"), _left);
      table.Attach(left, 1, 2, 0, 1);
    }

    void CreateBottomWidget(GimpTable table)
    {
      var bottom = new GimpCheckButton(_("_Bottom"), _bottom);
      table.Attach(bottom, 0, 1, 1, 2);
    }

    void CreateRightWidget(GimpTable table)
    {
      var right = new GimpCheckButton(_("_Right"), _right);
      table.Attach(right, 1, 2, 1, 2);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var src = new PixelRgn(drawable, false, false);
      PixelRgn.Register(src);

      var trimColor = GetTrimColor(src, drawable);

      Predicate<bool> notTrue = (b) => {return !b;};

      int height = drawable.Height;
      var rows = new bool[height];
      int y = 0;
      src.ForEachRow(row => rows[y++] = AllEqual(row, trimColor));

      int y1 = (_top.Value) ? Array.FindIndex(rows, notTrue) : 0;
      int y2 = (_bottom.Value) ? 
	Array.FindLastIndex(rows, notTrue) + 1 : height;

      int width = drawable.Width;
      var cols = new bool[width];
      int x = 0;
      src.ForEachColumn(col => cols[x++] = AllEqual(col, trimColor));

      int x1 = (_left.Value) ? Array.FindIndex(cols, notTrue) : 0;
      int x2 = (_right.Value) ? Array.FindLastIndex(cols, notTrue) + 1 : width;

      if (x1 != 0 || y1 != 0 || x2 != width || y2 != height)
	{
	  image.Crop(new Rectangle(x1, y1, x2, y2));
	}
    }

    Pixel GetTrimColor(PixelRgn src, Drawable drawable)
    {
      if (_basedOn.Value == 0)
	{
	  return new Pixel(0, 0, 0, 0);
	}
      else if (_basedOn.Value == 1)
	{
	  return src[0, 0];
	}
      else
	{
	  return src.GetPixel(drawable.Width - 1, drawable.Height - 1);
	}
    }

    bool AllEqual(Pixel[] array, Pixel p)
    {
      return array.All(pixel => pixel.IsSameColor(p));
    }
  }
}
