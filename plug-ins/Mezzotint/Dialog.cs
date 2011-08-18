// The Mezzotint plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
  public class Dialog : GimpDialogWithPreview<DrawablePreview>
  {
    public Dialog(Drawable drawable, VariableSet variables) : 
      base("Mezzotint", drawable, variables)
    {
      var type = GimpComboBox.NewText(GetVariable<int>("type"));
      type.AppendText("Fine dots");
      type.AppendText("Medium dots");
      type.AppendText("Grainy dots");
      type.AppendText("Coarse dots");
      type.AppendText("Short lines");
      type.AppendText("Medium lines");
      type.AppendText("Long lines");
      type.AppendText("Short strokes");
      type.AppendText("Medium strokes");
      type.AppendText("Long strokes");

      Vbox.PackStart(type, false, false, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(preview as DrawablePreview);
    }
  }
}
