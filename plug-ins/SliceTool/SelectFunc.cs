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

		public SelectFunc(SliceTool parent, SliceData sliceData, Preview preview) :
			base(preview, false, false)
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
					Redraw();
					_parent.GetRectangleData(rectangle);
				}
			}
			else if (!slice.Locked)
			{
				_slice = slice;
				_renderer.Function = Gdk.Function.Equiv;
				AddReleaseEvent();
				AddMotionNotifyEvent();
			}
		}

		override protected void OnRelease() 
		{
			_renderer.Function = Gdk.Function.Copy;
			_sliceData.Cleanup(_slice);
			Redraw();
		}
		
		override protected void OnMove(int x, int y) 
		{
			_slice.Draw(_renderer);
			_slice.SetPosition(x, y);
			_slice.Draw(_renderer);		
		}
	}
}