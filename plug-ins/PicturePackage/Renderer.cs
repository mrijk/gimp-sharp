using System;

using Gdk;

namespace Gimp.PicturePackage
{
	abstract public class Renderer
	{
		Image _cache;
		Int32 _cachedID = -1;
		double _w = double.NaN;
		double _h = double.NaN;

		public Renderer()
		{
		}

		abstract public void Render(Image image, double x, double y, double w, double h);

		protected Image RotateAndScale(Image image, double w, double h)
		{
			if (_cachedID != image.ID || _w != w || _h != h)
			{
				_cachedID = image.ID;
				ClearCache();

				_cache = new Image(image);
				_w = w;
				_h = h;

				if (w < h ^ _cache.Width < _cache.Height)
				{
					_cache.Rotate(RotationType.ROTATE_90);
				}

				double zoom = Math.Min(w / _cache.Width, h / _cache.Height);
				w = _cache.Width * zoom;
				h = _cache.Height * zoom;

				_cache.Scale((int) w, (int) h);
			}
			return _cache;
		}

		void ClearCache()
		{
			if (_cache != null)
			{
				_cache.Delete();
			}
		}

		public void Cleanup()
		{
			ClearCache();
		}
	}
}
