using System;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {
    RectangleSet _rectangles = new RectangleSet();
    SliceSet _horizontalSlices = new SliceSet();
    SliceSet _verticalSlices = new SliceSet();

    Preview _preview;
    Entry _xy;

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
		       "Slice Tool",
		       "Maurits Rijk",
		       "(C) Maurits Rijk",
		       "2005",
		       "Slice Tool...",
		       "RGB*, GRAY*",
		       null);

      MenuRegister("<Image>/Filters/Web");
      IconRegister("SliceTool.png");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("SliceTool", true);

      Dialog dialog = DialogNew("Slice Tool", "SliceTool",
				IntPtr.Zero, 0, null, "SliceTool");

      VBox vbox = new VBox(false, 12);
      vbox.BorderWidth = 12;
      dialog.VBox.PackStart(vbox, true, true, 0);

      HBox hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);

      HandleBox handle = new HandleBox();
      hbox.PackStart(handle, true, true, 0);

      Toolbar tools = new Toolbar();
      tools.Orientation = Gtk.Orientation.Vertical;
      tools.ToolbarStyle = Gtk.ToolbarStyle.Icons;
      handle.Add(tools);

      Button button = new Button("gimp-grid");
      tools.AppendWidget(button, "blah", "foo");

      int width = _drawable.Width;
      int height = _drawable.Height;

      _preview = new Preview(_drawable, this);
      _preview.WidthRequest = width;
      _preview.HeightRequest = height;
      _preview.ButtonPressEvent += new ButtonPressEventHandler(OnButtonPress);
      _preview.ButtonReleaseEvent += 
	new ButtonReleaseEventHandler(OnButtonRelease);
      
      _preview.MotionNotifyEvent +=
      	new MotionNotifyEventHandler(OnShowCoordinates);

      hbox.PackStart(_preview, true, true, 0);

      _xy = new Entry();
      _xy.Editable = false;
      vbox.Add(_xy);

      GimpFrame frame = new GimpFrame("Cell Properties");
      vbox.Add(frame);
      GimpTable table = new GimpTable(3, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;

      frame.Add(table);

      Entry url = new Entry();
      table.AttachAligned(0, 0, "URL:", 0.0, 0.5, url, 1, false);

      Entry altText = new Entry();
      table.AttachAligned(0, 1, "Alt text:", 0.0, 0.5, altText, 1, false);

      Entry target = new Entry();
      table.AttachAligned(0, 2, "Target:", 0.0, 0.5, target, 1, false);

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

      dialog.ShowAll();
      return DialogRun();
    }

    public void Redraw()
    {
      _renderer = _preview.GetRenderer();
      _horizontalSlices.Draw(_renderer);
      _verticalSlices.Draw(_renderer);
    }

    Slice _slice;
    PreviewRenderer _renderer;
    Rectangle _rectangle;
    bool _foo;

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
      _slice = GetSlice(x, y);

      _renderer = _preview.GetRenderer();

      _renderer.Function = Gdk.Function.Equiv;
      _renderer.Draw(_slice);
      _preview.MotionNotifyEvent += new MotionNotifyEventHandler(OnMotionNotify);
    }

    void OnButtonRelease(object o, ButtonReleaseEventArgs args)
    {
      _preview.MotionNotifyEvent -= new MotionNotifyEventHandler(OnMotionNotify);
      _renderer.Draw(_slice);
      if (_foo)
	_horizontalSlices.Add(_slice);
      else
	_verticalSlices.Add(_slice);
      _rectangles.Slice(_slice);
      _rectangle = null;
      _renderer.Function = Gdk.Function.Copy;
      _renderer.Draw(_slice);
    }

    void OnMotionNotify(object o, MotionNotifyEventArgs args)
    {
      int x = (int) args.Event.X;
      int y = (int) args.Event.Y;

      _renderer.Draw(_slice);
      _slice = GetSlice(x, y);
      _slice.SetPosition(x, y);
      _renderer.Draw(_slice);
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
      _horizontalSlices.Sort();
      _verticalSlices.Sort();

      Console.WriteLine("Name: " + image.Name);
      Console.WriteLine("Filename: " + image.Filename);

      Console.WriteLine("<html>");
      Console.WriteLine("<head>");
      Console.WriteLine("<meta name=\"Author\" content=\"{0}\">",
			Environment.UserName);
      Console.WriteLine("<meta name=\"Generator\" content=\"GIMP {0}\">",
			Gimp.Version);
      Console.WriteLine("<title></title>");
      Console.WriteLine("</head>");
      Console.WriteLine("<body");
      Console.WriteLine("");
      Console.WriteLine("<!-- Begin Table -->");
      Console.WriteLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"{0}\">", drawable.Width);

      _rectangles.WriteHTML();

      Console.WriteLine("</table>");
      Console.WriteLine("<!-- End Table -->");
      Console.WriteLine("");
      Console.WriteLine("</body");
      Console.WriteLine("</html>");
    }
  }
  }
