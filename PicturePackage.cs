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

	gimp_ui_init("PicturePackage", true);

	Dialog dialog = DialogNew("Picture Package", "PicturePackage",
				  IntPtr.Zero, 0, null, "PicturePackage", 
				  Stock.Cancel, ResponseType.Cancel,
				  Stock.Ok, ResponseType.Ok);

	HBox hbox = new HBox(false, 1);
	hbox.BorderWidth = 12;
	dialog.VBox.PackStart(hbox, true, true, 0);

	VBox vbox = new VBox(false, 1);
	hbox.PackStart(vbox, true, true, 0);

	BuildSourceFrame(vbox);
	BuildDocumentFrame(vbox);
	BuildLabelFrame(vbox);

	dialog.ShowAll();
	DialogRun();
      }

      void BuildSourceFrame(VBox vbox)
      {
	GimpFrame frame = new GimpFrame("Source");
	vbox.PackStart(frame, true, true, 0);
      }

      void BuildDocumentFrame(VBox vbox)
      {
	Frame frame = new GimpFrame("Document");
	vbox.PackStart(frame, true, true, 0);

	GimpTable table = new GimpTable(3, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	frame.Add(table);

	OptionMenu size = new OptionMenu();
	Menu menu = new Menu();
	menu.Append(new MenuItem("8.0 x 10.0 inches"));
	size.Menu = menu;
	table.AttachAligned(0, 0, "Page Size:", 0.0, 0.5,
			    size, 1, true);

	OptionMenu layout = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("(2)5x7)"));
	layout.Menu = menu;
	table.AttachAligned(0, 1, "Layout:", 0.0, 0.5,
			    layout, 1, true);

	OptionMenu mode = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("RGB Color"));
	mode.Menu = menu;
	table.AttachAligned(0, 2, "Mode:", 0.0, 0.5,
			    mode, 1, true);
      }

      void BuildLabelFrame(VBox vbox)
      {
	GimpFrame frame = new GimpFrame("Label");
	vbox.PackStart(frame, true, true, 0);

	GimpTable table = new GimpTable(3, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	frame.Add(table);

	OptionMenu content = new OptionMenu();
	Menu menu = new Menu();
	menu.Append(new MenuItem("None"));
	content.Menu = menu;
	table.AttachAligned(0, 0, "Content:", 0.0, 0.5,
			    content, 1, true);

	OptionMenu position = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("Centered"));
	position.Menu = menu;
	table.AttachAligned(0, 1, "Position:", 0.0, 0.5,
			    position, 1, true);

	OptionMenu rotate = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("None"));
	rotate.Menu = menu;
	table.AttachAligned(0, 2, "Rotate:", 0.0, 0.5,
			    rotate, 1, true);

      }

      override protected void DoSomething()
      {
      }
    }
  }
