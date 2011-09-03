// The PicturePackage plug-in
// Copyright (C) 2004-2011 Maurits Rijk, Massimo Perga
//
// Dialog.cs
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

using Gdk;
using Gtk;

namespace Gimp.PicturePackage
{
  public class Dialog : GimpDialog
  {
    readonly Variable<ProviderFactory> _loader;
    readonly Variable<Layout> _layout;

    SourceFrame _sf;
    Preview _preview;

    // TODO: improve it
    enum TargetType {
      String,
      RootWindow
    };

    private Coordinate<double> _beginDnDPoint;
    private Coordinate<double> _leaveDnDPoint;
    private bool _beginDnDSet;
    private bool _leaveDnDSet = false;

    static bool _haveDrag = false;

    DialogStateType _currentDialogState = DialogStateType.SrcImgInvalid;

    private static TargetEntry[] targetTable = new TargetEntry [] {
      new TargetEntry ("dummy", 0, (uint)TargetType.String),
      new TargetEntry ("application/x-gimpsharp-picturepackage-drop", 0, 
		       (uint)TargetType.String)
    };

    public Dialog(VariableSet variables, LayoutSet layouts, 
		  Variable<Layout> layout, Variable<ProviderFactory> loader) : 
      base("PicturePackage", variables)
    {
      _layout = layout;
      _loader = loader;

      layouts.Load();

      var hbox = new HBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(hbox, true, true, 0);

      var vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, false, 0);

      _sf = new SourceFrame(loader);
      vbox.PackStart(_sf, false, false, 0);

      vbox.PackStart(new DocumentFrame(variables, layouts), false, false, 0);

      vbox.PackStart(new LabelFrame(variables), false, false, 0);

      var frame = new Frame();
      hbox.PackStart(frame, true, true, 0);

      var fbox = new VBox() {BorderWidth = 12};
      frame.Add(fbox);

      var tips = new Tooltips();

      var eventBox = new EventBox();
      fbox.Add(eventBox);
      tips.SetTip(eventBox, _("Right click to select picture"), "preview");

      _preview = new Preview(this, variables) 
	{WidthRequest = 400, HeightRequest = 500};
      _preview.ButtonPressEvent += PreviewClicked;

      //      _preview.DragDataReceived += OnDragDataReceived;

      eventBox.Add(_preview);

      layouts.Selected = layouts[0];
      _layout.Value = layouts[0];
      layouts.SelectEvent += SetLayout;

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
	      var selection = new FileSelection("Select image");
	      selection.Response += OnFileSelectionResponse;
	      selection.Run();
	    }
	}
      else if (args.Event.Button == 1)
	{

	}
    }

    Rectangle FindRectangle(Coordinate<double> c)
    {
      var layout = _layout.Value;

      int offx, offy;
      double zoom = layout.Boundaries(_preview.WidthRequest, 
				      _preview.HeightRequest, 
				      out offx, out offy);
      return layout.Find(new Coordinate<double>((c.X - offx) / zoom, 
						(c.Y - offy) / zoom));
    }

    void SetLayout(Layout layout)
    {
      _layout.Value = layout;
      RedrawPreview();
    }

    public void RenderLayout()
    {
      var layout = _layout.Value;

      if (_loader.Value == null)
	{
	  Image image = _sf.Image;

	  if (image != null)
	    {
	      _loader.Value = new FrontImageProviderFactory(image);
	    }
	}

      if (_loader.Value != null)
	{
	  _preview.Clear();
	  if (layout.Render(_loader.Value, _preview.GetRenderer(layout)))
	    DialogState = DialogStateType.SrcFileValid;
	  else
	    DialogState = DialogStateType.SrcFileInvalid;
	}
    }

    void RedrawPreview()
    {
      RenderLayout();
      _preview.QueueDraw();
    }

    void OnDragDataReceived(object o, DragDataReceivedArgs args)
    {
      var data = args.SelectionData;
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

    void LoadRectangle(Coordinate<double> c, string filename)
    {
      var rectangle = FindRectangle(c);
      if (rectangle != null)
	{
	  RenderRectangle(rectangle, filename);
	}
    }

    void RenderRectangle(Rectangle rectangle, string filename)
    {
      var provider = new FileImageProvider(filename);
      rectangle.Provider = provider;
      var image = provider.GetImage();
      if (image != null)
	{
	  var renderer = _preview.GetRenderer(_layout.Value);
	  rectangle.Render(image, renderer);
	  // Fix for OK button when Drag & Drop happens
	  if (DialogState == DialogStateType.SrcImgInvalid)
	    {
	      DialogState = DialogStateType.SrcImgValid;
	    }
	  renderer.Cleanup();
	  provider.Release();
	}
      else
	{
	  //	  Console.WriteLine("Couldn't load: " + filename);
	  // Error dialog here.
	}		
    }

    void OnFileSelectionResponse (object o, ResponseArgs args)
    {
      var fs = o as FileSelection;
      if (args.ResponseId == ResponseType.Ok)
	{
	  RenderRectangle(_rectangle, fs.Filename);
	}

      fs.Hide();
      _preview.QueueDraw();
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

    public DialogStateType DialogState
    {
      set
	{
	  _currentDialogState = value;

	  if (_currentDialogState == DialogStateType.SrcImgValid ||
	      _currentDialogState == DialogStateType.SrcFileValid)
	    {
	      SetResponseSensitive(ResponseType.Ok, true);
	    }
	  else
	    {
	      SetResponseSensitive(ResponseType.Ok, false);
	    }
	}
      get
	{
	  return _currentDialogState;
	}
    }
  }
}
