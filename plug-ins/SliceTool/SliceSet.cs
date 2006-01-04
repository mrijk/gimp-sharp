// The SliceTool plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// SliceSet.cs
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
using System.IO;

namespace Gimp.SliceTool
{
  public class SliceSet : IEnumerable
  {
    List<Slice> _set = new List<Slice>();

    bool _changed = false;

    public SliceSet()
    {
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Add(Slice slice)
    {
      _changed = true;
      _set.Add(slice);
    }

    public Slice this[int index]
    {
      get {return _set[index];}
    }

    public void Clear()
    {
      _changed = true;
      _set.Clear();
    }

    public bool Changed
    {
      get 
	  {
	  if (_changed == false)
	    {
	    foreach (Slice slice in _set)
	      {
	      if (slice.Changed)
		{
		return true;
		}
	      }
	    }
	  return _changed;
	  }
      set {_changed = value;}
    }

    public void Sort()
    {
      _set.Sort();

      int index = 1;
      Slice prev = this[0];
      prev.Index = index;

      for (int i = 1; i < _set.Count; i++)
	{
	if (this[i] != prev)
	  {
	  index++;
	  }
	prev = this[i];
	prev.Index = index;
	}
    }

    public void Draw(PreviewRenderer renderer)
    {
      foreach (Slice slice in _set)
	{
	slice.Draw(renderer);
	}
    }

    public Slice Find(int x, int y)
    {
      foreach (Slice slice in _set)
	{
	if (slice.PointOn(x, y))
	  {
	  return slice;
	  }
	}
      return null;
    }

    public bool IsEndPoint(Slice s)
    {
      foreach (Slice slice in _set)
	{
	if (slice.Begin == s || slice.End == s)
	  {
	  return true;
	  }
	}
      return false;
    }

    public void Remove(Slice slice)
    {
      _set.Remove(slice);
    }

    public void SetIndex()
    {
      int index = 0;
      foreach (Slice slice in _set)
	{
	slice.Index = index++;
	}
    }

    public void Save(StreamWriter w)
    {
      foreach (Slice slice in _set)
	{
	slice.Save(w);
	}
      _changed = false;
    }

    public void Resolve(SliceSet slices)
    {
      foreach (Slice slice in _set)
	{
	slice.Resolve(slices);
	}
    }
  }
  }
