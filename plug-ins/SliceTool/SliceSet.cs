using System;
using System.Collections;

namespace Gimp.SliceTool
{
  public class SliceSet : IEnumerable
  {
    ArrayList _set = new ArrayList();

    public SliceSet()
    {
    }

    public IEnumerator GetEnumerator()
    {
      return _set.GetEnumerator();
    }

    public void Add(Slice slice)
    {
      _set.Add(slice);
    }

    Slice this[int index]
    {
      get {return (Slice) _set[index];}
    }

    public void Sort()
    {
      _set.Sort();

      int index = 1;
      Slice prev = this[0];
      prev.Index = index;

      for (int i = 1; i < _set.Count; i++)
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
      foreach (Slice slice in _set)
	{
	slice.Draw(renderer);
	}
    }

    public Slice Find(int x, int y)
    {
      foreach (Slice slice in _set)
	{
	if (slice.PointOn(x, y))
	  {
	  return slice;
	  }
	}
      return null;
    }

    public void Remove(Slice slice)
    {
      _set.Remove(slice);
    }
  }
  }
