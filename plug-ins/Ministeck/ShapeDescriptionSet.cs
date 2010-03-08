// The Ministeck plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// ShapeDescriptionSet.cs
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

namespace Gimp.Ministeck
{
  public class ShapeDescriptionSet
  {
    readonly List<ShapeDescription> _set;

    public ShapeDescriptionSet(params ShapeDescription[] shapes)
    {
      _set = new List<ShapeDescription>(shapes);
    }

    public ShapeDescriptionSet(ShapeDescriptionSet s)
    {
      _set = new List<ShapeDescription>(s._set);
    }

    public IEnumerator<ShapeDescription> GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Insert(int index, ShapeDescription val)
    {
      _set.Insert(index, val);
    }

    public int Count
    {
      get {return _set.Count;}
    }
  }
}
