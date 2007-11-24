// The PicturePackage plug-in
// Copyright (C) 2004-2007 Maurits Rijk, Massimo Perga
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

using Gtk;
using Gdk;
using GLib;

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
    System.Threading.Thread _renderThread;

    [SaveAttribute("flatten")]
    bool _flatten = false;
    [SaveAttribute("resolution")]
    int _resolution = 72;
    [SaveAttribute("units")]
    int _units = 0;		// Fix me: should become an enum
    [SaveAttribute("color_mode")]
    int _colorMode = 1;		// ColorMode.Color
    [SaveAttribute("label")]
    string _label = "";
    [SaveAttribute("position")]
    int _position;

    private Coordinate<double> _beginDnDPoint;
    private Coordinate<double> _leaveDnDPoint;
    private bool _beginDnDSet;
    private bool _leaveDnDSet = false;

    public enum DialogStateType
    {
      SrcImgValid,  	 // Source combo, Image selected, No image
      SrcImgInvalid,	 // Source combo, Image selected, With image
      SrcFileValid, 	 // Source combo, File selected, No file
      SrcFileInvalid   // Source combo, File selected, With file 
    };

    // TODO: improve it
    enum TargetType {
      String,
      RootWindow
    };

    static bool _haveDrag = false;

    private static TargetEntry[] targetTable = new TargetEntry [] {
      new TargetEntry ("dummy", 0, (uint)TargetType.String),
      new TargetEntry ("application/x-gimpsharp-picturepackage-drop", 0, 
		       (uint)TargetType.String)
    };

    [SaveAttribute]
    DialogStateType _currentDialogState = DialogStateType.SrcImgInvalid;

    static void Main(string[] args)
    {
      new PicturePackage(args);
    }

    public PicturePackage(string[] args) : base(args, "PicturePackage")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      ParamDefList inParams = new ParamDefList();

      yield return new Procedure("plug_in_picture_package",
				 _("Picture package"),
				 _("Picture package"),
				 "Maurits Rijk, Massimo Perga",
				 "Maurits Rijk, Massimo Perga",
				 "2004-2007",
				 _("Picture Package..."),
				 "",
				 inParams)
	{
	  MenuPath = "<Toolbox>/Xtns/Extensions",
	  IconFile = "PicturePackage.png"
	};
    }

    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("PicturePackage", true);

      _layoutSet.Load();

      GimpDialog dialog = DialogNew(_("Picture Package 0.6.2"),
				    _("PicturePackage"), IntPtr.Zero, 0, null, 
				    _("PicturePackage"));

      HBox hbox = new HBox(false, 12) {BorderWidth = 12};
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

      VBox fbox = new VBox() {BorderWidth = 12};
      frame.Add(fbox);

      Tooltips tips = new Tooltips();

      EventBox eventBox = new EventBox();
      fbox.Add(eventBox);
      tips.SetTip(eventBox, _("Right click to select picture"), "preview");

      _preview = new Preview(this) {WidthRequest = 400, HeightRequest = 500};
      _preview.ButtonPressEvent += PreviewClicked;

      //      _preview.DragDataReceived += OnDragDataReceived;

      eventBox.Add(_preview);

      _layoutSet.Selected = _layoutSet[0];
      _layout = _layoutSet[0];
      _layoutSet.SelectEvent += SetLayout;


      Gtk.Drag.DestSet(_preview, DestDefaults.All, targetTable, 
		       DragAction.Copy | DragAction.Move);

      _preview.DragMotion += new DragMotionHandler(HandlePopupMotion);

      Gtk.Drag.SourceSet(_preview, Gdk.ModifierType.Button1Mask, targetTable, 
			 DragAction.Copy | DragAction.Move);

      _preview.DragBegin += new DragBeginHandler(HandlePopupBegin);
      _preview.DragEnd += new DragEndHandler(HandlePopupEnd);
      _preview.DragDataGet += new DragDataGetHandler(HandlePopupDataGet);
      _preview.DragDataDelete += 
	new DragDataDeleteHandler(HandlePopupDataDelete);

      _preview.DragLeave += new DragLeaveHandler(HandlePopupLeave);

      _preview.DragDataReceived += 
	new DragDataReceivedHandler(HandleLabelDragDataReceived);

      DialogState = DialogStateType.SrcImgInvalid;

      return dialog;
    }

    void SetLayout(Layout layout)
    {
      _layout = layout;
      RedrawPreview();
    }

    public void RenderLayout()
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
	  if(_layout.Render(_loader, _preview.GetRenderer(_layout)))
	    DialogState = DialogStateType.SrcFileValid;
	  else
	    DialogState = DialogStateType.SrcFileInvalid;

	}

    }

    public void RenderX()
    {
      _renderThread = new System.Threading.Thread(new ThreadStart(RenderThread));
      _renderThread.Start();
    }

    void RenderThread()

    {
      if(_layout.Render(_loader, _preview.GetRenderer(_layout)))
        DialogState = DialogStateType.SrcImgValid;
      else
        DialogState = DialogStateType.SrcImgInvalid;
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
      RenderLayout();
    }
#else
    void RedrawPreview()
    {
      RenderLayout();
      _preview.QueueDraw();
    }
#endif

    Rectangle FindRectangle(Coordinate<double> c)
    {
      int offx, offy;
      double zoom = _layout.Boundaries(_preview.WidthRequest, 
				       _preview.HeightRequest, 
				       out offx, out offy);
      return _layout.Find(new Coordinate<double>((c.X - offx) / zoom, 
						 (c.Y - offy) / zoom));
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
	  // Fix for OK button when Drag & Drop happens
	  if(DialogState == DialogStateType.SrcImgInvalid)
	    DialogState = DialogStateType.SrcImgValid; 
	  renderer.Cleanup();
	  provider.Release();
	}
      else
	{
	  //	  Console.WriteLine("Couldn't load: " + filename);
	  // Error dialog here.
	}		
    }

    void LoadRectangle(Coordinate<double> c, string filename)
    {
      Rectangle rectangle = FindRectangle(c);
      if (rectangle != null)
	{
	  RenderRectangle(rectangle, filename);
	}
    }

    Rectangle _rectangle;

    void PreviewClicked(object o, ButtonPressEventArgs args)
    {
      _rectangle = FindRectangle(new Coordinate<double>(args.Event.X, 
							args.Event.Y));
      if (_rectangle != null)
	{
	  if (args.Event.Button == 3)
	    {
	      FileSelection selection = new FileSelection("Select image");
	      selection.Response += OnFileSelectionResponse;
	      selection.Run();
	    }
	}
      else if (args.Event.Button == 1)
	{

	}
    }

    void OnDragDataReceived(object o, DragDataReceivedArgs args)
    {
      SelectionData data = args.SelectionData;
      string text = (new System.Text.ASCIIEncoding()).GetString(data.Data);
      string draggedFileName = null;

      if (text.StartsWith("file:"))
	{
	  draggedFileName = (text.Substring(7)).Trim('\t',(char)0x0a,
						     (char)0x0d);
	}
      else if (text.StartsWith("http://"))
	{
	  draggedFileName = (text.Trim('\t',(char)0x0a,(char)0x0d));
	}
      LoadRectangle(new Coordinate<double>(args.X, args.Y), draggedFileName);
      Gtk.Drag.Finish(args.Context, true, false, args.Time);
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

    override protected void Render()
    {
      PageSize size = _layout.GetPageSizeInPixels(_resolution);

      int width = (int) size.Width;
      int height = (int) size.Height;
      Image composed = new Image(width, height, ImageBaseType.Rgb);

      if(_layout.Render(_loader, new ImageRenderer(_layout, composed, 
						   _resolution)))
        DialogState = DialogStateType.SrcImgValid;
      else
        DialogState = DialogStateType.SrcImgInvalid;

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

    public DialogStateType DialogState
    {
      set
	{
	  _currentDialogState = value;
	  if (Dialog != null)
	    {
	      if (_currentDialogState == DialogStateType.SrcImgValid ||
		  _currentDialogState == DialogStateType.SrcFileValid)
		{
		  Dialog.SetResponseSensitive(ResponseType.Ok, true);
		}
	      else
		{
		  Dialog.SetResponseSensitive(ResponseType.Ok, false);
		}
	    }
	}
      get
	{
	  return _currentDialogState;
	}
    }

    static void HandleTargetDragLeave(object sender, DragLeaveArgs args)
    {
      Console.WriteLine("HandleTargetDragLeave");
      _haveDrag = false;

      // FIXME?  Kinda wonky binding.
      //(sender as Gtk.Image).FromPixbuf = trashcan_closed_pixbuf;
    }

    static void HandleTargetDragMotion (object sender, DragMotionArgs args)
    {
      Console.WriteLine("HandleTargetDragMotion");
      if (! _haveDrag) {
        _haveDrag = true;
        // FIXME?  Kinda wonky binding.
        // (sender as Gtk.Image).FromPixbuf = trashcan_open_pixbuf;
      }

      Widget source_widget = Gtk.Drag.GetSourceWidget (args.Context);
      Console.WriteLine ("motion, source {0}", source_widget == null ? 
			 "null" : source_widget.ToString ());
      
      Atom[] targets = args.Context.Targets;
      foreach (Atom a in targets)
        Console.WriteLine (a.Name); 
      
      Gdk.Drag.Status (args.Context, args.Context.SuggestedAction, args.Time);
      args.RetVal = true;
    }

    static void HandleTargetDragDrop(object sender, DragDropArgs args)
    {
      Console.WriteLine("HandleTargetDragDrop");
      Console.WriteLine ("drop");
      _haveDrag = false;
      //  (sender as Gtk.Image).FromPixbuf = trashcan_closed_pixbuf;
      
#if BROKEN   // Context.Targets is not defined in the bindings
      if (Context.Targets.Length != 0) 
	{
	  Drag.GetData (sender, context, Context.Targets.Data as Gdk.Atom, 
			args.Time);
	  args.RetVal = true;
      }
#endif

      args.RetVal = false;
    }

    static void HandleTargetDragDataReceived(object sender, 
					     DragDataReceivedArgs args)
    {
      Console.WriteLine("HandleTargetDragDataReceived");
      if (args.SelectionData.Length >=0 && args.SelectionData.Format == 8) 
	{
	  Console.WriteLine ("Received {0} in trashcan", args.SelectionData);
	  Gtk.Drag.Finish (args.Context, true, false, args.Time);
      }
      
      Gtk.Drag.Finish (args.Context, false, false, args.Time);
    }

    static void HandleLabelDragDataReceived(object sender, 
					    DragDataReceivedArgs args)
    {
      Console.WriteLine ("HandleLabelDragDataReceived");
      if (args.SelectionData.Length >=0 && args.SelectionData.Format == 8) 
	{
	  Console.WriteLine ("Received {0} in label", args.SelectionData);
	  Gtk.Drag.Finish (args.Context, true, false, args.Time);
	}
      
      Gtk.Drag.Finish (args.Context, false, false, args.Time);
    }

    static void HandleSourceDragDataGet(object sender, DragDataGetArgs args)
    {
      Console.WriteLine ("HandleSourceDragDataGet");
      if (args.Info == (uint) TargetType.RootWindow)
        Console.WriteLine ("I was dropped on the rootwin");
      else
        args.SelectionData.Text = "I'm data!";
    }

    static bool HandlePopdownCallback ()
    {
      Console.WriteLine ("HandlePopdownCallback");
      /*
	popdown_timer = 0;
	popup_window.Hide ();
	popped_up = false;
      */
      return false;
    }

    void HandlePopupMotion (object sender, DragMotionArgs args)
    {
      if (!_beginDnDSet)
	{
	  _beginDnDPoint = new Coordinate<double>(args.X, args.Y);
	  _beginDnDSet = true;
	}
      else if (!_leaveDnDSet)
	{
	  _leaveDnDPoint = new Coordinate<double>(args.X, args.Y);
	}
      //Console.WriteLine ("HandlePopupMotion {0} {1}", args.X, args.Y);
      Console.WriteLine ("HandlePopupMotion {0} {1}", args.X, args.Y);
      /*
	if (! in_popup) {
	in_popup = true;
	if (popdown_timer != 0) {
	Console.WriteLine ("removed popdown");
	GLib.Source.Remove (popdown_timer);
	popdown_timer = 0;
	}
	}
      */

      args.RetVal = true;
    }

    static void HandlePopupLeave (object sender, DragLeaveArgs args)
    {
      Console.WriteLine ("HandlePopupLeave");
      /*
	if (in_popup) {
	in_popup = false;
	if (popdown_timer == 0) {
	Console.WriteLine ("added popdown");
	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	}
	}
      */
    }

    static void HandlePopupBegin (object sender, DragBeginArgs args)
    {
      Console.WriteLine ("HandlePopupBegin");
      /*
	if (in_popup) {
	in_popup = false;
	if (popdown_timer == 0) {
	Console.WriteLine ("added popdown");
	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	}
	}
      */
    }

    void HandlePopupEnd (object sender, DragEndArgs args)
    {
      Console.WriteLine("HandlePopupEnd");

      Rectangle rectA = FindRectangle(_beginDnDPoint);
      Rectangle rectB = FindRectangle(_leaveDnDPoint);
      if (rectA != null && rectB != null)
	{
	  Rectangle.SwapCoordinates(ref rectA, ref rectB);
	}

      //      Rectangle rectB = 


      // Swap the two rectangles
      //_layout.Swap(_beginDnDPoint, _leaveDnDPoint);
      RedrawPreview();
      
      // Resetting flags for DnD
      _beginDnDSet = _leaveDnDSet = false;
      /*
	if (in_popup) {
	in_popup = false;
	if (popdown_timer == 0) {
	Console.WriteLine ("added popdown");
	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	}
	}
      */
    }

    static void HandlePopupDataGet (object sender, DragDataGetArgs args)
    {
      Console.WriteLine("HandlePopupDataGet");
      /*
	if (in_popup) {
	in_popup = false;
	if (popdown_timer == 0) {
	Console.WriteLine ("added popdown");
	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	}
	}
      */
    }

    static void HandlePopupDataDelete (object sender, DragDataDeleteArgs args)
    {
      Console.WriteLine("HandlePopupDataDelete");
      /*
	if (in_popup) {
	in_popup = false;
	if (popdown_timer == 0) {
	Console.WriteLine ("added popdown");
	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	}
	}
      */
    }

    static bool HandlePopupCallback ()
    {
      Console.WriteLine("HandlePopupCallback");
      /*
	if (! popped_up) {
	if (popup_window == null) {
	Button button;
	Table table;

	popup_window = new Gtk.Window (Gtk.WindowType.Popup);
	popup_window.SetPosition (WindowPosition.Mouse);

	table = new Table (3, 3, false);

	for (int i = 0; i < 3; i++)
	for (int j = 0; j < 3; j++) {
	string label = String.Format ("{0},{1}", i, j);
	button = Button.NewWithLabel (label);

	table.Attach (button, (uint) i, (uint) i + 1, (uint) j, (uint) j + 1,
	AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill,
	0, 0);

	Gtk.Drag.DestSet (button, DestDefaults.All,
	target_table, DragAction.Copy | DragAction.Move);

	button.DragMotion += new DragMotionHandler (HandlePopupMotion);
	button.DragLeave += new DragLeaveHandler (HandlePopupLeave);
	}

	table.ShowAll ();
	popup_window.Add (table);
	}

	popup_window.Show ();
	popped_up = true;
	}

	popdown_timer = GLib.Timeout.Add (500, new TimeoutHandler (HandlePopdownCallback));
	popup_timer = 0;
      */
      return false;
    }
  }
}

