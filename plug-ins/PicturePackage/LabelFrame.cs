// The PicturePackage plug-in
// Copyright (C) 2004-2007 Maurits Rijk
//
// LabelFrame.cs
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

namespace Gimp.PicturePackage
{
  public class LabelFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    Entry _entry;
    ComboBox _position;
    ComboBox _rotate;
    GimpColorButton _color;
    SpinButton _opacity;

    public LabelFrame(PicturePackage parent) : base(3, 3, "Label")
    {
      _parent = parent;

      ComboBox content = CreateComboBox(_("None"), _("Custom Text"), 
					_("Filename"), _("Copyright"),
					_("Caption"), _("Credits"), 
					_("Title"));
      content.Active = 0;
      content.Changed += delegate
	{
	  SetLabelFrameSensitivity(content.Active);
	};
      Table.AttachAligned(0, 0, _("Content:"), 0.0, 0.5, content, 1, false);

      _entry = new Entry();
      _entry.Changed += delegate
	{
	  _parent.Label = _entry.Text;
	};
      Table.AttachAligned(0, 1, _("Custom Text:"), 0.0, 0.5, _entry, 1, true);

      Button font = new Button(Stock.SelectFont);
      font.Clicked += OnFontClicked;
      Table.AttachAligned(0, 2, _("Font:"), 0.0, 0.5, font, 1, true);

      HBox hbox = new HBox(false, 12);

      _color = new GimpColorButton("", 16, 16, new RGB(0, 0, 0),
				   ColorAreaType.COLOR_AREA_FLAT);
      _color.Update = true;
      hbox.Add(_color);

      _opacity = new SpinButton(0, 100, 1);
      hbox.Add(new Label(_("Opacity:")));
      hbox.Add(_opacity);
      hbox.Add(new Label("%"));
      Table.AttachAligned(0, 3, _("Color:"), 0.0, 0.5, hbox, 1, true);

      _position = CreateComboBox(_("Centered"), _("Top Left"),
				 _("Bottom Left"), _("Top Right"),
				 _("Bottom Right"));
      _position.Changed += delegate
	{
	  _parent.Position = _position.Active;
	};
      Table.AttachAligned(0, 4, _("Position:"),
          0.0, 0.5, _position, 1, false);

      _rotate = CreateComboBox(_("None"), 
			       _("45 Degrees Right"),
			       _("90 Degrees Right"), 
			       _("45 Degrees Left"),
			    _("90 Degrees Left"));
      Table.AttachAligned(0, 5, _("Rotate:"), 
          0.0, 0.5, _rotate, 1, false);

      SetLabelFrameSensitivity(0);
    }

    void SetLabelFrameSensitivity(int history)
    {
      bool sensitive = (history == 1);

      _entry.Sensitive = sensitive;
      _color.Sensitive = sensitive;
      _opacity.Sensitive = sensitive;
      _position.Sensitive = sensitive;
      _rotate.Sensitive = sensitive;
    }

    void OnFontClicked (object o, EventArgs args) 
    {
      FontSelectionDialog fs = new FontSelectionDialog(_("Select Font"));
      fs.Run();
      fs.Hide();
    }
  }
}
