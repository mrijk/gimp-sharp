using System;
using Gtk;
 
namespace Gimp
  {
    public class Preview : DrawingArea
    {
      Gdk.GC _gc;
      Layout _layout;

      public Preview()
      {
	Realized += OnRealized;
	ExposeEvent += OnExposed;
      }

      public Layout Layout
      {
	set {_layout = value;}
      }

      void OnExposed (object o, ExposeEventArgs args)
      {
	// GdkWindow.DrawLine (_gc, 10, 10, 100, 100);
	if (_layout != null)
	  {
	  _layout.Draw(_gc);
	  }
      }
 
      void OnRealized (object o, EventArgs args)
      {
	_gc = new Gdk.GC(this.GdkWindow);
      }
    }
  }
