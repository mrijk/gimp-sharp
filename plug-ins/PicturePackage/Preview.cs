using System;
using Gtk;
using Gdk;

namespace Gimp.PicturePackage
{
  public class Preview : DrawingArea
  {
    public Pixmap _pixmap;
    Gdk.GC _gc;
    Layout _layout;
    Image _image;
    double _zoom;

    public Preview()
    {
      Realized += new EventHandler(OnRealized);
      ExposeEvent += new ExposeEventHandler(OnExposed);
      ButtonPressEvent += new ButtonPressEventHandler(OnButtonPress);

      TargetEntry[] targets = new TargetEntry[]{
	new TargetEntry("text/plain", 0, 1),
	new TargetEntry("STRING", 0, 2)};

      Gtk.Drag.DestSet(this, DestDefaults.All, targets, DragAction.Copy);
      DragDataReceived += new DragDataReceivedHandler(OnDragDataReceived);

      Events = EventMask.ButtonPressMask;
    }

    public void SetLayout(Layout layout)
    {
      _layout = layout;
      if (IsRealized)
	{
	RenderPixmap();
	QueueDraw();
	}
    }

    public Image Image
    {
      set {_image = value;}
    }

    static bool first = true;

    void OnExposed (object o, ExposeEventArgs args)
    {
      GdkWindow.DrawDrawable(_gc, _pixmap, 0, 0, 0, 0, -1, -1);
    }

    void OnRealized (object o, EventArgs args)
    {
      if (_pixmap == null)
	{
	_pixmap = new Pixmap(this.GdkWindow, WidthRequest, HeightRequest, -1);
	}
      _gc = new Gdk.GC(this.GdkWindow);
      RenderPixmap();
    }

    void RenderPixmap()
    {
      _pixmap.DrawRectangle(_gc, true, 0, 0, WidthRequest, HeightRequest);
      _layout.Render(new PreviewRenderer(this, _layout, _image, _pixmap, _gc));
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
