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

      _mouseOver = CreateEntry(table, "_Mouse over", 0);
      _mouseOut = CreateEntry(table, "Mo_use out", 1);
      _mouseClick = CreateEntry(table, "Mous_e click", 2);
      _mouseDoubleClick = CreateEntry(table, "Mouse dou_ble click", 3);
      _mouseUp = CreateEntry(table, "Mouse _up", 4);
      _mouseDown = CreateEntry(table, "Mouse _down", 5);

      Label label = new Label("If a file is not given for the rollover, the original file will be used.");
      table.Attach(label, 0, 2, 6, 7);
    }

    FileEntry CreateEntry(GimpTable table, string label, uint row)
    {
      CheckButton button = new CheckButton(label);
      // button.Clicked += new EventHandler(OnMouseOver);
      table.Attach(button, 0, 1, row, row + 1);

      FileEntry entry = new FileEntry("Select Image", "", false, true);
      entry.Sensitive = false;
      table.Attach(entry, 1, 2, row, row + 1);

      return entry;
    }

    void OnMouseOver(object o, EventArgs args)
    {

    }
  }
  }
