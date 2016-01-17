// The Slice Tool plug-in
// Copyright (C) 2004-2016 Maurits Rijk
//
// JavaScriptProperty.cs
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
using System.Collections;

namespace Gimp.SliceTool
{
  public class JavaScriptProperty : Property
  {
    static Hashtable _preload = new Hashtable();

    public JavaScriptProperty(string name) : base(name)
    {
    }

    public override string Value
    {
      set 
	{
          if (value != base.Value)
            {
	      if (_preload.ContainsKey(base.Value))
		{
		  int refCount = (int) _preload[base.Value];
		  refCount--;
		  if (refCount == 0)
		    {
		      _preload.Remove(base.Value);
		    }
		  else
		    {
		      _preload[base.Value] = refCount;
		    }
		}

	      if (_preload.ContainsKey(value))
		{
		  int refCount = (int) _preload[value];
		  refCount++;
		  _preload[value] = refCount;
		}
	      else if (value.Length > 0)
		{
		  _preload.Add(value, 1);
		}

	      base.Value = value;
            }
	}
    }

    public static string[] Preload
    {
      get 
	{
          var array = new string[_preload.Count];
          _preload.Keys.CopyTo(array, 0);
          return array;
	}
    }
  }
}
