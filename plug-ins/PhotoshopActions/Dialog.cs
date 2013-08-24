// The PhotoshopActions plug-in
// Copyright (C) 2006-2013 Maurits Rijk
//
// Dialog.cs
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
  public class Dialog : GimpDialog
  {
    List<ActionSet> _set = new List<ActionSet>();
    readonly Image _image;
    readonly Drawable _drawable;
    bool _first_run = true;

    public Dialog(Image image, Drawable drawable, VariableSet variables) : 
      base("Photoshop Actions", variables)
    {
      _image = image;
      _drawable = drawable;

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      var store = CreateActionTree();

      var sw = new ScrolledWindow() {HeightRequest = 400};
      vbox.PackStart(sw, true, true, 0);
      
      var view = new TreeView(store);
      sw.Add(view);        

      var activeRenderer = new CellRendererToggle() {Activatable = true};
      var columnOne = view.AppendColumn("Enabled", activeRenderer, 
					new TreeCellDataFunc(RenderActive));
      activeRenderer.Toggled += delegate(object o, ToggledArgs args)
	{
	  TreeIter iter;
	  var path = new TreePath(args.Path);
	  if (store.GetIter(out iter, path))
	  {
	    var executable = store.GetValue(iter, 1) as IExecutable;
	    executable.IsEnabled = !executable.IsEnabled;

	    path.Down();
	    while (store.GetIter(out iter, path))
	      {
		store.EmitRowChanged(path, iter);
		path.Next();
	      }
	  }
	};

      var textRenderer = new CellRendererText();
      var column = view.AppendColumn("Set Name", textRenderer, 
				     new TreeCellDataFunc(RenderText));

      var hbox = new HBox();
      vbox.PackStart(hbox, false, true, 0);

      var play = new Button(Stock.Execute);
      play.Clicked += delegate
	{
	  RenameToBackground();

	  var paths = view.Selection.GetSelectedRows();
	  var path = paths[0];	// Assume only 1 is selected

	  var indices = path.Indices;

	  var actions = _set[indices[0]];

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
	  Gimp.DisplaysFlush();
	};      
      hbox.PackStart(play, false, true, 0);

      view.Selection.Changed += delegate
	{
	  var paths = view.Selection.GetSelectedRows();
	  var indices = paths[0].Indices;

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

      ActionEvent.ActionSetCollection = _set;
    }

    void RenameToBackground()
    {
      // First layer in Photoshop is always called 'Background'
      if (_first_run)
	{
	  _first_run = false;
	  var image = ActionEvent.ActiveImage;
	  var layer = image.Layers[0];
	  layer.Name = "Background";
	}
    }

    void RenderActive(TreeViewColumn column, CellRenderer cell, 
		    TreeModel model, TreeIter iter)
    {
      var executable = model.GetValue(iter, 1) as IExecutable;
      var toggle = cell as CellRendererToggle;

      if (executable != null)
	{
	  toggle.Visible = true;
	  toggle.Active = executable.IsEnabled;
	}
      else
	{
	  toggle.Visible = false;
	}
    }

    void RenderText(TreeViewColumn column, CellRenderer cell, 
		    TreeModel model, TreeIter iter)
    {
      string name = model.GetValue(iter, 0) as string;
      var executable = model.GetValue(iter, 1) as IExecutable;
      var text = cell as CellRendererText;

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
      var store = new TreeStore(typeof(string), typeof(IExecutable));

      string scriptDir = Gimp.Directory + 
	System.IO.Path.DirectorySeparatorChar + "scripts";

      var parser = new ActionParser(_image, _drawable);

      int nrScripts = 0;

      DebugOutput.Quiet = false;

      foreach (string fileName in Directory.GetFiles(scriptDir))
	{
	  if (fileName.EndsWith(".atn"))
	    {
	      Console.WriteLine(fileName);
	      nrScripts++;

	      var actions = parser.Parse(fileName);

	      if (actions != null)
		{
		  _set.Add(actions);

		  var iter = store.AppendValues(actions.ExtendedName, actions);
		  foreach (var action in actions)
		    {
		      var iter1 = store.AppendValues(iter, action.Name,
							  action);
		      foreach (var actionEvent in action)
			{
			  Console.WriteLine(actionEvent.EventForDisplay);
			  actionEvent.FillStore(store, iter1);
			}
		    }
		}
	    }
	}

      DumpStatistics(parser, nrScripts);

      return store;
    }

    void DumpStatistics(ActionParser parser, int nrScripts)
    {
      int nrExecutable = 0;
      int nrActions = 0;
      int nrEvents = 0;
      int nrExecutableActions = 0;
      int nrExecutableEvents = 0;

      foreach (var actions in _set)
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
    }
  }
}
