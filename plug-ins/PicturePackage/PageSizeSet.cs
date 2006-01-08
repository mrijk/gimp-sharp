// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// PageSizeSet.cs
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

namespace Gimp.PicturePackage
{
  public class PageSizeSet
  {
#if false
    List<PageSize> _set = new List<PageSize>();
#else
  ArrayList _set = new ArrayList();
#endif

    public PageSizeSet()
    {
    }

    public void Add(PageSize size)
    {
      int index = _set.BinarySearch(size);
      // Console.WriteLine("index: " + index);
      if (index < 0)
	{
	_set.Insert(-index - 1, size);
	}
      // Console.WriteLine("Size: " + _set.Count);
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public PageSize this[int index]
    {
      get {return (PageSize) _set[index];}
    }
  }
  }
