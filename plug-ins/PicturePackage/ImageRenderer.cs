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

    override public void Render(Image image, double x, double y, 
				double w, double h)
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

      Image clone = new Image(image);

      if (w < h ^ clone.Width < clone.Height)
	{
	clone.Rotate(RotationType.ROTATE_90);
	}

      double zoom = Math.Min((double) iw / clone.Width, 
			     (double) ih / clone.Height);
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
