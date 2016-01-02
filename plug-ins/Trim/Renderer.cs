// The Trim plug-in
// Copyright (C) 2004-2016 Maurits Rijk
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
using System.Collections.Generic;
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

      var tb = GetLowerUpper(trimColor, src.Rows, "top", "bottom");
      var lr = GetLowerUpper(trimColor, src.Columns, "left", "right");

      var croppingArea = new Rectangle(lr.Item1, tb.Item1, lr.Item2, tb.Item2);
      if (croppingArea != image.Bounds)
	{
	  image.Crop(croppingArea);
	}
    }

    Tuple<int, int> GetLowerUpper(Pixel trimColor, IEnumerable<Pixel[]> array,
				  string lower, string upper)
    {
      Predicate<bool> notTrue = (b) => {return !b;};

      var rows = array.Select(x => AllEqual(x, trimColor)).ToList();
      
      return Tuple.Create((GetValue<bool>(lower)) ? rows.FindIndex(notTrue) : 0,
			  (GetValue<bool>(upper)) ? rows.FindLastIndex(notTrue) + 1 
			  : rows.Count());
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

    bool AllEqual(Pixel[] array, Pixel p) => 
      array.All(pixel => pixel.IsSameColor(p));
  }
}
