using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class LabelFrame : GimpFrame
  {
    public LabelFrame() : base("Label")
    {
      GimpTable table = new GimpTable(3, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      Add(table);

      OptionMenu content = new OptionMenu();
      Menu menu = new Menu();
      menu.Append(new MenuItem("None"));
      menu.Append(new MenuItem("Custom Text"));
      menu.Append(new MenuItem("Filename"));
      menu.Append(new MenuItem("Copyright"));
      menu.Append(new MenuItem("Caption"));
      menu.Append(new MenuItem("Credits"));
      menu.Append(new MenuItem("Title"));
      content.Menu = menu;
      table.AttachAligned(0, 0, "Content:", 0.0, 0.5,
			  content, 1, false);

      Entry entry = new Entry();
      table.AttachAligned(0, 1, "Custom Text:", 0.0, 0.5,
			  entry, 1, true);
#if false
      GimpFontSelectWidget font = new GimpFontSelectWidget(null, 
							   "Monospace");
      table.AttachAligned(0, 2, "Font:", 0.0, 0.5,
			  font, 1, true);
#endif
      RGB rgb = new RGB(0, 0, 0);

      GimpColorButton color = new GimpColorButton("", 16, 16, rgb.GimpRGB,
						  ColorAreaType.COLOR_AREA_FLAT);
      table.AttachAligned(0, 2, "Color:", 0.0, 0.5,
			  color, 1, true);

      OptionMenu position = new OptionMenu();
      menu = new Menu();
      menu.Append(new MenuItem("Centered"));
      menu.Append(new MenuItem("Top Left"));
      menu.Append(new MenuItem("Bottom Left"));
      menu.Append(new MenuItem("Top Right"));
      menu.Append(new MenuItem("Bottom Right"));
      position.Menu = menu;
      table.AttachAligned(0, 3, "Position:", 0.0, 0.5,
			  position, 1, false);

      OptionMenu rotate = new OptionMenu();
      menu = new Menu();
      menu.Append(new MenuItem("None"));
      menu.Append(new MenuItem("45 Degrees Right"));
      menu.Append(new MenuItem("90 Degrees Right"));
      menu.Append(new MenuItem("45 Degrees Left"));
      menu.Append(new MenuItem("90 Degrees Left"));
      rotate.Menu = menu;
      table.AttachAligned(0, 4, "Rotate:", 0.0, 0.5,
			  rotate, 1, false);		
    }
  }
  }
