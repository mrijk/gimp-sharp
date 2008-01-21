// The GemImg plug-in
// Copyright (C) 2004-2008 Maurits Rijk
//
// GemImg.cs
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

namespace Gimp.GemImg
{	
  class GemImg : FilePlugin
  {    
    int _planes;
    int _patternSize;
    int _imageWidth;
    int _imageHeight;

    static void Main(string[] args)
    {
      new GemImg(args);
    }

    public GemImg(string[] args) : base(args, "GemImg")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return 
	FileLoadProcedure("file_img_load",
			  _("loads images of the GEM-Image file format"),
			  _("This plug-in loads images of the GEM-Image file format."),
			  "Maurits Rijk",
			  "(C) Maurits Rijk",
			  "2008",
			  _("GEM Image"));
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("IMG", "");
    }

    override protected Image Load(string filename)
    {
      if (File.Exists(filename))
	{
	  BinaryReader reader = new BinaryReader(File.Open(filename, 
							   FileMode.Open));

	  if (ReadHeader(reader))
	    {
	      RGB[] colormap = new RGB[] {
		new RGB(255,255,255),    
		new RGB(255,0,0),        
		new RGB(0,255,0),        
		new RGB(255,255,0),
		new RGB(0,0,255),
		new RGB(255,0,255),      
		new RGB(0,255,255),
		new RGB(181,181,181),
		new RGB(84,84,84),
		new RGB(127,0,0),
		new RGB(0,127,0),
		new RGB(127,127,0),
		new RGB(0,0,127),
		new RGB(127,0,127),
		new RGB(0,127,127),
		new RGB(0,0,0)
	      };

	      Image image = NewImage(_imageWidth, _imageHeight,
				     ImageBaseType.Indexed,
				     ImageType.Indexed, filename);
	      image.Colormap = colormap;
	      PixelRgn rgn = new PixelRgn(image.Layers[0], true, false);

	      int bparrow = (_imageWidth + 7) / 8;

	      for (int y = 0; y < _imageHeight; )
		{
		  // byte[] line = new byte[bparrow * 8];
		  byte[] line = new byte[_imageWidth];
		  int count = ReadLine(reader, line);
		  do
		    {
		      rgn.SetRow(line, 0, y++);
		    }
		  while (--count > 0);
		}

	      return image;
	    }
	  reader.Close();
	  return null;
	}
      return null;
    }

    bool ReadHeader(BinaryReader reader)
    {
      int version = ReadShort(reader);
      if (version != 1)
	{
	  return false;
	}

      int headSize = ReadShort(reader);
      if (headSize != 8)
	{
	  return false;
	}

      _planes = ReadShort(reader);
      if (_planes != 1 && _planes != 4)
	{
	  return false;
	}

      _patternSize = ReadShort(reader);
      int pixelWidth = ReadShort(reader);
      int pixelHeight = ReadShort(reader);
      _imageWidth = ReadShort(reader);
      _imageHeight = ReadShort(reader);

      Console.WriteLine("Version     : " + version);
      Console.WriteLine("Header size : " + headSize);
      Console.WriteLine("Planes      : " + _planes);
      Console.WriteLine("Pattern size: " + _patternSize);
      Console.WriteLine("Image width : " + _imageWidth);
      Console.WriteLine("Image height: " + _imageHeight);

      return true;
    }

    int ReadLine(BinaryReader reader, byte[] line)
    {
      int vrc = 0;
      for (int plane = 0; plane < _planes; plane++)
	{
	  byte shift = (byte) (1 << plane);
	  int x = 0;

	  while (x < _imageWidth)
	    {
	      byte ch = reader.ReadByte();
	      if (ch == 0)
		{
		  ch = reader.ReadByte();
		  if (ch == 0)
		    {
		      vrc = ReadRepeatedRows(reader);
		    }
		  else
		    {
		      x = ReadPattern(reader, ch, x, line, shift);
		    }
		}
	      else if (ch == 0x80)
		{
		  x = ReadLiteralString(reader, x, line, shift);
		}
	      else
		{
		  x = ReadRLE(ch, x, line, shift);
		}
	    }
	}
      return vrc;
    }

    int ReadRepeatedRows(BinaryReader reader)
    {
      byte ch = reader.ReadByte();
      if (ch != 0xff)
	{
	  Console.WriteLine("img: Invalid character!");
	  Console.WriteLine("img: File may be corrupted or not in the expexted format!");
	  throw new GimpSharpException();
	}
      return reader.ReadByte();
    }

    int ReadPattern(BinaryReader reader, int repetitions, int x, byte[] line,
		    int shift)
    {
      byte[] pattern = reader.ReadBytes(_patternSize);

      for (int i = 0; i < repetitions; i++)
	{
	  for (int j = 0; j < _patternSize ; j++)
	    {
	      for (int k = 7; k >= 0; k--)
		{
		  byte val = (byte) 
		    (((pattern[j] & (0x01 << k)) >> k) * shift);
		  line[x++] |= val;
		}
	    }
	}

      return x;
    }

    int ReadLiteralString(BinaryReader reader, int x, byte[] line, byte shift)
    {
      int count = reader.ReadByte();
      byte[] tmp = reader.ReadBytes(count);

      for (int i = 0; i < count; i++)
	{
	  int bit = 0x01 << 7;
	  for (int j = 7; j >= 0 ; j--, x++)
	    {
	      if ((tmp[i] & bit) != 0)
		{
		  line[x] += shift;
		}
	      bit >>= 1;
	    }
	}
      return x;
    }

    int ReadRLE(byte ch, int x, byte[] line, byte shift)
    {
      int count = ch & 0x7F;

      for (int i = 0; i < count; i++)
	{
	  for (int j = 0; j < 8; j++, x++)
	    {
	      if ((ch & 0x80) != 0)
		{
		  line[x] += shift;
		}
	    }
	}
      return x;
    }

    int ReadShort(BinaryReader reader)
    {
      byte[] tmp = reader.ReadBytes(2);
      return tmp[0] * 256 + tmp[1];
    }
  }
}


