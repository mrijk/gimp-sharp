// The QR plug-in
// Copyright (C) 2004-2010 Maurits Rijk
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
using System.Collections.Generic;
using Gtk;

namespace Gimp.QR
{
  class QR : PluginWithPreview
  {
    [SaveAttribute("text")]
    string _text = "";
    [SaveAttribute("encoding")]
    int _encoding = 0;
    [SaveAttribute("error_correction")]
    string _errorCorrection = "L";
    [SaveAttribute("margin")]
    int _margin = 4;

    static void Main(string[] args)
    {
      new QR(args);
    }

    QR(string[] args) : base(args, "QR")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList() {
	new ParamDef("text", "", typeof(string),
		     _("Text for QR code")),
	new ParamDef("encoding", 0, typeof(int), 
		     _("Encoding (0 = UTF-8, 1 = Shift-JIS, 2 = ISO-8859-1)")),
	new ParamDef("error_correction", "L", typeof(string),
		     _("Error Correction Level (L, M, Q or H)")),
	new ParamDef("margin", 4, typeof(int),
		     _("Margin")),
      };

      yield return new Procedure("plug_in_QR",
				 _("Generates QR codes"),
				 _("Generates QR codes"),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2010",
				 "QR codes...",
				 "*",
				 inParams)
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

      InvalidatePreview();
			
      return dialog;
    }

    Widget CreateText()
    {
      var frame = new GimpFrame(_("Text"));

      var text = new TextView();
      text.SetSizeRequest(-1, 100);

      var buffer = text.Buffer;
      buffer.Text = _text;
      buffer.Changed += delegate {
	_text = buffer.Text;
	InvalidatePreview();
      };
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
      var button = new RadioButton(previous, description);
      vbox.Add(button);
      if (_encoding == type) {
	button.Active = true;
      }
      button.Clicked += delegate {
	if (button.Active) {
	  _encoding = type;
	  InvalidatePreview();
	}
      };
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
      var button = new RadioButton(previous, type + " " + _(description));
      if (_errorCorrection == type) {
	button.Active = true;
      }
      button.Clicked += delegate {
	if (button.Active) {
	  _errorCorrection = type;
	  InvalidatePreview();
	}
      };
      vbox.Add(button);
      return button;
    }

    void CreateMargin(Table table)
    {
      var margin = new ScaleEntry(table, 0, 3, _("Margin:"), 150, 3, 
				  _margin, 0.0, 64.0, 1.0, 8.0, 0);
      margin.ValueChanged += delegate
	{
	  _margin = margin.ValueAsInt;
	  InvalidatePreview();
	};
    }

    Image GetImageFromGoogleCharts(Dimensions dimensions)
    {
      var chl = "&chl=" + Uri.EscapeDataString(_text);
      var chs = string.Format("&chs={0}x{1}", dimensions.Width, 
			      dimensions.Height);
      var choe = "&choe=" + GetEncodingString();
      var chld = string.Format("&chld={0}|{1}", _errorCorrection, _margin);
      var url = "http://chart.apis.google.com/chart?cht=qr" 
	+ chl + chs + choe + chld;

      Console.WriteLine("url: " + url);

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
      if (_encoding == 1) 
	{
	  return "Shift_JIS";
	}
      else if (_encoding == 2)
	{
	  return "ISO-8859-1";
	}
      else
	{
	  return "UTF-8";
	}
    }

    override protected void UpdatePreview(AspectPreview preview)
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
