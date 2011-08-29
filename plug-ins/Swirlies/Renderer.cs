// The Swirlies plug-in
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
using System.Collections.Generic;
using System.Linq;

namespace Gimp.Swirlies
{
  public class Renderer : BaseRenderer
  {
    readonly Random _random;
    readonly int _width;
    readonly int _height;
    readonly List<Swirly> _swirlies = new List<Swirly>();

    public Renderer(VariableSet variables, Drawable drawable) : base(variables)
    {
      _random = new Random((int) GetValue<UInt32>("seed"));
      Swirly.Random = _random;

      _width = drawable.Width;
      _height = drawable.Height;

      for (int i = 0; i < GetValue<int>("points"); i++)
	_swirlies.Add(Swirly.CreateRandom());
    }

    public void Render(Drawable drawable)
    {
      var iter = new RgnIterator(drawable, _("Swirlies"));
      iter.IterateDest(DoSwirlies);
    }

    public void Render(AspectPreview preview)
    {
      preview.Update(DoSwirlies);
    }

    Pixel DoSwirlies(int x, int y)
    {
      var rgb = new RGB(0.0, 0.0, 0.0);

      const double zoom = 0.5;
      const int terms = 5;

      _swirlies.ForEach(swirly => 
			swirly.CalculateOnePoint(terms, _width, _height, zoom, 
						 x, y, rgb));

      return new Pixel(FloatToIntPixel(RemapColorRange(rgb.R)),
		       FloatToIntPixel(RemapColorRange(rgb.G)),
		       FloatToIntPixel(RemapColorRange(rgb.B)));
    }
    
    double RemapColorRange(double val)
    {
      const double postGain = 0.35;
      const double preGain = 10000;

      val = Math.Abs(val);
      return Math.Tanh(postGain * Math.Log(1 + preGain * val));
    }

    int FloatToIntPixel(double val)
    {
      val *= 255;
      val += 1 - 2 * _random.NextDouble();
      val += 1 - 2 * _random.NextDouble();

      if (val < 0)
	{
	  return 0;
	}
      else if (val > 255)
	{
	  return 255;
	}
      else
	{
	  return (int) val;
	}
    }
  }
}
