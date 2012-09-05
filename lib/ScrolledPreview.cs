// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2012 Maurits Rijk
//
// ScrolledPreview.cs
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
  public class ScrolledPreview : GimpPreview
  {
    public ScrolledPreview(IntPtr ptr) : base (ptr)
    {
    }

    public void SetPosition(int x, int y)
    {
      gimp_scrolled_preview_set_position(Handle, x, y);
    }

    public void SetPolicy(PolicyType hscrollbarPolicy, 
			  PolicyType vscrollbarPolicy)
    {
      gimp_scrolled_preview_set_policy(Handle, hscrollbarPolicy, 
				       vscrollbarPolicy);
    }

    protected void Freeze()
    {
      gimp_scrolled_preview_freeze(Handle);
    }

    protected void Thaw()
    {
      gimp_scrolled_preview_thaw(Handle);
    }

    [DllImport("libgimpui-2.0-0.dll")]
    extern static void gimp_scrolled_preview_set_position(IntPtr preview,
							  int x, int y);
    [DllImport("libgimpui-2.0-0.dll")]
    extern static void gimp_scrolled_preview_set_policy(IntPtr preview,
						PolicyType hscrollbar_policy,
						PolicyType vscrollbar_policy);
    [DllImport("libgimpui-2.0-0.dll")]
    extern static void gimp_scrolled_preview_freeze(IntPtr preview);
    [DllImport("libgimpui-2.0-0.dll")]
    extern static void gimp_scrolled_preview_thaw(IntPtr preview);
  }
}
