// GIMP# - A C# wrapper around the GIMP Library
// Copyright (C) 2004-2006 Maurits Rijk
//
// GimpDialog.cs
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
  public delegate void GimpHelpFunc(string help_id, IntPtr obj_data);

  public class GimpDialog : Dialog
  {
    public GimpDialog(string title, string role, IntPtr parent,
		      Gtk.DialogFlags flags,
		      GimpHelpFunc help_func, string help_id,
		      string button1, Gtk.ResponseType action1,
		      string button2, Gtk.ResponseType action2) :

      base(gimp_dialog_new(title, role, parent, flags, 
			   help_func, help_id, 
			   button1, action1, 
			   button2, action2, null))
    {
    }

    public GimpDialog(string title, string role, IntPtr parent,
		      Gtk.DialogFlags flags,
		      GimpHelpFunc help_func, string help_id,
		      string button1, Gtk.ResponseType action1,
		      string button2, Gtk.ResponseType action2,
		      string button3, Gtk.ResponseType action3) :

      base(gimp_dialog_new(title, role, parent, flags, 
			   help_func, help_id, 
			   button1, action1,
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
      try 
	{
	  return gimp_dialog_run(Handle);
	}
      catch (Exception e)
	{
	  Console.WriteLine("GimpDialog.Run");
	  Console.WriteLine(e.StackTrace);
	}
      return ResponseType.None;
    }

    public void SetTransient()
    {
      gimp_window_set_transient((this as Window).Handle);
    }

    static public void ShowHelpButton(bool show)
    {
      gimp_dialogs_show_help_button(show);
    }

    [DllImport("libgimpwidgets-2.0-0.dll")]
    static extern 
    IntPtr gimp_dialog_new(string title,
			   string role,
			   IntPtr parent,
			   Gtk.DialogFlags  flags,
			   GimpHelpFunc    help_func,
			   string help_id,
			   string button1, Gtk.ResponseType action1,
			   string button2, Gtk.ResponseType action2,
			   string end);

    [DllImport("libgimpwidgets-2.0-0.dll")]
    static extern 
    IntPtr gimp_dialog_new(string title,
			   string role,
			   IntPtr parent,
			   Gtk.DialogFlags  flags,
			   GimpHelpFunc    help_func,
			   string help_id,
			   string button1, Gtk.ResponseType action1,
			   string button2, Gtk.ResponseType action2,
			   string button3, Gtk.ResponseType action3,
			   string end);
    
    [DllImport("libgimpwidgets-2.0-0.dll")]
    static extern Gtk.ResponseType gimp_dialog_run(IntPtr dialog);

    [DllImport("libgimpwidgets-2.0-0.dll")]
    static extern void gimp_dialogs_show_help_button(bool show);
    [DllImport("libgimpui-2.0-0.dll")]
    public static extern void gimp_window_set_transient(IntPtr window);
  }
}
