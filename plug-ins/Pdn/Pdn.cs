// The Paint .NET file format import/export plug-in
// Copyright (C) 2006 Massimo Perga
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
using System.IO;

using PaintDotNet;

namespace Gimp.Pdn
{
  class Pdn : FilePlugin
  {
    //	private XmlDocument headerXml;

    //		private const string headerXmlSkeleton =
    //			"<pdnImage><custom></custom></pdnImage>";

    [STAThread]
    static void Main(string[] args)
    {
      new Pdn(args);
    }

    public Pdn(string[] args) : base(args)
    {
    }

    override protected ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      set.Add(FileLoadProcedure("file_pdn_load",
				"loads images of the Paint.NET file format",
				"This plug-in loads images of the Paint.NET file format.",
				"Massimo Perga",
				"(C) Massimo Perga",
				"2006",
				"Paint.NET Image"));
      return set;
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("pdn", "");
    }

    //	public static void Main(string[] args)
    override protected Image Load(string filename)
    {
      Image image = null;
      Console.WriteLine("Filename = " +filename);
      Console.ReadLine();
      int layerPosition = 0;
      int colorOffset = 0;

      //		StreamReader stream = new Stream
      if (File.Exists(/*filename*/filename))
	{
	  Console.WriteLine("File exists");
	  Console.ReadLine();
	  /*				Stream openedFile = File.Open(filename,
					FileMode.Open);*/
	  StreamReader stream = new StreamReader(filename);
	  Document document = Document.FromStream(stream.BaseStream);

	  Console.WriteLine("Width  : " + document.Width);
	  Console.WriteLine("height : " + document.Height);

	  image = new Image(document.Width, document.Height,
			    ImageBaseType.Rgb); // Is it the best type ?
	  image.Filename = filename;

	  PaintDotNet.LayerList layers = document.Layers;
	  Console.WriteLine("#layers: " + layers.Count);

	  foreach (PaintDotNet.Layer readLayer in layers)
	    {
	      try
		{
		  Console.WriteLine(readLayer.Name);
		  Console.ReadLine();
		  Layer layer = new Layer(image, readLayer.Name,
					  document.Width, document.Height,
					  ImageType.Rgba,  
					  (readLayer.Opacity / 255) * 100, // 100 what means ?
					  LayerModeEffects.Normal);
		  Console.WriteLine("11");
		  image.AddLayer(layer, layerPosition++);

		  Console.WriteLine("1");

		  PixelRgn rgn = new PixelRgn(layer, 0, 0, 
					      document.Width, document.Height,
					      true, false);

		  byte[] buf = new byte[document.Width * document.Height * 4];
		  byte[] color_conv_ary = new byte[4];
		  int lastPixelConverted = 0;
		  colorOffset = 0;
		  Surface surf = (readLayer as BitmapLayer).Surface;

					
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

	  /*				Surface surface =
					(layers[0] as
					BitmapLayer).Surface;
					MemoryBlock memory1 =
					surface.GetRow(13);
					byte[] bytes =
					memory1.ToByteArray();
					Console.WriteLine("length: " + bytes.Length);*/
	}	
      Console.ReadLine();
      return image;
    }

    byte[] FromBGRAToRGBA(byte[] bgra)
    {
      byte r, g, b, a;

      //			Console.Write("From BGRAToRGBA called");
      a = bgra[3];
      r = bgra[2];
      g = bgra[1];
      b = bgra[0];

      /*			if(a == 0xFF)
				{
				Console.WriteLine(": Alpha opaque r=" + r + " g=" + g + " b=" + b );
				}*/

      return new byte[]{r, g, b, a};
    }
  }
}
