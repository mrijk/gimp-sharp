// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
    Entry _entry;
    ComboBox _position;
    ComboBox _rotate;
    GimpColorButton _color;
    SpinButton _opacity;

    public LabelFrame(VariableSet variables) : base(3, 3, "Label")
    {
      CreateContentTypeWidget();
      CreateTextWidget(variables);
      CreateFontWidget();
      CreateColorAndOpacityWidget();
      CreatePositionWidget(variables);
      CreateRotateWidget();

      SetLabelFrameSensitivity(0);
    }

    void CreateContentTypeWidget()
    {
      var content = CreateComboBox(_("None"), _("Custom Text"), 
				   _("Filename"), _("Copyright"),
				   _("Caption"), _("Credits"), 
				   _("Title"));
      content.Active = 0;
      content.Changed += delegate {SetLabelFrameSensitivity(content.Active);};
      AttachAligned(0, 0, _("Content:"), 0.0, 0.5, content, 1, false);
    }

    void CreateTextWidget(VariableSet variables)
    {
      _entry = new GimpEntry(variables.Get<string>("label"));
      AttachAligned(0, 1, _("Custom Text:"), 0.0, 0.5, _entry, 1, true);
    }

    void CreateFontWidget()
    {
      var font = new Button(Stock.SelectFont);
      font.Clicked += delegate
	{
	  var fs = new FontSelectionDialog(_("Select Font"));
	  fs.Run();
	  fs.Hide();
	};
      AttachAligned(0, 2, _("Font:"), 0.0, 0.5, font, 1, true);
    }

    void CreateColorAndOpacityWidget()
    {
      var hbox = new HBox(false, 12);

      _color = new GimpColorButton("", 16, 16, new RGB(0, 0, 0),
				   ColorAreaType.Flat);
      _color.Update = true;
      hbox.Add(_color);

      _opacity = new SpinButton(0, 100, 1);
      hbox.Add(new Label(_("Opacity:")));
      hbox.Add(_opacity);
      hbox.Add(new Label("%"));
      AttachAligned(0, 3, _("Color:"), 0.0, 0.5, hbox, 1, true);
    }

    void CreatePositionWidget(VariableSet variables)
    {
      _position = new GimpComboBox(variables.Get<int>("position"),
				   new string[]{_("Centered"), _("Top Left"),
						_("Bottom Left"), 
						_("Top Right"),
						_("Bottom Right")});
      AttachAligned(0, 4, _("Position:"), 0.0, 0.5, _position, 1, false);
    }

    void CreateRotateWidget()
    {
      _rotate = CreateComboBox(_("None"), 
			       _("45 Degrees Right"),
			       _("90 Degrees Right"), 
			       _("45 Degrees Left"),
			       _("90 Degrees Left"));
      AttachAligned(0, 5, _("Rotate:"), 0.0, 0.5, _rotate, 1, false);
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
  }
}
