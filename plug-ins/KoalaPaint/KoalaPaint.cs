// The KoalaPaint plug-in
// Copyright (C) 2004-2010 Maurits Rijk
//
// KoalaPaint.cs
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

namespace Gimp.KoalaPaint
{	
  class KoalaPaint : FilePlugin
  {
    const int KOALA_WIDTH = 320;
    const int KOALA_HEIGHT = 200;

    byte[] _mcolor;
    byte[] _color;
    byte _background;

    static void Main(string[] args)
    {
      new KoalaPaint(args);
    }

    public KoalaPaint(string[] args) : base(args, "KoalaPaint")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return 
	FileLoadProcedure("file_koala_paint_load",
			  _("loads images of the Koala Paint file format"),
			  _("This plug-in loads images of the Koala Paint file format."),
			  "Maurits Rijk",
			  "(C) Maurits Rijk",
			  "1999 - 2010",
			  _("KoalaPaint Image"));
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("koa", "");
    }

    override protected Image Load()
    {
      ReadBytes(2);
      var bitmap = ReadBytes(8000);
      _mcolor = ReadBytes(1000);
      _color = ReadBytes(1000);
      _background = ReadByte();
      
      var image = NewImage(KOALA_WIDTH, KOALA_HEIGHT, 
			   ImageBaseType.Indexed, ImageType.Indexed, 
			   Filename);
      image.Colormap = GetColormap();
 
      var rgn = new PixelRgn(image.Layers[0], true, false);
      
      var buf = new byte[KOALA_WIDTH * KOALA_HEIGHT];
      int bufp = 8;
      
      for (int row = 0; row < KOALA_HEIGHT; row++) 
	{
	  for (int col = 0; col < KOALA_WIDTH / 8; col++) 
	    {
	      byte p = bitmap[(row / 8) * KOALA_WIDTH + row % 8 + col * 8];
	      
	      for (int i = 0; i < 4; i++) 
		{
		  byte index = GetColor(row / 8, col, p & 3);
		  buf[--bufp] = index;
		  buf[--bufp] = index;
		  p >>= 2;
		}
	      bufp += 16;
	    }
	}
      
      rgn.SetRect(buf, 0, 0, KOALA_WIDTH, KOALA_HEIGHT);
      
      return image;
    }

    RGB[] GetColormap()
    {
      return new RGB[]
	{
	  new RGB(0x00, 0x00, 0x00),
	  new RGB(0xff, 0xff, 0xff),
	  new RGB(0x88, 0x00, 0x00),
	  new RGB(0xaa, 0xff, 0xee),
	  new RGB(0xcc, 0x44, 0xcc),
	  new RGB(0x00, 0xcc, 0x55),
	  new RGB(0x00, 0x00, 0xaa),
	  new RGB(0xee, 0xee, 0x77),
	  new RGB(0xdd, 0x88, 0x55),
	  new RGB(0x66, 0x44, 0x00),
	  new RGB(0xff, 0x77, 0x77),
	  new RGB(0x33, 0x33, 0x33),
	  new RGB(0x77, 0x77, 0x77),
	  new RGB(0xaa, 0xff, 0x66),
	  new RGB(0x00, 0x88, 0xff),
	  new RGB(0xbb, 0xbb, 0xbb)
	};
    }

    byte GetColor(int row, int col, int index)
    {
      int offset = row * KOALA_WIDTH / 8 + col;
      if (index == 0)
	return LowNibble(_background);
      else if (index == 1)
	return HighNibble(_mcolor[offset]);
      else if (index == 2)
	return LowNibble(_mcolor[offset]);
      else
	return LowNibble(_color[offset]);
    }

    byte LowNibble(byte val)
    {
      return (byte) (val & 0x0f);
    }

    byte HighNibble(byte val)
    {
      return (byte) ((val >> 4) & 0x0f);
    }
  }
}
