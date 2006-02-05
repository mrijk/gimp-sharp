// The wbmp plug-in
// Copyright (C) 2004-2006 Maurits Rijk
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
				"Maurits Rijk",
				"(C) Maurits Rijk",
				"2005-2006",
				"wbmp Image"));

      set.Add(FileSaveProcedure("file_wbmp_save",
				"Saves wbmp images",
				"This plug-in saves wbmp images.",
				"Maurits Rijk",
				"(C) Maurits Rijk",
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
	      return null;
	    }

	  byte header = reader.ReadByte();
	  if (header != 0)
	    {
	      Console.WriteLine("Fixed header should be zero!");
	      return null;
	    }

	  byte width = reader.ReadByte();
	  byte height = reader.ReadByte();

	  // Fix me: check high bit here for larger sizes

	  Image image = new Image(width, height,
				  ImageBaseType.GRAY);

	  Layer layer = new Layer(image, "Background", width, height,
				  ImageType.GRAY, 100, 
				  LayerModeEffects.NORMAL);
	  image.AddLayer(layer, 0);
 
	  image.Filename = filename;

	  PixelRgn rgn = new PixelRgn(layer, 0, 0, width, height, true, false);
	  byte[] buf = new byte[width * height];
	  int bufp = 0;

	  for (int row = 0; row < height; row++) 
	    {
	      byte[] src = reader.ReadBytes((width + 7) / 8);

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

      if (width > 127 || height > 127)
	{
	  Console.WriteLine("width > 127 or height > 127 not supported yet!");
	  return false;
	}

      // Convert image to B&W picture
      if (!image.ConvertIndexed(ConvertDitherType.NO, 
				ConvertPaletteType.MONO,
				0, false, false, ""))
	{
	  Console.WriteLine("Conversion to B&W failed");
	  return false;
	}

      BinaryWriter writer = new BinaryWriter(File.Open(filename, 
						       FileMode.Create));

      writer.Write((byte) 0);	// Write type
      writer.Write((byte) 0);	// Fixed header
      writer.Write((byte) width);
      writer.Write((byte) height);

      writer.Close();

      return true;
    }
  }
}
