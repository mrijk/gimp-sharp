using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class DocumentFrame : GimpFrame
  {
    OptionMenu _layout;
    Entry _resolution;
    LayoutSet _layoutSet;
    PageSizeSet _sizes;
    bool _flatten;

    public DocumentFrame(LayoutSet layoutSet) : base("Document")
    {
      _layoutSet = layoutSet;

      GimpTable table = new GimpTable(5, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      Add(table);

      OptionMenu _size = new OptionMenu();
      Menu menu = new Menu();
      _sizes = _layoutSet.GetPageSizeSet(72 /* _res */);
      foreach (PageSize size in _sizes)
	{
	menu.Append(new MenuItem(String.Format("{0,1:f1} x {1,1:f1} inches", size.Width, size.Height)));
	}

      _size.Menu = menu;
      _size.Changed += new EventHandler(OnSizeChanged);
      table.AttachAligned(0, 0, "_Page Size:", 0.0, 0.5, _size, 2, false);

      _layout = new OptionMenu();
      FillLayoutMenu(_layoutSet);
      _layout.Changed += new EventHandler(OnLayoutChanged);
      table.AttachAligned(0, 1, "_Layout:", 0.0, 0.5,
			  _layout, 2, false);

      _resolution = new Entry();
      _resolution.WidthChars = 4;
      _resolution.Text = "72";
      table.AttachAligned(0, 2, "_Resolution:", 0.0, 0.5, _resolution, 1, true);
	
      OptionMenu units = new OptionMenu();
      menu = new Menu();
      menu.Append(new MenuItem("pixels/inch"));
      menu.Append(new MenuItem("pixels/cm"));
      menu.Append(new MenuItem("pixels/mm"));
      units.Menu = menu;
      table.Attach(units, 2, 3, 2, 3);	

      OptionMenu mode = new OptionMenu();
      menu = new Menu();
      menu.Append(new MenuItem("Grayscale"));
      menu.Append(new MenuItem("RGB Color"));
      mode.Menu = menu;
      mode.SetHistory(1);
      table.AttachAligned(0, 3, "_Mode:", 0.0, 0.5, mode, 2, false);

      CheckButton flatten = new CheckButton("Flatten All Layers");
      flatten.Toggled += new EventHandler(FlattenToggled);
      table.Attach(flatten, 0, 2, 4, 5);
    }

    void FillLayoutMenu(LayoutSet layoutSet)
    {
      Menu menu = new Menu();
      foreach (Layout layout in layoutSet)
	{
	menu.Append(new MenuItem(layout.Name));
	}
      menu.ShowAll();
      _layout.Menu = menu;
    }

    void OnSizeChanged (object o, EventArgs args) 
    {
      int nr = (o as OptionMenu).History;
      // LayoutSet set = _layoutSet.GetLayouts(_sizes[nr], 72); // Problem here!!??
      // FillLayoutMenu(set);
    }

    void OnLayoutChanged (object o, EventArgs args) 
    {
      int nr = (o as OptionMenu).History;
      _layoutSet.Selected = _layoutSet[nr];
    }

    void FlattenToggled (object sender, EventArgs args)
    {
      _flatten = (sender as CheckButton).Active;
    }

    public bool Flatten
    {
      get {return _flatten;}
    }
  }
  }
