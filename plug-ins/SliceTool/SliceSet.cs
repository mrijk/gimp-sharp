// The SliceTool plug-in
// Copyright (C) 2004-2013 Maurits Rijk
//
// SliceSet.cs
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

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gimp.SliceTool
{
  public class SliceSet
  {
    readonly List<Slice> _set = new List<Slice>();

    bool _changed = false;

    public IEnumerator<Slice> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Add(Slice slice)
    {
      Changed = true;
      _set.Add(slice);
    }

    public Slice this[int index]
    {
      get {return _set[index];}
    }

    public void Clear()
    {
      Changed = true;
      _set.Clear();
    }

    public bool Changed
    {
      get 
	{
	  return _changed || _set.Exists(slice => slice.Changed);
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
      _set.ForEach(slice => slice.Draw(renderer));
    }

    public Slice Find(IntCoordinate c)
    {
      return _set.Find(slice => slice.PointOn(c));
    }

    bool IsEndPoint(Slice s)
    {
      return _set.Exists(slice => slice.Begin == s || slice.End == s);
    }

    public Slice MayRemove(SliceSet orthogonal, IntCoordinate c)
    {
      var slice = Find(c);
      return (slice == null || orthogonal.IsEndPoint(slice)) ? null : slice; 
    }

    public void Remove(Slice slice)
    {
      _set.Remove(slice);
    }

    public void SetIndex()
    {
      int index = 0;
      _set.ForEach(slice => {slice.Index = index++;});
    }

    public void Save(StreamWriter w)
    {
      _set.ForEach(slice => slice.Save(w));
      Changed = false;
    }

    public void Resolve(SliceSet slices)
    {
      _set.ForEach(slice => slice.Resolve(slices));
    }
  }
}
