using System;
using Gdk;

namespace Gimp.PicturePackage
{
  public class PreviewRenderer : Renderer
  {
    Preview _preview;
    Pixmap _pixmap;
    Gdk.GC _gc;
    double _zoom;
    int _pw, _ph;
    int _offx, _offy;

    public PreviewRenderer(Preview preview, Layout layout, Pixmap pixmap, 
			   Gdk.GC gc)
    {
      _pixmap = pixmap;
      _gc = gc;

      _preview = preview;
      _pw = preview.WidthRequest;
      _ph = preview.HeightRequest;

      _zoom = layout.Boundaries(_pw, _ph, out _offx, out _offy);
    }

    override public void Render(Image image, double x, double y, 
				double w, double h)
    {
      // Draw rectangle
      x *= _zoom;
      y *= _zoom;
      w *= _zoom;
      h *= _zoom;

      int ix = _offx + (int) x;
      int iy = _offy + (int) y;
      int iw = (int) w;
      int ih = (int) h;

      if (ix + iw == _pw)
	iw--;
      if (iy + ih == _ph)
	ih--;

      _pixmap.DrawRectangle (_gc, false, ix, iy, iw, ih);
      _pixmap.DrawRectangle (_gc, true, ix, iy, iw, ih);

      Image clone = RotateAndScale(image, w, h);
      int tw = clone.Width;
      int th = clone.Height;

      ix += (iw - tw) / 2;
      iy += (ih - th) / 2;

      Pixbuf pixbuf = clone.GetThumbnail(tw, th, Transparency.KEEP_ALPHA);

      pixbuf.RenderToDrawable(_pixmap, _gc, 0, 0, ix, iy, -1, -1, 
			      RgbDither.Normal, 0, 0);
      pixbuf.Dispose();
      // _preview.QueueDrawArea(ix, iy, tw, th);
    }
  }
  }
