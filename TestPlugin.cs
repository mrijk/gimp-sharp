using System;
using System.Collections;

using Gtk;
using GtkSharp;

namespace Gimp
  {
    public class TestPlugin : Plugin
    {
      [STAThread]
      static void Main(string[] args)
      {
	TestPlugin plugin = new TestPlugin(args);
      }

      public TestPlugin(string[] args) : base(args)
      {
      }

      struct Point 
      {
	public int x;
	public int y;
      }

      Drawable drawable;
      GimpParam[] values = new GimpParam[1];

      override protected void Query()
      {
	GimpParamDef[] args = new GimpParamDef[3];
	args[0].type = GimpPDBArgType.INT32;
	args[0].name = "run_mode";
	args[0].description = "Interactive, non-interactive";

	args[1].type = GimpPDBArgType.IMAGE;
	args[1].name = "image";
	args[1].description = "Input image (unused)";

	args[2].type = GimpPDBArgType.DRAWABLE;
	args[2].name = "drawable";
	args[2].description = "Input drawable";

	InstallProcedure("gimp#",
			 "blurb",
			 "help me too",
			 "Maurits Rijk",
			 "Maurits Rijk",
			 "Today",
			 "<Image>/Filters/Web/Gimp#",
			 "RGB*",
			 args);
      }

      override protected void Run(string name, GimpParam[] param,
				  out GimpParam[] return_vals)
      {
	values[0].type = GimpPDBArgType.STATUS;
	values[0].data.d_status = 3;
	return_vals = values;

	drawable = new Drawable(param[2].data.d_drawable);
	drawable.Name = "Foo";

	// image.Scale(image.Width / 2, image.Height / 2);
	// image.Crop(100, 100, 0, 0);

	// Fill area
	int x1, y1, x2, y2;
	drawable.MaskBounds(out x1, out y1, out x2, out y2);
	// drawable.Fill(FillType.PATTERN_FILL);
	//			drawable.Flush();
	//			drawable.Update(x1, y1, x2 - x1, y2 - y1);

	// Add a new layer
	//			Layer layer = new Layer(image, "test", 100, 100, ImageType.RGB_IMAGE, 1.0, LayerModeEffects.NORMAL_MODE);
	//			image.AddLayer(layer, 0);

	gimp_ui_init("gimp#", true);

	IntPtr dialogPtr = DialogNew("gimp#", "gimp#",
				     IntPtr.Zero, 0, null, "plug-in-gimp#", 
				     Stock.Cancel, ResponseType.Cancel,
				     Stock.Ok, ResponseType.Ok);

	Dialog dialog = new Dialog(dialogPtr);
	// dialog.AddButton("Dummy", 13);

	// dialog.VBox.PackStart(new Label("Bla"), true, true, 0);
	// Combo combo = new Combo();
	// hbox.Add(combo);
	// combo.Show();
	// hbox.Show();

	dialog.Show();
	DialogRun(dialogPtr);

	drawable.Detach();
      }
		
      /* override */ protected void DoSomething2()
      {
	Image image = drawable.Image;
	Image copy = image.Duplicate();
	copy.Scale(copy.Width / 2, copy.Height / 2);
	Display display = new Display(copy);

	int count = 0;
	foreach (Guide guide in image.Guides)
	  {
	  count++;
	  }

	// Write some pixel data
	RgnIterator iter = new RgnIterator(drawable);
	iter.Iterate(new RgnIterator.IterFuncSrcDest(Foo));

	// Read some pixel data
	iter.Iterate(new RgnIterator.IterFuncSrc(Foo));		
	Display.DisplaysFlush();
      }

      const int _points = 12;
      const int _closest = 1;
      const bool color = true;

      Point[,] vp;

      int[] _distances;

      int bpp;
      bool has_alpha;
      int xmid, ymid;
      int width, height;

      // Try to implement the ncp plug-in
      override protected void DoSomething()
      {
	const int _seed = 2;

	int x1, y1, x2, y2;
	drawable.MaskBounds(out x1, out y1, out x2, out y2);

	Random random = new Random(_seed);

	bpp = drawable.Bpp;
	has_alpha = drawable.HasAlpha();
	if (has_alpha)
	  bpp--;

	Console.WriteLine("bpp: " + bpp);

	width = x2 - x1;
	height = y2 - y1;

	xmid = width / 2;
	ymid = height / 2;

	_distances = new int[4 * _points];
	vp = new Point[bpp, 4 * _points];

	for (int b = 0; b < bpp; b++) 
	  {
	  for (int i = 0; i < _points; i++)
	    {
	    int px = random.Next(0, width - 1);
	    int py = random.Next(0, height - 1);
	    vp[b, i].x = px + xmid;
	    vp[b, i].y = py + ymid;
	    vp[b, i + _points].x = (px < xmid) ? (vp[b, i].x + width) 
	      : (vp[b, i].x - width);
	    vp[b, i + _points].y = vp[b, i].y;
	    vp[b, i + 2 * _points].x = vp[b, i].x;
	    vp[b, i + 2 * _points].y = (py < ymid) ? (vp[b, i].y + height) 
	      : (vp[b, i].y - height);
	    vp[b, i + 3 * _points].x = (px < xmid) ? (vp[b, i].x + width) 
	      : (vp[b, i].x - width);
	    vp[b, i + 3 * _points].y = (py < ymid) ? (vp[b, i].y + height) 
	      : (vp[b, i].y - height);
	    }
	  }
				
	RgnIterator iter = new RgnIterator(drawable);
	iter.Iterate(new RgnIterator.IterFuncDest(DoNCP));
			
	Display.DisplaysFlush();
      }

      void DoNCP(int x, int y, ref byte[] dest)
      {
	for (int b = 0; b < bpp; b++) 
	  {
#if _EMPTY_
	  /* compute distance to each point */
	  for (int k = 0; k < _points * 4; k++) 
	    {
	    int x2 = x + xmid - vp[b, k].x;
	    int y2 = y + ymid - vp[b, k].y;
	    _distances[k] = x2 * x2 + y2 * y2;
	    }
	  Array.Sort(_distances);

	  // Console.WriteLine("2b");

	  byte val = (byte) (255.0 * Math.Sqrt((double) _distances[_closest - 1] / (width * height)));
#else
	  byte val = 128;
#endif
	  /* invert */ 
	  val = (byte) (255 - val);
	  if (color) 
	    { 
	    dest[b] = val;
	    }
	  else 
	    {
	    for (int l = 0; l < bpp; l++) 
	      dest[l] = val;
	    break;
	    }
	  }
	if (has_alpha) 
	  dest[bpp]= 255;
      }

      void Foo(byte[] from)
      {
      }

      void Foo(int x, int y, byte[] from, ref byte[] to)
      {
	to[0] = from[1];
	to[1] = from[2];
	to[2] = from[0];
      }
    }
  }
