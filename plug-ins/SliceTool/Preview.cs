using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class Preview : PreviewArea
  {
    Gdk.GC _gc;
    Drawable _drawable;
    SliceTool _parent;

    public Preview(Drawable drawable, SliceTool parent)
    {
      _drawable = drawable;
      _parent = parent;

      ExposeEvent += new ExposeEventHandler(OnExposed);
      Realized += new EventHandler(OnRealized);

      Events = EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | 
	EventMask.PointerMotionHintMask | EventMask.PointerMotionMask;
    }

    static bool firstTime = true;

    void OnExposed (object o, ExposeEventArgs args)
    {		
      if (firstTime)
	{
	firstTime = false;
	int width = _drawable.Width;
	int height = _drawable.Height;

	PixelRgn rgn = new PixelRgn(_drawable, 0, 0, width, height, 
				    false, false);
			
	byte[] buf = rgn.GetRect(0, 0, width, height);
	Draw(0, 0, width, height, ImageType.RGB, buf, width * _drawable.Bpp);
	}
      _parent.Redraw();
    }

    void OnRealized (object o, EventArgs args)
    {
      Gdk.Color red = new Gdk.Color (0xff, 0, 0);
      Gdk.Colormap colormap = Gdk.Colormap.System;
      colormap.AllocColor (ref red, true, true);
      _gc = new Gdk.GC(GdkWindow);
      _gc.Foreground = red;
    }

    public PreviewRenderer GetRenderer()
    {
      return new PreviewRenderer(this, _gc);
    }
  }
  }
