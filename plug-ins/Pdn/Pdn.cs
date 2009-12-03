// The Paint .NET file format import/export plug-in
// Copyright (C) 2006-2009 Massimo Perga, Maurits Rijk
//
// pdn.cs
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
using System.IO;

using PaintDotNet;

namespace Gimp.Pdn
{
  class Pdn : FilePlugin
  {
    static void Main(string[] args)
    {
      new Pdn(args);
    }

    public Pdn(string[] args) : base(args, "Pdn")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return FileLoadProcedure("file_pdn_load",
				"loads images of the Paint.NET file format",
				"This plug-in loads images of the Paint.NET file format.",
				"Massimo Perga",
				"(C) Massimo Perga",
				"2006-2009",
				"Paint.NET Image");
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("pdn", "");
    }

    override protected Image Load()
    {
      int layerPosition = 0;
      int colorOffset = 0;

      Document document = Document.FromStream(Reader.BaseStream);
      
      Console.WriteLine("Width  : " + document.Width);
      Console.WriteLine("height : " + document.Height);
      
      var image = new Image(document.Width, document.Height,
			    ImageBaseType.Rgb); // Is it the best type ?
      image.Filename = Filename;
      
      PaintDotNet.LayerList layers = document.Layers;
      Console.WriteLine("#layers: " + layers.Count);
      
      foreach (PaintDotNet.Layer readLayer in layers)
	{
	  try
	    {
	      Console.WriteLine(readLayer.Name);

	      var layer = new Layer(image, readLayer.Name,
				    document.Width, document.Height,
				    ImageType.Rgba,  
				    (readLayer.Opacity / 255) * 100, // 100 what means ?
				    LayerModeEffects.Normal);
	      Console.WriteLine("11");
	      image.AddLayer(layer, layerPosition++);
	      
	      Console.WriteLine("1");
	      
	      var rgn = new PixelRgn(layer, 0, 0, 
				     document.Width, document.Height,
				     true, false);
	      
	      var buf = new byte[document.Width * document.Height * 4];
	      var color_conv_ary = new byte[4];
	      int lastPixelConverted = 0;
	      colorOffset = 0;
	      var surf = (readLayer as BitmapLayer).Surface;

	      for (int row = 0; row < document.Height; row++)
		{
		  MemoryBlock memory = surf.GetRow(row);
		  byte[] bitmapBytes = memory.ToByteArray();
		  lastPixelConverted = 0;
		  colorOffset = 0;
		  for (int col = 0; col < document.Width * 4; col++)
		    {
		      color_conv_ary[colorOffset++] = bitmapBytes[col];
		      //							Console.WriteLine("ColorOffset = " + colorOffset);
		      
		      if (colorOffset >= 4)
			{
			  byte[] tmpArray = FromBGRAToRGBA(color_conv_ary);
			  
			  for (int j = 0; j < colorOffset; j++)
			    {
			      //									buf[row * document.Height + (lastPixelConverted++)] = tmpArray[j];
			      buf[(row * document.Width * 4) + (lastPixelConverted++)] = tmpArray[j];
			      
			      /*										Console.WriteLine("Scritto il byte[" + ((row *
														document.Width * 4) + (lastPixelConverted-1)) + "] : " +
														tmpArray[j]);*/
			    }
			  colorOffset = 0;
			}
		      
		      //							buf[row * document.Height + col] =  bitmapBytes[col]; //0x7F;
		    }
		  //						Console.WriteLine(memory.Length);
		}
	      //					Console.ReadLine();
	      
	      rgn.SetRect(buf, 0, 0, 
			  document.Width, document.Height);
	      layer.Flush();
	    }
	  catch (Exception e)
	    {
	      Console.WriteLine("Exception : " + e.Message + " - " +
				e.StackTrace);
	    }
	}
      // missing colormap, mcolor and background
      
      Console.WriteLine("2");
      
      /* Surface surface =
	 (layers[0] as
	 BitmapLayer).Surface;
	 MemoryBlock memory1 =
	 surface.GetRow(13);
	 byte[] bytes =
	 memory1.ToByteArray();
	 Console.WriteLine("length: " + bytes.Length);*/

      return image;
    }

    byte[] FromBGRAToRGBA(byte[] bgra)
    {
      byte r, g, b, a;

      a = bgra[3];
      r = bgra[2];
      g = bgra[1];
      b = bgra[0];

      return new byte[]{r, g, b, a};
    }
  }
}
