using System;

namespace Gimp.SliceTool
{
  public class PreviewRenderer
  {
    Preview _preview;
    Gdk.GC _gc;

    public PreviewRenderer(Preview preview, Gdk.GC gc)
    {
      _preview = preview;
      _gc = gc;
    }

    public void Draw(Slice slice)
    {
      slice.Draw(_preview, _gc);
    }

    public Gdk.Function Function
    {
      set {_gc.Function = value;}
    }
  }
  }
