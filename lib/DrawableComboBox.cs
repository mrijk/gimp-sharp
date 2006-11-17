// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// DrawableComboBox.cs
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
  public delegate bool DrawableConstraintFunc(Int32 imageId, Int32 drawableId,
					      IntPtr data);

  public class DrawableComboBox : IntComboBox
  {
    public DrawableComboBox() : this(null, IntPtr.Zero)
    {
    }

    public DrawableComboBox(DrawableConstraintFunc constraint, IntPtr data) : 
      base(gimp_drawable_combo_box_new(constraint, data))
    {
    }

    public new Drawable Active
    {
      get 
	{
	  return new Drawable(base.Active);
	}
      set
	{
	  base.Active = value.ID;
	}
    }
 
    [DllImport("libgimpui-2.0-0.dll")]
    extern static IntPtr 
    gimp_drawable_combo_box_new(DrawableConstraintFunc constraint,
				IntPtr data);
  }
}
