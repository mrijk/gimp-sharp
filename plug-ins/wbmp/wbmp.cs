// The wbmp plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// wbmp.cs
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
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

    override protected void Query()
    {
      GimpParamDef[] load_args = new GimpParamDef[3];
      load_args[0].type = PDBArgType.INT32;
      load_args[0].name = "run_mode";
      load_args[0].description = "Interactive, non-interactive";
      load_args[1].type = PDBArgType.STRING;
      load_args[1].name = "filename";
      load_args[1].description = "The name of the file to load";
      load_args[2].type = PDBArgType.STRING;
      load_args[2].name = "raw_filename";
      load_args[2].description = "The name entered";

      GimpParamDef[] load_return_vals = new GimpParamDef[1];
      load_return_vals[0].type = PDBArgType.IMAGE;
      load_return_vals[0].name = "image";
      load_return_vals[0].description = "output image";

      InstallProcedure("file_wbmp_load",
		       "Loads wbmp images",
		       "This plug-in loads wbmp images.",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2005-2006",
		       "wbmp Image",
		       null,
		       load_args,
		       load_return_vals);

      Gimp.RegisterLoadHandler("file_wbmp_load", "wbmp", "");
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
  }
  }
