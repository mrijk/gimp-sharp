using System;
using Gdk;

namespace Gimp.PicturePackage
{
	public class PreviewRenderer : Renderer
	{
		Pixmap _pixmap;
		Preview _preview;
		Image _image;
		Gdk.GC _gc;
		double _zoom;
		int _pw, _ph;

		public PreviewRenderer(Preview preview, Layout layout, Image image, Pixmap pixmap, Gdk.GC gc)
		{
			_preview = preview;
			_image = image;
			_pixmap = pixmap;
			_gc = gc;

			_pw = preview.WidthRequest;
			_ph = preview.HeightRequest;

			_zoom = Math.Min((double) _pw / layout.Width, 
							 (double) _ph / layout.Height);
		}

		override public void Render(double x, double y, double w, double h)
		{
			// Draw rectangle
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

			_pixmap.DrawRectangle (_gc, false, ix, iy, iw, ih);

			// Draw image
			Image clone = new Image(_image);

			if ((double) iw / ih < 1 ^ (double) clone.Width / clone.Height < 1)
			{
				clone.Rotate(RotationType.ROTATE_90);
			}

			double zoom = Math.Min((double) iw / clone.Width, (double) ih / clone.Height);
			int tw = (int) (clone.Width * zoom);
			int th = (int) (clone.Height * zoom);

			ix += (iw - tw) / 2;
			iy += (ih - th) / 2;

			clone.Scale(tw, th);
			Pixbuf pixbuf = clone.GetThumbnail(tw, th, Transparency.KEEP_ALPHA);
			clone.Delete();

			pixbuf.RenderToDrawable(_pixmap, _gc, 0, 0, ix, iy, -1, -1, 
				RgbDither.Normal, 0, 0);
			pixbuf.Dispose();
		}
	}
}
