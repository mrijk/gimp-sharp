// The Difference Clouds plug-in
// Copyright (C) 2006-2011 Maurits Rijk (maurits.rijk@gmail.com)
//
// IndexedColorsMap.cs
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

using System.Linq;

namespace Gimp.DifferenceClouds
{
  class IndexedColorsMap
  {
    readonly RGB[] _indexedColorsMap;

    public IndexedColorsMap()
    {
      var fgBytes = Context.Foreground;
      var bgBytes = Context.Background;

      _indexedColorsMap = Enumerable.Range(0, 256).
	Select(i => RGB.Interpolate(i / 256.0, fgBytes, bgBytes)).ToArray();
    }

    public RGB this[int index]
    {
      get {return _indexedColorsMap[index];}
    }

    public byte this[int index, int b]
    {
      get {return this[index].Bytes[b];}
    }
  }
}

