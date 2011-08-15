// The GemImg plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
      GimpMain<GemImg>(args);
    }

    override protected Procedure GetProcedure()
    {
      return 
	FileLoadProcedure("file_img_load",
			  _("loads images of the GEM-Image file format"),
			  _("This plug-in loads images of the GEM-Image file format."),
			  "Maurits Rijk",
			  "(C) Maurits Rijk",
			  "2008-2011",
			  _("GEM Image"));
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("IMG", "");
    }

    override protected Image Load()
    {
      if (ReadHeader())
	{
	  var colormap = new RGB[] {
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
	  
	  var image = NewImage(_imageWidth, _imageHeight,
			       ImageBaseType.Indexed,
			       ImageType.Indexed, Filename);
	  image.Colormap = colormap;
	  var rgn = new PixelRgn(image.Layers[0], true, false);
	  
	  int bparrow = (_imageWidth + 7) / 8;
	  
	  for (int y = 0; y < _imageHeight; )
	    {
	      // byte[] line = new byte[bparrow * 8];
	      var line = new byte[_imageWidth];
	      int count = ReadLine(line);
	      do
		{
		  rgn.SetRow(line, 0, y++);
		}
	      while (--count > 0);
	    }
	  
	  return image;
	}
      return null;
    }

    bool ReadHeader()
    {
      int version = ReadShort();
      if (version != 1)
	{
	  return false;
	}

      int headSize = ReadShort();
      if (headSize != 8)
	{
	  return false;
	}

      _planes = ReadShort();
      if (_planes != 1 && _planes != 4)
	{
	  return false;
	}

      _patternSize = ReadShort();
      int pixelWidth = ReadShort();
      int pixelHeight = ReadShort();
      _imageWidth = ReadShort();
      _imageHeight = ReadShort();

      return true;
    }

    int ReadLine(byte[] line)
    {
      int vrc = 0;
      for (int plane = 0; plane < _planes; plane++)
	{
	  byte shift = (byte) (1 << plane);
	  int x = 0;

	  while (x < _imageWidth)
	    {
	      byte ch = ReadByte();
	      if (ch == 0)
		{
		  ch = ReadByte();
		  if (ch == 0)
		    {
		      vrc = ReadRepeatedRows();
		    }
		  else
		    {
		      x = ReadPattern(ch, x, line, shift);
		    }
		}
	      else if (ch == 0x80)
		{
		  x = ReadLiteralString(x, line, shift);
		}
	      else
		{
		  x = ReadRLE(ch, x, line, shift);
		}
	    }
	}
      return vrc;
    }

    int ReadRepeatedRows()
    {
      byte ch = ReadByte();
      if (ch != 0xff)
	{
	  Console.WriteLine("img: Invalid character!");
	  Console.WriteLine("img: File may be corrupted or not in the expexted format!");
	  throw new GimpSharpException();
	}
      return ReadByte();
    }

    int ReadPattern(int repetitions, int x, byte[] line, int shift)
    {
      var pattern = ReadBytes(_patternSize);

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

    int ReadLiteralString(int x, byte[] line, byte shift)
    {
      int count = ReadByte();
      var tmp = ReadBytes(count);

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

    int ReadShort()
    {
      var tmp = ReadBytes(2);
      return tmp[0] * 256 + tmp[1];
    }
  }
}


