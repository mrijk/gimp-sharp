// The QR plug-in
// Copyright (C) 2004-2016 Maurits Rijk
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

namespace Gimp.QR
{
  class QR : Plugin
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
			   "2010-2016",
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
      return new Dialog(_drawable, Variables);
    }

    override protected void Render(Image image, Drawable drawable)
    {
      var renderer = new Renderer(Variables);
      renderer.Render(image, drawable);
    }
  }
}
