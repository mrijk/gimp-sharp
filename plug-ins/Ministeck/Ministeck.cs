using System;
using System.Collections;

using Gtk;

namespace Gimp.Ministeck
  {
    public class Ministeck : Plugin
    {
      GimpColorButton _colorButton;

      [SaveAttribute]
      int _size = 16;

      // [SaveAttribute]
      RGB _color;

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

	GimpTable table = new GimpTable(2, 2, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	vbox.PackStart(table, false, false, 0);

	SpinButton size = new SpinButton(3, 100, 1);
	size.Value = _size;
	table.AttachAligned(0, 0, "_Size:", 0.0, 0.5, size, 2, true);
	size.ValueChanged += SizeChanged;

	RGB rgb = new RGB(0, 0, 0);

	_colorButton = new GimpColorButton(
	  "", 16, 16, rgb.GimpRGB, ColorAreaType.COLOR_AREA_FLAT);
	table.AttachAligned(0, 1, "C_olor:", 0.0, 0.5, _colorButton, 1, true);

	dialog.ShowAll();
	return DialogRun();
      }

      void SizeChanged(object sender, EventArgs e)
      {
	_size = (sender as SpinButton).ValueAsInt;
      }

      override protected void GetParameters()
      {
	_color = _colorButton.Color;
      }

      override protected void DoSomething(Drawable drawable, Image image)
      {
	image.UndoGroupStart();

	MinisteckPalette palette = new MinisteckPalette();

	RunProcedure("plug_in_pixelize", _size);

	image.ConvertIndexed(ConvertDitherType.NO_DITHER, 
			     ConvertPaletteType.CUSTOM_PALETTE, 
			     0, false, false, "Ministeck");
	palette.Delete();
	image.ConvertRgb();
	image.UndoGroupEnd();

	// And finally calculate the Ministeck pieces
	
	Random random = new Random();
	int width = drawable.Width / _size;
	int height = drawable.Height / _size;

	Painter painter = new Painter(drawable, _size, _color);
	Shape.Painter = painter;

	bool[,] A = new bool[width, height];
	Array.Clear(A, 0, width * height);

	// Fill in shapes
	
	ArrayList shapes = new ArrayList();
	shapes.Add(new TwoByTwoShape());
	shapes.Add(new ThreeByOneShape());
	shapes.Add(new TwoByOneShape());
	shapes.Add(new CornerShape());
	shapes.Add(new OneByOneShape());

	for (int y = 0; y < height; y++)
	  {
	  for (int x = 0; x < width; x++)
	    {
	    if (!A[x, y])
	      {
	      ArrayList copy = (ArrayList) shapes.Clone();
	      while (copy.Count > 0)
		{
		int index = random.Next(copy.Count - 1);
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
	
	painter.Destroy();

	// foreach (Shape shape in shapes)
	//   Console.WriteLine(shape._match);
	
	drawable.Flush();
	drawable.Update(0, 0, drawable.Width, drawable.Height);

	Display.DisplaysFlush();
      }
    }
}
