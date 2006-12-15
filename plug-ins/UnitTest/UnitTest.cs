// The UnitTest plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
using NUnit.Core;
using NUnit.Util;

using Gtk;

namespace Gimp.UnitTest
{
  public class UnitTest : Plugin
  {
    [SaveAttribute]
    string _testDll;
    Progress _progress;
    int    _testsPerformed = 0;
    int    _testCasesTotalNumber = 0;

    static void Main(string[] args)
    {
      new UnitTest(args);
    }

    public UnitTest(string[] args) : base(args, "UnitTest")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      Procedure procedure = new Procedure("plug_in_unit_test",
					  "Unit Test",
					  "Unit Test",
					  "Maurits Rijk, Massimo Perga",
					  "(C) Maurits Rijk, Massimo Perga",
					  "2004-2006",
					  "Unit Test...",
					  "");

      procedure.MenuPath = "<Toolbox>/Xtns/Extensions";
      procedure.IconFile = "UnitTest.png";

      yield return procedure;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("UnitTest", true);

      Dialog dialog = DialogNew("UnitTest", "UnitTest", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "UnitTest");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      FileChooserButton entry = 
	new FileChooserButton("Open...", FileChooserAction.Open);
      // entry.CurrentName = "blah"; // _testDll;
      entry.SelectionChanged += delegate(object o, EventArgs args)
	{
	  _testDll = entry.Filename;
	};
      vbox.PackStart(entry, false, false, 0);

      dialog.ShowAll();
      return DialogRun();
    }

    override protected void Render()
    {
      UnitTester tester = new UnitTester(this);

      _progress  = new Progress("UnitTest execution progress :");

      tester.Test(_testDll);
    }

    public void UpdateProgressStatus()
    {
      _testsPerformed++;
      double ratio = (double) _testsPerformed / _testCasesTotalNumber;

      if (_progress != null)
      {
        _progress.Update(ratio);
	/*
        if(testsPerformed == _testCasesTotalNumber)
        {
        }
        */
      }
    }

    public int TestCasesTotalNumber
    {
      set {_testCasesTotalNumber = value;}
      get {return _testCasesTotalNumber;}
    }
  }
}
