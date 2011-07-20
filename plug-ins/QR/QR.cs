// The QR plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// QR.cs
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
using Gtk;

namespace Gimp.QR
{
  class QR : PluginWithPreview<AspectPreview>
  {
    Variable<string> _text = new Variable<string>
    ("text", _("Text for QR code"), "");
    Variable<int> _encoding = new Variable<int>
    ("encoding", _("Encoding (0 = UTF-8, 1 = Shift-JIS, 2 = ISO-8859-1)"), 0);
    Variable<string> _errorCorrection = new Variable<string>
    ("error_correction", _("Error Correction Level (L, M, Q or H)"), "L");
    Variable<int> _margin = new Variable<int>("margin", _("Margin"), 4);

    static void Main(string[] args)
    {
      GimpMain<QR>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_QR",
			   _("Generates QR codes"),
			   _("Generates QR codes"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2010-2011",
			   "QR codes...",
			   "*",
			   new ParamDefList(_text, _encoding, _errorCorrection,
					    _margin))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "QR.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("QR", true);

      var dialog = DialogNew("QR", "QR", IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, "QR");

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

      _text.ValueChanged += delegate {InvalidatePreview();};
      _margin.ValueChanged += delegate {InvalidatePreview();};
      _encoding.ValueChanged += delegate {InvalidatePreview();};
      _errorCorrection.ValueChanged += delegate {InvalidatePreview();};

      InvalidatePreview();
      
      return dialog;
    }

    Widget CreateText()
    {
      var frame = new GimpFrame(_("Text"));

      var text = new GimpTextView(_text);
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
					    _encoding);
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
      var button = new GimpRadioButton<string>(previous, 
					       type + " " + _(description),
					       type, _errorCorrection);
      vbox.Add(button);
      return button;
    }

    void CreateMargin(Table table)
    {
      new ScaleEntry(table, 0, 3, _("Margin:"), 150, 3, 
		     _margin, 0.0, 64.0, 1.0, 8.0, 0);
    }

    Image GetImageFromGoogleCharts(Dimensions dimensions)
    {
      var chl = "&chl=" + Uri.EscapeDataString(_text.Value);
      var chs = string.Format("&chs={0}x{1}", dimensions.Width, 
			      dimensions.Height);
      var choe = "&choe=" + GetEncodingString();
      var chld = string.Format("&chld={0}|{1}", _errorCorrection.Value, 
			       _margin.Value);
      var url = "http://chart.apis.google.com/chart?cht=qr" 
	+ chl + chs + choe + chld;

      var procedure = new Procedure("file-uri-load");

      try 
	{
	  var returnArgs = procedure.Run(url, url);

	  return returnArgs[0] as Image;
	}
      catch (GimpSharpException e)
	{
	  new Message(e.Message);
	  return null;
	}
    }

    string GetEncodingString()
    {
      int encoding = _encoding.Value;
      if (encoding == 1) 
	{
	  return "Shift_JIS";
	}
      else if (encoding == 2)
	{
	  return "ISO-8859-1";
	}
      else
	{
	  return "UTF-8";
	}
    }

    override protected void UpdatePreview(GimpPreview preview)
    {
      var image = GetImageFromGoogleCharts(preview.Size);
      
      preview.Redraw(image.ActiveDrawable);
      image.Delete();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var newImage = GetImageFromGoogleCharts(drawable.Dimensions);
      if (newImage != null)
	{
	  Display.Reconnect(image, newImage);
	}
    }
  }
}
