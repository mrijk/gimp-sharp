using System;

namespace Gimp.PicturePackage
{
	public class ImageRenderer : Renderer
	{
		Image _composed;
		Image _source;
		double _resolution;

		public ImageRenderer(Image composed, Image source, double resolution)
		{
			_composed = composed;
			_source = source;
			_resolution = resolution;
		}

		override public void Render(double x, double y, double w, double h)
		{
			x *= _resolution;
			y *= _resolution;
			w *= _resolution;
			h *= _resolution;

			int ix = (int) x;
			int iy = (int) y;
			int iw = (int) w;
			int ih = (int) h;

			Image clone = new Image(_source);

			if ((double) iw / ih < 1 ^ (double) clone.Width / clone.Height < 1)
			{
				clone.Rotate(RotationType.ROTATE_90);
			}

			double zoom = Math.Min((double) iw / clone.Width, (double) ih / clone.Height);
			int tw = (int) (clone.Width * zoom);
			int th = (int) (clone.Height * zoom);
			clone.Scale(tw, th);

			Layer layer = new Layer(clone.ActiveDrawable, _composed);
			ix += (iw - tw) / 2;
			iy += (ih - th) / 2;
			layer.Translate(ix, iy);

			clone.Delete();
			_composed.AddLayer(layer, -1);
		}
	}
}
