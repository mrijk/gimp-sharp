// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2009 Maurits Rijk
//
// GimpFontSelectWidget.cs
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
  public class GimpFontSelectWidget : Widget
  {
    public GimpFontSelectWidget(string title, string font_name) : 
      base(gimp_font_select_widget_new(title, font_name, 
				       IntPtr.Zero, IntPtr.Zero))
    {
    }
    
    public void Close()
    {
    }
    
    public void Set(string font_name)
    {
    }
    
    [DllImport("libgimpui-2.0-0.dll")]
    extern static IntPtr gimp_font_select_widget_new(string title,
						     string font_name,
						     IntPtr callback,
						     IntPtr data);
  }
}
