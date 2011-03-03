// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// SaveAttributeSet.cs
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
using System.Linq;
using System.Reflection;

namespace Gimp
{
  public sealed class SaveAttributeSet
  {
    internal Type _type;

    public SaveAttributeSet(Type type)
    {
      _type = type;
    }

    public IEnumerator<SaveAttribute> GetEnumerator()
    {
      foreach (var field in _type.GetFields(BindingFlags.Instance |  
					    BindingFlags.NonPublic | 
					    BindingFlags.Public))
	{
	  var query = field.GetCustomAttributes(true).OfType<SaveAttribute>();
	  foreach (var attribute in query)
	    {
	      attribute.Field = field;
	      yield return attribute;
	    }
	}
    }
  }
}
