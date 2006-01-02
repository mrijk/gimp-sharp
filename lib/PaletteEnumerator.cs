// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// PaletteEnumerator.cs
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

// TODO: reimplement this with C# 2.0 yield

using System;
using System.Collections;

namespace Gimp
  {
    public class PaletteEnumerator : IEnumerator
    {
      Palette _palette;
      int _numColors;
      int _index;

      public PaletteEnumerator(Palette palette)
      {
	_palette = palette;
	palette.GetInfo(out _numColors);
	Reset();
      }
      
      public bool MoveNext()
      {
	if (_index >= _numColors)
	  {
	  return false;
	  }
	else
	  {
	  _index++;
	  return true;
	  }
      }
      
      public object Current
      {
	get {return _palette[_index - 1];}
      }
      
      public void Reset()
      {
	_index = 0;
      }
    }
  }
