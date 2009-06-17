// TestReportDialog for UnitTest plug-in
// Copyright (C) 2006-2007 Massimo Perga  massimo.perga@gmail.com
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
    GimpColorButton _active;
    GimpColorButton _inactive;
    List<string>    _resultsAL;
    Entry filterEntry;

    TreeModelFilter filter;

    public TestReportDialog(int passedNumber, int failedNumber, 
			    List<string> resultsAL) :
      base("UnitTest", "UnitTest", IntPtr.Zero, 0, null, "UnitTest", 
	   Stock.Ok, ResponseType.Ok)
    {
      string testReport;

      _resultsAL = resultsAL;

      SetSizeRequest (700,400);

      // ---------- Start Frame ---------- // 
      // Create a nice label describing the number of passed tests
      Label passedLabel = new Label ("Passed : " + passedNumber);
      // Create a nice label describing the number of failed tests
      Label failedLabel = new Label ("Failed : " + failedNumber);
      // Create the table in order to have the previous labels left aligned
      GimpTable table = new GimpTable(2, 10, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      // Create a frame containing the previous labels
      Frame frame = new Frame("Tests results:");
      // ---------- End Frame ---------- // 

      // ---------- Start TreeView Filter ---------- // 
      // Create an Entry used to filter the tree
      filterEntry = new Entry ();
      // Fire off an event when the text in the Entry changes
      filterEntry.Changed += delegate
	{
	  filter.Refilter ();
	};

      // Create a nice label describing the Entry
      Label filterLabel = new Label ("Assembly Search:");
      // Put them both into a little box so they show up side by side
      HBox filterBox = new HBox ();
      filterBox.PackStart (filterLabel, false, false, 20);
      filterBox.PackEnd (filterEntry, true, true, 20);
      // ---------- End TreeView Filter ---------- // 

      // ---------- Start TreeView ---------- // 
      // Create our TreeView
      TreeView tree = new TreeView ();
      // Create a column for the assembly name
      TreeViewColumn assemblyColumn = new TreeViewColumn ();
      assemblyColumn.Title = "Assembly";
      // Create the text cell that will display the assenbly name
      CellRendererText assemblyNameCell = new CellRendererText ();
      // Add the cell to the column
      assemblyColumn.PackStart (assemblyNameCell, true);
      // Create a column for the result 
      TreeViewColumn resultColumn = new TreeViewColumn ();
      resultColumn.Title = "Result";
      // Create the text cell that will display the result 
      CellRendererText resultReportCell = new CellRendererText ();
      // Avoid to edit the cell
      resultReportCell.Editable = false;
      // Add the cell to the column
      resultColumn.PackStart (resultReportCell, true);
      // Add the columns to the TreeView
      tree.AppendColumn (assemblyColumn);
      tree.AppendColumn (resultColumn);

      // Tell the Cell Renderers which items in the model to display
      /*
      assemblyColumn.AddAttribute (assemblyNameCell, "text", 0);
      resultColumn.AddAttribute (resultReportCell, "text", 1);
      */

      // Create a model that will hold two strings - Assembly Name 
      // and Unit Test Error 
      ListStore resultListStore = new ListStore(typeof (string), 
						typeof (string));
      // Add some data to the store
      for (int i = 0; i < _resultsAL.Count; i++)
	{
	  string tmp = _resultsAL[i];

	  int pos = tmp.IndexOf(":");
	  string assembly = tmp.Substring(0, pos);
	  // +2 because of the ': '
	  testReport = tmp.Substring(pos + 2, tmp.Length - (pos + 2));
	  resultListStore.AppendValues(assembly, testReport);
	}

      Console.WriteLine("TRD: 2");

      // Set the renderer for the assembly cell
      assemblyColumn.SetCellDataFunc(assemblyNameCell, 
				     new TreeCellDataFunc(RenderAssembly));
      // Set the renderer for the result cell
      resultColumn.SetCellDataFunc(resultReportCell, 
				   new TreeCellDataFunc(RenderResult));

      filter = new TreeModelFilter(resultListStore, null);
      filter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree);
      tree.Model = filter;
      // Insert the TreeView inside a ScrolledWindow to add scrolling 
      ScrolledWindow sw = new ScrolledWindow();
      sw.ShadowType = ShadowType.EtchedIn;
      sw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
      sw.Add(tree);

      HBox swBox = new HBox();

      swBox.PackStart(sw, true, true, 20);

      // Attach the labels to the table
      table.AttachAligned(0, 0, "", 0.0, 0.5, passedLabel, 1, true);
      table.AttachAligned(0, 1, "", 0.0, 0.5, failedLabel, 1, true);
      // Include the table inside the frame 
      frame.Add(table);
      HBox dummyFrameBox = new HBox();
      // Pack the frame
      dummyFrameBox.PackStart(frame, true, true, 20);
      VBox.PackStart(dummyFrameBox, false, false, 0);

      VBox.PackStart(swBox, true, true, 0);
      VBox.PackStart(filterBox, false, false , 0);
    }

    public RGB ActiveColor
    {
      get {return _active.Color;}
      set {_active.Color = value;}
    }

    public RGB InactiveColor
    {
      get {return _inactive.Color;}
      set {_inactive.Color = value;}
    }

    bool FilterTree(TreeModel model, TreeIter iter)
    {
      string testName = model.GetValue (iter, 0).ToString ();

      if (filterEntry.Text == "")
        return true;

      return (testName.IndexOf(filterEntry.Text) > -1);
    }

    void RenderAssembly(TreeViewColumn column, CellRenderer cell, 
			TreeModel model, TreeIter iter)
    {
      string assembly = (string) model.GetValue(iter, 0);
      string result = (string) model.GetValue(iter, 1);
      CellRendererText text = cell as CellRendererText;

      text.Foreground = (result == "OK") ? "darkgreen" : "red";
      text.Text = assembly;
    }

    void RenderResult(TreeViewColumn column, CellRenderer cell, 
		      TreeModel model, TreeIter iter)
    {
      string result = (string) model.GetValue (iter, 1);

      if (result.CompareTo("OK") == 0) 
	{
	  (cell as CellRendererText).Foreground = "darkgreen";
	} 
      else 
	{
	  (cell as CellRendererText).Foreground = "red";
	}
      (cell as CellRendererText).Text = result;
    }
  }
}
