// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// DrawableList.cs
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
using System.Runtime.InteropServices;

namespace Gimp
{
  public abstract class DrawableList<T> where T : Drawable, new()
  {
    readonly List<T> _list = new List<T>();

    public int Count => _list.Count;
    public T this[int index] => _list[index];

    protected delegate IntPtr GetDrawablesFunc(Int32 drawable_ID, 
					       out int num_drawables);

    protected DrawableList() {}

    protected DrawableList(Image image, GetDrawablesFunc getDrawables)
    {
      IntPtr ptr = getDrawables(image.ID, out int numDrawables);
      FillListFromPtr(ptr, numDrawables);
    }
    
    protected void FillListFromPtr(IntPtr ptr, int numDrawables)
    {
      if (numDrawables > 0)
	{
	  var dest = new int[numDrawables];
	  Marshal.Copy(ptr, dest, 0, numDrawables);
	  Array.ForEach(dest, 
			drawableID => Add(NewFromItem(new Item(drawableID))));
	}
    }

    protected virtual T NewFromItem(Item item) => new T() {ID = item.ID};

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    protected void Add(T drawable) => _list.Add(drawable);

    public void ForEach(Action<T> action) => _list.ForEach(action);

    public T this[string name] => _list.Find(drawable => drawable.Name == name);

    public int GetIndex(T key) =>
      _list.FindIndex(drawable => drawable.Name == key.Name);
  }
}
