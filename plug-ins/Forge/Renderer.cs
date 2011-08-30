// The Forge plug-in
// Copyright (C) 2006-2021 Maurits Rijk
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

namespace Gimp.Forge
{
  public class Renderer : BaseRenderer
  {
    readonly Random _random;

    public Renderer(VariableSet variables) : base(variables)
    {
      _random = new Random((int) GetValue<UInt32>("seed"));

      for (int i = 0; i < 7; i++)
	{
	  _random.Next();
	}
    }

    public void Render(Image image, Drawable drawable)
    {
      var dimensions = drawable.Dimensions;

      if (dimensions.Width < dimensions.Height)
      {
        new Message(_("This filter can be applied just if height <= width"));
        return;
      }

      Tile.CacheDefault(drawable);

      RenderForge(drawable, null, dimensions);
    }

    void RenderForge(Drawable drawable, byte[] pixelArray, Dimensions dimensions)
    {
      InitParameters();
      Planet(drawable, pixelArray, dimensions);
    }

    void InitParameters()
    {
      // Set defaults when explicit specifications were not given.
      // The  default  fractal  dimension  and  power  scale depend upon
      // whether we're generating a planet or clouds.

      //      if (!dimspec)
      {
	GetVariable<double>("dimension").Value = 
	  GetValue<bool>("clouds") ? Cast(1.9, 2.3) : Cast(2.0, 2.7);
      }

      //      if (!powerspec)
      {
	GetVariable<double>("power").Value = 
	  GetValue<bool>("clouds") ? Cast(0.6, 0.8) : Cast(1.0, 1.5);
      }

      //      if (!icespec)
	{
	  GetVariable<double>("ice_level").Value = Cast(0.2, 0.6);
	}

      //      if (!glacspec)
	{
	  GetVariable<double>("glaciers").Value = Cast(0.6, 0.85);
	}

	//      if (!starspec)
	{
	  GetVariable<double>("stars_fraction").Value = Cast(75, 125);
	}

	//      if (!starcspec) 
	{
	  GetVariable<double>("saturation").Value = Cast(100, 150);
	}
    }

    void Planet(Drawable drawable, byte[] pixelArray, Dimensions dimensions)
    {
      // Fix me!
      bool hourspec = true;
      bool inclspec = true;

      new Planet(drawable, pixelArray, dimensions, 
		 GetValue<bool>("stars"),
		 GetValue<double>("stars_fraction"),
		 GetValue<double>("saturation"),
		 GetValue<bool>("clouds"), 
		 _random, 
		 GetValue<double>("ice_level"),
		 GetValue<double>("glaciers"),
		 GetValue<double>("dimension"),
		 hourspec, GetValue<double>("hour"), 
		 inclspec, GetValue<double>("inclination"),
		 GetValue<double>("power"));
    }

    public void Render(AspectPreview preview)
    {
      Console.WriteLine("UpdatePreview!");

      var dimensions = preview.Size;

      int width = dimensions.Width;
      int height = dimensions.Height;

      var pixelArray = new byte[width * height * 3];
      RenderForge(null, pixelArray, dimensions);

      preview.DrawBuffer(pixelArray, width * 3);
    }

    double Cast(double low, double high)
    {
      return low + (high - low) * _random.NextDouble();
    }
  }
}
