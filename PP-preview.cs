using System;
using Gtk;
using Gdk;

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
	ButtonPressEvent += OnButtonPress;

	Events = EventMask.ButtonPressMask;
      }

      public Layout Layout
      {
	set {_layout = value;}
      }

      void OnExposed (object o, ExposeEventArgs args)
      {
	if (_layout != null)
	  {
	  _layout.Draw(this, 240, 300);	// Fix me!
	  }
      }

      public void DrawRectangle(int x, int y, int w, int h)
	{
	  GdkWindow.DrawRectangle (_gc, false, x, y, x + w, y + h);
	}
 
      void OnRealized (object o, EventArgs args)
      {
	_gc = new Gdk.GC(this.GdkWindow);
      }

      void OnButtonPress(object o, ButtonPressEventArgs args)
      {
	Console.WriteLine("OnButtonPress");
      }
    }
  }
