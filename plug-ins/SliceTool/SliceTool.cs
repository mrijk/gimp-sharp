using System;
using System.IO;
using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {
    MouseFunc _func;

    SliceData _sliceData = new SliceData();

    Preview _preview;
    Entry _xy;

    Entry _url;
    Entry _altText;
    Entry _target;
    CheckButton _include;
    Format _format;

    Label _left;
    Label _right;
    Label _top;
    Label _bottom;

    string _filename = null;

    [STAThread]
    static void Main(string[] args)
    {
      new SliceTool(args);
    }
		
    public SliceTool(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      InstallProcedure("plug_in_slice_tool",
		       "Slice Tool",
		       "The Image Slice Tool is used to apply image slicing and rollovers.",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2005",
		       "Slice Tool...",
		       "RGB*, GRAY*",
		       null);

      MenuRegister("<Image>/Filters/Web");
      IconRegister("SliceTool.png");
      // IconRegister("SliceTool.SliceTool.png"); // Win32
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("SliceTool", true);

      CreateStockIcons();

      Dialog dialog = DialogNew("Slice Tool 0.1", "SliceTool",
				IntPtr.Zero, 0, null, "SliceTool",
				Stock.SaveAs, (Gtk.ResponseType) 0,
				Stock.Save, (Gtk.ResponseType) 1,
				Stock.Close, ResponseType.Close);

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      HBox hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);

      Widget toolbar = CreateToolbar();
      hbox.PackStart(toolbar, false, true, 0);

      Widget preview = CreatePreview();
      hbox.PackStart(preview, true, true, 0);

      // Create coordinates
      _xy = new Entry();
      _xy.Editable = false;
      vbox.Add(_xy);

      hbox = new HBox(false, 24);
      vbox.PackStart(hbox, true, true, 0);

      Widget properties = CreateCellProperties();
      hbox.PackStart(properties, false, true, 0);

      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);

      Widget rollover = CreateRollover();
      vbox.PackStart(rollover, false, true, 0);

      _format = new Format();
      _format.Extension = System.IO.Path.GetExtension(_image.Name).ToLower();
      vbox.PackStart(_format, false, true, 0);

      _sliceData.Init(_drawable);

      _func = new SelectFunc(this, _sliceData, _preview);

      dialog.ShowAll();
      return DialogRun();
    }

    void SaveBlank(string path)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream input = assembly.GetManifestResourceStream("blank.png");
      BinaryReader reader = new BinaryReader(input);
      byte[] buffer = reader.ReadBytes((int) input.Length);
      FileStream fs = new FileStream(path + "/blank.png", FileMode.Create, 
				     FileAccess.Write);
      BinaryWriter writer = new BinaryWriter(fs);
      writer.Write(buffer);
      writer.Close();
    }

    void Save()
    {
      SetRectangleData(_sliceData.Selected);
      _sliceData.Save(_filename, _format.Extension, _image, _drawable);
      SaveBlank(System.IO.Path.GetDirectoryName(_filename));
    }

    override protected void DialogRun(ResponseType type)
    {
      if ((int) type == 0 || ((int) type == 1 && _filename == null))
	{
	FileSelection fs = new FileSelection("HTML Save As");
	fs.Response += new ResponseHandler (OnFileSelectionResponse);
	fs.Run();
	fs.Hide();
	}
      else // type == 1
	{
	Save();
	}
    }

    void OnFileSelectionResponse (object o, ResponseArgs args)
    {
      if (args.ResponseId == ResponseType.Ok)
	{
	_filename = (o as FileSelection).Filename;
	Save();
	}
    }

    Widget CreatePreview()
    {
      ScrolledWindow window = new ScrolledWindow();
      window.SetSizeRequest(600, 400);

      _preview = new Preview(_drawable, this);
      _preview.WidthRequest = _drawable.Width;
      _preview.HeightRequest = _drawable.Height;
      _preview.ButtonPressEvent += new ButtonPressEventHandler(OnButtonPress);
      
      //      _preview.MotionNotifyEvent +=
      // 	new MotionNotifyEventHandler(OnShowCoordinates);

      window.AddWithViewport(_preview);

      return window;
    }

    void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      _func.OnButtonPress(o, args);
    }

    Widget CreateToolbar()
    {
      HandleBox handle = new HandleBox();

      Toolbar tools = new Toolbar();
      tools.Orientation = Gtk.Orientation.Vertical;
      tools.ToolbarStyle = Gtk.ToolbarStyle.Icons;
      handle.Add(tools);

      Button button = new Button();
      Gtk.Image image = new Gtk.Image("slice-tool-arrow", 
				      IconSize.SmallToolbar);
      button.Add(image);
      tools.AppendWidget(button, "Select Rectangle", "arrow");
      button.Clicked += new EventHandler(OnSelect);

      button = new Button();
      image = new Gtk.Image(GimpStock.TOOL_CROP, IconSize.SmallToolbar);
      button.Add(image);
      tools.AppendWidget(button, "Create a new Slice", "create");
      button.Clicked += new EventHandler(OnCreateSlice);

      button = new Button();
      image = new Gtk.Image(GimpStock.TOOL_ERASER, IconSize.SmallToolbar);
      button.Add(image);
      tools.AppendWidget(button, "Remove Slice", "delete");
      button.Clicked += new EventHandler(OnRemoveSlice);

      button = new Button();
      image = new Gtk.Image(GimpStock.GRID, IconSize.SmallToolbar);
      button.Add(image);
      tools.AppendWidget(button, "Insert Table", "grid");
      button.Clicked += new EventHandler(OnCreateTable);

      return handle;
    }

    Widget CreateCellProperties()
    {
      GimpFrame frame = new GimpFrame("Cell Properties");
      GimpTable table = new GimpTable(5, 4, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;

      frame.Add(table);

      _url = new Entry();
      table.AttachAligned(0, 0, "URL:", 0.0, 0.5, _url, 3, false);

      _altText = new Entry();
      table.AttachAligned(0, 1, "Alt text:", 0.0, 0.5, _altText, 3, false);

      _target = new Entry();
      table.AttachAligned(0, 2, "Target:", 0.0, 0.5, _target, 3, false);

      _left = new Label("    ");
      table.AttachAligned(0, 3, "Left:", 0.0, 0.5, _left, 1, false);

      _right = new Label("    ");
      table.AttachAligned(0, 4, "Right:", 0.0, 0.5, _right, 1, false);

      _top = new Label("    ");
      table.AttachAligned(2, 3, "Top:", 0.0, 0.5, _top, 1, false);

      _bottom = new Label("    ");
      table.AttachAligned(2, 4, "Bottom:", 0.0, 0.5, _bottom, 1, false);

      _include = new CheckButton("_Include cell in table");
      _include.Active = true;
      table.Attach(_include, 0, 2, 5, 6);

      return frame;
    }

    Widget CreateRollover()
    {
      GimpFrame frame = new GimpFrame("Rollovers");

      VBox vbox = new VBox(false, 12);
      frame.Add(vbox);

      Button button = new Button("Rollover Creator...");
      vbox.Add(button);

      Label label = new Label("Rollover enabled: no");
      vbox.Add(label);

      return frame;
    }

    void AddStockIcon(IconFactory factory, string stockId, string filename)
    {
      Pixbuf pixbuf = LoadImage(filename);

      IconSource source = new IconSource();
      source.Pixbuf = pixbuf;
      source.SizeWildcarded = true;
      source.Size = IconSize.SmallToolbar;

      IconSet set = new IconSet();
      set.AddSource(source);
      source.Free();

      factory.Add(stockId, set);
      set.Unref();
    }

    void CreateStockIcons()
    {
      IconFactory factory = new IconFactory();
      factory.AddDefault();
      AddStockIcon(factory, "slice-tool-arrow", "stock-arrow.png");
    }

    void Redraw()
    {
      _preview.QueueDraw();
    }

    public void Redraw(PreviewRenderer renderer)
    {
      _sliceData.Draw(renderer);
    }

    public void SetRectangleData(Rectangle rectangle)
    {
      if (rectangle != null)
	{
	rectangle.URL = _url.Text;
	rectangle.AltText = _altText.Text;
	rectangle.Target = _target.Text;
	rectangle.Include = _include.Active;
	}
    }

    public void GetRectangleData(Rectangle rectangle)
    {
      _url.Text = rectangle.URL;
      _altText.Text = rectangle.AltText;
      _target.Text = rectangle.Target;
      _include.Active = rectangle.Include;		

      _left.Text = rectangle.X1.ToString();
      _right.Text = rectangle.X2.ToString();
      _top.Text = rectangle.Y1.ToString();
      _bottom.Text = rectangle.Y2.ToString();
    }

    void OnSelect(object o, EventArgs args)
    {
      _func = new SelectFunc(this, _sliceData, _preview);
    }

    void OnCreateSlice(object o, EventArgs args)
    {
      _func = new CreateFunc(_sliceData, _preview);
    }

    void OnRemoveSlice(object o, EventArgs args)
    {
      _func = new RemoveFunc(_sliceData, _preview);
    }

    void OnCreateTable(object o, EventArgs args)
    {
      _func = new CreateTableFunc(_sliceData, _preview);
    }

    void OnShowCoordinates(object o, MotionNotifyEventArgs args)
    {
      int x, y;
      EventMotion ev = args.Event;
      
      if (ev.IsHint) 
	{
	ModifierType s;
	ev.Window.GetPointer (out x, out y, out s);
	} 
      else 
	{
	x = (int) ev.X;
	y = (int) ev.Y;
	}
      
      _xy.Text = "x: " + x + ", y: " + y;
      args.RetVal = true;
    }

    override protected void DoSomething(Image image, Drawable drawable)
    {
      // Fix me. Only used to fill in _image and _drawable;
    }
  }
  }
