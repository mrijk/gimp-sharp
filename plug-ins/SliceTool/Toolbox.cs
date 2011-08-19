// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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

    public Toolbox(SliceTool parent, SliceData sliceData)
    {
      _parent = parent;

      CreateStockIcons();
      
      Orientation = Gtk.Orientation.Vertical;
      ToolbarStyle = Gtk.ToolbarStyle.Icons;

      var toggle = CreateSelectToggle(sliceData);
      toggle = CreateSliceToggle(toggle, sliceData);
      toggle = CreateEraserToggle(toggle, sliceData);
      CreateTableToggle(toggle, sliceData);
    }

    RadioToolButton CreateSelectToggle(SliceData sliceData)
    {
      var toggle = AddToggle(null, "slice-tool-arrow", 
			     _("Select Rectangle or Slice"));

      toggle.Active = true;
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new SelectFunc(sliceData, _parent.Preview));
	};
      return toggle;
    }

    RadioToolButton CreateSliceToggle(RadioToolButton group,
				      SliceData sliceData)
    {
      var toggle = AddToggle(group, GimpStock.TOOL_CROP, 
			     _("Create a new Slice"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new CreateFunc(sliceData, _parent.Preview));
	};
      return toggle;
    }

    RadioToolButton CreateEraserToggle(RadioToolButton group, 
				       SliceData sliceData)
    {
      var toggle = AddToggle(group, GimpStock.TOOL_ERASER, _("Remove Slice"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new RemoveFunc(sliceData, _parent.Preview));
	};
      return toggle;
    }

    RadioToolButton CreateTableToggle(RadioToolButton group, 
				      SliceData sliceData)
    {
      var toggle = AddToggle(group, GimpStock.GRID, _("Insert Table"));
      toggle.Clicked += delegate
	{
	  OnFunc(toggle, new CreateTableFunc(sliceData, _parent.Preview));
	};
      return toggle;
    }

    RadioToolButton AddToggle(RadioToolButton group, string icon, 
			      string tooltipText)
    {
      var list = (group == null) ? null : group.Group;

      var toggle = new RadioToolButton(list, icon) {TooltipText = tooltipText};
      Insert(toggle, -1);
      return toggle;
    }

    void OnFunc(ToggleToolButton toggle, MouseFunc func)
    {
      if (toggle.Active)
      {
	_parent.Func = func;
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
