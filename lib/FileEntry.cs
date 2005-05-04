// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2005 Maurits Rijk
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
using System.Collections;
using System.Runtime.InteropServices;

using GLib;
using Gtk;

namespace Gimp
  {
    public class FileEntry : HBox
    {
      public FileEntry(string title, string filename, bool dir_only, 
		       bool check_valid) :
	base(gimp_file_entry_new(title, filename, dir_only, check_valid))
      {
      }

      public string FileName
      {
	get {return gimp_file_entry_get_filename(Handle);}
	set {gimp_file_entry_set_filename(Handle, value);}
      }

      [Signal("filename_changed")]
      public event EventHandler FilenameChanged
      {
	add 
	    {
	    if (value.Method.GetCustomAttributes(
		  typeof(ConnectBeforeAttribute), false).Length > 0) 
	      {
	      if (BeforeHandlers["filename_changed"] == null)
		BeforeSignals["filename_changed"] = 
		  new voidObjectSignal(this, "filename_changed", value, 
				       typeof (System.EventArgs), 0);
	      else
		((SignalCallback) 
		 BeforeSignals ["filename_changed"]).AddDelegate (value);
	      BeforeHandlers.AddHandler("filename_changed", value);
	      } 
	    else 
	      {
	      if (AfterHandlers["filename_changed"] == null)
		AfterSignals["filename_changed"] = 
		  new voidObjectSignal(this, "filename_changed", value, 
				       typeof (System.EventArgs), 1);
	      else
		((SignalCallback) 
		 AfterSignals ["filename_changed"]).AddDelegate (value);
	      AfterHandlers.AddHandler("filename_changed", value);
	      }
	    }
	remove 
	    {
	    System.ComponentModel.EventHandlerList event_list = AfterHandlers;
	    Hashtable signals = AfterSignals;
	    if (value.Method.GetCustomAttributes(
		  typeof(ConnectBeforeAttribute), false).Length > 0) 
	      {
	      event_list = BeforeHandlers;
	      signals = BeforeSignals;
	      }
	    SignalCallback cb = signals ["filename_changed"] as SignalCallback;
	    event_list.RemoveHandler("filename_changed", value);
	    if (cb == null)
	      return;

	    cb.RemoveDelegate (value);

	    if (event_list["filename_changed"] == null) 
	      {
	      signals.Remove("filename_changed");
	      cb.Dispose ();
	      }
	    }
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_file_entry_new (string title, string filename, 
						bool dir_only, 
						bool check_valid);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static string gimp_file_entry_get_filename (IntPtr entry);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_file_entry_set_filename (IntPtr entry, 
						       string filename);
    }
  }
