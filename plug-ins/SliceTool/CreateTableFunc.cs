using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
	public class CreateTableFunc : MouseFunc
	{
		SliceData _sliceData;

		public CreateTableFunc(SliceData sliceData, Preview preview) : base(preview, false, false)
		{
			_sliceData = sliceData;
		}

		override protected void OnPress(int x, int y) 
		{
			TableDialog dialog = new TableDialog();
			dialog.ShowAll();
			ResponseType type = dialog.Run();
			if (type == ResponseType.Ok)
			{
				_sliceData.CreateTable(x, y, dialog.Rows, dialog.Columns);
				Redraw();
			}
			dialog.Destroy();
		}

		override public CursorType GetCursorType(int x, int y)
		{
			return CursorType.RtlLogo;	// Fix me!
		}
	}
}