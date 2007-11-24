// The UnitTest plug-in
// Copyright (C) 2004-2007 Maurits Rijk
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
using System.IO;
using System.Threading;
using NUnit.Core;
using NUnit.Util;

using Gtk;

namespace Gimp.UnitTest
{
  public class UnitTest : Plugin
  {
    [SaveAttribute]
    static string _testDll;
    ProgressBar _progressBar;
    int    _testsPerformed = 0;
    public int TestCasesTotalNumber {get; set;}

    static void Main(string[] args)
    {
      new UnitTest(args);
    }

    public UnitTest(string[] args) : base(args, "UnitTest")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_unit_test",
				 "Unit Test",
				 "Unit Test",
				 "Maurits Rijk, Massimo Perga",
				 "(C) Maurits Rijk, Massimo Perga",
				 "2004-2007",
				 "Unit Test...",
				 "")
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "UnitTest.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("UnitTest", true);

      GimpDialog dialog = DialogNew("UnitTest", "UnitTest", IntPtr.Zero, 0,
				    Gimp.StandardHelpFunc, "UnitTest");

      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      FileChooserButton entry = new FileChooserButton("Open...", 
						      FileChooserAction.Open);
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
      UnitTester tester = new UnitTester(this);
      tester.Test(_testDll);
    }

    public void UpdateProgressStatus()
    {
      _testsPerformed++;
      double ratio = (double) _testsPerformed / TestCasesTotalNumber;

      _progressBar.Update(ratio);
      while (Application.EventsPending ())
	{
	  Gtk.Application.RunIteration();
	}
    }
  }
}
