using System;

namespace Gimp.SliceTool
{
  public class TableDialog : GimpDialog
  {
    int _columns = 3;
    int _rows = 3;

    public TableDialog() : base("Insert Table", "SliceTool",
				IntPtr.Zero, 0, null, "SliceTool")
    {
      GimpTable table = new GimpTable(2, 3, false);
      table.BorderWidth = 12;
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      VBox.PackStart(table, true, true, 0);

      new ScaleEntry(table, 0, 1, "Co_lumns", 150, 3,
		     _columns, 1.0, 16.0, 1.0, 1.0, 0,
		     true, 0, 0, null, null);

      new ScaleEntry(table, 0, 2, "_Rows", 150, 3,
		     _rows, 1.0, 16.0, 1.0, 1.0, 0,
		     true, 0, 0, null, null);
    }
  }
  }
