using System;

using Gtk;

namespace Gimp.SliceTool
{
  public class RolloverDialog : GimpDialog
  {
    FileEntry _mouseOver;
    FileEntry _mouseOut;
    FileEntry _mouseClick;
    FileEntry _mouseDoubleClick;
    FileEntry _mouseUp;
    FileEntry _mouseDown;

    public RolloverDialog() : base("Rollover Creator", "SliceTool",
				   IntPtr.Zero, 0, null, "SliceTool")
    {
      GimpTable table = new GimpTable(7, 3, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      _mouseOver = CreateEntry(table, "_Mouse over", 0, 
			       new EventHandler(OnMouseOver));
      _mouseOut = CreateEntry(table, "Mo_use out", 1,
			       new EventHandler(OnMouseOut));
      _mouseClick = CreateEntry(table, "Mous_e click", 2,
			       new EventHandler(OnMouseClick));
      _mouseDoubleClick = CreateEntry(table, "Mouse dou_ble click", 3,
			       new EventHandler(OnMouseDoubleClick));
      _mouseUp = CreateEntry(table, "Mouse _up", 4,
			       new EventHandler(OnMouseUp));
      _mouseDown = CreateEntry(table, "Mouse _down", 5,
			       new EventHandler(OnMouseDown));

      Label label = new Label("If a file is not given for the rollover, the original file will be used.");
      table.Attach(label, 0, 2, 6, 7);
    }

    FileEntry CreateEntry(GimpTable table, string label, uint row,
			  EventHandler clicked)
    {
      CheckButton button = new CheckButton(label);
      button.Clicked += clicked;
      table.Attach(button, 0, 1, row, row + 1);

      FileEntry entry = new FileEntry("Select Image", "", false, true);
      entry.Sensitive = false;
      table.Attach(entry, 1, 2, row, row + 1);

      return entry;
    }

    void SetFileEntry(object o, FileEntry entry)
    {
      bool active = (o as CheckButton).Active;
      entry.Sensitive = active;

      if (!active)
	{
	entry.FileName = "";
	}
    }

    void OnMouseOver(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseOver);
    }

    void OnMouseOut(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseOut);
    }

    void OnMouseClick(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseClick);
    }

    void OnMouseDoubleClick(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseDoubleClick);
    }

    void OnMouseUp(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseUp);
    }

    void OnMouseDown(object o, EventArgs args)
    {
      SetFileEntry(o, _mouseDown);
    }

    public bool Enabled
    {
      get {return false;}	// Fix me!
    }
  }
  }
