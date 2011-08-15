// The QR plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// QRDialog.cs
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

namespace Gimp.QR
{
  public class QRDialog : GimpDialogWithPreview<AspectPreview>
  {
    readonly QR _parent;

    public QRDialog(QR parent, Drawable drawable, VariableSet variables) : 
      base("QR", drawable, variables)
    {
      // fix me: replace by a Renderer class
      _parent = parent;

      var table = new GimpTable(4, 2) {
	ColumnSpacing = 6, RowSpacing = 6};

      var text = CreateText();
      table.Attach(text, 0, 2, 0, 2);

      var encoding = CreateOutputEncoding();
      table.Attach(encoding, 0, 1, 2, 3);

      var errorCorrection = CreateErrorCorrection();
      table.Attach(errorCorrection, 1, 2, 2, 3);

      CreateMargin(table);

      Vbox.PackStart(table, false, false, 0);

      InvalidatePreview();
    }

    Widget CreateText()
    {
      var frame = new GimpFrame(_("Text"));

      var text = new GimpTextView(GetVariable<string>("text"));
      text.SetSizeRequest(-1, 100);
      frame.Add(text);

      return frame;
    }

    Widget CreateOutputEncoding()
    {
      var frame = new GimpFrame(_("Encoding"));

      var vbox = new VBox(false, 1);
      frame.Add(vbox);

      var button = AddEncodingButton(vbox, null, 0, _("_UTF-8"));
      button = AddEncodingButton(vbox, button, 1, _("_Shift-JIS"));
      AddEncodingButton(vbox, button, 2, _("_ISO-8859-1"));

      return frame;
    }

    RadioButton AddEncodingButton(VBox vbox, RadioButton previous,
				  int type, string description)
    {
      var button = new GimpRadioButton<int>(previous, description, type, 
					    GetVariable<int>("encoding"));
      vbox.Add(button);
      return button;
    }

    Widget CreateErrorCorrection()
    {
      var frame = new GimpFrame(_("Error Correction Level"));

      var vbox = new VBox(false, 1);
      frame.Add(vbox);

      var button = AddErrorCorrectionButton(vbox, null, "L", "(7 % data loss)");
      button = AddErrorCorrectionButton(vbox, button, "M", "(15 % data loss)");
      button = AddErrorCorrectionButton(vbox, button, "Q", "(25 % data loss)");
      AddErrorCorrectionButton(vbox, button, "H", "(30 % data loss)");

      return frame;
    }

    RadioButton AddErrorCorrectionButton(VBox vbox, RadioButton previous, 
					 string type, string description)
    {
      var variable = GetVariable<string>("error_correction");
      var button = new GimpRadioButton<string>(previous, 
					       type + " " + _(description),
					       type, variable);
      vbox.Add(button);
      return button;
    }

    void CreateMargin(Table table)
    {
      new ScaleEntry(table, 0, 3, _("Margin:"), 150, 3, 
		     GetVariable<int>("margin"), 0.0, 64.0, 1.0, 8.0, 0);
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var image = _parent.GetImageFromGoogleCharts(preview.Size);
      
      preview.Redraw(image.ActiveDrawable);
      image.Delete();
    }
  }
}
