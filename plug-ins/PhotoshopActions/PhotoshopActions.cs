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
using System.IO;

using Gtk;

namespace Gimp.PhotoshopActions
{
  public class PhotoshopActions : Plugin
  {
    static void Main(string[] args)
    {
      new PhotoshopActions(args);
    }

    public PhotoshopActions(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_photoshop_actions",
					  "Play Photoshop action files",
					  "Play Photoshop action files",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2006",
					  "Photoshop Actions...",
					  "",
					  in_params);
      procedure.MenuPath = "<Toolbox>/Xtns/Extensions";
      procedure.IconFile = "PhotoshopActions.png";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("PhotoshopActions", true);

      Dialog dialog = DialogNew("Photoshop Actions", "PhotoshopActions",
				IntPtr.Zero, 0, Gimp.StandardHelpFunc, 
				"PhotoshopActions");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      TreeStore store = CreateActionTree();

      ScrolledWindow sw = new ScrolledWindow();
      vbox.Add(sw);
      
      TreeView view = new TreeView(store);
      sw.Add(view);        

      view.AppendColumn("SetName", new CellRendererText(), "text", 0);

      dialog.ShowAll();
      return DialogRun();
    }

    TreeStore CreateActionTree()
    {
      TreeStore store = new TreeStore(typeof(string), typeof(string));

      string scriptDir = Gimp.Directory + "/scripts";

      ActionParser parser = new ActionParser();

      foreach (string fileName in Directory.GetFiles(scriptDir))
	{
	  if (fileName.EndsWith(".atn"))
	    {
	      ActionSet actions = parser.Parse(fileName);
	      if (actions != null)
		{
		  TreeIter iter = store.AppendValues(actions.Name);
		  foreach (Action action in actions)
		    {
		      TreeIter iter1 = store.AppendValues(iter, action.Name);
		      foreach (ActionEvent actionEvent in action)
			{
			  store.AppendValues(iter1, 
					     actionEvent.EventForDisplay);
			}
		    }
		}
	    }
	}    

      return store;
    }

    override protected void Render()
    {
    }
  }
}
