// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// FloatingSelection.cs
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

namespace Gimp
{
  public sealed class FloatingSelection : Layer
  {
    public FloatingSelection(Int32 floatingSelID) : base(floatingSelID)
    {
    }

    public void Remove()
    {
      if (!gimp_floating_sel_remove (_ID))
        {
	  throw new Exception();
        }
    }

    public void Anchor()
    {
      if (!gimp_floating_sel_anchor (_ID))
        {
	  throw new Exception();
        }
    }

    public void Attach(Drawable drawable)
    {
      if (!gimp_floating_sel_attach (_ID, drawable.ID))
	{
	  throw new GimpSharpException();
	}
    }

    public void Rigor(bool undo)
    {
      if (!gimp_floating_sel_rigor (_ID, undo))
        {
	  throw new Exception();
        }
    }

    public void Relax(bool undo)
    {
      if (!gimp_floating_sel_relax (_ID, undo))
        {
	  throw new Exception();
        }
    }

    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_remove (Int32 floating_sel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_anchor (Int32 floating_sel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_to_layer (Int32 floating_sel_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_attach (Int32 floating_sel_ID,
                                                 Int32 drawable_ID);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_rigor (Int32 floating_sel_ID,
                                                bool undo);
    [DllImport("libgimp-2.0-0.dll")]
    extern static bool gimp_floating_sel_relax (Int32 floating_sel_ID,
                                                bool undo);
  }
}
