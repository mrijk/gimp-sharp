// The Ministeck plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// ShapeDescription.cs
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

namespace Gimp.Ministeck
  {
    public class ShapeDescription : IEnumerable 
    {
      List<Coordinate> _shape = new List<Coordinate>();

      public ShapeDescription()
      {
      }

      public void Add(int x, int y)
      {
	_shape.Add(new Coordinate(x, y));
      }

      IEnumerator IEnumerable.GetEnumerator() 
      {
	return _shape.GetEnumerator();
      }
    }
  }
