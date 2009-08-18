// The CountTool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// CountTool.cs
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
using System.Collections.Generic;

using Gtk;

namespace Gimp.CountTool
{
  class CountTool : Plugin
  {
    readonly CoordinateList<int> _coordinates = new CoordinateList<int>();

    static void Main(string[] args)
    {
      new CountTool(args);
    }

    CountTool(string[] args) : base(args, "CountTool")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_count_tool",
				 _("Count Tool"),
				 _("Count Tool"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2006-2009",
				 _("Count Tool..."),
				 "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Generic"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("CountTool", true);

      var dialog = DialogNew("CountTool 0.1", "CountTool", IntPtr.Zero, 
			     0, Gimp.StandardHelpFunc, "CountTool");

      var hbox = new HBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(hbox, true, true, 0);

      var preview = new Preview(_drawable, _coordinates);
      hbox.PackStart(preview, true, true, 0);

      var sw = new ScrolledWindow();
      hbox.Add(sw);

      var store = new TreeStore(typeof(Coordinate<int>));
      for (int i = 0; i < 10; i++)
	{
	  var coordinate = new Coordinate<int>(10 * i, 10 * i);
	  _coordinates.Add(coordinate);
	  store.AppendValues(coordinate);
	}

      var view = new TreeView(store);
      sw.Add(view);        

      var textRenderer = new CellRendererText();
      view.AppendColumn("X", textRenderer, new TreeCellDataFunc(RenderX));
      view.AppendColumn("Y", textRenderer, new TreeCellDataFunc(RenderY));
      
      return dialog;
    }

    void RenderX(TreeViewColumn column, CellRenderer cell, 
		 TreeModel model, TreeIter iter)
    {
      RenderCoordinate(cell, getCoordinate(model, iter).X);
    }

    void RenderY(TreeViewColumn column, CellRenderer cell, 
		 TreeModel model, TreeIter iter)
    {
      RenderCoordinate(cell, getCoordinate(model, iter).Y);
    }

    Coordinate<int> getCoordinate(TreeModel model, TreeIter iter)
    {
      return model.GetValue(iter, 0) as Coordinate<int>;
    }

    void RenderCoordinate(CellRenderer cell, int value)
    {
      var text = cell as CellRendererText;
      text.Text = String.Format("{0}", value);
      text.Editable = true;
    }

    override protected void Render(Drawable drawable)
    {
    }
  }
}
