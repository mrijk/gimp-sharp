using System;
using System.IO;

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

      Drawable drawable;
      GimpParam[] values = new GimpParam[1];

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
	// Dialog dialog = new Dialog();
	dialog.AddButton("Dummy", 13);

	// dialog.VBox.PackStart(new Label("Bla"), true, true, 0);
	// stream.WriteLine("vbox: " + dialog.VBox);
	// Combo combo = new Combo();
	// hbox.Add(combo);
	// combo.Show();
	// hbox.Show();

	dialog.Show();
	DialogRun(dialogPtr);

	drawable.Detach();
      }
		
      override protected void DoSomething()
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
	Console.WriteLine("#guides: " + count);

	// Write some pixel data
	RgnIterator iter = new RgnIterator(drawable);
	iter.Iterate(new RgnIterator.IterFuncSrcDest(Foo));

	// Read some pixel data
	iter.Iterate(new RgnIterator.IterFuncSrc(Foo));		
	Display.DisplaysFlush();
      }

      void Foo(byte[] from)
      {
      }

      void Foo(byte[] from, ref byte[] to)
      {
	to[0] = from[1];
	to[1] = from[2];
	to[2] = from[0];
      }
    }
  }
