// The PicturePackage plug-in
// Copyright (C) 2004-2013 Maurits Rijk
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
using System.Linq;

namespace Gimp.PicturePackage
{
  public class PageSizeSet
  {
    readonly SortedSet<PageSize> _set = new SortedSet<PageSize>();

    public void Add(PageSize size)
    {
      _set.Add(size);
    }

    public PageSize this[int index]
    {
      get {return _set.ElementAt(index);}
    }

    public void ForEach(Action<PageSize> action)
    {
      _set.ToList().ForEach(action);
    }
  }
}
