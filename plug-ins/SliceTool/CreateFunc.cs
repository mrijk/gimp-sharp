using System;

namespace Gimp.SliceTool
{
  public class CreateFunc : MouseFunc
  {
    PreviewRenderer _renderer;
    SliceData _sliceData;
    Slice _slice;

    public CreateFunc(SliceData sliceData, Preview preview) : 
      base(preview, true, true)
    {
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(int x, int y) 
    {
      _slice = _sliceData.GetSlice(x, y);
      _renderer.Function = Gdk.Function.Equiv;
      _slice.Draw(_renderer);
    }

    override protected void OnRelease() 
    {
      _slice.Draw(_renderer);
      _sliceData.AddSlice(_slice);
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
