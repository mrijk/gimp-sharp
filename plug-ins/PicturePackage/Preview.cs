using System;
using Gtk;
using Gdk;

namespace Gimp.PicturePackage
{
  public class Preview : DrawingArea
  {
    Gdk.GC _gc;
    Layout _layout;
    Image _image;
    double _zoom;

    public Preview()
    {
      Realized += new EventHandler(OnRealized);
      ExposeEvent += new ExposeEventHandler(OnExposed);
      ButtonPressEvent += new ButtonPressEventHandler(OnButtonPress);

      TargetEntry[] targets = new TargetEntry[2];
      targets[0] = new TargetEntry("text/plain", 0, 1);
      targets[1] = new TargetEntry("STRING", 0, 2);

      Gtk.Drag.DestSet(this, DestDefaults.All, targets, DragAction.Copy);
      DragDataReceived += new DragDataReceivedHandler(OnDragDataReceived);

      Events = EventMask.ButtonPressMask;
    }

    public Layout Layout
    {
      set {_layout = value;}
    }

    public Image Image
    {
      set {_image = value;}
    }

    void OnExposed (object o, ExposeEventArgs args)
    {
      if (_layout != null)
	{
	_layout.Draw(new Painter(this, _layout, _image, _gc));
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

    void OnDragDataReceived(object o, DragDataReceivedArgs args)
    {
      Console.WriteLine("OnDragDataReceived");
    }

    public void DrawPixbuf(Pixbuf pixbuf)
    {
      pixbuf.RenderToDrawable(this.GdkWindow, _gc, 0, 0, 0, 0, -1, -1, 
			      RgbDither.Normal, 0, 0);
    }
  }
  }
