// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// PhotoshopActions.cs
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
using System.IO;

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class PhotoshopActions : Plugin
  {
    List<ActionSet> _set = new List<ActionSet>();

    static void Main(string[] args)
    {
      new PhotoshopActions(args);
    }

    public PhotoshopActions(string[] args) : base(args, "PhotoshopActions")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_photoshop_actions",
					  "Play Photoshop action files",
					  "Play Photoshop action files",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Photoshop Actions...",
					  "",
					  inParams);
      procedure.MenuPath = "<Toolbox>/Xtns/Extensions";
      procedure.IconFile = "PhotoshopActions.png";

      yield return procedure;
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("PhotoshopActions", true);

      GimpDialog dialog = DialogNew("Photoshop Actions 0.3", 
				    "PhotoshopActions",
				    IntPtr.Zero, 0, Gimp.StandardHelpFunc, 
				    "PhotoshopActions");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      TreeStore store = CreateActionTree();

      ScrolledWindow sw = new ScrolledWindow();
      sw.HeightRequest = 400;
      vbox.PackStart(sw, true, true, 0);
      
      TreeView view = new TreeView(store);
      sw.Add(view);        

      CellRendererText textRenderer = new CellRendererText ();
      TreeViewColumn column = 
	view.AppendColumn("Set Name",textRenderer, 
			  new TreeCellDataFunc(RenderText));

      HBox hbox = new HBox();
      vbox.PackStart(hbox, false, true, 0);

      Button play = new Button(Stock.Execute);
      play.Clicked += delegate(object sender, EventArgs args)
	{
	  TreePath[] paths = view.Selection.GetSelectedRows();
	  TreePath path = paths[0];	// Assume only 1 is selected

	  int[] indices = path.Indices;

	  ActionSet actions = _set[indices[0]];

	  if (indices.Length > 2)
	    {
	      actions.Execute(indices[1], indices[2]);
	      path.Next();
	      view.Selection.SelectPath(path);
	    }
	  else
	    {
	      actions.Execute(indices[1]);
	    }

	  Display.DisplaysFlush();
	};      
      hbox.PackStart(play, false, true, 0);

      view.Selection.Changed += delegate(object sender, EventArgs args)
	{
	  TreePath[] paths = view.Selection.GetSelectedRows();
	  int[] indices = paths[0].Indices;

	  play.Sensitive = (indices.Length > 1);
	};

      view.Selection.SetSelectFunction(delegate(TreeSelection selection, 
						TreeModel model, 
						TreePath path, 
						bool path_currently_selected)
      {
	return path.Indices.Length <= 3;
      }, 
				       IntPtr.Zero, null);

      return dialog;
    }

    private void RenderText(TreeViewColumn column, CellRenderer cell, 
			    TreeModel model, TreeIter iter)
    {
      string name = model.GetValue (iter, 0) as string;
      IExecutable executable = model.GetValue (iter, 1) as IExecutable;
      CellRendererText text = cell as CellRendererText;

      text.Text = name;

      if (executable != null)
	{
	  text.Foreground =  (executable.IsExecutable) ? "darkgreen" : "red";
	}
      else
	{
	  text.Foreground = "black";
	}
    }

    TreeStore CreateActionTree()
    {
      TreeStore store = new TreeStore(typeof(string), typeof(IExecutable));

      string scriptDir = Gimp.Directory + 
	System.IO.Path.DirectorySeparatorChar + "scripts";

      ActionParser parser = new ActionParser(_image, _drawable);

      int nrScripts = 0;

      DebugOutput.Quiet = true;

      foreach (string fileName in Directory.GetFiles(scriptDir))
	{
	  if (fileName.EndsWith(".atn"))
	    {
	      Console.WriteLine(fileName);
	      nrScripts++;

	      ActionSet actions = parser.Parse(fileName);

	      if (actions != null)
		{
		  _set.Add(actions);

		  TreeIter iter = store.AppendValues(actions.Name, actions);
		  foreach (Action action in actions)
		    {
		      TreeIter iter1 = store.AppendValues(iter, action.Name,
							  action);
		      foreach (ActionEvent actionEvent in action)
			{
			  // Console.WriteLine(actionEvent.EventForDisplay);
			  actionEvent.FillStore(store, iter1);
			}
		    }
		}
	    }
	}

      // Dump some statistics

      int nrExecutable = 0;
      int nrActions = 0;
      int nrEvents = 0;
      int nrExecutableActions = 0;
      int nrExecutableEvents = 0;

      foreach (ActionSet actions in _set)
	{
	  if (actions.IsExecutable)
	    {
	      nrExecutable++;
	    }
	  nrActions += actions.NrOfActions;
	  nrEvents += actions.ActionEvents;

	  nrExecutableActions += actions.ExecutableActions;
	  nrExecutableEvents += actions.ExecutableActionEvents;
	}

      double percParsed = (nrScripts - parser.ParsingFailed) * 100.0 / 
	nrScripts;
      double percExecutable = nrExecutable * 100.0 / nrScripts;
      double percExecutableActions = nrExecutableActions * 100.0 / nrActions;
      double percExecutableEvents = nrExecutableEvents * 100.0 / nrEvents;

      Console.WriteLine("#Total scripts      : " + nrScripts);
      Console.WriteLine("#Total actions      : " + nrActions);
      Console.WriteLine("#Total events       : " + nrEvents);
      Console.WriteLine("#Parsed             : " + _set.Count);
      Console.WriteLine("#Old                : " + parser.OldVersions);
      Console.WriteLine("#Failed             : " + parser.ParsingFailed);
      Console.WriteLine("#Scripts executable : " + nrExecutable);
      Console.WriteLine("#Actions executable : " + nrExecutableActions);
      Console.WriteLine("#Events executable  : " + nrExecutableEvents);
      Console.WriteLine("% parsed            : " + percParsed);
      Console.WriteLine("% executable scripts: " + percExecutable);
      Console.WriteLine("% executable actions: " + percExecutableActions);
      Console.WriteLine("% executable events : " + percExecutableEvents);

      Console.WriteLine();
      parser.DumpStatistics();

      return store;
    }

    override protected void Render(Image image, Drawable drawable)
    {
    }
  }
}
