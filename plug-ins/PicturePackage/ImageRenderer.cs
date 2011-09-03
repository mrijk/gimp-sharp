// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// ImageRendererer.cs
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

namespace Gimp.PicturePackage
{
  public class ImageRenderer : ParentRenderer
  {
    readonly Image _composed;
    readonly double _resolution;
    readonly bool _convert;

    public ImageRenderer(Layout layout, Image composed, double resolution)
    {
      _convert = (layout.Unit == Unit.Inch);
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
