using System;

namespace Gimp.SliceTool
{
  public class RemoveFunc : MouseFunc
  {
    SliceData _sliceData;

    public RemoveFunc(SliceData sliceData, Preview preview) : 
      base(preview, false, false)
    {
      _sliceData = sliceData;
    }

    override protected void OnPress(int x, int y) 
    {
      Slice slice = _sliceData.MayRemove(x, y);
      if (slice != null)
	{
	_sliceData.Remove(slice);
	Redraw();
	}
    }
  }
  }
