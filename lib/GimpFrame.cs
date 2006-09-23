// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// GimpFrame.cs
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

using Mono.Unix;

using Gtk;

namespace Gimp
  {
    public class GimpFrame : Frame
    {
      public GimpFrame(string label) : base(gimp_frame_new(label))
      {
      }

      static protected string _(string s)
      {
	return Catalog.GetString(s);
      }

      [DllImport("libgimpwidgets-2.0-0.dll")]
      extern static IntPtr gimp_frame_new(string label);
    }
  }
