using System;
using System.Threading;

using Gtk;

namespace Gimp.PicturePackage
{
  public class PicturePackage : Plugin
  {
    LayoutSet _layoutSet = new LayoutSet();
    Layout _layout;

    ProviderFactory _loader;

    DocumentFrame _df;
    Preview _preview;
    Thread _renderThread;

    [SaveAttribute]
    bool _flatten = false;

    [SaveAttribute]
    int _resolution = 72;

    // [SaveAttribute]
    string _label;

    [SaveAttribute]
    int _position;

    [STAThread]
    static void Main(string[] args)
    {
      new PicturePackage(args);
    }

    public PicturePackage(string[] args) : base(args)
    {
    }

    override protected void Query()
    {
      InstallProcedure("plug_in_picture_package",
		       "Picture package",
		       "Picture package",
		       "Maurits Rijk",
		       "Maurits Rijk",
		       "2004",
		       "Picture Package...",
		       "RGB*, GRAY*",
		       null);

      MenuRegister("plug_in_picture_package", "<Image>/Filters/Render");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("PicturePackage", true);

      _layoutSet.Load();
      _loader = new FrontImageProviderFactory(_image);

      Dialog dialog = DialogNew("Picture Package 0.4", "PicturePackage",
				IntPtr.Zero, 0, null, "PicturePackage");

      HBox hbox = new HBox(false, 12);
      hbox.BorderWidth = 12;
      dialog.VBox.PackStart(hbox, true, true, 0);

      VBox vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, false, 0);

      SourceFrame sf = new SourceFrame(this);
      vbox.PackStart(sf, false, false, 0);

      _df = new DocumentFrame(this, _layoutSet);
      vbox.PackStart(_df, false, false, 0);

      LabelFrame lf = new LabelFrame(this);
      vbox.PackStart(lf, false, false, 0);

      Frame frame = new Frame();
      hbox.PackStart(frame, true, true, 0);

      VBox fbox = new VBox();
      fbox.BorderWidth = 12;
      frame.Add(fbox);

      _preview = new Preview(this);
      _preview.WidthRequest = 400;
      _preview.HeightRequest = 500;
      _preview.ButtonPressEvent += new ButtonPressEventHandler(PreviewClicked);
      fbox.Add(_preview);

      _layoutSet.Selected = _layoutSet[0];
      _layout = _layoutSet[0];
      _layoutSet.SelectEvent += new SelectHandler(SetLayout);
	
      dialog.ShowAll();
      return DialogRun();
    }

    void SetLayout(Layout layout)
    {
      _layout = layout;
      RedrawPreview();
    }

    public void Render()
    {
      _layout.Render(_loader, _preview.GetRenderer(_layout));
    }

    public void RenderX()
    {
      _renderThread = new Thread(new ThreadStart(RenderThread));
      _renderThread.Start();
    }

    void RenderThread()
    {
      _layout.Render(_loader, _preview.GetRenderer(_layout));
    }
#if false
    void RedrawPreview()
    {
      if (_renderThread != null)
	{
	_renderThread.Abort();
	_renderThread.Join();
	}
      _preview.Clear();
      Render();
    }
#else
    void RedrawPreview()
    {
      Render();
      _preview.QueueDraw();
    }
#endif

    Rectangle _rectangle;

    public void PreviewClicked(object o, ButtonPressEventArgs args)
    {
      int offx, offy;
      double zoom = _layout.Boundaries(_preview.WidthRequest, 
				       _preview.HeightRequest, 
				       out offx, out offy);
      double x = (args.Event.X - offx) / zoom;
      double y = (args.Event.Y - offy) / zoom;

      _rectangle = _layout.Find(x, y);
      if (_rectangle != null)
	{
	FileSelection selection = new FileSelection("Select image");
	selection.Response += new ResponseHandler (OnFileSelectionResponse);
	selection.Run();
	}
    }

    void OnFileSelectionResponse (object o, ResponseArgs args)
    {
      FileSelection fs = o as FileSelection;
      if (args.ResponseId == ResponseType.Ok)
	{
	Console.WriteLine("Selected: " + fs.Filename);
	ImageProvider provider = new FileImageProvider(fs.Filename);
	_rectangle.Provider = provider;
	Image image = provider.GetImage();
	if (image != null)
	  {
	  Renderer renderer = _preview.GetRenderer(_layout);
	  _rectangle.Render(image, renderer);
	  renderer.Cleanup();
	  provider.Release();
	  }
	else
	  {
	  // Error dialog here.
	  }
	}
      fs.Hide();
    }

    override protected void DoSomething(Image image)
    {
      PageSize size = _layout.GetPageSizeInPixels(_resolution);

      int width = (int) size.Width;
      int height = (int) size.Height;
      Image composed = new Image(width, height, ImageBaseType.RGB);

      _layout.Render(_loader, new ImageRenderer(_layout, composed, _resolution));

      if (_flatten)
	{
	composed.Flatten();
	}

      new Display(composed);
      Display.DisplaysFlush();
    }

    public ProviderFactory Loader
    {
      set 
	  {
	  _loader = value;
	  RedrawPreview();
	  }
    }

    public Image Image
    {
      get {return _image;}
    }

    public string Label
    {
      set
	  {
	  _label = value;
	  _preview.DrawLabel(_position, _label);
	  }
    }

    public int Position
    {
      set
	  {
	  _position = value;
	  _preview.DrawLabel(_position, _label);
	  }
    }

    public int Resolution
    {
      set {_resolution = value;}
      get {return _resolution;}
    }

    public bool Flatten
    {
      set {_flatten = value;}
      get {return _flatten;}
    }
  }
  }
