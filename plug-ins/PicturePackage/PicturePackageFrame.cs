using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class PicturePackageFrame : GimpFrame
  {
    protected GimpTable _table;

    protected PicturePackageFrame(uint rows, uint columns, string label) : 
      base(label)
    {
      _table = new GimpTable(rows, columns, false);
      _table.ColumnSpacing = 6;
      _table.RowSpacing = 6;
      Add(_table);
    }

    protected OptionMenu CreateOptionMenu(params string[] items)
    {
      OptionMenu option = new OptionMenu();
      Menu menu = new Menu();

      foreach (string item in items)
	{
	menu.Append(new MenuItem(item));
	}
      option.Menu = menu;

      return option;
    }

    protected GimpTable Table
    {
      get {return _table;}
    }
  }
  }
