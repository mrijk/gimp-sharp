// The neo plug-in
// Copyright (C) 2006 Maurits Rijk
// Original code for GIMP 1.0 by Alain Gaymard
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
using System.IO;

namespace Gimp.neo
{	
  class neo : FilePlugin
  {
    const int NEO_WIDTH = 320;
    const int NEO_HEIGHT = 200;

    [STAThread]
    static void Main(string[] args)
    {
      new neo(args);
    }

    public neo(string[] args) : base(args)
    {
    }

    override protected  ProcedureSet GetProcedureSet()
    {
      ProcedureSet set = new ProcedureSet();

      set.Add(FileLoadProcedure("file_neo_load",
				"Loads neo images",
				"This plug-in loads Neochrome images.",
				"Maurits Rijk",
				"(C) Maurits Rijk",
				"2006",
				"Neochrome Image"));
      return set;
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("neo", "");
      // Fix me: set magic load handler
    }

    override protected Image Load(string filename)
    {
      if (File.Exists(filename))
	{
	  BinaryReader reader = new BinaryReader(File.Open(filename, 
							   FileMode.Open));
	  
	  // Read the header
	  byte[] head = reader.ReadBytes(4);
	  byte[] pal = reader.ReadBytes(32);
	  byte[] fill = reader.ReadBytes(92);

	  int width = fill[22] * 256 + fill[23];
	  int height = fill[24] * 256 + fill[25];

	  // convert pal: 0..7 -> 0..255
	  byte[] tab = new byte[]{0, 36, 73, 109, 146, 182, 219, 255};
	  byte[] cmap = new byte[16 * 3];

	  for (int i = 0, j = 0; i < 16; i++)
	    {
	      uint col = (uint) ((pal[2 * i] << 8) | pal[2 * i + 1]);
	      cmap[j++] = tab[(col >> 8) & 7];
	      cmap[j++] = tab[(col >> 4) & 7];
	      cmap[j++] = tab[(col >> 0) & 7];
	    }

	  Image image = new Image(NEO_WIDTH, NEO_HEIGHT, 
				  ImageBaseType.INDEXED);
	  
	  Layer layer = new Layer(image, "Background", NEO_WIDTH, 
				  NEO_HEIGHT, ImageType.INDEXED, 100, 
				  LayerModeEffects.NORMAL);
	  image.AddLayer(layer, 0);
	  
	  image.Filename = filename;
	  image.Colormap = cmap;

	  PixelRgn rgn = new PixelRgn(layer, 0, 0, NEO_WIDTH, NEO_HEIGHT, 
				      true, false);
	  byte[] buf = new byte[NEO_WIDTH * NEO_HEIGHT];
	  int bufp = 0;
	  
	  for (int y = 0; y < NEO_HEIGHT; y++, bufp += NEO_WIDTH) 
	    {
	      byte[] line = reader.ReadBytes(160);
	      int l = 0;
	      for (int x = 0; x < NEO_WIDTH; l += 8) 
		{
		  uint p0 = (uint) ((line[l + 0] << 8) | line[l + 1]);
		  uint p1 = (uint) ((line[l + 2] << 8) | line[l + 3]);
		  uint p2 = (uint) ((line[l + 4] << 8) | line[l + 5]);
		  uint p3 = (uint) ((line[l + 6] << 8) | line[l + 7]);

		  // decode planes
		  for (int c = 0; c < 16; c++)
		    {
		      int tmp = (int) (
			((p0 >> 15) & 1) |
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
	  
	  rgn.SetRect(buf, 0, 0, NEO_WIDTH, NEO_HEIGHT);
	  
	  reader.Close();

	  return image;
	}
      return null;
    }
  }
}
