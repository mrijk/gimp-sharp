// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// DocumentFrame.cs
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

      ComboBox content = CreateComboBox(
	"None", "Custom Text", "Filename",
	"Copyright", "Caption", "Credits",
	"Title");
      content.Active = 0;
      content.Changed += OnContentChanged;
      Table.AttachAligned(0, 0, "Content:", 0.0, 0.5, content, 1, false);

      _entry = new Entry();
      _entry.Changed += OnCustomTextChanged;
      Table.AttachAligned(0, 1, "Custom Text:", 0.0, 0.5, _entry, 1, true);

      Button font = new Button(Stock.SelectFont);
      font.Clicked += OnFontClicked;
      Table.AttachAligned(0, 2, "Font:", 0.0, 0.5, font, 1, true);

      HBox hbox = new HBox(false, 12);

      RGB rgb = new RGB(0, 0, 0);

      _color = new GimpColorButton("", 16, 16, rgb.GimpRGB,
				   ColorAreaType.COLOR_AREA_FLAT);
      _color.Update = true;
      hbox.Add(_color);

      _opacity = new SpinButton(0, 100, 1);
      hbox.Add(new Label("Opacity:"));
      hbox.Add(_opacity);
      hbox.Add(new Label("%"));
      Table.AttachAligned(0, 3, "Color:", 0.0, 0.5, hbox, 1, true);

      _position = CreateComboBox(
	"Centered", "Top Left", "Bottom Left",
	"Top Right", "Bottom Right");
      _position.Changed += new EventHandler(OnPositionChanged);
      Table.AttachAligned(0, 4, "Position:", 0.0, 0.5, _position, 1, false);

      _rotate = CreateComboBox(
	"None", "45 Degrees Right",
	"90 Degrees Right", "45 Degrees Left",
	"90 Degrees Left");
      Table.AttachAligned(0, 5, "Rotate:", 0.0, 0.5, _rotate, 1, false);

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

    void OnContentChanged (object o, EventArgs args) 
    {
      SetLabelFrameSensitivity((o as ComboBox).Active);
    }

    void OnCustomTextChanged (object o, EventArgs args) 
    {
      _parent.Label = (o as Entry).Text;
    }

    void OnPositionChanged (object o, EventArgs args) 
    {
      _parent.Position = (o as ComboBox).Active;
    }

    void OnFontClicked (object o, EventArgs args) 
    {
      FontSelectionDialog fs = new FontSelectionDialog("Select Font");
      fs.Run();
      fs.Hide();
    }
  }
}
