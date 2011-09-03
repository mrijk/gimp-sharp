// The WebServer plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// WebServer.cs
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

namespace Gimp.WebServer
{
  class WebServer : Plugin
  {   
    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<int>("port", _("Port number"), 8080)
      };
      GimpMain<WebServer>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_webserver",
			   "Embedded Webserver",
			   "Embedded Webserver",
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2011",
			   "Webserver...",
			   "",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "WebServer.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("WebServer", true);
      return new Dialog(Variables);
    }

    override protected void Render()
    {
    }
  }
}
