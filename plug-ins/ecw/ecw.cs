// The ecw plug-in
// Copyright (C) 2006-2009 Maurits Rijk
//
// ecw.cs
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
using System.Runtime.InteropServices;

namespace Gimp.ecw
{	
  class ecw : FilePlugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new ecw(args);
    }

    public ecw(string[] args) : base(args, "ecw")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return FileLoadProcedure("file_ecw_load",
				     "Loads ecw images",
				     "This plug-in loads ECW images.",
				     "Maurits Rijk",
				     "(C) Maurits Rijk",
				     "2006-2009",
				     "ecw Image");
      
      yield return FileSaveProcedure("file_ecw_save",
				     "Saves ecw images",
				     "This plug-in saves ECW images.",
				     "Maurits Rijk",
				     "(C) Maurits Rijk",
				     "2006-2009",
				     "ecw Image",
				     "RGB*");
    }

    override protected void Query()
    {
      base.Query();
      RegisterLoadHandler("ecw,jp2", "");
      RegisterSaveHandler("ecw", "");
    }

    override protected Image Load(string filename)
    {
      if (File.Exists(filename))
	{
	  ecw_wrapper_init();

	  FileView view = FileView.Open(filename);
	  view.Set();

	  FileViewInfo info = view.Info;
	  int width = (int) (info.SizeX - 1);
	  int height = (int) (info.SizeY - 1);

	  Image image = NewImage(width, height, ImageBaseType.Rgb,
				 ImageType.Rgb, filename);

	  PixelRgn rgn = new PixelRgn(image.Layers[0], true, false);

	  byte[] line = new byte[width * 3];

	  for (int y = 0; y < height; y++)
	    {
	      view.ReadLineRGB(line);
	      rgn.SetRow(line, 0, y, width);
	    }

	  view.Close();

	  return image;
	}
      return null;
    }

    override protected bool Save(Image image, Drawable drawable, 
				 string filename)
    {
      BinaryWriter writer = new BinaryWriter(File.Open(filename, 
						       FileMode.Create));

      PixelRgn rgn = new PixelRgn(drawable, false, false);
      for (IntPtr pr = PixelRgn.Register(rgn); pr != IntPtr.Zero; 
	   pr = PixelRgn.Process(pr))
	{
	}

      writer.Close();

      return true;
    }

    // TODO: fix mappings from .so to .dll
    [DllImport("ecwwrapper.so")]
    static extern void ecw_wrapper_init();

    // [DllImport("libNCSUtil.so")]
    // static extern IntPtr NCSGetErrorText(int error);
  }
}
