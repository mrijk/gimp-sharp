using System;
using Gdk;

namespace Gimp.PicturePackage
{
  public class Painter
  {
    Preview _preview;
    Gdk.GC _gc;
    double _zoom;
    int _pw, _ph;

    public Painter(Preview preview, Layout layout, Gdk.GC gc)
    {
      _preview = preview;
      _gc = gc;

      _pw = preview.WidthRequest;
      _ph = preview.HeightRequest;

      _zoom = Math.Min((double) _pw / layout.Width, 
		       (double) _ph / layout.Height);
    }

    public void DrawRectangle(double x, double y, double w, double h)
    {
      x *= _zoom;
      y *= _zoom;
      w *= _zoom;
      h *= _zoom;

      int ix = (int) x;
      int iy = (int) y;
      int iw = (int) w;
      int ih = (int) h;

      if (ix + iw == _pw)
	iw--;
      if (iy + ih == _ph)
	ih--;

      _preview.GdkWindow.DrawRectangle (_gc, false, ix, iy, iw, ih);
    }
  }
  }
