using System;
using System.Collections.Generic;
using Gtk;

namespace Gimp
{
  public class PythonPlugin : Plugin
  {
    public PythonPlugin(string[] args, string package) : base(args, package)
    {
      Console.WriteLine("PythonPlugin ctor");
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      Console.WriteLine("ListProcedures");
      yield break;
    }
  }
}
