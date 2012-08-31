// The Splitter plug-in
// Copyright (C) 2004-2012 Maurits Rijk
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

namespace Gimp.Splitter
{
  public class Renderer : BaseRenderer
  {
    public Renderer(VariableSet variables) : base(variables)
    {
    }

    public void Render(Image image, Drawable drawable)
    {
      var parser = new MathExpressionParser();
      parser.Init(GetValue<string>("formula"), image.Dimensions);

      var newImage = new Image(image.Dimensions, image.BaseType);
      var srcPR = new PixelRgn(drawable, image.Bounds, false, false);

      PixelRgn destPR1 = null;
      var layer1 = AddLayer(newImage, 1, _("layer_one"), "translate_1_x",
			    "translate_1_y", out destPR1);

      PixelRgn destPR2 = null;
      var layer2 = AddLayer(newImage, 2, _("layer_two"), "translate_2_x",
			    "translate_2_y", out destPR2);

      var transparent = new Pixel(4);

      if (destPR1 != null && destPR2 != null)
	{
	  var iterator = new RegionIterator(srcPR, destPR1, destPR2);
	  iterator.ForEach((src, dest1, dest2) =>
	    {
	      var tmp = Copy(src);
	      if (parser.Eval(src) < 0)
	      {
		dest1.Set(tmp);
		dest2.Set(transparent);
	      }
	      else
	      {
		dest2.Set(tmp);
		dest1.Set(transparent);
	      }
	    });
	}
      else if (destPR1 != null)
	{
	  var iterator = new RegionIterator(srcPR, destPR1);
	  iterator.ForEach((src, dest) =>
			   dest.Set((parser.Eval(src) < 0) 
				    ? Copy(src) : transparent));
	}
      else	// destPR2 != null
	{
	  var iterator = new RegionIterator(srcPR, destPR2);
	  iterator.ForEach((src, dest) =>
			   dest.Set((parser.Eval(src) >= 0) 
				    ? Copy(src) : transparent));
	}

      Rotate(layer1, GetValue<int>("rotate_1"));
      Rotate(layer2, GetValue<int>("rotate_2"));

      if (GetValue<bool>("merge"))
	{
	  var merged = 
	    newImage.MergeVisibleLayers(MergeType.ExpandAsNecessary);
	  merged.Offsets = new Offset(0, 0);
	  newImage.Resize(merged.Dimensions, merged.Offsets);
	}

      new Display(newImage);
      
      Display.DisplaysFlush();
    }

    Layer AddLayer(Image image, int layerNr, string name, 
		   string translate_x, string translate_y, 
		   out PixelRgn destPR)
    {
      destPR = null;
      int keepLayer = GetValue<int>("keep_layer");
      if (keepLayer == 0 || keepLayer == layerNr)
	{
	  var layer = new Layer(image, name, ImageType.Rgba);
	  layer.Translate(GetValue<int>(translate_x), GetValue<int>(translate_y));
	  layer.AddAlpha();
	  image.InsertLayer(layer, 0);

	  destPR = new PixelRgn(layer, image.Bounds, true, false);
	  return layer;
	}
      return null;
    }

    void Rotate(Layer layer, int angle)
    {
      if (angle != 0 && layer != null)
	{
	  layer.TransformRotate(angle * Math.PI / 180.0,
				true, 0, 0, true, false);
	}
    }

    Pixel Copy(Pixel src)
    {
      return (src.HasAlpha) 
	? src 
	: new Pixel(4) {Red = src.Red, Green = src.Green, Blue = src.Blue, 
			  Alpha = 255};
    }
  }
}
