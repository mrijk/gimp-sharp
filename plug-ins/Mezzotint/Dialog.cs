// The Mezzotint plug-in
// Copyright (C) 2004-2013 Maurits Rijk
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

using Gtk;

namespace Gimp.Mezzotint
{
  public class Dialog : GimpDialogWithPreview
  {
    public Dialog(Drawable drawable, VariableSet variables) : 
      base("Mezzotint", drawable, variables, 
	   () => new DrawablePreview(drawable))
    {
      var type = new GimpComboBox(GetVariable<int>("type"),
	new string[] {_("Fine dots"), _("Medium dots"), _("Grainy dots"),
		      _("Coarse dots"), _("Short lines"), _("Medium lines"),
		      _("Long lines"), _("Short strokes"), _("Medium strokes"),
		      _("Long strokes")});

      Vbox.PackStart(type, false, false, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(preview as DrawablePreview);
    }
  }
}
