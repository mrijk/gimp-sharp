using System;
using System.Collections.Generic;
using Gtk;

namespace Gimp
{
  public class PythonPlugin : Plugin
  {
    public PythonPlugin(string[] args, string package) : 
      base(PythonPlugin.StripName(args), package)
    {
      Console.WriteLine("PythonPlugin ctor");
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      Console.WriteLine("ListProcedures");
      // yield break;
      return ListProceduresTwo();
    }

    virtual protected IEnumerable<Procedure> ListProceduresTwo()
    {
      yield break;
    }

    static string[] StripName(string[] src)
    {
      int len = src.Length - 1;
      string[] dest = new string[len];
      Array.Copy(src, 1, dest, 0, len);
      return dest;
    }
  }
}
