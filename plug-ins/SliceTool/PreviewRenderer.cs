using System;

using Gdk;

namespace Gimp.SliceTool
{
  public class PreviewRenderer
  {
    Gdk.Drawable _window;
    Gdk.GC _gc;
    int _width, _height;
    Gdk.Color _red, _green;

    public PreviewRenderer(Preview preview, Gdk.GC gc, int width, int height)
    {
      _window = preview.GdkWindow;
      _gc = gc;
      _width = width - 1;
      _height = height - 1;

      Colormap colormap = Colormap.System;
      _red = new Gdk.Color (0xff, 0, 0);
      _green = new Gdk.Color (0, 0xff, 0);
      colormap.AllocColor (ref _red, true, true);
      colormap.AllocColor (ref _green, true, true);
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
      _gc.Foreground = _red;
      x1 = Math.Min(x1, _width);
      y1 = Math.Min(y1, _height);
      x2 = Math.Min(x2, _width);
      y2 = Math.Min(y2, _height);
      _window.DrawLine(_gc, x1, y1, x2, y2);
    }

    public void DrawRectangle(int x, int y, int width, int height)
    {
      _gc.Foreground = _green;
      if (x + width > _width)
	width--;
      if (y + height > _height)
	height--;

      _window.DrawRectangle(_gc, false, x, y, width, height);
    }

    public Gdk.Function Function
    {
      set {_gc.Function = value;}
    }
  }
  }
