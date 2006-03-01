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

		const int KOALA_WIDTH = 320;
		const int KOALA_HEIGHT = 200;

		byte[] _mcolor;
		byte[] _color;
		byte _background;

		byte[] _colormap = new byte[]
		{
			0x00, 0x00, 0x00,
				0xff, 0xff, 0xff,
				0x88, 0x00, 0x00,
				0xaa,
				0xff,
				0xee,
				0xcc,
				0x44,
				0xcc,
				0x00,
				0xcc,
				0x55,
				0x00,
				0x00,
				0xaa,
				0xee,
				0xee,
				0x77,
				0xdd,
				0x88,
				0x55,
				0x66,
				0x44,
				0x00,
				0xff,
				0x77,
				0x77,
				0x33,
				0x33,
				0x33,
				0x77,
				0x77,
				0x77,
				0xaa,
				0xff,
				0x66,
				0x00,
				0x88,
				0xff,
				0xbb,
				0xbb,
				0xbb
		};

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
													ImageBaseType.INDEXED); // Is it the best type ?

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
							ImageType.INDEXED,  
							(readLayer.Opacity / 255) * 100, // 100 what means ?
							LayerModeEffects.NORMAL);

					Console.WriteLine("1");

					PixelRgn rgn = new PixelRgn(layer, 0, 0, 
							document.Width, document.Height,
							true, false);

					byte[] buf = new byte[document.Width * document.Height];
					Surface surf = (readLayer as BitmapLayer).Surface;

					for(int row = 0; row < document.Height; row++)
					{
						MemoryBlock memory = surf.GetRow(row);
						byte[] bitmapBytes = memory.ToByteArray();
						for(int col = 0; col < document.Width; col++)
						{
							buf[row * document.Height + col] =  bitmapBytes[col]; //0x7F;
						}
						Console.WriteLine(memory.Length);
					}
					Console.ReadLine();

					rgn.SetRect(buf, 0, 0, 
							document.Width, document.Height);
					layer.Flush();
					}
					catch(Exception e)
					{
						Console.WriteLine("Exception : " + e.Message + " - " +
						e.StackTrace);
					}
				}
				image.Filename = filename;
				// missing colormap, mcolor and background

				Console.WriteLine("2");

				Surface surface =
					(layers[0] as
					 BitmapLayer).Surface;
				MemoryBlock memory1 =
					surface.GetRow(13);
				byte[] bytes =
					memory1.ToByteArray();
				Console.WriteLine("length: " + bytes.Length);
			}	
			Console.ReadLine();
			return image;
		}
	}
}
