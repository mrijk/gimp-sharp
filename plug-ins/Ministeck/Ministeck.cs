using System;
using System.Collections;

using Gtk;

namespace Gimp.Ministeck
  {
    public class Ministeck : Plugin
    {
      GimpColorButton _colorButton;
      DrawablePreview _preview;

      [SaveAttribute]
      int _size = 16;
      [SaveAttribute]
      RGB _color = new RGB(0, 0, 0);

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
	InstallProcedure("plug_in_ministeck",
			 "Generates Ministeck",
			 "Generates Ministeck",
			 "Maurits Rijk",
			 "(C) Maurits Rijk",
			 "2004",
			 "Ministeck...",
			 "RGB*, GRAY*",
			 null);

	MenuRegister("<Image>/Filters/Artistic");
	IconRegister("Ministeck.png");
      }

      override protected bool CreateDialog()
      {
	gimp_ui_init("ministeck", true);

	Dialog dialog = DialogNew("Ministeck", "ministeck",
				  IntPtr.Zero, 0, null, "ministeck");
	
	VBox vbox = new VBox(false, 12);
	vbox.BorderWidth = 12;
	dialog.VBox.PackStart(vbox, true, true, 0);

	_preview = new DrawablePreview(_drawable, false);
	_preview.Invalidated += new EventHandler(UpdatePreview);
	vbox.PackStart(_preview, true, true, 0);

	GimpTable table = new GimpTable(2, 2, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	vbox.PackStart(table, false, false, 0);

	SpinButton size = new SpinButton(3, 100, 1);
	size.Value = _size;
	table.AttachAligned(0, 0, "_Size:", 0.0, 0.5, size, 2, true);
	size.ValueChanged += new EventHandler(SizeChanged);

	_colorButton = new GimpColorButton(
	  "", 16, 16, _color.GimpRGB, ColorAreaType.COLOR_AREA_FLAT);
	_colorButton.Update = true;
	_colorButton.ColorChanged += new EventHandler(ColorChanged);
	table.AttachAligned(0, 1, "C_olor:", 0.0, 0.5, _colorButton, 1, true);

	dialog.ShowAll();
	return DialogRun();
      }

      void ColorChanged(object sender, EventArgs e)
      {
	_color = (sender as GimpColorButton).Color;
      }

      void SizeChanged(object sender, EventArgs e)
      {
	_size = (sender as SpinButton).ValueAsInt;
	_preview.Invalidate();
      }

      void UpdatePreview(object sender, EventArgs e)
      {
	int x, y, width, height;
 	
	_preview.GetPosition(out x, out y);
	_preview.GetSize(out width, out height);
	Image clone = new Image(_image);
	clone.Crop(width, height, x, y);

	RenderMinisteck(clone, clone.ActiveDrawable, true);
	PixelRgn rgn = new PixelRgn(clone.ActiveDrawable, 0, 0, width, height, 
				    false, false);
	_preview.DrawRegion(rgn);
	
	clone.Delete();
      }

      void RenderMinisteck(Image image, Drawable drawable, bool preview)
      {
	image.UndoGroupStart();
	RunProcedure("plug_in_pixelize", image, drawable, _size);

	MinisteckPalette palette = new MinisteckPalette();
	image.ConvertIndexed(ConvertDitherType.NO_DITHER, 
			     ConvertPaletteType.CUSTOM,
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

	Progress progress = null;
	if (!preview)
	  progress = new Progress("Ministeck...");

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
	  if (!preview)
	    progress.Update((double) y / height);
	  }
	
	painter.Destroy();

	// foreach (Shape shape in shapes)
	//   Console.WriteLine(shape._match);
	
	drawable.Flush();
	drawable.Update(0, 0, drawable.Width, drawable.Height);

	if (!preview)
	  Display.DisplaysFlush();
      }

      override protected void DoSomething(Image image, Drawable drawable)
      {
	RenderMinisteck(image, drawable, false);
      }
    }
}
