// The wbmp plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.Collections.Generic;
using System.IO;

namespace Gimp.wbmp
{	
  class wbmp : FilePlugin
  {
    static void Main(string[] args)
    {
      GimpMain<wbmp>(args);
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return FileLoadProcedure("file_wbmp_load",
				     _("Loads wbmp images"),
				     _("This plug-in loads wbmp images."),
				     "Maurits Rijk",
				     "(C) Maurits Rijk",
				     "2005-2011",
				     _("wbmp Image"));
      
      yield return FileSaveProcedure("file_wbmp_save",
				     _("Saves wbmp images"),
				     _("This plug-in saves wbmp images."),
				     "Maurits Rijk",
				     "(C) Maurits Rijk",
				     "2006-2011",
				     _("wbmp Image"),
				     "RGB*");
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("wbmp", "");
      RegisterSaveHandler("wbmp", "");
    }

    override protected Image Load()
    {
      byte type = ReadByte();
      if (type != 0)
	{
	  new Message("Invalid file (Type should be zero)");
	  return null;
	}
      
      byte header = ReadByte();
      if (header != 0)
	{
	  new Message("Invalid file (Fixed header should be zero)");
	  return null;
	}

      int width = ReadDimension();
      int height = ReadDimension();

      var image = NewImage(width, height, ImageBaseType.Gray,
			   ImageType.Gray, Filename);
      
      var buf = new byte[width * height];
      int bufp = 0;
      
      for (int row = 0; row < height; row++) 
	{
	  try
	    {
	      var src = ReadBytes((width + 7) / 8);
	      
	      for (int col = 0; col < width; col++) 
		{
		  buf[bufp] = (byte) ((BitIsSet(src, col)) ? 255 : 0);
		  bufp++;
		}
	    }
	  catch (Exception e)
	    {
	      Console.WriteLine("Exception: {0} {1} row={2} bufp={3}", 
				e.Message, e.StackTrace, row, bufp);
	    }
	}
      
      var layer = image.Layers[0];
      layer.SetBuffer(buf);
      layer.Flush();
       
      return image;
    }

    bool BitIsSet(byte[] row, int col)
    {
      return ((row[col / 8] >> (7 - (col % 8))) & 1) == 1;
    }

    override protected bool Save(Image image, Drawable drawable, 
				 string filename)
    {
      int width = drawable.Width;
      int height = drawable.Height;

      if (!drawable.IsIndexed)
	{
	  image.ConvertIndexed(ConvertDitherType.No, ConvertPaletteType.Mono,
			       0, false, false, "");
	}

      var writer = new BinaryWriter(File.Open(filename, FileMode.Create));

      writer.Write((byte) 0);	// Write type
      writer.Write((byte) 0);	// Fixed header

      writer.Write(EncodeInteger(width));
      writer.Write(EncodeInteger(height));

      image.Flatten();

      var rgn = new PixelRgn(drawable, true, false);
      var wbmpImage = new byte[(width + 7) / 8 * height];
      var buf = rgn.GetRect(0, 0, width, height);

      for (int row = 0; row < height; row++) 
	{
	  for (int col = 0; col < width; col++) 
	    {
	      int offset = col / 8 + row * ((width + 7) / 8);
	      if (buf[row * width + col] != 0)
		{
		  wbmpImage[offset] |= (byte)(1 << (7 - col % 8));
		}
	      else
		{
		  wbmpImage[offset] &= (byte)(~(1 << (7 - col % 8)));
		}

	      // Check if it's at the end of the byte...
	      if (((col + 1) % 8 == 0) || ((col + 1) == width))
		{
		  writer.Write(wbmpImage[offset]);
		}
	    }
	}

      writer.Close();

      return true;
    } 

    int ReadDimension()
    {
      byte readByte;
      int dimension = 0;

      while (((readByte = ReadByte()) & 0x80) != 0)
	{
	  dimension = (dimension << 7) + (int)(readByte & 0x7F);
	}
      dimension = (dimension << 7) + (int)(readByte & 0x7F);

      return dimension;
    }

    int BytesNeededForEncoding(int numberToEncode)
    {
      int bytesNeeded = 0;

      while (numberToEncode != 0)
	{
	  bytesNeeded++;
	  numberToEncode >>= 7;
	}

      return bytesNeeded;
    }

    byte[] EncodeInteger(int number)
    {
      int bytesToEncode = BytesNeededForEncoding(number);
      var seq = new byte[bytesToEncode];

      // Start from the less significant part
      for (int index = bytesToEncode - 1; index >= 0; index--)
	{
	  seq[index] = (byte) (number & 0x7f);
	  number >>= 7;
	  if (index != bytesToEncode - 1)
	    {
	      seq[index] |= 0x80;
	    }
	}
      return seq;
    }
  } 
}
