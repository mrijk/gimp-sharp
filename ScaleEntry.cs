using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public class ScaleEntry
    {
      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_scale_entry_new (
	IntPtr       table,
	int          column,
	int          row,
	string	text,
	int          scale_width,
	int          spinbutton_width,
	double       value,
	double       lower,
	double       upper,
	double       step_increment,
	double       page_increment,
	uint         digits,
	bool         constrain,
	double       unconstrained_lower,
	double       unconstrained_upper,
	string 	tooltip,
	string 	help_id);

      public ScaleEntry(Table table, int column, int row, string text,
			int          scale_width,
			int          spinbutton_width,
			double       value,
			double       lower,
			double       upper,
			double       step_increment,
			double       page_increment,
			uint         digits,
			bool         constrain,
			double       unconstrained_lower,
			double       unconstrained_upper,
			string 	tooltip,
			string 	help_id) 
      {
	IntPtr adj = gimp_scale_entry_new(table.Handle, column, row, text, scale_width,
			     spinbutton_width, value, lower, upper, 
			     step_increment, page_increment, digits,
			     constrain, unconstrained_lower,
			     unconstrained_upper, tooltip, help_id);
      }
    }
  }
