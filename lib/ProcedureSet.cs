// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// ProcedureSet.cs
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

using System.Collections.Generic;
using System.Linq;

namespace Gimp
{
  public class ProcedureSet
  {
    readonly Dictionary<string, Procedure> _set;
    public int Count => _set.Count;

    public Procedure this[string name] => _set[name];

    public ProcedureSet(IEnumerable<Procedure> procedures)
    {
      _set = procedures.ToDictionary(p => p.Name);
    }

    public void Add(Procedure procedure)
    {
      _set[procedure.Name] = procedure;
    }

    public void Install()
    {
      _set.Values.ToList().ForEach(p => p.Install());
    }
  }
}
