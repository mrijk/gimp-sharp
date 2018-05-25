// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2018 Maurits Rijk
//
// FileEntry.cs
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

using GLib;
using Gtk;

namespace Gimp
{
  public class FileEntry : HBox
  {
    public FileEntry(string title, string filename, bool dir_only, 
		     bool checkValid) :
      base(gimp_file_entry_new(title, filename, dir_only, checkValid))
    {
    }

    public string FileName
    {
      get => gimp_file_entry_get_filename(Handle);
      set => gimp_file_entry_set_filename(Handle, value);
    }

    [GLib.Signal("filename_changed")]
    public event EventHandler FilenameChanged {
      add {
	var sig = GLib.Signal.Lookup(this, "filename_changed");
	sig.AddDelegate (value);
      }
      remove {
	var sig = GLib.Signal.Lookup(this, "filename_changed");
	sig.RemoveDelegate (value);     
      }
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static IntPtr gimp_file_entry_new(string title, string filename, 
					     bool dir_only, bool check_valid);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static string gimp_file_entry_get_filename(IntPtr entry);
    [DllImport("libgimpwidgets-2.0-0.dll")]
    extern static void gimp_file_entry_set_filename(IntPtr entry, 
						    string filename);
  }
}
