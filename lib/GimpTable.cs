// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2010 Maurits Rijk
//
// GimpTable.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
//

using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
{
  public class GimpTable : Table
  {
    public GimpTable(uint rows, uint columns, bool homogeneous = false) :
      base(rows, columns, homogeneous)
    {
    }

    public void AttachAligned(int column, int row, string label_text,
			      double xalign, double yalign, Widget widget,
			      int colspan, bool leftAlign)
    {
      gimp_table_attach_aligned(Handle, column, row, 
				label_text, (float) xalign, 
				(float) yalign, widget.Handle, 
				colspan, leftAlign);
      // return new Widget(ptr);
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_table_attach_aligned(IntPtr  table,
						   int	   column,
						   int     row,
						   string  label_text,
						   float   xalign,
						   float   yalign,
						   IntPtr  widget,
						   int     colspan,
						   bool    left_align);
  }
}
