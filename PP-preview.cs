using System;
using Gdk;
using Gtk;
 
namespace Gimp
  {
    public class Preview : DrawingArea
    {
      Gdk.GC _gc;

      public Preview()
      {
	// Realized += OnRealized;
	// ExposeEvent += OnExposed;
      }

      void OnExposed (object o, ExposeEventArgs args)
      {
	// GdkWindow.DrawLine (_gc, 10, 10, 100, 100);
      }
 
      void OnRealized (object o, EventArgs args)
      {
      }

      override protected bool OnExposeEvent(Gdk.EventExpose e)
      {
	return base.OnExposeEvent(e);
	GdkWindow.DrawLine (_gc, 10, 10, 100, 100);
	return true;
      }

      override protected void OnRealized()
      {
	base.OnRealized();
	_gc = new Gdk.GC(this.GdkWindow);
      }
    }
  }
