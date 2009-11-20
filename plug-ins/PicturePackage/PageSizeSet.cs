// The PicturePackage plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// PageSizeSet.cs
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
using System.Collections.Generic;

namespace Gimp.PicturePackage
{
  public class PageSizeSet
  {
    readonly List<PageSize> _set = new List<PageSize>();

    public void Add(PageSize size)
    {
      int index = _set.BinarySearch(size);
      if (index < 0)
	{
	  _set.Insert(-index - 1, size);
	}
    }

    public PageSize this[int index]
    {
      get {return _set[index];}
    }

    public void ForEach(Action<PageSize> action)
    {
      _set.ForEach(action);
    }
  }
}
