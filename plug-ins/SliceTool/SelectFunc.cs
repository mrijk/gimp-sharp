using System;

namespace Gimp.SliceTool
{
  public class SelectFunc : MouseFunc
  {
    SliceTool _parent;
    PreviewRenderer _renderer;
    SliceData _sliceData;
    Slice _slice;
    Rectangle _rectangle;

    public SelectFunc(SliceTool parent, SliceData sliceData, Preview preview) : base(preview, true, true)
    {
      _parent = parent;
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(int x, int y) 
    {
      Slice slice = _sliceData.FindSlice(x, y);
      if (slice == null)
	{
	Rectangle rectangle = _sliceData.SelectRectangle(x, y);
	if (rectangle != _rectangle)
	  {
	  _parent.SetRectangleData(_rectangle);
	  _rectangle = rectangle;
	  _sliceData.Draw(_renderer);
	  _parent.GetRectangleData(rectangle);
	  }
	}
      else
	{
	_slice = slice;
	_renderer.Function = Gdk.Function.Equiv;
	}
    }

    override protected void OnRelease() 
    {
      _renderer.Function = Gdk.Function.Copy;
      _slice.Draw(_renderer);
    }
		
    override protected void OnMove(int x, int y) 
    {
      _slice.Draw(_renderer);
      _slice = _sliceData.GetSlice(x, y);
      _slice.SetPosition(x, y);
      _slice.Draw(_renderer);		
    }
  }
  }
