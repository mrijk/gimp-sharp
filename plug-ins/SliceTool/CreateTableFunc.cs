using System;

using Gtk;

namespace Gimp.SliceTool
{
  public class CreateTableFunc : MouseFunc
  {
    PreviewRenderer _renderer;
    SliceData _sliceData;

    public CreateTableFunc(SliceData sliceData, Preview preview) : 
      base(preview, false, false)
    {
      _sliceData = sliceData;
      _renderer = preview.Renderer;
    }

    override protected void OnPress(int x, int y) 
    {
      TableDialog dialog = new TableDialog();
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      if (type == ResponseType.Ok)
	{
	_sliceData.CreateTable(x, y, dialog.Rows, dialog.Columns);
	_sliceData.Draw(_renderer);
	}
      dialog.Destroy();
    }
  }
  }
