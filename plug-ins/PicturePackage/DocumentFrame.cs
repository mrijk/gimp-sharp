using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class DocumentFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    OptionMenu _size;
    OptionMenu _layout;

    LayoutSet _fullLayoutSet;
    LayoutSet _layoutSet;
    PageSizeSet _sizes;

    int _resolution;

    public DocumentFrame(PicturePackage parent, LayoutSet layoutSet) : 
      base(5, 3, "Document")
    {
      _parent = parent;
      _fullLayoutSet = layoutSet;
      _layoutSet = layoutSet;
      _resolution = parent.Resolution;

      _size = new OptionMenu();
      FillPageSizeMenu(layoutSet);
      _size.Changed += new EventHandler(OnSizeChanged);
      Table.AttachAligned(0, 0, "_Page Size:", 0.0, 0.5, _size, 2, false);

      _layout = new OptionMenu();
      FillLayoutMenu(_layoutSet);
      _layout.Changed += new EventHandler(OnLayoutChanged);
      Table.AttachAligned(0, 1, "_Layout:", 0.0, 0.5, _layout, 2, false);

      SpinButton resolution = new SpinButton (_resolution, 1200, 1);
      Table.AttachAligned(0, 2, "_Resolution:", 0.0, 0.5, resolution, 1, true);
      resolution.ValueChanged += new EventHandler(OnResolutionChanged);

      OptionMenu units = CreateOptionMenu("pixels/inch", "pixels/cm",
					  "pixels/mm");
      units.SetHistory((uint) parent.Units);
      units.Changed += new EventHandler(OnUnitsChanged);
      Table.Attach(units, 2, 3, 2, 3);	

      OptionMenu mode = CreateOptionMenu("Grayscale", "RGB Color");
      mode.SetHistory((uint) _parent.ColorMode);
      mode.Changed += new EventHandler(OnModeChanged);
      Table.AttachAligned(0, 3, "_Mode:", 0.0, 0.5, mode, 2, false);

      CheckButton flatten = new CheckButton("Flatten All Layers");
      flatten.Active = parent.Flatten;
      flatten.Toggled += new EventHandler(FlattenToggled);
      Table.Attach(flatten, 0, 2, 4, 5);
    }

    void FillPageSizeMenu(LayoutSet layoutSet)
    {
      Menu menu = new Menu();
      _sizes = layoutSet.GetPageSizeSet(_resolution);
      foreach (PageSize size in _sizes)
	{
	menu.Append(new MenuItem(String.Format("{0,1:f1} x {1,1:f1} inches", 
					       size.Width, size.Height)));
	}
      _size.Menu = menu;
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
      _layoutSet = _fullLayoutSet.GetLayouts(_sizes[nr], _resolution);
      FillLayoutMenu(_layoutSet);
    }

    void OnLayoutChanged (object o, EventArgs args) 
    {
      int nr = (o as OptionMenu).History;
      _fullLayoutSet.Selected = _layoutSet[nr];
    }

    void OnResolutionChanged (object o, EventArgs args) 
    {
      _parent.Resolution = (o as SpinButton).ValueAsInt;
    }

    void OnUnitsChanged (object o, EventArgs args) 
    {
      _parent.Resolution = (o as OptionMenu).History;
    }

    void OnModeChanged (object o, EventArgs args) 
    {
      _parent.ColorMode = (o as OptionMenu).History;
    }

    void FlattenToggled (object sender, EventArgs args)
    {
      _parent.Flatten = (sender as CheckButton).Active;
    }
  }
  }
