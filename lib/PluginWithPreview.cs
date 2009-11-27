// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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
    protected AspectPreview _preview;
    VBox _vbox;

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

      _vbox = new VBox(false, 0);
      _vbox.BorderWidth = 12;
      dialog.VBox.PackStart(_vbox, true, true, 0);

      _preview = new AspectPreview(_drawable, false);
      _preview.Invalidated += delegate
	{
	  UpdatePreview(_preview);
	};

      _vbox.PackStart(_preview, true, true, 0);

      return dialog;
    }

    protected void InvalidatePreview()
    {
      _preview.Invalidate();
    }

    protected AspectPreview Preview
    {
      get {return _preview;}
    }

    protected VBox Vbox
    {
      get {return _vbox;}
    }

    virtual protected void UpdatePreview(AspectPreview preview) {}
  }
}
