// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// Toolbox.cs
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
using System.Reflection;

using Mono.Unix;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Toolbox : Toolbar // HandleBox
  {
    readonly SliceTool _parent;

    ToggleToolButton _toggle;
    bool _lock;

    public Toolbox(SliceTool parent, SliceData sliceData)
    {
      _parent = parent;

      CreateStockIcons();
      
      Orientation = Gtk.Orientation.Vertical;
      ToolbarStyle = Gtk.ToolbarStyle.Icons;

      CreateSelectToggle(sliceData);
      CreateSliceToggle(sliceData);
      CreateEraserToggle(sliceData);
      CreateTableToggle(sliceData);
    }

    void CreateSelectToggle(SliceData sliceData)
    {
      var toggle = AddToggle("slice-tool-arrow", _("Select Rectangle"));
      _toggle = toggle;
      toggle.Active = true;
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new SelectFunc(_parent, sliceData));
	};
    }

    void CreateSliceToggle(SliceData sliceData)
    {
      var toggle = AddToggle(GimpStock.TOOL_CROP, _("Create a new Slice"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new CreateFunc(sliceData, _parent.Preview));
	};
    }

    void CreateEraserToggle(SliceData sliceData)
    {
      var toggle = AddToggle(GimpStock.TOOL_ERASER, _("Remove Slice"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new RemoveFunc(sliceData, _parent.Preview));
	};

    }

    void CreateTableToggle(SliceData sliceData)
    {
      var toggle = AddToggle(GimpStock.GRID, _("Insert Table"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new CreateTableFunc(sliceData, _parent.Preview));
	};
    }

    ToggleToolButton AddToggle(string icon, string tooltipText)
    {
      var toggle = new ToggleToolButton(icon) {TooltipText = tooltipText};
      Insert(toggle, -1);
      return toggle;
    }

    void OnFunc(object o, MouseFunc func)
    {
      if (!_lock)
	{
	  _lock = true;
	  var toggle = (o as ToggleToolButton);
	  if (toggle != _toggle)
	    {
	      _toggle.Active = false;
	      _toggle = toggle;
	      _parent.Func = func;
	    } 
	  else
	    {
	      _toggle.Active = true;
	    }
	  _lock = false;
	}
    }

    string _(string s)
    {
      return Catalog.GetString(s);
    }

    void AddStockIcon(IconFactory factory, string stockId, string filename)
    {
      var pixbuf = new Pixbuf(Assembly.GetCallingAssembly(), filename);

      var source = new IconSource() {
	Pixbuf = pixbuf, 
	SizeWildcarded = true,
	Size = IconSize.SmallToolbar};

      var set = new IconSet();
      set.AddSource(source);

      factory.Add(stockId, set);
    }

    void CreateStockIcons()
    {
      var factory = new IconFactory();
      factory.AddDefault();
      AddStockIcon(factory, "slice-tool-arrow", "stock-arrow.png");
    }
  }
}
