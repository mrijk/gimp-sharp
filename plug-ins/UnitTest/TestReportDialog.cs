// TestReportDialog for UnitTest plug-in
// Copyright (C) 2006-2010 Maurits Rijk
//
// TestReportDialog.cs
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
using System.Collections;
using System.Collections.Generic;
using Gtk;

namespace Gimp.UnitTest
{
  public class TestReportDialog : GimpDialog
  {
    Entry _filterEntry;

    TreeModelFilter filter;

    public TestReportDialog(int passedNumber, int failedNumber, 
			    List<string> resultsAL) :
      base("UnitTest", "UnitTest", IntPtr.Zero, 0, null, "UnitTest", 
	   Stock.Ok, ResponseType.Ok)
    {
      string testReport;

      SetSizeRequest(700,400);

      var passedLabel = new Label("Passed : " + passedNumber);
      var failedLabel = new Label("Failed : " + failedNumber);
      var table = new GimpTable(2, 10, false) {
	BorderWidth = 12, ColumnSpacing = 6, RowSpacing = 6};

      var frame = new Frame("Tests results:");

      _filterEntry = new Entry();
      _filterEntry.Changed += delegate
	{
	  filter.Refilter();
	};

      var filterLabel = new Label("Assembly Search:");
      var filterBox = new HBox();
      filterBox.PackStart(filterLabel, false, false, 20);
      filterBox.PackEnd(_filterEntry, true, true, 20);

      var tree = new TreeView();
      var assemblyColumn = new TreeViewColumn() {Title = "Assembly"};

      var assemblyNameCell = new CellRendererText();
      assemblyColumn.PackStart(assemblyNameCell, true);
      var resultColumn = new TreeViewColumn() {Title = "Result"};

      var resultReportCell = new CellRendererText() {Editable = false};

      resultColumn.PackStart(resultReportCell, true);
      tree.AppendColumn(assemblyColumn);
      tree.AppendColumn(resultColumn);

      var resultListStore = new ListStore(typeof(string), typeof(string));
      foreach (string tmp in resultsAL)
	{
	  int pos = tmp.IndexOf(":");
	  string assembly = tmp.Substring(0, pos);
	  // +2 because of the ': '
	  testReport = tmp.Substring(pos + 2, tmp.Length - (pos + 2));
	  resultListStore.AppendValues(assembly, testReport);
	}

      assemblyColumn.SetCellDataFunc(assemblyNameCell, 
				     new TreeCellDataFunc(RenderAssembly));
      resultColumn.SetCellDataFunc(resultReportCell, 
				   new TreeCellDataFunc(RenderResult));

      filter = new TreeModelFilter(resultListStore, null);
      filter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree);
      tree.Model = filter;

      var sw = new ScrolledWindow() {ShadowType = ShadowType.EtchedIn};
      sw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
      sw.Add(tree);

      var swBox = new HBox();

      swBox.PackStart(sw, true, true, 20);

      table.AttachAligned(0, 0, "", 0.0, 0.5, passedLabel, 1, true);
      table.AttachAligned(0, 1, "", 0.0, 0.5, failedLabel, 1, true);
      frame.Add(table);
      var dummyFrameBox = new HBox();
      dummyFrameBox.PackStart(frame, true, true, 20);
      VBox.PackStart(dummyFrameBox, false, false, 0);

      VBox.PackStart(swBox, true, true, 0);
      VBox.PackStart(filterBox, false, false , 0);
    }

    bool FilterTree(TreeModel model, TreeIter iter)
    {
      string testName = model.GetValue(iter, 0).ToString();

      if (_filterEntry.Text == "")
        return true;

      return testName.IndexOf(_filterEntry.Text) > -1;
    }

    void RenderAssembly(TreeViewColumn column, CellRenderer cell, 
			TreeModel model, TreeIter iter)
    {
      string assembly = (string) model.GetValue(iter, 0);
      string result = (string) model.GetValue(iter, 1);
      var text = cell as CellRendererText;

      text.Foreground = (result == "OK") ? "darkgreen" : "red";
      text.Text = assembly;
    }

    void RenderResult(TreeViewColumn column, CellRenderer cell, 
		      TreeModel model, TreeIter iter)
    {
      string result = (string) model.GetValue(iter, 1);
      var text = cell as CellRendererText;
      text.Foreground = (result == "OK") ? "darkgreen" : "red";
      text.Text = result;
    }
  }
}
