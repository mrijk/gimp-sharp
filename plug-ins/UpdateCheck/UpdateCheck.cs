// The UpdateCheck plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// UpdateCheck.cs
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

namespace Gimp.UpdateCheck
{
  class UpdateCheck : Plugin
  {
    static void Main(string[] args)
    {
      var variables = new VariableSet {
	new Variable<bool>("check_gimp", "", true),
	new Variable<bool>("check_gimp_sharp", "", true),
	new Variable<bool>("check_unstable", "", true),
	new Variable<bool>("enable_proxy", "", false),
	new Variable<string>("http_proxy", "", ""),
	new Variable<string>("port", "", "")
      };
      GimpMain<UpdateCheck>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_update_check",
			   _("Check for updates"),
			   _("Check for updates"),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2006-2016",
			   _("Check for Updates..."),
			   "",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "UpdateCheck.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("UpdateCheck", true);
      return new Dialog(Variables);
    }

    override protected void Render()
    {
      var renderer = new Renderer(Variables);
      renderer.Render();
    }
  }
}
