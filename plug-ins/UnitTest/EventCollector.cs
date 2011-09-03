// The UnitTest plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// EventCollector.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Core;
using NUnit.Util;

using Gtk;

namespace Gimp.UnitTest
{
  public class EventCollector : MarshalByRefObject, EventListener
  {
    int _nrOk = 0;
    int _nrFailed = 0;
    readonly List<string> _resultsAL;
    Variable<int> _testsPerformed;

    public EventCollector(TextWriter outWriter, TextWriter errorWriter, 
			  Variable<int> testsPerformed) :
      this(outWriter, errorWriter)
    {
      _testsPerformed = testsPerformed;
    }

    public EventCollector(TextWriter outWriter, TextWriter errorWriter )
    {
      _resultsAL = new List<string>();
    }

    public void RunStarted(Test[] tests)
    {
    }

    public void RunStarted(string foo, int bar)
    {
    }

    public void RunFinished(TestResult[] results)
    {
    }

    public void RunFinished(Exception exception)
    {
      Console.WriteLine("RunFinished!");
    }

    public void RunFinished(TestResult testResult)
    {
      var dialog = new TestReportDialog(_nrOk, _nrFailed, _resultsAL);
      TestReportDialog.ShowHelpButton(false);
      dialog.ShowAll();
      dialog.Run();
      dialog.Destroy();
    }

    public void TestOutput(TestOutput testOutput)
    {
    }

    public void TestFinished(TestCaseResult testResult)
    {
      if (testResult.Executed)
	{
	  if (testResult.IsFailure)
	    {
	      _resultsAL.Add(testResult.Name + ": ");
	      _nrFailed++;
	    }
	  else
	    {
	      _resultsAL.Add(testResult.Name + ": OK");
	      _nrOk++;
	    }
	  _testsPerformed.Value = _testsPerformed.Value + 1;
	}
    }
    
    public void TestStarted(TestCase testCase)
    {
    }

    public void TestStarted(TestName testName)
    {
    }

    public void SuiteStarted(TestName name) 
    {
    }

    public void SuiteFinished(TestSuiteResult suiteResult) 
    {
    }
    
    public void UnhandledException(Exception exception)
    {
      Console.WriteLine("UnhandledException");
    }
  }
}
