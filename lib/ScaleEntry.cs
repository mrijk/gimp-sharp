// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2011 Maurits Rijk
//
// ScaleEntry.cs
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
  public class ScaleEntry : Adjustment
  {
    public ScaleEntry(Table table, int column, int row, string text,
		      int    scaleWidth,
		      int    spinbuttonWidth,
		      double value,
		      double lower,
		      double upper,
		      double stepIncrement,
		      double pageIncrement,
		      uint   digits,
		      bool   constrain = true,
		      double unconstrainedLower = 0.0,
		      double unconstrainedUpper = 0.0,
		      string tooltip = null,
		      string helpId = null) :
      base(gimp_scale_entry_new(table.Handle, column, row, text, scaleWidth,
				spinbuttonWidth, value, lower, upper, 
				stepIncrement, pageIncrement, digits,
				constrain, unconstrainedLower,
				unconstrainedUpper, tooltip, helpId))
    {
    }
 
    public ScaleEntry(Table table, int column, int row, string text,
		      int    scaleWidth,
		      int    spinbuttonWidth,
		      Variable<int> variable,
		      double lower,
		      double upper,
		      double stepIncrement,
		      double pageIncrement,
		      uint   digits) :
      this(table, column, row, text, scaleWidth, spinbuttonWidth,
	   variable.Value, lower, upper, stepIncrement, pageIncrement, digits)
    {
      ValueChanged += delegate {variable.Value = ValueAsInt;};
      variable.ValueChanged += delegate {Value = variable.Value;};
    } 

    public ScaleEntry(Table table, int column, int row, string text,
		      int    scaleWidth,
		      int    spinbuttonWidth,
		      Variable<double> variable,
		      double lower,
		      double upper,
		      double stepIncrement,
		      double pageIncrement,
		      uint   digits) :
      this(table, column, row, text, scaleWidth, spinbuttonWidth,
	   variable.Value, lower, upper, stepIncrement, pageIncrement, digits)
    {
      ValueChanged += delegate {variable.Value = Value;};
      variable.ValueChanged += delegate {Value = variable.Value;};
    }

    public int ValueAsInt
    {
      get {return (int) Value;}
    }

    public bool Sensitive
    {
      set {gimp_scale_entry_set_sensitive(Handle, value);}
    }

    public bool Logarithmic
    {
      get {return gimp_scale_entry_get_logarithmic(Handle);}
      set {gimp_scale_entry_set_logarithmic(Handle, value);}
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_scale_entry_new(IntPtr table,
					      int    column,
					      int    row,
					      string	text,
					      int    scale_width,
					      int    spinbutton_width,
					      double value,
					      double lower,
					      double upper,
					      double step_increment,
					      double page_increment,
					      uint   digits,
					      bool   constrain,
					      double	unconstrained_lower,
					      double unconstrained_upper,
					      string tooltip,
					      string help_id);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_scale_entry_set_sensitive(IntPtr adjustment,
						      bool sensitive);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_scale_entry_get_logarithmic(IntPtr adjustment);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_scale_entry_set_logarithmic(IntPtr adjustment,
							bool logarithmic);
  }
}
