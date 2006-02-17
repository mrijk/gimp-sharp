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
using System.IO;
using NUnit.Core;
using NUnit.Util;

// using NUnit.Framework;

using Gtk;

namespace Gimp.UnitTest
{
  public class UnitTest : Plugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new UnitTest(args);
    }

    public UnitTest(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      ParamDefList in_params = new ParamDefList();

      Procedure procedure = new Procedure("plug_in_unit_test",
					  "Unit Test",
					  "Unit Test",
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2004-2006",
					  "Unit Test...",
					  "",
					  in_params);
      procedure.MenuPath = "<Toolbox>/Xtns/Extensions";

      set.Add(procedure);

      return set;
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("UnitTest", true);

      Dialog dialog = DialogNew("UnitTest", "UnitTest", IntPtr.Zero, 0,
				Gimp.StandardHelpFunc, "UnitTest");
      dialog.ShowAll();
      return DialogRun();
    }

    override protected void DoSomething()
    {
      Console.WriteLine("Testing!");
      Tester tester = new Tester();
      tester.Test();
    }
  }

  public class Tester
  {
    TestDomain testDomain;
    TestRunner testRunner;

    public Tester()
    {
      testDomain = new TestDomain();
      testRunner = testDomain;
    }


    private static Test MakeTestFromCommandLine(TestDomain testDomain)
    {
      NUnitProject project;

      project = NUnitProject.FromAssemblies(new string[]{"/tmp/gimptest.dll"});
      Console.WriteLine("project: " + project);

      return testDomain.Load("/tmp/gimptest.dll" );
    }

    public void Test()
    {
      TextWriter outWriter = Console.Out;
      TextWriter errorWriter = Console.Error;

      Test test = MakeTestFromCommandLine(testDomain);

      if (test == null)
	{
	  Console.Error.WriteLine("Unable to locate fixture");
	  return;
	}

      EventListener collector = new EventCollector( outWriter, errorWriter );
      TestResult result = testRunner.Run(collector);
    }
  }

  public class EventCollector : LongLivingMarshalByRefObject, EventListener
  {
    public EventCollector( TextWriter outWriter, TextWriter errorWriter )
    {
    }

    public void RunStarted(Test[] tests)
    {
    }

    public void RunFinished(TestResult[] results)
    {
    }

    public void RunFinished(Exception exception)
    {
    }

    public void TestFinished(TestCaseResult testResult)
    {
      // Console.WriteLine("TestFinished");
      if (testResult.Executed)
	{
	  if(testResult.IsFailure)
	    {
	      Console.WriteLine("Failure!");
	    }
	  else
	    {
	      Console.WriteLine("Ok!");
	    }
	}
    }

    public void TestStarted(TestCase testCase)
    {
      // Console.WriteLine("TestStarted");
    }

    public void SuiteStarted(TestSuite suite) 
    {
      Console.WriteLine("SuiteStarted");
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
