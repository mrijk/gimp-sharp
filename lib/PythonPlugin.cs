// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// PythonPlugin.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Collections.Generic;
using Gtk;

namespace Gimp
{
  public class PythonPlugin : Plugin
  {
    public PythonPlugin() {}

#if false
    public PythonPlugin(string[] args, string package) : 
      base(PythonPlugin.StripName(args), package)
    {
    }
#endif
    override protected IEnumerable<Procedure> ListProcedures()
    {
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
