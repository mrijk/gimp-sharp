// The UnitTest plug-in
// Copyright (C) 2004-2010 Maurits Rijk
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

using System;
using System.Collections.Generic;
using NUnit.Core;
using NUnit.Util;

using Gtk;

namespace Gimp.UnitTest
{
  public class UnitTest : Plugin
  {
    [SaveAttribute("testDll")]
    string _testDll;
    ProgressBar _progressBar;
    int _testsPerformed = 0;
    public int TestCasesTotalNumber {get; set;}

    static void Main(string[] args)
    {
      new UnitTest(args);
    }

    UnitTest(string[] args) : base(args, "UnitTest")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      var inParams = new ParamDefList() {
	new ParamDef("testDll", "gimptest.dll", typeof(string), 
		     _("Test dll to load"))
      };
      yield return new Procedure("plug_in_unit_test",
				 "Unit Test",
				 "Unit Test",
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2004-2010",
				 "Unit Test...",
				 "",
				 inParams)
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "UnitTest.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("UnitTest", true);

      var dialog = DialogNew("UnitTest", "UnitTest", IntPtr.Zero, 0,
			     Gimp.StandardHelpFunc, "UnitTest");

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var entry = new FileChooserButton(_("Open..."), FileChooserAction.Open);
      if (_testDll != null)
	{
	  entry.SetFilename(_testDll);
	}
      entry.SelectionChanged += delegate
	{
	  _testDll = entry.Filename;
	};
      vbox.PackStart(entry, false, false, 0);

      _progressBar = new ProgressBar();
      vbox.PackEnd(_progressBar);
      
      return dialog;
    }

    override protected void Render()
    {
      var tester = new UnitTester(this);
      tester.Test(_testDll);
    }

    public void UpdateProgressStatus()
    {
      _testsPerformed++;

      _progressBar.Fraction = (double) _testsPerformed / TestCasesTotalNumber;
      while (Application.EventsPending())
	{
	  Gtk.Application.RunIteration();
	}
    }
  }
}
