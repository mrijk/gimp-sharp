// The wbmp plug-in
// Copyright (C) 2004-2009 Maurits Rijk, Massimo Perga
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
      new wbmp(args);
    }

    public wbmp(string[] args) : base(args, "wbmp")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return FileLoadProcedure("file_wbmp_load",
				     _("Loads wbmp images"),
				     _("This plug-in loads wbmp images."),
				     "Maurits Rijk, Massimo Perga",
				     "(C) Maurits Rijk, Massimo Perga",
				     "2005-2009",
				     _("wbmp Image"));
      
      yield return FileSaveProcedure("file_wbmp_save",
				     _("Saves wbmp images"),
				     _("This plug-in saves wbmp images."),
				     "Maurits Rijk, Massimo Perga",
				     "(C) Maurits Rijk, Massimo Perga",
				     "2006-2009",
				     _("wbmp Image"),
				     "RGB*");
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
	  var reader = new BinaryReader(File.Open(filename, FileMode.Open));

	  byte type = reader.ReadByte();
	  if (type != 0)
	    {
	      new Message("Invalid file (Type should be zero)");
	      return null;
	    }

	  byte header = reader.ReadByte();
	  if (header != 0)
	    {
	      new Message("Invalid file (Fixed header should be zero)");
	      return null;
	    }

	  var progress = new Progress(_("Loading ") + filename);

	  int width = ReadDimension(reader);
	  int height = ReadDimension(reader);

	  var image = new Image(width, height, ImageBaseType.Gray);

	  var layer = new Layer(image, "Background", width, height,
				ImageType.Gray, 100,
				LayerModeEffects.Normal);
	  image.AddLayer(layer, 0);

	  image.Filename = filename;

	  var rgn = new PixelRgn(layer, true, false);
	  byte[] buf = new byte[width * height];
	  int bufp = 0;

	  for (int row = 0; row < height; row++) 
	    {
	      try
		{
		  byte[] src = reader.ReadBytes((width + 7) / 8);

		  for (int col = 0; col < width; col++) 
		    {
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
		  progress.Update((double) row / height);
		}
	      catch (Exception e)
		{
		  Console.WriteLine("Exception: {0} {1} row={2} bufp={3}", 
				    e.Message, e.StackTrace, row, bufp);
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
      int width = drawable.Width;
      int height = drawable.Height;
			
      var progress = new Progress(_("Saving ") + filename);

      // If the image is not already indexed
      if (!drawable.IsIndexed)
	{
	  // Convert image to B&W picture if not already B&W
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
      byte[] wbmpImage = new byte[(width + 7) / 8 * height];
      byte[] buf = rgn.GetRect(0, 0, width, height);

      for (int row = 0; row < height; row++) 
	{
	  for (int col = 0; col < width; col++) 
	    {
	      int indexInWbmpImage = col / 8 + row * ((width + 7) / 8);
	      if (buf[row * width + col] != 0)
		{
		  wbmpImage[indexInWbmpImage] |= (byte)(1 << (7 - col % 8));
		}
	      else
		{
		  wbmpImage[indexInWbmpImage] &= (byte)(~(1 << (7 - col % 8)));
		}

	      // Check if it's at the end of the byte...
	      if (((col + 1) % 8 == 0) || ((col + 1) == width))
		{
		  writer.Write(wbmpImage[indexInWbmpImage]);
		}
	    }
	  progress.Update((double) row / height);
	}

      writer.Close();

      return true;
    } 

    int ReadDimension(BinaryReader reader)
    {
      byte readByte;
      int dimension = 0;

      while (((readByte = reader.ReadByte()) & 0x80) != 0)
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
      byte[] seq = new byte[bytesToEncode];

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
