// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// GimpPreview.cs
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

using Gdk;
using GLib;
using Gtk;

namespace Gimp
{
  public class GimpPreview : VBox
  {
    public GimpPreview(IntPtr ptr) : base(ptr)
    {
    }

    public bool Update
    {
      get {return gimp_preview_get_update(Handle);}
      set {gimp_preview_set_update(Handle, value);}
    }

    public void SetBounds(int xmin, int ymin, int xmax, int ymax)
    {
      gimp_preview_set_bounds(Handle, xmin, ymin, xmax, ymax);
    }

    public void GetPosition(out int x, out int y)
    {
      gimp_preview_get_position(Handle, out x, out y);
    }

    public void GetSize(out int width, out int height)
    {
      gimp_preview_get_size(Handle, out width, out height);
    }

    public void Draw()
    {
      gimp_preview_draw(Handle);
    }

    public void DrawBuffer(byte[] buffer, int rowstride)
    {
      gimp_preview_draw_buffer(Handle, buffer, rowstride);
    }

    public void Invalidate()
    {
      gimp_preview_invalidate(Handle);
    }

    public Cursor DefaultCursor
    {
      set {gimp_preview_set_default_cursor(Handle, value);}
    }

    [GLib.Signal("invalidated")]
    public event EventHandler Invalidated {
      add {
	GLib.Signal sig = GLib.Signal.Lookup (this, "invalidated");
	sig.AddDelegate (value);
      }
      remove {
	GLib.Signal sig = GLib.Signal.Lookup (this, "invalidated");
	sig.RemoveDelegate (value);                        }
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_update (IntPtr preview,
						bool update);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static bool gimp_preview_get_update (IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_bounds (IntPtr preview,
						int xmin, int ymin, 
						int xmax, int ymax);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_get_position(IntPtr preview,
						 out int x, out int y);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_get_size(IntPtr preview,
					     out int width, out int height);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_draw(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_draw_buffer(IntPtr preview,
						byte[] buffer, 
						int rowstride);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_invalidate(IntPtr preview);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_preview_set_default_cursor(IntPtr preview,
						       Cursor cursor);
  }
}
