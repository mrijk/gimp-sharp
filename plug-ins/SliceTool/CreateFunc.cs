using System;

namespace Gimp.SliceTool
{
  public class CreateFunc : MouseFunc
  {
    PreviewRenderer _renderer;
    SliceData _sliceData;
    Rectangle _rectangle;
    Slice _slice;
    int _x, _y;
    bool _horizontal;

    public CreateFunc(SliceData sliceData, Preview preview) : 
      base(preview, true, true)
    {
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(int x, int y) 
    {
      _x = x;
      _y = y;
      _rectangle = _sliceData.FindRectangle(x, y);
      _slice = _rectangle.CreateHorizontalSlice(y);
      _horizontal = true;
      _renderer.Function = Gdk.Function.Equiv;
      _slice.Draw(_renderer);
    }

    override protected void OnRelease() 
    {
      _slice.Draw(_renderer);
      _sliceData.AddSlice(_slice);
      _renderer.Function = Gdk.Function.Copy;
      Redraw();
    }
		
    override protected void OnMove(int x, int y) 
    {
      if (_horizontal ^ ((Math.Abs(x - _x) > Math.Abs(y - _y))))
	{
	_slice.Draw(_renderer);
	if (_horizontal)
	  {
	  _horizontal = false;
	  _slice = _rectangle.CreateVerticalSlice(_x);
	  }
	else
	  {
	  _horizontal = true;
	  _slice = _rectangle.CreateHorizontalSlice(_y);
	  }
	_slice.Draw(_renderer);
	}
    }
  }
  }
