using System;
// using System.Collections;
// using System.IO;

using Gtk;

namespace Gimp
  {
    public class PicturePackage : Plugin
    {
      GimpParam[] values = new GimpParam[1];

      [STAThread]
      static void Main(string[] args)
      {
	PicturePackage plugin = new PicturePackage(args);
      }

      public PicturePackage(string[] args) : base(args)
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
	args[1].description = "Input image (unused)";

	args[2].type = PDBArgType.DRAWABLE;
	args[2].name = "drawable";
	args[2].description = "Input drawable";

	InstallProcedure("plug_in_picture_package",
			 "Picture package",
			 "Picture package",
			 "Maurits Rijk",
			 "Maurits Rijk",
			 "Today",
			 "Picture Package...",
			 "RGB*, GRAY*",
			 args);

	MenuRegister("plug_in_picture_package",
		     "<Image>/Filters/Render");
      }

      override protected void Run(string name, GimpParam[] param,
				  out GimpParam[] return_vals)
      {
	values[0].type = PDBArgType.STATUS;
	values[0].data.d_status = PDBStatusType.PDB_SUCCESS;
	return_vals = values;

	gimp_ui_init("ncp", true);

	IntPtr dialogPtr = DialogNew("PicturePackage", "PicturePackage",
				     IntPtr.Zero, 0, null, "PicturePackage", 
				     Stock.Cancel, ResponseType.Cancel,
				     Stock.Ok, ResponseType.Ok);

	Dialog dialog = new Dialog(dialogPtr);
	dialog.Show();
	DialogRun(dialogPtr);
      }

      override protected void DoSomething()
      {
      }
    }
  }
