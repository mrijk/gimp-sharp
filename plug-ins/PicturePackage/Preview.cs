using System;
using Gtk;
using Gdk;

namespace Gimp.PicturePackage
{
  public class Preview : DrawingArea
  {
    Gdk.GC _gc;
    Layout _layout;

    public Preview()
    {
      Realized += new EventHandler(OnRealized);
      ExposeEvent += new ExposeEventHandler(OnExposed);
      ButtonPressEvent += new ButtonPressEventHandler(OnButtonPress);

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
	_layout.Draw(new Painter(this, _layout, _gc));
	}
    }

    void OnRealized (object o, EventArgs args)
    {
      _gc = new Gdk.GC(this.GdkWindow);
    }

    void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      Rectangle rectangle = _layout.Find((int) args.Event.X, 
					 (int) args.Event.Y);
      if (rectangle == null)
	Console.WriteLine("No rectangle!");
      else
	Console.WriteLine("Rectangle found");
    }

    public void DrawPixbuf(Pixbuf pixbuf)
    {
      pixbuf.RenderToDrawable(this.GdkWindow, _gc, 0, 0, 0, 0, -1, -1, 
			      RgbDither.Normal, 0, 0);
    }
  }
  }
