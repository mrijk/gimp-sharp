using System;
using System.Collections;

using Gtk;

namespace Gimp
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
			 "Maurits Rijk",
			 "2004",
			 "Ministeck...",
			 "RGB*, GRAY*",
			 args);

	MenuRegister("plug_in_ministeck",
		     "<Image>/Filters/Artistic");
      }

      override protected bool CreateDialog()
      {
	gimp_ui_init("ncp", true);

	Dialog dialog = DialogNew("ministeck", "ministeck",
				  IntPtr.Zero, 0, null, "Ministeck", 
				  Stock.Cancel, ResponseType.Cancel,
				  Stock.Ok, ResponseType.Ok);
			       
	dialog.ShowAll();
	return DialogRun();
      }

      override protected void DoSomething(Drawable drawable,
					  Image image)
      {
	// First apply Pixelize plug-in

	// Next convert to indexed
	image.ConvertIndexed(ConvertDitherType.NO_DITHER, 
			     ConvertPaletteType.CUSTOM_PALETTE, 
			     0, false, false, "Default");

	// And finally calculate the Ministeck pieces

	Display.DisplaysFlush();
      }
    }
}
