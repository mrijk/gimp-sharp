// The UnitTest plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// UnitTest.cs
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

namespace Gimp.UnitTest
{
  public class UnitTest : Plugin
  {
    Variable<int> _testsPerformed = new Variable<int>("tests_performed", "", 0);
    Variable<int> _testsTotal = new Variable<int>("tests_total", "", 0);

    static void Main(string[] args)
    {
      var variables = new VariableSet() {
	new Variable<string>("test_dll", _("Test dll to load"), "gimptest.dll")
      };
      GimpMain<UnitTest>(args, variables);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_unit_test",
			   "Unit Test",
			   "Unit Test",
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2004-2011",
			   "Unit Test...",
			   "",
			   new ParamDefList(Variables))
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "UnitTest.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("UnitTest", true);
      return new Dialog(Variables, _testsPerformed, _testsTotal);
    }

    override protected void Render()
    {
      var tester = new UnitTester();
      tester.Test(GetValue<string>("test_dll"), _testsPerformed, _testsTotal);
    }
  }
}
