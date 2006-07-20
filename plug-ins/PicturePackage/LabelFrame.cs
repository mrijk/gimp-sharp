// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
using Mono.Unix;

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

    public LabelFrame(PicturePackage parent) : 
        base(3, 3, Catalog.GetString("Label"))
    {
      _parent = parent;

      ComboBox content = CreateComboBox(Catalog.GetString("None"),
          Catalog.GetString("Custom Text"), Catalog.GetString("Filename"),
					Catalog.GetString("Copyright"), Catalog.GetString("Caption"),
          Catalog.GetString("Credits"), Catalog.GetString("Title"));
      content.Active = 0;
      content.Changed += delegate(object o, EventArgs args) 
	{
	  SetLabelFrameSensitivity(content.Active);
	};
      Table.AttachAligned(0, 0, Catalog.GetString("Content:"), 
          0.0, 0.5, content, 1, false);

      _entry = new Entry();
      _entry.Changed += delegate(object o, EventArgs args) 
	{
	  _parent.Label = _entry.Text;
	};
      Table.AttachAligned(0, 1, Catalog.GetString("Custom Text:"), 
          0.0, 0.5, _entry, 1, true);

      Button font = new Button(Stock.SelectFont);
      font.Clicked += OnFontClicked;
      Table.AttachAligned(0, 2, Catalog.GetString("Font:"), 
          0.0, 0.5, font, 1, true);

      HBox hbox = new HBox(false, 12);

      _color = new GimpColorButton("", 16, 16, new RGB(0, 0, 0),
				   ColorAreaType.COLOR_AREA_FLAT);
      _color.Update = true;
      hbox.Add(_color);

      _opacity = new SpinButton(0, 100, 1);
      hbox.Add(new Label(Catalog.GetString("Opacity:")));
      hbox.Add(_opacity);
      hbox.Add(new Label("%"));
      Table.AttachAligned(0, 3, Catalog.GetString("Color:"), 
          0.0, 0.5, hbox, 1, true);

      _position = CreateComboBox(
          Catalog.GetString("Centered"), Catalog.GetString("Top Left"),
          Catalog.GetString("Bottom Left"), Catalog.GetString("Top Right"),
          Catalog.GetString("Bottom Right"));
      _position.Changed += delegate(object o, EventArgs args) 
	{
	  _parent.Position = _position.Active;
	};
      Table.AttachAligned(0, 4, Catalog.GetString("Position:"),
          0.0, 0.5, _position, 1, false);

      _rotate = CreateComboBox(Catalog.GetString("None"), 
          Catalog.GetString("45 Degrees Right"),
          Catalog.GetString("90 Degrees Right"), 
          Catalog.GetString("45 Degrees Left"),
			    Catalog.GetString("90 Degrees Left"));
      Table.AttachAligned(0, 5, Catalog.GetString("Rotate:"), 
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
      FontSelectionDialog fs = new FontSelectionDialog(
          Catalog.GetString("Select Font"));
      fs.Run();
      fs.Hide();
    }
  }
}
