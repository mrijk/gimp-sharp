using System;

using Gdk;

namespace Gimp.SliceTool
{
  public class PreviewRenderer
  {
    Gdk.Drawable _window;
    Gdk.GC _gc;
    Gdk.Color _red, _green;

    public PreviewRenderer(Preview preview, Gdk.GC gc)
    {
      _window = preview.GdkWindow;
      _gc = gc;

      Colormap colormap = Colormap.System;
      _red = new Gdk.Color (0xff, 0, 0);
      _green = new Gdk.Color (0, 0xff, 0);
      colormap.AllocColor (ref _red, true, true);
      colormap.AllocColor (ref _green, true, true);
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
      _gc.Foreground = _red;
      _window.DrawLine(_gc, x1, y1, x2, y2);
    }

    public void DrawRectangle(int x, int y, int width, int height)
    {
      _gc.Foreground = _green;
      _window.DrawRectangle(_gc, false, x, y, width, height);
    }

    public Gdk.Function Function
    {
      set {_gc.Function = value;}
    }
  }
  }
