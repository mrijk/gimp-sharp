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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gimp
{
  public class Variable<T> : IVariable
  {
    public event EventHandler ValueChanged;

    T _value;

    public string Identifier {get; private set;}
    public string Description {get; private set;}
    public T DefaultValue {get; set;}

    public Variable(string identifier, string description, T defaultValue)
    {
      Identifier = identifier;
      Description = description;
      _value = DefaultValue = defaultValue;
    }

    public Variable(T value)
    {
      _value = DefaultValue = value;
    }

    public T Value
    {
      get {return _value;}
      set 
	{
	  if (!value.Equals(_value))
	    {
	      _value = value;
	      if (ValueChanged != null)
		{
		  // Fix me: maybe pass new and old value?
		  ValueChanged(this, new EventArgs());
		}
	    }
	}
    }

    public Type Type
    {
      get {return typeof(T);}
    }

    public void Register(VariableSet variables)
    {
      ValueChanged += delegate {variables.Changed();};
    }

    public void Reset()
    {
      Value = DefaultValue;
    }

    public void Serialize(BinaryFormatter formatter, MemoryStream stream)
    {
      formatter.Serialize(stream, Value);
    }

    public void Deserialize(BinaryFormatter formatter, MemoryStream stream)
    {
      Value = (T) formatter.Deserialize(stream);
    }
  }
}
