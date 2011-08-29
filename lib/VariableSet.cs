// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// Variable.cs
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
using System.Collections;
using System.Collections.Generic;

namespace Gimp
{
  public class ClonedVariable<T> : Variable<T>
  {
    readonly Variable<T> _original;

    public ClonedVariable(Variable<T> original) : base(original.Value)
    {
      _original = original;
    }

    public void Commit()
    {
      _original.Value = Value;
    }
  }

  public class VariableSet : IEnumerable<IVariable>
  {
    List<IVariable> _set = new List<IVariable>();
    
    public event EventHandler ValueChanged;

    public void Add(IVariable v)
    {
      _set.Add(v);
      v.Register(this);
    }
    
    public IEnumerator<IVariable> GetEnumerator()
    {
      return _set.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void ForEach(Action<IVariable> action)
    {
      _set.ForEach(action);
    }

    public IVariable this[string identifier]
    {
      get 
      {
	var found = _set.Find(v => v.Identifier == identifier);
	if (found == null) 
	  {
	    throw new GimpSharpException(String.Format("VariableSet: variable {0} not found",
					 identifier));
	  }
	return found;
      }
    }

    public Variable<T> Get<T>(string identifier)
    {
      return this[identifier] as Variable<T>;
    }

    public T GetValue<T>(string identifier)
    {
      return Get<T>(identifier).Value;
    }

    public ClonedVariable<T> GetClone<T>(string identifier)
    {
      return new ClonedVariable<T>(Get<T>(identifier));
    }

    public void Changed()
    {
      if (ValueChanged != null)
	{
	  ValueChanged(this, new EventArgs());
	}
    }

    public void Reset()
    {
      ForEach(v => v.Reset());
    }
  }
}
