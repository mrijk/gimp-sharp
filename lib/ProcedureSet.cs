// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
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

using System.Collections;
using System.Collections.Generic;

namespace Gimp
{
  public class ProcedureSet : IEnumerable
  {
    Dictionary<string, Procedure> _set = new Dictionary<string, Procedure>();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public Procedure this[string name]
    {
      get {return _set[name];}
    }

    public void Add(Procedure procedure)
    {
      _set[procedure.Name] = procedure;
    }
    
    public void Install(bool usesImage, bool usesDrawable)
    {
      foreach (var procedure in _set.Values)
	{
	  procedure.Install(usesImage, usesDrawable);
	}
    }
  }
}
