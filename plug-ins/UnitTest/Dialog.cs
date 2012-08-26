// The UnitTest plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Dialog.cs
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

using Gtk;

namespace Gimp.UnitTest
{
  public class Dialog : GimpDialog
  {
    public Dialog(VariableSet variables, Variable<int> performed, 
		  Variable<int> total) : base("UnitTest", variables)
    {
      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      vbox.PackStart(CreateTestDllButton(), false, false, 0);
      vbox.PackEnd(CreateProgressBar(performed, total));
    }

    FileChooserButton CreateTestDllButton()
    {
      var entry = new FileChooserButton(_("Open..."), FileChooserAction.Open);
      var testDll = GetVariable<string>("test_dll");

      entry.SetFilename(testDll.Value);

      entry.SelectionChanged += delegate
	{
	  testDll.Value = entry.Filename ?? testDll.Value;
	};
      return entry;
    }

    ProgressBar CreateProgressBar(Variable<int> performed, Variable<int> total)
    {
      var progressBar = new ProgressBar();

      performed.ValueChanged += delegate
	{
	  progressBar.Fraction = (double) performed.Value / total.Value;
	  while (Application.EventsPending())
	  {
	    Gtk.Application.RunIteration();
	  }
	};
      return progressBar;
    }
  }
}
