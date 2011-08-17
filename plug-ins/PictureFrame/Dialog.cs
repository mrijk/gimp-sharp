// The PictureFrame plug-in
// Copyright (C) 2006-2011 Oded Coster
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

namespace Gimp.PictureFrame
{
  public class Dialog : GimpDialog
  {
    public Dialog(VariableSet variables) : base("Picture Frame", variables)
    {
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);
      
      var entry = new FileChooserButton(_("Load Frame..."), FileChooserAction.Open);

      entry.SelectionChanged += delegate
	{
	  GetVariable<string>("image_path").Value = entry.Filename;
	};
      vbox.PackStart(entry, false, false, 0);
    }
  }
}
