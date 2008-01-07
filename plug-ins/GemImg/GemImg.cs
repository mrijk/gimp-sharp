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
      byte[] bitmap;

      if (File.Exists(filename))
	{
	  BinaryReader reader = new BinaryReader(File.Open(filename, 
							   FileMode.Open));

	  if (ReadHeader(reader))
	    {
	      Image image = NewImage(_imageWidth, _imageHeight,
				     ImageBaseType.Indexed,
				     ImageType.Indexed, filename);

	      for (int y = 0; y < _imageHeight; y++)
		{
		  Console.Write("-----> {0}: ", y);
		  ReadLine(reader);
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

    void ReadLine(BinaryReader reader)
    {
      for (int plane = 0; plane < _planes; plane++)
	{
	  int x = 0;
	  while (x < _imageWidth)
	    {
	      byte ch = reader.ReadByte();
	      if (ch == 0)
		{
		  ch = reader.ReadByte();
		  if (ch == 0)
		    {
		      x += ReadRepeatedRows(reader);
		    }
		  else
		    {
		      x += ReadPattern(reader, ch);
		    }
		}
	      else if (ch == 0x80)
		{
		  x += ReadLiteralString(reader);
		}
	      else
		{
		  x += ReadRLE(ch);
		}
	    }
	  Console.WriteLine(x);
	}
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
      int repetitions = reader.ReadByte();

      Console.WriteLine("ReadRepeatedRows: " + repetitions);

      return repetitions * _imageWidth * 8;
    }

    int ReadPattern(BinaryReader reader, int repetitions)
    {
      byte[] tmp = reader.ReadBytes(_patternSize);
      Console.WriteLine("ReadPattern: " + repetitions);
      return repetitions * _patternSize * 8;
    }

    int ReadLiteralString(BinaryReader reader)
    {
      int count = reader.ReadByte();
      byte[] tmp = reader.ReadBytes(count);
      Console.WriteLine("ReadLiteralString: " + count);
      return count * 8;
    }

    int ReadRLE(byte ch)
    {
      int count = ch & 0x7F;
      Console.WriteLine("ReadRLE: " + count);
      return count * 8;
    }

    int ReadShort(BinaryReader reader)
    {
      byte[] tmp = reader.ReadBytes(2);
      return tmp[0] * 256 + tmp[1];
    }
  }
}


