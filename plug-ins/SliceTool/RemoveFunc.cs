using System;

namespace Gimp.SliceTool
{
	public class RemoveFunc : MouseFunc
	{
		PreviewRenderer _renderer;
		SliceData _sliceData;

		public RemoveFunc(SliceData sliceData, Preview preview) : base(preview, false, false)
		{
			_sliceData = sliceData;
			_renderer = preview.Renderer;
		}

		override protected void OnPress(int x, int y) 
		{
			if (_sliceData.Remove(x, y))
			{
				Redraw();
			}
		}
	}
}