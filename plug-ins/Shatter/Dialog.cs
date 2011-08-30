// The Shatter plug-in
// Copyright (C) 2006-2011 Maurits Rijk
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

namespace Gimp.Shatter
{
  public class Dialog : GimpDialogWithPreview<AspectPreview>
  {
    public Dialog(Drawable drawable, VariableSet variables) : 
      base("Shatter", drawable, variables)
    {
      var table = new GimpTable(4, 3, false) {
	ColumnSpacing = 6, RowSpacing = 6};
      Vbox.PackStart(table, false, false, 0);
      
      new ScaleEntry(table, 0, 1, "Pieces:", 150, 3,
		     GetVariable<int>("pieces"), 1.0, 256.0, 1.0, 8.0, 0);
    }
  }
}
