using System;
using Gtk;
 
namespace Gimp
  {
    public class Preview : DrawingArea
    {
      Gdk.GC _gc;

      public Preview()
      {
	Realized += OnRealized;
	ExposeEvent += OnExposed;
      }

      void OnExposed (object o, ExposeEventArgs args)
      {
	GdkWindow.DrawLine (_gc, 10, 10, 100, 100);
      }
 
      void OnRealized (object o, EventArgs args)
      {
	_gc = new Gdk.GC(this.GdkWindow);
      }
    }
  }
