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

namespace Gimp.QR
{
  public class QR : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<string>("text", _("Text for QR code"), ""),
	new Variable<int>("encoding", 
			  _("Encoding (0 = UTF-8, 1 = Shift-JIS, 2 = ISO-8859-1)"), 0),
	new Variable<string>("error_correction", 
			     _("Error Correction Level (L, M, Q or H)"), "L"),
	new Variable<int>("margin", _("Margin"), 4)
      };

      GimpMain<QR>(args, variables);
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
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Image>/Filters/Render",
	  IconFile = "QR.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("QR", true);
      return new QRDialog(this, _drawable, Variables);
    }

    public Image GetImageFromGoogleCharts(Dimensions dimensions)
    {
      var chl = "&chl=" + Uri.EscapeDataString(GetValue<string>("text"));
      var chs = string.Format("&chs={0}x{1}", dimensions.Width, 
			      dimensions.Height);
      var choe = "&choe=" + GetEncodingString();
      var chld = string.Format("&chld={0}|{1}", GetValue<string>("error_correction"), 
			       GetValue<int>("margin"));
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
      int encoding = GetValue<int>("encoding");
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
