using System;
using Gtk;

namespace Gimp.SliceTool
{
	public class Format : GimpFrame
	{
		OptionMenu _format;
		CheckButton _apply;

		public Format() : base("Format")
		{
			Table table = new Table(2, 2, true);
			table.RowSpacing = 6;
			Add(table);

			_format = new OptionMenu();
			table.Attach(_format, 0, 1, 0, 1);

			Menu menu = new Menu();
			menu.Append(new MenuItem("gif"));
			menu.Append(new MenuItem("jpg"));
			menu.Append(new MenuItem("png"));
			_format.Menu = menu;

			_apply = new CheckButton("Apply to whole image");
			table.Attach(_apply, 0, 2, 1, 2);
		}

		public string Extension
		{
			set
			{
				if (value == ".gif")
				{
					_format.SetHistory(0);
				}
				else if (value == ".jpg" || value == ".jpeg")
				{
					_format.SetHistory(1);
				}
				else if (value == ".png")
				{
					_format.SetHistory(2);
				}
				else
				{
					_format.SetHistory(2);
				}
			}

			get
			{
				switch (_format.History)
				{
					case 0:
						return "gif";
					case 1:
						return "jpg";
					case 2:
						return "png";
					default:
						return null;
				}
			}
		}
	}
}