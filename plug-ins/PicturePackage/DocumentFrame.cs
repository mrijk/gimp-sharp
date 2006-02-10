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
  public class DocumentFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    ComboBox _size;
    ComboBox _layout;

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

      _size = ComboBox.NewText();
      FillPageSizeMenu(layoutSet);
      _size.Changed += OnSizeChanged;
      Table.AttachAligned(0, 0, "_Page Size:", 0.0, 0.5, _size, 2, false);

      _layout = ComboBox.NewText();
      FillLayoutMenu(_layoutSet);
      _layout.Changed += OnLayoutChanged;
      Table.AttachAligned(0, 1, "_Layout:", 0.0, 0.5, _layout, 2, false);

      SpinButton resolution = new SpinButton (_resolution, 1200, 1);
      Table.AttachAligned(0, 2, "_Resolution:", 0.0, 0.5, resolution, 1, true);
      resolution.ValueChanged += OnResolutionChanged;

      ComboBox units = CreateComboBox("pixels/inch", "pixels/cm", "pixels/mm");
      units.Active = parent.Units;
      units.Changed += OnUnitsChanged;
      Table.Attach(units, 2, 3, 2, 3);	

      ComboBox mode = CreateComboBox("Grayscale", "RGB Color");
      mode.Active =  _parent.ColorMode;
      mode.Changed += OnModeChanged;
      Table.AttachAligned(0, 3, "_Mode:", 0.0, 0.5, mode, 2, false);

      CheckButton flatten = new CheckButton("Flatten All Layers");
      flatten.Active = parent.Flatten;
      flatten.Toggled += FlattenToggled;
      Table.Attach(flatten, 0, 2, 4, 5);
    }

    void FillPageSizeMenu(LayoutSet layoutSet)
    {
      _sizes = layoutSet.GetPageSizeSet(_resolution);
      // TODO: clear previous entries
      foreach (PageSize size in _sizes)
	{
	  _size.AppendText(String.Format("{0,1:f1} x {1,1:f1} inches", 
					 size.Width, size.Height));
	}
      _size.Active = 0;
    }

    void FillLayoutMenu(LayoutSet layoutSet)
    {
      // TODO: clear previous entries
      foreach (Layout layout in layoutSet)
	{
	  _layout.AppendText(layout.Name);
	}
      _layout.Active = 0;
    }

    void OnSizeChanged (object o, EventArgs args) 
    {
      int nr = (o as ComboBox).Active;
      _layoutSet = _fullLayoutSet.GetLayouts(_sizes[nr], _resolution);
      FillLayoutMenu(_layoutSet);
    }

    void OnLayoutChanged (object o, EventArgs args) 
    {
      int nr = (o as ComboBox).Active;
      _fullLayoutSet.Selected = _layoutSet[nr];
    }

    void OnResolutionChanged (object o, EventArgs args) 
    {
      _parent.Resolution = (o as SpinButton).ValueAsInt;
    }

    void OnUnitsChanged (object o, EventArgs args) 
    {
      _parent.Resolution = (o as ComboBox).Active;
    }

    void OnModeChanged (object o, EventArgs args) 
    {
      _parent.ColorMode = (o as ComboBox).Active;
    }

    void FlattenToggled (object sender, EventArgs args)
    {
      _parent.Flatten = (sender as CheckButton).Active;
    }
  }
}
