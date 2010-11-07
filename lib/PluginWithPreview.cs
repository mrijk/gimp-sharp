// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// PluginWithPreview.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;

using Gtk;

namespace Gimp
{
  abstract public class PluginWithPreview : Plugin
  {
    protected AspectPreview Preview {get; private set;}
    protected VBox Vbox {get; private set;}

    public PluginWithPreview(string[] args, string package) :
      base(args, package)
    {
    }

    override protected GimpDialog DialogNew(string title, string role, 
					    IntPtr parent, 
					    Gtk.DialogFlags flags, 
					    GimpHelpFunc help_func, 
					    string help_id)
    {
      gimp_ui_init(title, true);

      var dialog = base.DialogNew(title, role, parent, flags, 
				  help_func, help_id);

      Vbox = new VBox(false, 0) {BorderWidth = 12};
      dialog.VBox.PackStart(Vbox, true, true, 0);

      Preview = new AspectPreview(_drawable, false);
      Preview.Invalidated += delegate {UpdatePreview(Preview);};

      Vbox.PackStart(Preview, true, true, 0);

      return dialog;
    }

    protected void InvalidatePreview()
    {
      Preview.Invalidate();
    }

    virtual protected void UpdatePreview(AspectPreview preview) {}
  }
}
