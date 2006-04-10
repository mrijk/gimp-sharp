// The wbmp plug-in
// Copyright (C) 2004-2006 Maurits Rijk, Massimo Perga
//
// wbmp.cs
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

namespace Gimp.wbmp
{	
  class wbmp : FilePlugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new wbmp(args);
    }

    public wbmp(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();
      try
	{

	  set.Add(FileLoadProcedure("file_wbmp_load",
				    "Loads wbmp images",
				    "This plug-in loads wbmp images.",
				    "Maurits Rijk, Massimo Perga",
				    "(C) Maurits Rijk, Massimo Perga",
				    "2005-2006",
				    "wbmp Image"));

	  set.Add(FileSaveProcedure("file_wbmp_save",
				    "Saves wbmp images",
				    "This plug-in saves wbmp images.",
				    "Maurits Rijk, Massimo Perga",
				    "(C) Maurits Rijk, Massimo Perga",
				    "2006",
				    "wbmp Image",
				    "RGB*"));
	}
      catch(Exception e)
	{
	  Console.WriteLine("{0} {1}", e.Message, e.StackTrace);
	  Console.ReadLine();
	}

      return set;
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("wbmp", "");
      RegisterSaveHandler("wbmp", "");
    }

    override protected Image Load(string filename)
    {
      if (File.Exists(filename))
	{
	  BinaryReader reader = new BinaryReader(File.Open(filename, 
							   FileMode.Open));

	  byte type = reader.ReadByte();
	  if (type != 0)
	    {
	      Console.WriteLine("Type should be zero!");
	      Console.ReadLine();
	      return null;
	    }

	  byte header = reader.ReadByte();
	  if (header != 0)
	    {
	      Console.WriteLine("Fixed header should be zero!");
	      Console.ReadLine();
	      return null;
	    }

	  //				byte width = reader.ReadByte();
	  byte readByte;
	  int width = 0;
	  while(((readByte = reader.ReadByte()) & 0x80) == 1)
	    {
	      width = (width << 7) + (int)(readByte & 0x80);
	    }
	  width += (int)readByte;

	  //				byte height = reader.ReadByte();
	  int height = 0;
	  while(((readByte = reader.ReadByte()) & 0x80) == 1)
	    {
	      height = (height << 7) + (int)(readByte & 0x80);
	    }
	  height += (int)readByte;

	  // Fix me: check high bit here for larger sizes

	  Image image = new Image(width, height,
				  ImageBaseType.GRAY);

	  Layer layer = new Layer(image, "Background", width, height,
				  ImageType.GRAY, 100, 
				  LayerModeEffects.Normal);
	  image.AddLayer(layer, 0);

	  image.Filename = filename;

	  Console.WriteLine("Height = {0} Width = {1}", height, width);
	  Console.ReadLine();

	  PixelRgn rgn = new PixelRgn(layer, 0, 0, width, height, true, false);
	  byte[] buf = new byte[width * height];
	  int bufp = 0;

	  for (int row = 0; row < height; row++) 
	    {
	      int ext_copy_of_col = 0;
	      try
		{
		  byte[] src = reader.ReadBytes((width + 7) / 8);

		  for (int col = 0; col < width; col++) 
		    {
		      ext_copy_of_col = col;
		      if (((src[col / 8] >> (7 - col % 8)) & 1) == 1)
			{
			  buf[bufp] = 255;
			}
		      else
			{
			  buf[bufp] = 0;
			}
		      bufp++;
		    }
		}
	      catch(Exception e)
		{
		  Console.WriteLine("Exception: {0} {1} row={2} col={3}", e.Message, e.StackTrace, row, ext_copy_of_col);
		  Console.ReadLine();
		}
	    }

	  rgn.SetRect(buf, 0, 0, width, height);
	  layer.Flush();

	  reader.Close();

	  return image;
	}
      return null;
    }

    override protected bool Save(Image image, Drawable drawable, 
				 string filename)
    {
      // First check size
      int width = drawable.Width;
      int height = drawable.Height;

      System.Console.WriteLine("Dentro Save [{0}]", filename);
      Console.ReadLine();

      if (width > 127 || height > 127)
	{
	  Console.WriteLine("width > 127 or height > 127 not supported yet!");
	  Console.ReadLine();
	  //				return false;
	}

      // If the image is not already indexed
      if (!drawable.IsIndexed)
	{
	  // Convert image to B&W picture if not already B&W
	  if ( !image.ConvertIndexed(ConvertDitherType.No, 
				     ConvertPaletteType.Mono,
				     0, false, false, ""))
	    {
	      Console.WriteLine("Conversion to B&W failed");
	      Console.ReadLine();
	      //				return false;
	    }
	}
      else
	{
	  Console.WriteLine("Image already indexed");
	  Console.ReadLine();
	}

      Console.WriteLine("AAA");
      Console.ReadLine();

      BinaryWriter writer = new BinaryWriter(File.Open(filename, 
						       FileMode.Create));

      writer.Write((byte) 0);	// Write type
      writer.Write((byte) 0);	// Fixed header

      int bytesToEncode;
      byte[] seqEncoded = null;
		
      // Encode the width on the multi-byte integer
      bytesToEncode = bytesNeededForEncoding(width);
      if(bytesToEncode > 0)
	seqEncoded = new byte[bytesToEncode];

      encodeInteger(ref seqEncoded, width);
      for(int j = 0; j < seqEncoded.Length; j++)
	{
	  Console.WriteLine("{0:x}", seqEncoded[j]);
	  writer.Write(seqEncoded[j]);
	}
      Console.ReadLine();

      // Encode the height on the multi-byte integer
      bytesToEncode = bytesNeededForEncoding(height);
      if(bytesToEncode > 0)
	seqEncoded = new byte[bytesToEncode];

      encodeInteger(ref seqEncoded, height);
      for(int j = 0; j < seqEncoded.Length; j++)
	writer.Write(seqEncoded[j]);

      //			writer.Write((byte) width);
      //			writer.Write((byte) height);


      Layer layer = image.ActiveLayer;

      if(layer == null)
	{
	  Console.WriteLine("No active layer");
	  Console.ReadLine();
	  return false;
	}

      Drawable activeDrawable = image.ActiveDrawable;

      if(activeDrawable == null)
	{
	  Console.WriteLine("No active drawable");
	  Console.ReadLine();
	  return false;
	}
      /*			PixelRgn rgn = new PixelRgn(layer, 0, 0, width, height, true, false);
				byte[] wbmpImage = new byte[(width+7)/8 * height];
				buf = rgn.GetRect(0, 0, width, height);

				for (int row = 0; row < height; row++) 
				{

				for (int col = 0; col < width; col++) 
				{
				if (((src[col / 8] >> (7 - col % 8)) & 1) == 1)
				{
				buf[bufp] = 255;
				}
				else
				{
				buf[bufp] = 0;
				}
				bufp++;
				}
				for(int col = 0; col < width; col++)
				{
				}
				}

				rgn.SetRect(buf, 0, 0, width, height);
				layer.Flush();*/
			 
      //RgnIterator iter = new RgnIterator(activeDrawable, RunMode.NONINTERACTIVE);
				
      RgnIterator iter = new RgnIterator(drawable, RunMode.Noninteractive);

      Console.WriteLine("BBB");
      Console.ReadLine();
      int i=0;
      //byte [] wbmpImage = new byte[(width+7)/8 * height];
      byte byteToWrite = 0;
      //iter.IterateOnBuffer( delegate (int col, int row, byte[] img)
      iter.IterateSrc( delegate (int col, int row, byte[] img)
      {
	try
	  {
	    Console.Write("Col = {0} Row = {1} Width = {2}", col, row, width);
	    //						Console.Write("Byte {0}: {1:x}", i, img[0]/*, wbmpImage.Length*/);
	    //						if(img[col + (row * width)] == 0)
	    if(img != null && img[0] == 0)
	      {
		Console.Write("= 0");
		// Reset the corresponding bit
		//wbmpImage[(col ) / 8 + row * ((width+7)/8)] &= (byte)~(1 << (7 - col % 8));
		byteToWrite &= (byte)~(1 << (7 - col % 8));
	      }
	    else
	      {
		Console.Write("! 0");
		// Set the corresponding bit
		//					wbmpImage[(col ) / 8 + row * ((width+7)/8)] |= (byte)(1 << (7 - col % 8));
		byteToWrite |= (byte)(1 << (7 - col % 8));
	      }
	    //Console.WriteLine("+ {0:x}", wbmpImage[(col ) / 8 + row * ((width+7)/8)]);
	    Console.WriteLine("+ {0:x}", byteToWrite);
	  }
	catch(Exception e)
	  {
	    Console.WriteLine("-- {0} {1} {2} {3} -- {4}", col, row,
			      //wbmpImage.Length, (col + 7) / 8 + row * ((width+7)/8),
			      byteToWrite, (col + 7) / 8 + row * ((width+7)/8),
			      (e.StackTrace + e.Message));
	    Console.WriteLine("...........");
	    Console.WriteLine("Source " + e.Source);
	    Console.WriteLine("Message " + e.Message);
	    Console.WriteLine("TargetSite	 " + e.TargetSite);
	    Console.WriteLine("Data	 " + e.Data);
	    Console.WriteLine("...........");
	  }
	// Check if it's at the end of the byte...
	if( ((col+1)%8 == 0) ||
	    ((col+1) == width) )
	  {
	    //Console.WriteLine("writing {0} ", wbmpImage[(col ) / 8 + row * ((width+7)/8)]);
	    Console.WriteLine("## writing {0:x} ##", byteToWrite);
	    //						writer.Write((byte)wbmpImage[(col ) / 8 + row * ((width+7)/8)]);
	    writer.Write(byteToWrite);
	    if((col+1)==width)
	      Console.WriteLine("Raggiunto il fine linea");
	    byteToWrite = 0;
	  }

	// If so, write it to the WBMP file
	i++;
      }						
		       );
      Console.WriteLine("Read = " + i);
      Console.ReadLine();
      //			for(int z = 0; z < wbmpImage.Length; z++)
      //				writer.Write((byte)wbmpImage[z]);

      writer.Close();


      return true;
    } 

    private int bytesNeededForEncoding(int numberToEncode)
    {
      int bytesNeeded = 0;
      int stillToCalculate = numberToEncode;
			
      while (stillToCalculate != 0)
	{
	  bytesNeeded++;
	  stillToCalculate >>= 7;
	}

      return bytesNeeded;
    }

    private void encodeInteger(ref byte[] seq, int number)
    {
      // Start from the less significant part
      for (int index = (seq.Length - 1); index >= 0; index--)
	{
	  seq[index] = (byte)(number & 0x7f);
	  number >>= 7;
	  if (index != (seq.Length - 1))
	    seq[index] |= 0x80;
	}
    }
  } 
}
