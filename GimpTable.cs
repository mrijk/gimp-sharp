using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class GimpTable : Table
    {
      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_table_attach_aligned (
	IntPtr table,
	int             column,
	int             row,
	string label_text,
	float           xalign,
	float           yalign,
	IntPtr widget,
	int             colspan,
	bool         left_align);

      public GimpTable(uint rows, uint columns, bool homogeneous) :
	base(rows, columns, homogeneous)
      {
      }

      public Widget AttachAligned(int column, int row, string label_text,
				  double xalign, double yalign, Widget widget,
				  int colspan, bool left_align)
      {
	IntPtr ptr = gimp_table_attach_aligned(
	  Handle, column, row, label_text, (float) xalign, (float) yalign,
	  widget.Handle, colspan, left_align);
	return new Widget(ptr);
      }
    }
  }
