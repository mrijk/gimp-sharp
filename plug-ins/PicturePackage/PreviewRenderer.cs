using System;
using Gdk;

namespace Gimp.PicturePackage
{
  public class PreviewRenderer : Renderer
  {
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

      _pw = preview.WidthRequest;
      _ph = preview.HeightRequest;

      _pixmap.DrawRectangle(_gc, true, 0, 0, _pw, _ph);

      _zoom = Math.Min(_pw / layout.Width, 
		       _ph / layout.Height);

      int iw = (int) (layout.Width * _zoom);
      int ih = (int) (layout.Height * _zoom);

      _offx = (_pw - iw) / 2;
      _offy = (_ph - ih) / 2;
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

      Image clone = RotateAndScale(image, w, h);
      int tw = clone.Width;
      int th = clone.Height;

      ix += (iw - tw) / 2;
      iy += (ih - th) / 2;

      Pixbuf pixbuf = clone.GetThumbnail(tw, th, Transparency.KEEP_ALPHA);

      pixbuf.RenderToDrawable(_pixmap, _gc, 0, 0, ix, iy, -1, -1, 
			      RgbDither.Normal, 0, 0);
      pixbuf.Dispose();
    }
  }
  }
