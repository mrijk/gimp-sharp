using System;

using Gdk;

namespace Gimp.SliceTool
{
	public class RemoveFunc : MouseFunc
	{
		SliceData _sliceData;

		public RemoveFunc(SliceData sliceData, Preview preview) : base(preview, false, false)
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

		override public CursorType GetCursorType(int x, int y)
		{
			Slice slice = _sliceData.MayRemove(x, y);
			if (slice == null)
			{
				return base.GetCursorType(x, y);
			}
			else
			{
				return CursorType.XCursor;
			}
		}
	}
}