// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ActionSet.cs
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
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  public class ActionSet
  {
    List<Action> _set = new List<Action>();

    string _name;
    byte _expanded;
    int _setChildren;

    public ActionSet(string name)
    {
      _name = name;
    }

    public byte Expanded
    {
      set {_expanded = value;}
    }

    public int SetChildren
    {
      get {return _setChildren;}
      set {_setChildren = value;}
    }

    public string Name
    {
      get {return _name + " (" + _setChildren + ")";}
    }

    public void Add(Action action)
    {
      _set.Add(action);
    }

    public IEnumerator<Action> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Execute(int action)
    {
      _set[action].Execute();
    }

    public bool IsExecutable
    {
      get
	{
	  if (_setChildren != _set.Count)
	    {
	      return false;	// Not fully parsed
	    }

	  // TODO: there are smarter constructs for this!
	  foreach (Action action in _set)
	    {
	      if (!action.IsExecutable)
		{
		  return false;
		}
	    }
	  return true;
	}
    }
  }
}
