using System;
using System.Runtime.InteropServices;

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
