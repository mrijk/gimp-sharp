using System;
using System.Collections;
using System.Runtime.InteropServices;

using GLib;
using Gtk;

namespace Gimp
  {
    public enum ColorAreaType
      {
	COLOR_AREA_FLAT = 0,
	COLOR_AREA_SMALL_CHECKS,
	COLOR_AREA_LARGE_CHECKS
      };

    public class GimpColorButton : Button
    {
      public GimpColorButton(string title,
			     int width,
			     int height,
			     GimpRGB color,
			     ColorAreaType type) : 
	base(gimp_color_button_new(title, width, height, ref color, type))
      {
      }

      public RGB Color
      {
	get
	    {
	    GimpRGB rgb = new GimpRGB();
	    gimp_color_button_get_color(Handle, ref rgb);
	    return new RGB(rgb);
	    }
	set
	    {
	    GimpRGB rgb = value.GimpRGB;
	    gimp_color_button_set_color(Handle, ref rgb);
	    }
      }

      public bool Alpha
      {
	get
	    {
	    return gimp_color_button_has_alpha(Handle);
	    }
      }

      public ColorAreaType Type
      {
	set
	    {
	    gimp_color_button_set_type(Handle, value);
	    }
      }

      public bool Update
      {
	get
	    {
	    return gimp_color_button_get_update(Handle);
	    }
	set
	    {
	    gimp_color_button_set_update(Handle, value);
	    }
      }

      [GLib.Signal("color_changed")]
      public event EventHandler ColorChanged
      {
	add 
	    {
	    if (value.Method.GetCustomAttributes(typeof(ConnectBeforeAttribute), false).Length > 0) 
	      {
	      if (BeforeHandlers["color_changed"] == null)
		BeforeSignals["color_changed"] = new voidObjectSignal(this, "color_changed", value, typeof (System.EventArgs), 0);		 		 		 		 		 else
		  ((SignalCallback) BeforeSignals ["color_changed"]).AddDelegate (value);
	      BeforeHandlers.AddHandler("color_changed", value);
	      } 
	    else 
	      {
	      if (AfterHandlers["color_changed"] == null)
		AfterSignals["color_changed"] = new voidObjectSignal(this, "color_changed", value, typeof (System.EventArgs), 1);		 		 		 		 		 else
		  ((SignalCallback) AfterSignals ["color_changed"]).AddDelegate (value);
	      AfterHandlers.AddHandler("color_changed", value);
	      }
	    }
	remove 
	    {
	    System.ComponentModel.EventHandlerList event_list = AfterHandlers;
	    Hashtable signals = AfterSignals;
	    if (value.Method.GetCustomAttributes(typeof(ConnectBeforeAttribute), false).Length > 0) 
	      {
	      event_list = BeforeHandlers;
	      signals = BeforeSignals;
	      }
	    SignalCallback cb = signals ["color_changed"] as SignalCallback;
	    event_list.RemoveHandler("color_changed", value);
	    if (cb == null)
	      return;

	    cb.RemoveDelegate (value);

	    if (event_list["color_changed"] == null) 
	      {
	      signals.Remove("color_changed");
	      cb.Dispose ();
	      }
	    }
      }

      [DllImport("libgimpwidgets-2.0.so")]
      extern static IntPtr gimp_color_button_new(
	string title,
	int width,
	int height,
	ref GimpRGB color,
	ColorAreaType type);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_color(IntPtr button,
						     ref GimpRGB color);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_get_color(IntPtr button,
						     ref GimpRGB color);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static bool gimp_color_button_has_alpha(IntPtr button);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_type(IntPtr button,
						    ColorAreaType type);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static void gimp_color_button_set_update(IntPtr button,
						      bool continuous);
      [DllImport("libgimpwidgets-2.0.so")]
      extern static bool gimp_color_button_get_update(IntPtr button);
    }
  }
