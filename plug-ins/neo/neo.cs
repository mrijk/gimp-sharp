// The neo plug-in
// Copyright (C) 2006-2009 Maurits Rijk
// Original code (in C) for GIMP 1.0 by Alain Gaymard
//
// neo.cs
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

namespace Gimp.neo
{	
  class neo : FilePlugin
  {
    const int NEO_WIDTH = 320;
    const int NEO_HEIGHT = 200;

    static void Main(string[] args)
    {
      new neo(args);
    }

    public neo(string[] args) : base(args, "neo")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return FileLoadProcedure("file_neo_load",
				     _("Loads neo images"),
				     _("This plug-in loads Neochrome images."),
				     "Maurits Rijk",
				     "(C) Maurits Rijk",
				     "2006-2009",
				     _("Neochrome Image"));
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("neo", "");
      // Fix me: set magic load handler
    }

    override protected Image Load()
    {
      // Read the header
      var head = ReadBytes(4);
      var pal = ReadBytes(32);
      var fill = ReadBytes(92);

      int width = fill[22] * 256 + fill[23];
      int height = fill[24] * 256 + fill[25];

      var image = NewImage(NEO_WIDTH, NEO_HEIGHT, ImageBaseType.Indexed,
			   ImageType.Indexed, Filename);
      image.Colormap = CreateColormap(pal);
      
      var buf = new byte[NEO_WIDTH * NEO_HEIGHT];
      int bufp = 0;
      
      for (int y = 0; y < NEO_HEIGHT; y++, bufp += NEO_WIDTH)
	{
	  DecodeLine(buf, bufp);
	}
      
      image.Layers[0].SetBuffer(buf);

      return image;
    }

    void DecodeLine(byte[] buf, int bufp)
    {
      var line = ReadBytes(160);
      int l = 0;
      for (int x = 0; x < NEO_WIDTH; l += 8)
	{
	  uint p0 = GetInteger(line, l + 0);
	  uint p1 = GetInteger(line, l + 2);
	  uint p2 = GetInteger(line, l + 4);
	  uint p3 = GetInteger(line, l + 6);
	  
	  // decode planes
	  for (int c = 0; c < 16; c++)
	    {
	      int tmp = (int) (((p0 >> 15) & 1) |
			       ((p1 >> 14) & 2) |
			       ((p2 >> 13) & 4) |
			       ((p3 >> 12) & 8));
	      buf[bufp + x] = (byte) tmp;
	      x++;
	      p0 <<= 1;
	      p1 <<= 1;
	      p2 <<= 1;
	      p3 <<= 1;
	    }
	}
    }
    
    uint GetInteger(byte[] line, int index)
    {
      return (uint) ((line[index] << 8) | line[index + 1]);
    }

    RGB[] CreateColormap(byte[] pal)
    {
      // convert pal: 0..7 -> 0..255
      var tab = new byte[]{0, 36, 73, 109, 146, 182, 219, 255};
      var cmap = new RGB[16];

      for (int i = 0; i < 16; i++)
	{
	  uint col = GetInteger(pal, 2 * i);
	  cmap[i].R = tab[(col >> 8) & 7];
	  cmap[i].G = tab[(col >> 4) & 7];
	  cmap[i].B = tab[(col >> 0) & 7];
	}
      return cmap;
    }
  }
}
