// The Slice Tool plug-in
// Copyright (C) 2004-2016 Maurits Rijk
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

using System.Reflection;

using Mono.Unix;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Toolbox : Toolbar
  {
    readonly Preview _preview;

    public Toolbox(Preview preview, SliceData sliceData)
    {
      _preview = preview;

      CreateStockIcons();
      
      Orientation = Gtk.Orientation.Vertical;
      ToolbarStyle = Gtk.ToolbarStyle.Icons;

      var toggle = CreateSelectToggle(sliceData);
      toggle = CreateSliceToggle(toggle, sliceData);
      toggle.Active = true;
      toggle = CreateEraserToggle(toggle, sliceData);
      CreateTableToggle(toggle, sliceData);
    }

    RadioToolButton CreateSelectToggle(SliceData sliceData)
    {
      return AddToggle(null, "slice-tool-arrow",
		       _("Select Rectangle or Slice"),
		       new SelectFunc(sliceData, _preview));
    }

    RadioToolButton CreateSliceToggle(RadioToolButton group,
				      SliceData sliceData)
    {
      return AddToggle(group, GimpStock.TOOL_CROP, _("Create a new Slice"),
		       new CreateFunc(sliceData, _preview));
    }

    RadioToolButton CreateEraserToggle(RadioToolButton group, 
				       SliceData sliceData)
    {
      return AddToggle(group, GimpStock.TOOL_ERASER, _("Remove Slice"), 
		       new RemoveFunc(sliceData, _preview));
    }

    RadioToolButton CreateTableToggle(RadioToolButton group, 
				      SliceData sliceData)
    {
      return AddToggle(group, GimpStock.GRID, _("Insert Table"),
		       new CreateTableFunc(sliceData, _preview));
    }

    RadioToolButton AddToggle(RadioToolButton group, string icon, 
			      string tooltipText, MouseFunc func)
    {
      var list = group?.Group;

      var toggle = new RadioToolButton(list, icon) {TooltipText = tooltipText};
      toggle.Clicked += delegate
	{
	  if (toggle.Active)
	  {
	    _preview.Func = func;
	  }
	};

      Insert(toggle, -1);
      return toggle;
    }

    string _(string s) => Catalog.GetString(s);

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
