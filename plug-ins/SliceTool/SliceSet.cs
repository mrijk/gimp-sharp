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

    public void Sort()
    {
      _slices.Sort();
      foreach (Slice slice in _slices)
	{
	slice.Dump();
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
