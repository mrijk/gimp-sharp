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
