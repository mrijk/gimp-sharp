using System;
using System.Collections;

namespace Gimp.SliceTool
{
  public class SliceSet
  {
    ArrayList _slices = new ArrayList();

    public SliceSet()
    {
    }

    public void Add(Slice slice)
    {
      _slices.Add(slice);
    }

    public Slice this[int index]
    {
      get {return (Slice) _slices[index];}
    }

    public void Sort()
    {
      _slices.Sort();

      int index = 1;
      Slice prev = this[0];
      prev.Index = index;

      for (int i = 1; i < _slices.Count; i++)
	{
	if (this[i] != prev)
	  {
	  index++;
	  }
	prev = this[i];
	prev.Index = index;
	}
    }

    public void Draw(PreviewRenderer renderer)
    {
      foreach (Slice slice in _slices)
	{
	renderer.Draw(slice);
	}
    }
  }
  }
