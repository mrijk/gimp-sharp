// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// PreviewRenderer.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

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

    public PreviewRenderer(Preview preview, Gdk.GC gc, Dimensions dimensions)
    {
      _window = preview.GdkWindow;
      _gc = gc;
      _width = dimensions.Width - 1;
      _height = dimensions.Height - 1;

      var colormap = Colormap.System;
      _red = new Gdk.Color(0xff, 0, 0);
      _green = new Gdk.Color(0, 0xff, 0);
      colormap.AllocColor(ref _red, true, true);
      colormap.AllocColor(ref _green, true, true);
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

    void ReplaceColor(ref Gdk.Color color, RGB rgb)
    {
      var colors = new Gdk.Color[]{color};
      var colormap = Colormap.System;
      colormap.FreeColors(colors, 1);
      byte red, green, blue;
      rgb.GetUchar(out red, out green, out blue);
      color = new Gdk.Color(red, green, blue);
      colormap.AllocColor (ref color, true, true);
    }

    public RGB ActiveColor
    {
      set {ReplaceColor(ref _green, value);}
    }

    public RGB InactiveColor
    {
      set {ReplaceColor(ref _red, value);}
    }
  }
}
