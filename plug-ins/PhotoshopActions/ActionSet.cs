// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
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
using System.Linq;

namespace Gimp.PhotoshopActions
{
  public class ActionSet : IExecutable
  {
    List<Action> _set = new List<Action>();

    bool _enabled = true;

    public string Name {get;}
    public byte Expanded {private get; set;}
    public int SetChildren {get; set;}

    public ActionSet(string name)
    {
      Name = name;
    }

    public string ExtendedName => Name + " (" + SetChildren + ")";

    public void Add(Action action) => _set.Add(action);

    public IEnumerator<Action> GetEnumerator() => _set.GetEnumerator();

    public void Execute(int action)
    {
      _set[action].Execute();
    }

    public bool Execute(string actionName)
    {
      foreach (var action in _set)
	{
	  if (action.Name == actionName)
	    {
	      action.Execute();
	      return true;
	    }
	}
      return false;
    }

    public void Execute(int action, int n)
    {
      _set[action].Execute(n);
    }

    public bool IsExecutable => SetChildren == NrOfActions &&
      _set.TrueForAll(action => action.IsExecutable);

    public bool IsEnabled
    {
      get {return _enabled;}
      set 
	{
	  _enabled = value;
	  _set.ForEach(action => {action.IsEnabled = value;});
	}
    }

    public int ActionEvents => _set.Select(action => action.ActionEvents).Sum();
    
    public int ExecutableActionEvents =>
      _set.Select(action => action.ExecutableActionEvents).Sum();

    public int NrOfActions => _set.Count;

    public int ExecutableActions =>
      _set.Where(action => action.IsExecutable).Count();
  }
}
