// The UnitTest plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// UnitTester.cs
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

using Gtk;

namespace Gimp.UnitTest
{
  public class UnitTester
  {
    TestDomain testDomain;
    TestRunner testRunner;

    public UnitTester()
    {
      testDomain = new TestDomain();
      testRunner = testDomain;
    }

    private static Test MakeTestFromCommandLine(TestDomain testDomain,
						string testDll)
    {
      NUnitProject project;

      project = NUnitProject.FromAssemblies(new string[]{testDll});

      return testDomain.Load(testDll);
    }

    public void Test(string testDll)
    {
      TextWriter outWriter = Console.Out;
      TextWriter errorWriter = Console.Out; // Console.Error;

      Test test = MakeTestFromCommandLine(testDomain, testDll);

      if (test == null)
	{
	  Console.Error.WriteLine("Unable to locate fixture");
	  return;
	}

      EventListener collector = new EventCollector( outWriter, errorWriter );
      TestResult result = testRunner.Run(collector);
    }
  }
}
