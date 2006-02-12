// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// IntComboBox.cs
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
using System.Collections;
using System.Runtime.InteropServices;

using GLib;
using Gtk;

namespace Gimp
{
  public class IntComboBox : Widget
  {
    public IntComboBox(IntPtr raw) : base(raw)
    {
    }

    public int Active
    {
      get 
	{
	  int val;
	  if (!gimp_int_combo_box_get_active(Handle, out val))
	    {
	      throw new Exception();
	    }
	  return val;
	}
      set 
	{
	  if (!gimp_int_combo_box_set_active(Handle, value))
	    {
	      throw new Exception();
	    }
	}
    }

    public void Append(string label, int value)
    {
      gimp_int_combo_box_append(Handle, IntStoreColumns.LABEL, label,
				IntStoreColumns.VALUE, value, -1);
    }

    [GLib.Signal("changed")]
    public event EventHandler ColorChanged {
      add {
	GLib.Signal sig = GLib.Signal.Lookup (this, "changed");
	sig.AddDelegate (value);
      }
      remove {
	GLib.Signal sig = GLib.Signal.Lookup (this, "changed");
	sig.RemoveDelegate (value);     
      }
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_int_combo_box_get_active (IntPtr combo_box,
						      out int value);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_int_combo_box_set_active (IntPtr combo_box,
						      int value);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_int_combo_box_append (IntPtr combo_box,
						  IntStoreColumns col1,
						  string label,
						  IntStoreColumns col2,
						  int value,
						  int minus_one);
  }
}
