using System;
using System.IO;

using Gimp;

namespace Gimp.KoalaPaint
{	
  class KoalaPaint : FilePlugin
  {
    [STAThread]
    static void Main(string[] args)
    {
      new KoalaPaint(args);
    }

    public KoalaPaint(string[] args) : base(args)
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

      InstallProcedure("file_koala_paint_load",
		       "loads images of the Koala Paint file format",
		       "This plug-in loads images of the Koala Paint file format.",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "1999 - 2004",
		       "KoalaPaint Image",
		       null,
		       load_args,
		       load_return_vals);

      Gimp.RegisterLoadHandler("file_koala_paint_load", "koa", "");
    }

    override protected Image Load(string filename)
    {
      byte[] colormap = new byte[]
	{
	  0x00, 0x00, 0x00,
	  0xff, 0xff, 0xff,
	  0x88, 0x00, 0x00,
	  0xaa, 0xff, 0xee,
	  0xcc, 0x44, 0xcc,
	  0x00, 0xcc, 0x55,
	  0x00, 0x00, 0xaa,
	  0xee, 0xee, 0x77,
	  0xdd, 0x88, 0x55,
	  0x66, 0x44, 0x00,
	  0xff, 0x77, 0x77,
	  0x33, 0x33, 0x33,
	  0x77, 0x77, 0x77,
	  0xaa, 0xff, 0x66,
	  0x00, 0x88, 0xff,
	  0xbb, 0xbb, 0xbb
	};

      Console.WriteLine("File: " + filename);

      Image image = new Image(320, 200, ImageBaseType.RGB);
      image.Colormap = colormap;

      Layer layer = new Layer(image, "Background", 320, 200, 
			      ImageType.INDEXED_IMAGE, 100, 
			      LayerModeEffects.NORMAL_MODE);
      image.AddLayer(layer, 0);

      image.Filename = filename;

      return image;
    }
  }
  }
