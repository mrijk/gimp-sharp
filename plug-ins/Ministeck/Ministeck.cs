using System;
using System.Collections;

using Gimp;
using Gtk;

namespace Ministeck
  {
    public class Ministeck : Plugin
    {
      [STAThread]
      static void Main(string[] args)
      {
	Ministeck plugin = new Ministeck(args);
      }

      public Ministeck(string[] args) : base(args)
      {
      }

      override protected void Query()
      {
	GimpParamDef[] args = new GimpParamDef[3];

	args[0].type = PDBArgType.INT32;
	args[0].name = "run_mode";
	args[0].description = "Interactive, non-interactive";

	args[1].type = PDBArgType.IMAGE;
	args[1].name = "image";
	args[1].description = "Input image";

	args[2].type = PDBArgType.DRAWABLE;
	args[2].name = "drawable";
	args[2].description = "Input drawable";

	InstallProcedure("plug_in_ministeck",
			 "Generates Ministeck",
			 "Generates Ministeck",
			 "Maurits Rijk",
			 "(C) Maurits Rijk",
			 "2004",
			 "Ministeck...",
			 "RGB*",
			 args);

	MenuRegister("plug_in_ministeck", "<Image>/Filters/Artistic");
      }

      override protected bool CreateDialog()
      {
	gimp_ui_init("ministeck", true);

	Dialog dialog = DialogNew("Ministeck", "ministeck",
				  IntPtr.Zero, 0, null, "ministeck");
	
	VBox vbox = new VBox(false, 12);
	vbox.BorderWidth = 12;
	dialog.VBox.PackStart(vbox, true, true, 0);

	GimpTable table = new GimpTable(4, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	vbox.PackStart(table, false, false, 0);

	SpinButton size = new SpinButton(3, 100, 1);
	table.AttachAligned(0, 0, "_Size", 0.0, 0.5, size, 2, true);

	dialog.ShowAll();
	return DialogRun();
      }

      override protected void DoSomething(Drawable drawable,
					  Gimp.Image image)
      {
	image.UndoGroupStart();

	CreatePalette();

	// First apply Pixelize plug-in
	RunProcedure("plug_in_pixelize", 16);

	// Next convert to indexed
	image.ConvertIndexed(ConvertDitherType.NO_DITHER, 
			     ConvertPaletteType.CUSTOM_PALETTE, 
			     0, false, false, "Ministeck");
	DeletePalette();
	image.ConvertRgb();
	image.UndoGroupEnd();

	// And finally calculate the Ministeck pieces
	
	Random random = new Random();
	int width = _drawable.Width / 16;
	int height = _drawable.Height / 16;

	PixelRgn srcPR = new PixelRgn(_drawable, 0, 0, 
				      _drawable.Width, _drawable.Height,
				      false, false);
	int[,] A = new int[width, height];
	byte[] buf = new byte[4];

	for (int i = 0; i < width; i++)
	  {
	  for (int j = 0; j < height; j++)
	    {
	    srcPR.GetPixel(buf, i * 16, j * 16);
	    A[i, j] = buf[0];
	    }
	  }

	// Fill in shapes
	
	ArrayList shapes = new ArrayList();
	shapes.Add(new TwoByTwoShape());
	shapes.Add(new ThreeByOneShape());
	shapes.Add(new TwoByOneShape());
	shapes.Add(new CornerShape());
	shapes.Add(new OneByOneShape());

	for (int x = 0; x < width; x++)
	  {
	  for (int y = 0; y < height; y++)
	    {
	    if (A[x, y] > 0)
	      {
	      ArrayList copy = (ArrayList) shapes.Clone();
	      while (copy.Count > 0)
		{
		int index = random.Next(copy.Count);
		Shape shape = (Shape) copy[index];
		if (shape.Fits(A, x, y))
		  {
		  break;
		  }
		copy.RemoveAt(index);
		}
	      }
	    }
	  }

	// foreach (Shape shape in shapes)
	//  Console.WriteLine(shape._match);

	Display.DisplaysFlush();
      }

      Palette _palette;

      void CreatePalette()
      {
	_palette = new Palette("Ministeck");

	_palette.AddEntry("", new RGB(253, 254, 253));
	_palette.AddEntry("", new RGB(206, 153,  50));
	_palette.AddEntry("", new RGB(155, 101,  52));
	_palette.AddEntry("", new RGB( 50,  50,  50));
	_palette.AddEntry("", new RGB(  4,   3,  98));
	_palette.AddEntry("", new RGB(  2, 102,  54));

	_palette.AddEntry("", new RGB(  2,  50, 154));
	_palette.AddEntry("", new RGB(254,  50, 102));
	_palette.AddEntry("", new RGB(206, 154, 102));
	_palette.AddEntry("", new RGB(254, 254,  50));
	_palette.AddEntry("", new RGB(250,  90,   6));
	_palette.AddEntry("", new RGB( 55, 101,  53));

	_palette.AddEntry("", new RGB(103, 102, 101));
	_palette.AddEntry("", new RGB(206,   2,  50));
	_palette.AddEntry("", new RGB(254, 154,  54));
	_palette.AddEntry("", new RGB(102,  50,  50));
	_palette.AddEntry("", new RGB(253, 154, 154));
	_palette.AddEntry("", new RGB( 50, 102, 206));

	_palette.AddEntry("", new RGB(  3,  50,  56));
	_palette.AddEntry("", new RGB( 50,   2, 102));
	_palette.AddEntry("", new RGB(251, 155, 101));
	_palette.AddEntry("", new RGB(254, 254, 202));
	_palette.AddEntry("", new RGB(  4,   2,   2));
	_palette.AddEntry("", new RGB(206, 206, 206));
      }
      
      void DeletePalette()
      {
	_palette.Delete();
      }
    }
}
