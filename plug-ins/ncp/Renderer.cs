// The ncp plug-in
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

namespace Gimp.ncp
{
  public class Renderer
  {
    readonly Drawable _drawable;
    readonly Pixel _pixel;
    readonly Calculator _calculator;
    readonly bool _color;

    public Renderer(VariableSet variables, Drawable drawable)
    {
      _drawable = drawable;
      int bpp = drawable.Bpp;
      _pixel = drawable.CreatePixel();
      _color = variables.GetValue<bool>("color");

      if (drawable.HasAlpha)
	{
	  bpp--;
	  _pixel.Alpha = 255;
	}

      _calculator = new Calculator(variables.GetValue<int>("points"),
				   variables.GetValue<int>("closest"),
				   bpp, drawable.MaskBounds, 
				   (int) variables.GetValue<UInt32>("seed"));
    }

    public void Render()
    {
      var iter = new RgnIterator(_drawable, "NCP");
      iter.IterateDest(DoNCP);
    }

    public void Render(AspectPreview preview)
    {
      preview.Update(DoNCP);
    }

    Pixel DoNCP(IntCoordinate c)
    {
      int b = 0;
      Func<int> func = () => _calculator.Calc(b++, c);

      if (_color)
	{
	  _pixel.Fill(func);
	}
      else
	{
	  _pixel.FillSame(func);
	}
      return _pixel;
    }
  }
}
