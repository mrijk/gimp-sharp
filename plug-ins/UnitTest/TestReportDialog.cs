// TestReportDialog for UnitTest plug-in
// Copyright (C) 2006 Massimo Perga  massimo.perga@gmail.com
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
using Gtk;
//using NUnit.Core;
//using NUnit.Util;

namespace Gimp.UnitTest
{
  public class TestReportDialog : GimpDialog
  {
    GimpColorButton _active;
    GimpColorButton _inactive;
    ArrayList				_resultsAL;
    Gtk.Entry filterEntry;

    Gtk.TreeModelFilter filter;

    public TestReportDialog(
        int passedNumber, int failedNumber, ArrayList resultsAL) :
      base("UnitTest",
          "UnitTest", IntPtr.Zero, 0, null, "UnitTest", Stock.Ok, ResponseType.Ok)
    {
      string assembly;
      string testReport;
      string tmp;


      _resultsAL = resultsAL;

      SetSizeRequest (700,400);

      // ---------- Start Frame ---------- // 
      // Create a nice label describing the number of passed tests
      Gtk.Label passedLabel = new Gtk.Label ("Passed : " + passedNumber);
      // Create a nice label describing the number of failed tests
      Gtk.Label failedLabel = new Gtk.Label ("Failed : " + failedNumber);
      // Create the table in order to have the previous labels left aligned
      GimpTable table = new GimpTable(2, 10, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      // Create a frame containing the previous labels
      Frame frame = new Gtk.Frame("Tests results:");
      // ---------- End Frame ---------- // 

      // ---------- Start TreeView Filter ---------- // 
      // Create an Entry used to filter the tree
      filterEntry = new Gtk.Entry ();
      // Fire off an event when the text in the Entry changes
      filterEntry.Changed += OnFilterEntryTextChanged;
      // Create a nice label describing the Entry
      Gtk.Label filterLabel = new Gtk.Label ("Assembly Search:");
      // Put them both into a little box so they show up side by side
      Gtk.HBox filterBox = new Gtk.HBox ();
      filterBox.PackStart (filterLabel, false, false, 20);
      filterBox.PackEnd (filterEntry, true, true, 20);
      // ---------- End TreeView Filter ---------- // 

      // ---------- Start TreeView ---------- // 
      // Create our TreeView
      Gtk.TreeView tree = new Gtk.TreeView ();
      // Create a column for the assembly name
      Gtk.TreeViewColumn assemblyColumn = new Gtk.TreeViewColumn ();
      assemblyColumn.Title = "Assembly";
      // Create the text cell that will display the assenbly name
      Gtk.CellRendererText assemblyNameCell = new Gtk.CellRendererText ();
      // Add the cell to the column
      assemblyColumn.PackStart (assemblyNameCell, true);
      // Create a column for the result 
      Gtk.TreeViewColumn resultColumn = new Gtk.TreeViewColumn ();
      resultColumn.Title = "Result";
      // Create the text cell that will display the result 
      Gtk.CellRendererText resultReportCell = new Gtk.CellRendererText ();
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
      Gtk.ListStore resultListStore = new Gtk.ListStore (
          typeof (string), typeof (string));

      // Add some data to the store
      for(int i = 0; i < _resultsAL.Count; i++)
      {
        tmp = (string)_resultsAL[i];

        int pos = tmp.IndexOf(":");
        assembly = tmp.Substring(0, pos);
        // +2 because of the ': '
        testReport = tmp.Substring(pos+2, tmp.Length-(pos+2));
        resultListStore.AppendValues(assembly, testReport);
      }

      // Set the renderer for the assembly cell
      assemblyColumn.SetCellDataFunc (assemblyNameCell, new Gtk.TreeCellDataFunc (RenderAssembly));
      // Set the renderer for the result cell
      resultColumn.SetCellDataFunc (resultReportCell, new Gtk.TreeCellDataFunc (RenderResult));


      filter = new Gtk.TreeModelFilter (resultListStore, null);
      filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree);
      tree.Model = filter;
      // Insert the TreeView inside a ScrolledWindow to add scrolling 
      ScrolledWindow sw = new ScrolledWindow ();
      sw.ShadowType = ShadowType.EtchedIn;
      sw.SetPolicy (PolicyType.Automatic, PolicyType.Automatic);
      sw.Add(tree);

      HBox swBox = new HBox();

      swBox.PackStart (sw, true, true, 20);

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
    private void OnFilterEntryTextChanged (object o, System.EventArgs args)
    {
      // Since the filter text changed, tell the filter to re-determine which rows to display
      filter.Refilter ();
    }

    private bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
    {
      string testName = model.GetValue (iter, 0).ToString ();

      if (filterEntry.Text == "")
        return true;

      if (testName.IndexOf (filterEntry.Text) > -1)
        return true;
      else
        return false;
    }

    private void RenderAssembly (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, 
        Gtk.TreeModel model, Gtk.TreeIter iter)
    {
      string assembly = (string) model.GetValue (iter, 0);
      string result = (string) model.GetValue (iter, 1);
      if(result.CompareTo("OK") == 0) 
      {
        (cell as Gtk.CellRendererText).Foreground = "darkgreen";
      } 
      else 
      {
        (cell as Gtk.CellRendererText).Foreground = "red";
      }
      (cell as Gtk.CellRendererText).Text = assembly;
    }

    private void RenderResult (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, 
        Gtk.TreeModel model, Gtk.TreeIter iter)
    {
      string result = (string) model.GetValue (iter, 1);
      if(result.CompareTo("OK") == 0) 
      {
        (cell as Gtk.CellRendererText).Foreground = "darkgreen";
      } 
      else 
      {
        (cell as Gtk.CellRendererText).Foreground = "red";
      }
      (cell as Gtk.CellRendererText).Text = result;
    }

  }

}
