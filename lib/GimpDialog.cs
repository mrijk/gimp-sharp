using System;
using System.Runtime.InteropServices;

using Gtk;

namespace Gimp
  {
    public delegate void GimpHelpFunc(string help_id);

    public class GimpDialog : Dialog
    {
      public GimpDialog(string title, string role, IntPtr parent,
			Gtk.DialogFlags flags,
			GimpHelpFunc help_func, string help_id,
			// string button1, Gtk.ResponseType action1,
			string button2, Gtk.ResponseType action2,
			string button3, Gtk.ResponseType action3) :

	base(gimp_dialog_new(title, role, parent, flags, 
			     help_func, help_id, 
			     // button1, action1,
			     button2, action2, 
			     button3, action3, null))
      {
      }

      public GimpDialog(string title, string role, IntPtr parent,
			Gtk.DialogFlags flags,
			GimpHelpFunc help_func, string help_id) : 

	base(gimp_dialog_new(title, role, parent, flags, 
			     help_func, help_id, 
			     // Stock.Help, ResponseType.Help,
			     Stock.Cancel, ResponseType.Cancel,
			     Stock.Ok, ResponseType.Ok, null))
      {
      }

      public new ResponseType Run()
      {
	return gimp_dialog_run(Handle);
      }

      [DllImport("libgimpwidgets-2.0.so")]
      static extern IntPtr gimp_dialog_new(
	string title,
	string role,
	IntPtr parent,
	Gtk.DialogFlags  flags,
	GimpHelpFunc    help_func,
	string    help_id,
	// string button1, Gtk.ResponseType action1,
	string button2, Gtk.ResponseType action2,
	string button3, Gtk.ResponseType action3,
	string end);

      [DllImport("libgimpwidgets-2.0.so")]
      static extern Gtk.ResponseType gimp_dialog_run(IntPtr dialog);
    }
  }
