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

      PixbufFormat[] formats = Pixbuf.Formats;
      Console.WriteLine("Format: " + formats.Length);
      foreach (PixbufFormat format in formats)
	{
	Console.WriteLine(format.Name);
	}

      TargetEntry[] targets = new TargetEntry[] {
	new TargetEntry("text/plain", 0, 1),
	new TargetEntry("STRING", 0, 2)};

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
  }
  }
