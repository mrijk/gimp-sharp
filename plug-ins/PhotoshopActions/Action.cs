// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// Action.cs
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
  public class Action
  {
    List<ActionEvent> _set = new List<ActionEvent>();

    string _name;
    byte _shiftKey;
    byte _commandKey;
    int _colorIndex;
    byte _expanded;
    int _nrOfChildren;

    public Action()
    {
    }

    public byte ShiftKey
    {
      set {_shiftKey = value;}
    }

    public byte CommandKey
    {
      set {_commandKey = value;}
    }

    public int ColorIndex
    {
      set {_colorIndex = value;}
    }

    public byte Expanded
    {
      get {return _expanded;}
      set {_expanded = value;}
    }

    public int NrOfChildren
    {
      get {return _nrOfChildren;}
      set {_nrOfChildren = value;}
    }

    public string Name
    {
      get {return _name;}
      set {_name = value;}
    }

    public bool IsExecutable
    {
      get
	{
	  if (_set.Count != _nrOfChildren)
	    {
	      return false;
	    }

	  foreach (ActionEvent actionEvent in _set)
	    {
	      if (!actionEvent.IsExecutable)
		{
		  return false;
		}
	    }
	  return true;
	}
    }

    public int ActionEvents
    {
      get
	{
	  return _set.Count;
	}
    }

    public int ExecutableActionEvents
    {
      get
	{
	  int sum = 0;
	  foreach (ActionEvent actionEvent in _set)
	    {
	      if (actionEvent.IsExecutable)
		{
		  sum++;
		}
	    }
	  return sum;
	}
    }

    public void Add(ActionEvent actionEvent)
    {
      _set.Add(actionEvent);
    }

    public void Execute()
    {
      foreach (ActionEvent actionEvent in _set)
	{
	  if (!actionEvent.Execute())
	    break;
	}
    }

    public IEnumerator<ActionEvent> GetEnumerator()
    {
      return _set.GetEnumerator();
    }
  }
}
