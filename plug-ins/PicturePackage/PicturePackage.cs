// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// PicturePackage.cs
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;
using System.IO;
using System.Net;
using System.Threading;

using Gtk;

namespace Gimp.PicturePackage
{
  public class PicturePackage : Plugin
  {
    LayoutSet _layoutSet = new LayoutSet();
    Layout _layout;

    ProviderFactory _loader;

    SourceFrame _sf;
    DocumentFrame _df;
    Preview _preview;
    Thread _renderThread;

    [SaveAttribute]
    bool _flatten = false;

    [SaveAttribute]
    int _resolution = 72;

    [SaveAttribute]
    int _units = 0;		// Fix me: should become an enum

    [SaveAttribute]
    int _colorMode = 1;		// ColorMode.COLOR;

    [SaveAttribute]
    string _label = "";

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
		       "2004-2006",
		       "Picture Package...",
		       "");

      MenuRegister("<Toolbox>/Xtns/Extensions");
      IconRegister("PicturePackage.png");
    }

    override protected bool CreateDialog()
    {
      gimp_ui_init("PicturePackage", true);

      _layoutSet.Load();

      Dialog dialog = DialogNew("Picture Package 0.6", "PicturePackage",
				IntPtr.Zero, 0, null, "PicturePackage");

      HBox hbox = new HBox(false, 12);
      hbox.BorderWidth = 12;
      dialog.VBox.PackStart(hbox, true, true, 0);

      VBox vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, false, 0);

      _sf = new SourceFrame(this);
      vbox.PackStart(_sf, false, false, 0);

      _df = new DocumentFrame(this, _layoutSet);
      vbox.PackStart(_df, false, false, 0);

      LabelFrame lf = new LabelFrame(this);
      vbox.PackStart(lf, false, false, 0);

      Frame frame = new Frame();
      hbox.PackStart(frame, true, true, 0);

      VBox fbox = new VBox();
      fbox.BorderWidth = 12;
      frame.Add(fbox);

      Tooltips tips = new Tooltips();
      
      EventBox eventBox = new EventBox();
      fbox.Add(eventBox);
      tips.SetTip(eventBox, "Click to select picture", "preview");

      _preview = new Preview(this);
      _preview.WidthRequest = 400;
      _preview.HeightRequest = 500;
      _preview.ButtonPressEvent += new ButtonPressEventHandler(PreviewClicked);
      _preview.DragDataReceived += 
	new DragDataReceivedHandler(OnDragDataReceived);
      eventBox.Add(_preview);

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
      if (_loader == null)
	{
	Image image = _sf.Image;

	if (image != null)
	  {
	  _loader = new FrontImageProviderFactory(image);
	  }
	}

      if (_loader != null)
	{
	_preview.Clear();
	_layout.Render(_loader, _preview.GetRenderer(_layout));
	}
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

    Rectangle FindRectangle(double x, double y)
    {
      int offx, offy;
      double zoom = _layout.Boundaries(_preview.WidthRequest, 
				       _preview.HeightRequest, 
				       out offx, out offy);
      return _layout.Find((x - offx) / zoom, (y - offy) / zoom);		
    }

    void RenderRectangle(Rectangle rectangle, string filename)
    {
      ImageProvider provider = new FileImageProvider(filename);
      rectangle.Provider = provider;
      Image image = provider.GetImage();
      if (image != null)
	{
	Renderer renderer = _preview.GetRenderer(_layout);
	rectangle.Render(image, renderer);
	renderer.Cleanup();
	provider.Release();
	}
      else
	{
	Console.WriteLine("Couldn't load: " + filename);
	// Error dialog here.
	}		
    }

    void LoadRectangle(double x, double y, string filename)
    {
      Rectangle rectangle = FindRectangle(x, y);
      if (rectangle != null)
	{
	RenderRectangle(rectangle, filename);
	}
    }

    Rectangle _rectangle;

    void PreviewClicked(object o, ButtonPressEventArgs args)
    {
      _rectangle = FindRectangle(args.Event.X, args.Event.Y);
      if (_rectangle != null)
	{
	FileSelection selection = new FileSelection("Select image");
	selection.Response += new ResponseHandler (OnFileSelectionResponse);
	selection.Run();
	}
    }

    void OnDragDataReceived(object o, DragDataReceivedArgs args)
    {
      SelectionData data = args.SelectionData;
      string text = (new System.Text.ASCIIEncoding()).GetString(data.Data);
      // Console.WriteLine("OnDragDataReceived " + text);
      if (text.StartsWith("file:"))
	{
	LoadRectangle((double) args.X, (double) args.Y, text.Substring(5));
	}
      else if (text.StartsWith("http://"))
	{
#if true
	HttpWebRequest request = (HttpWebRequest) WebRequest.Create(text);
	request.KeepAlive = false;

	WebResponse response = request.GetResponse();
	Console.WriteLine("Length: " + response.ContentLength);
	response.Close();
#else
	Console.WriteLine("Implement this!");
#endif
	}
      Drag.Finish(args.Context, true, false, args.Time);
      _preview.QueueDraw();
    }

    void OnFileSelectionResponse (object o, ResponseArgs args)
    {
      FileSelection fs = o as FileSelection;
      if (args.ResponseId == ResponseType.Ok)
	{
	RenderRectangle(_rectangle, fs.Filename);
	}
      fs.Hide();
      _preview.QueueDraw();
    }

    override protected void DoSomething()
    {
      PageSize size = _layout.GetPageSizeInPixels(_resolution);

      int width = (int) size.Width;
      int height = (int) size.Height;
      Image composed = new Image(width, height, ImageBaseType.RGB);

      _layout.Render(_loader, new ImageRenderer(_layout, composed, 
						_resolution));

      if (_flatten)
	{
	composed.Flatten();
	}

      if (_colorMode == 0) // ColorMode.GRAY)
	{
	composed.ConvertGrayscale();
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
      get {return _position;}
    }

    public int Resolution
    {
      set {_resolution = value;}
      get {return _resolution;}
    }

    public int Units
    {
      set {_units = value;}
      get {return _units;}
    }

    public int ColorMode
    {
      set {_colorMode = value;}
      get {return _colorMode;}
    }

    public bool Flatten
    {
      set {_flatten = value;}
      get {return _flatten;}
    }
  }
  }
