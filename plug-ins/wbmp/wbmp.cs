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

	  Progress _progress = new Progress("Loading " + filename);

	  byte readByte;
	  int width = 0;
	  while (((readByte = reader.ReadByte()) & 0x80) != 0)
	    {
	      width = (width << 7) + (int)(readByte & 0x7F);
	    }
	  width = (width << 7) + (int)(readByte & 0x7F);

	  int height = 0;
	  while (((readByte = reader.ReadByte()) & 0x80) != 0)
	    {
	      height = (height << 7) + (int)(readByte & 0x7F);
	    }
	  height = (height << 7) + (int)(readByte & 0x7F);

	  Image image = new Image(width, height, ImageBaseType.Gray);

	  Layer layer = new Layer(image, "Background", width, height,
				  ImageType.Gray, 100,
				  LayerModeEffects.Normal);
	  image.AddLayer(layer, 0);

	  image.Filename = filename;

	  PixelRgn rgn = new PixelRgn(layer, 0, 0, width, height, true, false);
	  byte[] buf = new byte[width * height];
	  int bufp = 0;

	  for (int row = 0; row < height; row++) 
	    {
	      int ext_copy_of_col = 0;
	      try
		{
		  int bytesToRead = (width + 7) / 8;
		  byte[] src = reader.ReadBytes(bytesToRead);

		  for (int col = 0; col < width; col++) 
		    {
		      ext_copy_of_col = col;
		      if (((src[col / 8] >> (7 - (col % 8))) & 1) == 1)
			{
			  buf[bufp] = 255;
			}
		      else
			{
			  buf[bufp] = 0;
			}
		      bufp++;
		    }
		  _progress.Update((double)(row/height));
		}
	      catch(Exception e)
		{
		  Console.WriteLine("Exception: {0} {1} row={2} col={3} bufp={4}", e.Message, e.StackTrace, row, ext_copy_of_col, bufp);
		  Console.ReadLine();
		}
	    }

	  rgn.SetRect(buf, 0, 0, width, height);
	  layer.Flush();

	  reader.Close();

	  _progress.Update(1);
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
			
      Progress _progress = new Progress("Saving " + filename);

      // If the image is not already indexed
      if (!drawable.IsIndexed)
	{
	  // Convert image to B&W picture if not already B&W
	  image.ConvertIndexed(ConvertDitherType.No, 
			       ConvertPaletteType.Mono,
			       0, false, false, "");
	}
      // Image already indexed
      /* else
	 {
	 }
      */

      BinaryWriter writer = new BinaryWriter(File.Open(filename, 
						       FileMode.Create));

      writer.Write((byte) 0);	// Write type
      writer.Write((byte) 0);	// Fixed header

      byte[] seqEncoded = null;

      // Encode the width on the multi-byte integer
      int bytesToEncode = bytesNeededForEncoding(width);
      if (bytesToEncode > 0)
	seqEncoded = new byte[bytesToEncode];

      encodeInteger(ref seqEncoded, width);
      for (int j = 0; j < seqEncoded.Length; j++)
	{
	  writer.Write(seqEncoded[j]);
	}

      // Encode the height on the multi-byte integer
      bytesToEncode = bytesNeededForEncoding(height);
      if (bytesToEncode > 0)
	seqEncoded = new byte[bytesToEncode];

      encodeInteger(ref seqEncoded, height);
      for (int j = 0; j < seqEncoded.Length; j++)
	writer.Write(seqEncoded[j]);

      Layer layer = image.Flatten();

      if (layer == null)
	{
	  Console.WriteLine("No flatten image");
	  Console.ReadLine();
	  return false;
	}

      Drawable activeDrawable = image.ActiveDrawable;

      if (activeDrawable == null)
	{
	  Console.WriteLine("No active drawable");
	  Console.ReadLine();
	  return false;
	}
      PixelRgn rgn = new PixelRgn(activeDrawable, 0, 0, width, height, 
				  true, false);
      byte[] wbmpImage = new byte[(width + 7) / 8 * height];
      byte[] buf = rgn.GetRect(0, 0, width, height);

      for (int row = 0; row < height; row++) 
	{
	  for (int col = 0; col < width; col++) 
	    {
	      int indexInWbmpImage = (col/8) + row*((width+7)/8);
	      if (buf[row*width+col] != 0)
		{
		  wbmpImage[indexInWbmpImage] |= (byte)(1 << (7 - col % 8));
		}
	      else
		{
		  wbmpImage[indexInWbmpImage] &= (byte)(~(1 << (7 - col % 8)));
		}

	      // Check if it's at the end of the byte...
	      if (((col + 1) % 8 == 0) || ((col + 1) == width) )
		writer.Write((byte)wbmpImage[indexInWbmpImage]);
	    }
	  _progress.Update((double)(row/height));
	}

      writer.Close();
      _progress.Update(1);
      activeDrawable.Flush();
      Display.DisplaysFlush();

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
