using System;
using System.IO;

using Gimp;

namespace Gimp.KoalaPaint
{	
  class KoalaPaint : FilePlugin
  {
    const int KOALA_WIDTH = 320;
    const int KOALA_HEIGHT = 200;

    byte[] _mcolor;
    byte[] _color;
    byte _background;

    byte[] _colormap = new byte[]
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
      byte[] bitmap;

      if (File.Exists(filename))
	{
	BinaryReader reader = new BinaryReader(File.Open(filename, 
							 FileMode.Open));
	
	reader.ReadBytes(2);
	bitmap = reader.ReadBytes(8000);
	_mcolor = reader.ReadBytes(1000);
	_color = reader.ReadBytes(1000);
	_background = reader.ReadByte();

	Image image = new Image(KOALA_WIDTH, KOALA_HEIGHT, 
				ImageBaseType.INDEXED);

	Layer layer = new Layer(image, "Background", KOALA_WIDTH, 
				KOALA_HEIGHT, ImageType.INDEXED, 100, 
				LayerModeEffects.NORMAL);
	image.AddLayer(layer, 0);
 
	image.Filename = filename;
	image.Colormap = _colormap;

	PixelRgn rgn = new PixelRgn(layer, 0, 0, KOALA_WIDTH, KOALA_HEIGHT, 
				    true, false);
	byte[] buf = new byte[KOALA_WIDTH * KOALA_HEIGHT];
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
	layer.Flush();

	return image;
	}
      return null;
    }

    byte GetColor(int row, int col, int index)
    {
      if (index == 0)
	return LowNibble(_background);
      else if (index == 1)
	return HighNibble(_mcolor[row * KOALA_WIDTH / 8 + col]);
      else if (index == 2)
	return LowNibble(_mcolor[row * KOALA_WIDTH / 8 + col]);
      else
	return LowNibble(_color[row * KOALA_WIDTH / 8 + col]);
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
