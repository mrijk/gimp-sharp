using System;
using System.Xml;

using Gtk;

namespace Gimp
  {
    public class PicturePackage : Plugin
    {
      GimpParam[] values = new GimpParam[1];
      LayoutSet _layoutSet = new LayoutSet();

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

	ReadConfiguration();

	Dialog dialog = DialogNew("Picture Package", "PicturePackage",
				  IntPtr.Zero, 0, null, "PicturePackage", 
				  Stock.Cancel, ResponseType.Cancel,
				  Stock.Ok, ResponseType.Ok);

	HBox hbox = new HBox(false, 12);
	hbox.BorderWidth = 12;
	dialog.VBox.PackStart(hbox, true, true, 0);

	VBox vbox = new VBox(false, 12);
	hbox.PackStart(vbox, true, true, 0);

	BuildSourceFrame(vbox);
	BuildDocumentFrame(vbox);
	BuildLabelFrame(vbox);

	Frame frame = new Frame();
	hbox.PackStart(frame, true, true, 0);

	Preview preview = new Preview();
	preview.WidthRequest = 240;
	preview.Layout = _layoutSet.GetLayout(1);		// Fix me!
	// preview.HeightRequest = 300;
	frame.Add(preview);

	dialog.ShowAll();

	DialogRun();
	SetData();
      }

      void ReadConfiguration()
      {
	XmlDocument doc = new XmlDocument();
	doc.Load(GimpDirectory() + "/plug-ins/picture-package.xml");

	XmlNodeList nodeList;
	XmlElement root = doc.DocumentElement;

	nodeList = root.SelectNodes("/picture-package/layout");

	foreach (XmlNode layout in nodeList)
	  {
	  XmlAttributeCollection attributes = layout.Attributes;
	  XmlAttribute name = (XmlAttribute) attributes.GetNamedItem("name");
	  _layoutSet.Add(new Layout(layout));
	  }
      }

      void BuildSourceFrame(VBox vbox)
      {
	GimpFrame frame = new GimpFrame("Source");
	vbox.PackStart(frame, true, true, 0);

	GimpTable table = new GimpTable(2, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	frame.Add(table);

	OptionMenu use = new OptionMenu();
	Menu menu = new Menu();
	menu.Append(new MenuItem("File"));
	menu.Append(new MenuItem("Folder"));
	menu.Append(new MenuItem("Frontmost Document"));
	use.Menu = menu;
	table.AttachAligned(0, 0, "Use:", 0.0, 0.5,
			    use, 1, false);

	CheckButton include = new CheckButton("Include All Subfolders");
	table.Attach(include, 1, 2, 1, 2);

	Button choose = new Button("Choose...");
	table.Attach(choose, 1, 2, 2, 3, AttachOptions.Shrink,
		     AttachOptions.Fill, 0, 0);	
      }

      void BuildDocumentFrame(VBox vbox)
      {
	Frame frame = new GimpFrame("Document");
	vbox.PackStart(frame, true, true, 0);

	GimpTable table = new GimpTable(5, 3, false);
	table.ColumnSpacing = 6;
	table.RowSpacing = 6;
	frame.Add(table);

	OptionMenu size = new OptionMenu();
	Menu menu = new Menu();
	menu.Append(new MenuItem("8.0 x 10.0 inches"));
	size.Menu = menu;
	table.AttachAligned(0, 0, "Page Size:", 0.0, 0.5,
			    size, 2, false);

	OptionMenu layout = new OptionMenu();
	menu = new Menu();
	foreach (Layout l in _layoutSet)
	    {
		menu.Append(new MenuItem(l.Name));
	    }
	layout.Menu = menu;
	table.AttachAligned(0, 1, "Layout:", 0.0, 0.5,
			    layout, 2, false);

	Entry resolution = new Entry();
	resolution.WidthChars = 4;
	table.AttachAligned(0, 2, "Resolution:", 0.0, 0.5,
			    resolution, 1, true);
	
	OptionMenu units = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("pixels/inch"));
	menu.Append(new MenuItem("pixels/cm"));
	menu.Append(new MenuItem("pixels/mm"));
	units.Menu = menu;
	table.Attach(units, 2, 3, 2, 3);	

	OptionMenu mode = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("Grayscale"));
	menu.Append(new MenuItem("RGB Color"));
	mode.Menu = menu;
	table.AttachAligned(0, 3, "Mode:", 0.0, 0.5,
			    mode, 2, false);

	CheckButton flatten = new CheckButton("Flatten All Layers");
	table.Attach(flatten, 0, 2, 4, 5);

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
	menu.Append(new MenuItem("Custom Text"));
	menu.Append(new MenuItem("Filename"));
	menu.Append(new MenuItem("Copyright"));
	menu.Append(new MenuItem("Caption"));
	menu.Append(new MenuItem("Credits"));
	menu.Append(new MenuItem("Title"));
	content.Menu = menu;
	table.AttachAligned(0, 0, "Content:", 0.0, 0.5,
			    content, 1, false);

	Entry entry = new Entry();
	table.AttachAligned(0, 1, "Custom Text:", 0.0, 0.5,
			    entry, 1, true);
#if false
	GimpFontSelectWidget font = new GimpFontSelectWidget(null, 
							     "Monospace");
	table.AttachAligned(0, 2, "Font:", 0.0, 0.5,
			    font, 1, true);
#endif
	OptionMenu position = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("Centered"));
	menu.Append(new MenuItem("Top Left"));
	menu.Append(new MenuItem("Bottom Left"));
	menu.Append(new MenuItem("Top Right"));
	menu.Append(new MenuItem("Bottom Right"));
	position.Menu = menu;
	table.AttachAligned(0, 3, "Position:", 0.0, 0.5,
			    position, 1, false);

	OptionMenu rotate = new OptionMenu();
	menu = new Menu();
	menu.Append(new MenuItem("None"));
	menu.Append(new MenuItem("45 Degrees Right"));
	menu.Append(new MenuItem("90 Degrees Right"));
	menu.Append(new MenuItem("45 Degrees Left"));
	menu.Append(new MenuItem("90 Degrees Left"));
	rotate.Menu = menu;
	table.AttachAligned(0, 4, "Rotate:", 0.0, 0.5,
			    rotate, 1, false);

      }

      override protected void DoSomething()
      {
      }
    }
  }
