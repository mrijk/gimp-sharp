using System;
using System.IO;
using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {
    delegate void ClickHandler(int x, int y);

    RectangleSet _rectangles = new RectangleSet();
    SliceSet _horizontalSlices = new SliceSet();
    SliceSet _verticalSlices = new SliceSet();
    ClickHandler _onClick;

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

    Slice _slice;
    PreviewRenderer _renderer;
    Rectangle _rectangle;
    bool _foo;


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

      _onClick = new ClickHandler(Select);
      CreateStockIcons();

      Dialog dialog = DialogNew("Slice Tool 0.1", "SliceTool",
				IntPtr.Zero, 0, null, "SliceTool");

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
      _format.Exension = System.IO.Path.GetExtension(_image.Name).ToLower();
      vbox.PackStart(_format, false, true, 0);

      CreateInitialSlices();

      dialog.ShowAll();
      return DialogRun();
    }

    void CreateInitialSlices()
    {
      int width = _drawable.Width;
      int height = _drawable.Height;

      VerticalSlice left = new VerticalSlice(0, 0, height - 1);
      VerticalSlice right = new VerticalSlice(width - 1, 0, height - 1);
      HorizontalSlice top = new HorizontalSlice(0, width - 1, 0);
      HorizontalSlice bottom = new HorizontalSlice(0, width - 1, height - 1);
      _verticalSlices.Add(left);
      _verticalSlices.Add(right);
      _horizontalSlices.Add(top);
      _horizontalSlices.Add(bottom);
      Rectangle rectangle = new Rectangle(left, right, top, bottom);
      _rectangles.Add(rectangle);
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

    public void Redraw(PreviewRenderer renderer)
    {
      _horizontalSlices.Draw(renderer);
      _verticalSlices.Draw(renderer);
      if (_rectangle != null)
	{
	_rectangle.Draw(_preview.Renderer);
	}
    }

    void OnSelect(object o, EventArgs args)
    {
      _onClick = new ClickHandler(Select);
    }

    void SetRectangleData(Rectangle rectangle)
    {
      if (rectangle != null)
	{
	rectangle.URL = _url.Text;
	rectangle.AltText = _altText.Text;
	rectangle.Target = _target.Text;
	rectangle.Include = _include.Active;
	}
    }

    void GetRectangleData(Rectangle rectangle)
    {
      _url.Text = rectangle.URL;
      _altText.Text = rectangle.AltText;
      _target.Text = rectangle.Target;
      _include.Active = rectangle.Include;		
    }

    void Select(int x, int y)
    {
      Slice slice = _horizontalSlices.Find(x, y);
      if (slice == null)
	{
	slice = _verticalSlices.Find(x, y);
	}
      if (slice == null)
	{
	Rectangle rectangle = _rectangles.Find(x, y);
	if (rectangle != _rectangle)
	  {
	  SetRectangleData(_rectangle);
	  _rectangle = rectangle;
	  Redraw(_preview.Renderer);

	  GetRectangleData(rectangle);
	  
	  _left.Text = _rectangle.X1.ToString();
	  _right.Text = _rectangle.Y1.ToString();
	  _top.Text = _rectangle.X2.ToString();
	  _bottom.Text = _rectangle.Y2.ToString();
	  }
	}
      else
	{
	_slice = slice;
	_renderer.Function = Gdk.Function.Equiv;
	_preview.MotionNotifyEvent +=
	  new MotionNotifyEventHandler(OnMoveSlice);
	_preview.ButtonReleaseEvent += 
	  new ButtonReleaseEventHandler(OnMoveDone);
	}
    }

    void OnMoveDone(object o, ButtonReleaseEventArgs args)
    {
      _preview.MotionNotifyEvent -= new MotionNotifyEventHandler(OnMoveSlice);
      _preview.ButtonReleaseEvent -= 
	new ButtonReleaseEventHandler(OnMoveDone);
      _renderer.Function = Gdk.Function.Copy;
      _slice.Draw(_renderer);
    }

    void OnCreateSlice(object o, EventArgs args)
    {
      _onClick = new ClickHandler(CreateSlice);
    }

    void CreateSlice(int x, int y)
    {
      Console.WriteLine("CreateSlice");
      _slice = GetSlice(x, y);

      _renderer = _preview.Renderer;

      _renderer.Function = Gdk.Function.Equiv;
      _slice.Draw(_renderer);
      _preview.MotionNotifyEvent += new MotionNotifyEventHandler(OnMoveSlice);
      _preview.ButtonReleaseEvent += 
	new ButtonReleaseEventHandler(OnButtonRelease);
    }

    void OnRemoveSlice(object o, EventArgs args)
    {
      _onClick = new ClickHandler(RemoveSlice);
    }

    void RemoveSlice(int x, int y)
    {
      Console.WriteLine("RemoveSlice");
    }

    void OnCreateTable(object o, EventArgs args)
    {
      _onClick = new ClickHandler(CreateTable);
    }

    void CreateTable(int x, int y)
    {
      TableDialog dialog = new TableDialog();
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      if (type == ResponseType.Ok)
	{
	Rectangle rectangle = _rectangles.Find(x, y);
	int width = rectangle.Width;
	int height = rectangle.Height;
	int x1 = rectangle.X1;
	int x2 = rectangle.X2;
	int y1 = rectangle.Y1;
	int y2 = rectangle.Y2;

	SliceSet horizontalSlices = new SliceSet();
	for (int row = 1; row < dialog.Rows; row++)
	  {
	  int ypos = y1 + row * height / dialog.Rows;
	  horizontalSlices.Add(new HorizontalSlice(x1, x2, ypos));
	  }

	foreach (Slice slice in horizontalSlices)
	  {
	  _rectangles.Slice(slice);
	  _horizontalSlices.Add(slice);
	  }

	SliceSet verticalSlices = new SliceSet();
	for (int col = 1; col < dialog.Columns; col++)
	  {
	  int xpos = x1 + col * width / dialog.Columns;
	  verticalSlices.Add(new VerticalSlice(xpos, y1, y2));
	  }

	foreach (Slice slice in verticalSlices)
	  {
	  _rectangles.Slice(slice);
	  _verticalSlices.Add(slice);
	  }

	Redraw(_preview.Renderer);
	}
      dialog.Destroy();
    }

    Slice GetSlice(int x, int y)
    {
      Rectangle rectangle = _rectangles.Find(x, y);
      Slice slice;

      if (rectangle == _rectangle)
	{
	slice = _slice;
	}
      else
	{
	_rectangle = rectangle;
	if (_foo)
	  slice = rectangle.CreateVerticalSlice(x);
	else
	  slice = rectangle.CreateHorizontalSlice(y);
	_foo = !_foo;
	}
      return slice;
    }

    void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      int x = (int) args.Event.X;
      int y = (int) args.Event.Y;
      _onClick(x, y);
    }

    void OnButtonRelease(object o, ButtonReleaseEventArgs args)
    {
      _preview.MotionNotifyEvent -= new MotionNotifyEventHandler(OnMoveSlice);
      _preview.ButtonReleaseEvent -= 
	new ButtonReleaseEventHandler(OnButtonRelease);
      _slice.Draw(_renderer);
      if (_foo)
	_horizontalSlices.Add(_slice);
      else
	_verticalSlices.Add(_slice);
      _rectangles.Slice(_slice);
      _rectangle = null;
      _renderer.Function = Gdk.Function.Copy;
      _slice.Draw(_renderer);
    }

    void OnMoveSlice(object o, MotionNotifyEventArgs args)
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

      _slice.Draw(_renderer);
      _slice = GetSlice(x, y);
      _slice.SetPosition(x, y);
      _slice.Draw(_renderer);
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
      SetRectangleData(_rectangle);

      _horizontalSlices.Sort();
      _verticalSlices.Sort();

      string name = System.IO.Path.GetFileNameWithoutExtension(image.Name);

      FileStream fs = new FileStream("slicer.html", FileMode.Create, 
				     FileAccess.Write);
      StreamWriter w = new StreamWriter(fs);

      w.WriteLine("<html>");
      w.WriteLine("<head>");
      w.WriteLine("<meta name=\"Author\" content=\"{0}\">",
		  Environment.UserName);
      w.WriteLine("<meta name=\"Generator\" content=\"GIMP {0}\">",
		  Gimp.Version);
      w.WriteLine("<title></title>");
      w.WriteLine("</head>");
      w.WriteLine("<body");
      w.WriteLine("");
      w.WriteLine("<!-- Begin Table -->");
      w.WriteLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"{0}\">", 
		  drawable.Width);

      _rectangles.WriteHTML(w, name, _format.Exension);

      w.WriteLine("</table>");
      w.WriteLine("<!-- End Table -->");
      w.WriteLine("");
      w.WriteLine("</body");
      w.WriteLine("</html>");
      w.Close();

      _rectangles.Slice(image, name, _format.Exension);
    }
  }
  }
