using System;

using Gtk;
using Gdk;
using Pango;

namespace Gimp.PicturePackage
{
  public class Preview : DrawingArea
  {
    Pixmap _pixmap;
    Pixmap _labelPixmap;
    Gdk.GC _gc;
    Layout _layout;
    Image _image;
    int _width, _height;
    int _labelX, _labelY;

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

    void OnExposed (object o, ExposeEventArgs args)
    {
      GdkWindow.DrawDrawable(_gc, _pixmap, 0, 0, 0, 0, -1, -1);
      if (_labelPixmap != null)
	{
	GdkWindow.DrawDrawable(_gc, _labelPixmap, 0, 0, _labelX, _labelY, 
			       -1, -1);
	}
    }

    void OnRealized (object o, EventArgs args)
    {
      _width = WidthRequest;
      _height = HeightRequest;
      if (_pixmap == null)
	{
	_pixmap = new Pixmap(this.GdkWindow, _width, _height, -1);
	}
      _gc = new Gdk.GC(this.GdkWindow);
      RenderPixmap();
    }

    void RenderPixmap()
    {
      _pixmap.DrawRectangle(_gc, true, 0, 0, _width, _height);
      _layout.Render(_image, new PreviewRenderer(this, _layout, _pixmap, _gc));
    }

    public void LoadFromDirectory(string directory)
    {
      _layout.LoadFromDirectory(directory, new PreviewRenderer(this, _layout, 
							       _pixmap, _gc));
      QueueDraw();
    }

    public void DrawLabel(int position, string label)
    {
      Pango.Layout layout = new Pango.Layout(this.PangoContext);
      layout.FontDescription = FontDescription.FromString ("Tahoma 16");
      layout.SetMarkup (label);

      int width, height;
      layout.GetPixelSize(out width, out height);
      if (width != 0 && height != 0)
	{
	_labelPixmap = new Pixmap(this.GdkWindow, width, height, -1);
	CalculateXandY(position, width, height);
	_labelPixmap.DrawDrawable(_gc, _pixmap, _labelX, _labelY, 0, 0, 
				  width, height);
	_labelPixmap.DrawLayout(_gc, 0, 0, layout);

	QueueDraw();
	}
    }

    void CalculateXandY(int position, int width, int height)
    {
      switch (position)
	{
	case 0:
	  _labelX = (_width - width) / 2;
	  _labelY = (_height - height) / 2;
	  break;
	case 1:
	  _labelX = 0;
	  _labelY = 0;
	  break;
	case 2:
	  _labelX = 0;
	  _labelY = _height - height;
	  break;
	case 3:
	  _labelX = _width - width;
	  _labelY = 0;
	  break;
	case 4:
	  _labelX = _width - width;
	  _labelY = _height - height;
	  break;
	}		
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
