// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// DataObjectList.cs
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
  public abstract class DataObjectList<T> where T : DataObject
  {
    readonly List<T> _list; // = new List<T>();

    public DataObjectList(string filter)
    {
      int numDataObjects;
      IntPtr ptr = GetList(filter, out numDataObjects);
      _list = Util.ToList<T>(ptr, numDataObjects, CreateT);
    }

    protected abstract IntPtr GetList(string filter, out int numDataObjects);

    protected abstract T CreateT(string name);

    public IEnumerator<T> GetEnumerator()
    {
      return _list.GetEnumerator();
    }

    public void ForEach(Action<T> action)
    {
      _list.ForEach(action);
    }

    public int Count
    {
      get {return _list.Count;}
    }
  }
}
