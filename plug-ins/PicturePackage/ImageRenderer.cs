using System;

namespace Gimp.PicturePackage
{
	public class ImageRenderer : Renderer
	{
		Image _composed;
		double _resolution;
		bool _convert;

		public ImageRenderer(Layout layout, Image composed, double resolution)
		{
			_convert = (layout.Unit == Unit.INCH);
			_composed = composed;
			_resolution = resolution;
		}

		override public void Render(Image image, double x, double y, double w, double h)
		{
			if (_convert)
			{
				x *= _resolution;
				y *= _resolution;
				w *= _resolution;
				h *= _resolution;
			}
			int ix = (int) x;
			int iy = (int) y;
			int iw = (int) w;
			int ih = (int) h;

			Image clone = RotateAndScale(image, w, h);
			int tw = clone.Width;
			int th = clone.Height;

			Layer layer = new Layer(clone.ActiveDrawable, _composed);
			ix += (iw - tw) / 2;
			iy += (ih - th) / 2;
			layer.Translate(ix, iy);

			// clone.Delete();
			_composed.AddLayer(layer, -1);
		}
	}
}
