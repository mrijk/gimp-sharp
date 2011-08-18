// The Trim plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Renderer.cs
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
using System.Linq;

namespace Gimp.Trim
{
  class Renderer : BaseRenderer
  {    
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image, Drawable drawable)
    {
      var src = new PixelRgn(drawable, false, false);
      PixelRgn.Register(src);

      var trimColor = GetTrimColor(src, drawable);

      Predicate<bool> notTrue = (b) => {return !b;};

      int height = drawable.Height;
      var rows = new bool[height];
      int y = 0;
      src.ForEachRow(row => rows[y++] = AllEqual(row, trimColor));

      int y1 = (GetValue<bool>("top")) ? Array.FindIndex(rows, notTrue) : 0;
      int y2 = (GetValue<bool>("bottom")) ? 
	Array.FindLastIndex(rows, notTrue) + 1 : height;

      int width = drawable.Width;
      var cols = new bool[width];
      int x = 0;
      src.ForEachColumn(col => cols[x++] = AllEqual(col, trimColor));

      int x1 = (GetValue<bool>("left")) ? Array.FindIndex(cols, notTrue) : 0;
      int x2 = (GetValue<bool>("right")) ? 
	Array.FindLastIndex(cols, notTrue) + 1 : width;

      var croppingArea = new Rectangle(x1, y1, x2, y2);
      if (croppingArea != image.Bounds)
	{
	  image.Crop(croppingArea);
	}
    }

    Pixel GetTrimColor(PixelRgn src, Drawable drawable)
    {
      int basedOn = GetValue<int>("based_on");
      if (basedOn == 0)
	{
	  return new Pixel(0, 0, 0, 0);
	}
      else if (basedOn == 1)
	{
	  return src[0, 0];
	}
      else
	{
	  return src.GetPixel(drawable.Width - 1, drawable.Height - 1);
	}
    }

    bool AllEqual(Pixel[] array, Pixel p)
    {
      return array.All(pixel => pixel.IsSameColor(p));
    }
  }
}
