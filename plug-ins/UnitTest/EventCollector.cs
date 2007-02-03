// The UnitTest plug-in
// Copyright (C) 2004-2007 Maurits Rijk, Massimo Perga
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
using System.IO;
using NUnit.Core;
using NUnit.Util;

using Gtk;

namespace Gimp.UnitTest
{
  public class EventCollector : LongLivingMarshalByRefObject, EventListener
  {
    int _nrOk = 0;
    int _nrFailed = 0;
    UnitTest  _unitTestPlugin;
    readonly ArrayList resultsAL;

    public EventCollector(TextWriter outWriter, TextWriter errorWriter, 
			  UnitTest unitTestPlugin ) :
      this(outWriter, errorWriter)
    {
      _unitTestPlugin = unitTestPlugin;
    }

    public EventCollector(TextWriter outWriter, TextWriter errorWriter )
    {
      resultsAL = new ArrayList();
    }

    public void RunStarted(Test[] tests)
    {
    }

    public void RunFinished(TestResult[] results)
    {
      TestReportDialog dialog = new TestReportDialog(_nrOk, _nrFailed, 
						     resultsAL);
      TestReportDialog.ShowHelpButton(false);
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      /*
         if (type == ResponseType.Ok)
         {
         }
         */
      dialog.Destroy();
    }

    public void RunFinished(Exception exception)
    {
    }

    public void TestFinished(TestCaseResult testResult)
    {
      if (testResult.Executed)
	{
	  if(testResult.IsFailure)
	    {
          //Console.WriteLine(testResult.ToString() + " failed");
          //new Message (testResult.ToString() + " failed");
	      resultsAL.Add(testResult.ToString());
	      _nrFailed++;
	    }
	  else
	    {
	      resultsAL.Add(testResult.ToString() + "OK");
	      _nrOk++;
	    }
	  _unitTestPlugin.UpdateProgressStatus();
	}
    }
    
    public void TestStarted(TestCase testCase)
    {
    }

    public void SuiteStarted(TestSuite suite) 
    {
      //				Console.WriteLine("SuiteStarted");
    }

    public void SuiteFinished(TestSuiteResult suiteResult) 
    {
    }
    
    public void UnhandledException( Exception exception )
    {
      Console.WriteLine("UnhandledException");
    }
  }
}
