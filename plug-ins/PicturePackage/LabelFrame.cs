using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class LabelFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    Entry _entry;
    OptionMenu _position;
    OptionMenu _rotate;
    GimpColorButton _color;
    SpinButton _opacity;

    public LabelFrame(PicturePackage parent) : base(3, 3, "Label")
    {
      _parent = parent;

      OptionMenu content = CreateOptionMenu(
	"None", "Custom Text", "Filename",
	"Copyright", "Caption", "Credits",
	"Title");
      content.SetHistory(0);
      content.Changed += new EventHandler(OnContentChanged);
      Table.AttachAligned(0, 0, "Content:", 0.0, 0.5,
			  content, 1, false);

      _entry = new Entry();
      _entry.Changed += new EventHandler(OnCustomTextChanged);
      Table.AttachAligned(0, 1, "Custom Text:", 0.0, 0.5,
			  _entry, 1, true);

      Button font = new Button(Stock.SelectFont);
      font.Clicked += new EventHandler(OnFontClicked);
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
      Table.AttachAligned(0, 3, "Color:", 0.0, 0.5,
			  hbox, 1, true);

      _position = CreateOptionMenu(
	"Centered", "Top Left", "Bottom Left",
	"Top Right", "Bottom Right");
      _position.Changed += new EventHandler(OnPositionChanged);
      Table.AttachAligned(0, 4, "Position:", 0.0, 0.5,
			  _position, 1, false);

      _rotate = CreateOptionMenu(
	"None", "45 Degrees Right",
	"90 Degrees Right", "45 Degrees Left",
	"90 Degrees Left");
      Table.AttachAligned(0, 5, "Rotate:", 0.0, 0.5,
			  _rotate, 1, false);

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
      SetLabelFrameSensitivity((o as OptionMenu).History);
    }

    void OnCustomTextChanged (object o, EventArgs args) 
    {
      _parent.Label = (o as Entry).Text;
    }

    void OnPositionChanged (object o, EventArgs args) 
    {
      _parent.Position = (o as OptionMenu).History;
    }

    void OnFontClicked (object o, EventArgs args) 
    {
      FontSelectionDialog fs = new FontSelectionDialog("Select Font");
      fs.Run();
      fs.Hide();
    }
  }
  }
