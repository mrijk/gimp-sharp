using System;

using Gdk;

namespace Gimp.SliceTool
{
  public class CreateFunc : MouseFunc
  {
    PreviewRenderer _renderer;
    SliceData _sliceData;
    Rectangle _rectangle, _endRectangle;
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
      Rectangle rectangle;
      if (_horizontal)
	{
	rectangle = _sliceData.FindRectangle(x, _y);
	}
      else
	{
	rectangle = _sliceData.FindRectangle(_x, y);
	}

      bool rectangleChanged = rectangle != _endRectangle;
      if (rectangleChanged)
	{
	_endRectangle = rectangle;
	}

      bool orientationChanged = _horizontal ^ ((Math.Abs(x - _x) > 
						Math.Abs(y - _y)));
      if (orientationChanged)
	{
	_horizontal = !_horizontal;
	}

      if (orientationChanged || rectangleChanged)
	{
	_slice.Draw(_renderer);
	if (_horizontal)
	  {
	  if (rectangle.Left.X <= _rectangle.Left.X)
	    {
	    _slice = new HorizontalSlice(rectangle.Left, _rectangle.Right, _y);
	    }
	  else
	    {
	    _slice = new HorizontalSlice(_rectangle.Left, rectangle.Right, _y);
	    }
	  }
	else
	  {
	  if (rectangle.Top.Y <= _rectangle.Top.Y)
	    {
	    _slice = new VerticalSlice(rectangle.Top, _rectangle.Bottom, _x);
	    }
	  else
	    {
	    _slice = new VerticalSlice(_rectangle.Top, rectangle.Bottom, _x);
	    }
	  }
	_slice.Draw(_renderer);
	}
    }

    override public CursorType GetCursorType(int x, int y)
    {
      Slice slice = _sliceData.FindSlice(x, y);
      if (slice != null && !slice.Locked)
	{
	return slice.CursorType;
	}
      else
	{
	return CursorType.Pencil;
	}
    }

    override public MouseFunc GetActualFunc(SliceTool parent, int x, int y)
    {
      Slice slice = _sliceData.FindSlice(x, y);
      if (slice == null || slice.Locked)
	{
	return this;
	}
      else
	{
	return new SelectFunc(parent, _sliceData, _preview);
	}
    }
  }
  }
