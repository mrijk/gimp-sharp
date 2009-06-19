// The Slice Tool plug-in
// Copyright (C) 2004-2009 Maurits Rijk
//
// SliceTool.cs
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
using System.Reflection;

using Gdk;
using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {
    public MouseFunc Func {private get; set;}
    public Preview Preview {private set; get;}
    
    SliceData _sliceData = new SliceData();
    
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
    
    static void Main(string[] args)
    {
      new SliceTool(args);
    }
    
    SliceTool(string[] args) : base(args, "SliceTool")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      yield return new Procedure("plug_in_slice_tool",
				 _("Slice Tool"),
				 _("The Image Slice Tool is used to apply image slicing and rollovers."),
				 "Maurits Rijk",
				 "(C) Maurits Rijk",
				 "2005-2009",
				 _("Slice Tool..."),
				 "RGB*, GRAY*")
	{
	  MenuPath = "<Image>/Filters/Web",
	  IconFile = "SliceTool.png"
	};
    }
    
    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("SliceTool", true);
      
      CreateStockIcons();
      
      var dialog = DialogNew(_("Slice Tool"), _("SliceTool"), 
			     IntPtr.Zero, 0, null, _("SliceTool"),
			     Stock.SaveAs, (Gtk.ResponseType) 0,
			     Stock.Save, (Gtk.ResponseType) 1,
			     Stock.Close, ResponseType.Close);
      
      SetTitle(null);
      
      VBox vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);
      
      HBox hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      
      Widget toolbar = CreateToolbar();
      hbox.PackStart(toolbar, false, true, 0);
      
      Widget preview = CreatePreview();
      hbox.PackStart(preview, true, true, 0);
      
      // Create coordinates
      hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      _xy = new Entry() {WidthChars = 16, IsEditable = false};
      hbox.PackStart(_xy, false, false, 0);
      
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
      
      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);
      
      Button save = new Button(_("Save Settings..."));
      save.Clicked += OnSaveSettings;
      vbox.PackStart(save, false, true, 0);
      
      Button load = new Button(_("Load Settings..."));
      load.Clicked += OnLoadSettings;
      vbox.PackStart(load, false, true, 0);

      Button preferences = new Button(_("Preferences"));
      preferences.Clicked += OnPreferences;
      vbox.PackStart(preferences, false, true, 0);

      _sliceData.Init(_drawable);
      GetRectangleData(_sliceData.Selected);
      
      Func = new SelectFunc(this, _sliceData);
      
      return dialog;
    }
    
    // Fix me: move this to Plugin class?!
    void SetTitle(string filename)
    {
      _filename = filename;
      string p = (filename == null) 
	? "<Untitled>" : System.IO.Path.GetFileName(filename);
      string title = string.Format(_("Slice Tool 0.4 - {0}"), p);
      Dialog.Title = title;
    }
    
    void SaveBlank(string path)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream input = assembly.GetManifestResourceStream("blank.png");
      BinaryReader reader = new BinaryReader(input);
      byte[] buffer = reader.ReadBytes((int) input.Length);
      string fileName = path + System.IO.Path.DirectorySeparatorChar + 
	"blank.png";
      FileStream fs = new FileStream(fileName, FileMode.Create,
				     FileAccess.Write);
      BinaryWriter writer = new BinaryWriter(fs);
      writer.Write(buffer);
      writer.Close();
    }
    
    void Save()
    {
      SetRectangleData(_sliceData.Selected);
      try
	{
	  _sliceData.Save(_filename, _format.Apply, _image, _drawable);
	  SaveBlank(System.IO.Path.GetDirectoryName(_filename));
	}
      catch (Exception)
	{
	  MessageDialog message = 
	    new MessageDialog(null, DialogFlags.DestroyWithParent,
			      MessageType.Error, ButtonsType.Close,
			      _("Can't save to ") + _filename);
	  message.Run();
	  message.Destroy();
	}
    }
    
    override protected bool OnClose()
    {
      if (_sliceData.Changed)
	{
	  MessageDialog message = 
	    new MessageDialog(null, DialogFlags.DestroyWithParent,
			      MessageType.Warning, ButtonsType.YesNo, 
			      _("Some data has been changed!\n") + 
			      _("Do you really want to discard your changes?"));
	  ResponseType response = (ResponseType) message.Run();
	  return response == ResponseType.Yes;
	}
      return true;
    }
    
    override protected void DialogRun(ResponseType type)
    {
      if ((int) type == 0 || ((int) type == 1 && _filename == null))
	{
	  FileChooserDialog fc = 
	    new FileChooserDialog(_("HTML Save As"), Dialog,
				  FileChooserAction.Save,
				  "Cancel", ResponseType.Cancel,
				  "Save", ResponseType.Accept);
      
	  if (fc.Run() == (int) ResponseType.Accept)
	    {
	      string filename = fc.Filename;
	      if (System.IO.File.Exists(filename))
		{
		  FileExistsDialog message = new FileExistsDialog(filename);
		  if (!message.IsYes())
		    {
		      return;
		    }
		}
	      SetTitle(filename);
	      Save();
	    }
	  fc.Destroy();
	}
      else // type == 1
	{
	  Save();
	}
    }

    Widget CreatePreview()
    {
      ScrolledWindow window = new ScrolledWindow();
      window.SetSizeRequest(600, 400);

      Alignment alignment = new Alignment(0.5f, 0.5f, 0, 0);

      Preview = new Preview(_drawable, this);
      Preview.WidthRequest = _drawable.Width;
      Preview.HeightRequest = _drawable.Height;

      Preview.ButtonPressEvent += OnButtonPress;      
      Preview.MotionNotifyEvent += OnShowCoordinates;
      Preview.LeaveNotifyEvent += delegate
	{
	  _xy.Text = "";
	};
      
      alignment.Add(Preview);
      window.AddWithViewport(alignment);

      return window;
    }

    void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      var c = new Coordinate<int>((int) args.Event.X, (int) args.Event.Y);
      Func.GetActualFunc(this, c).OnButtonPress(o, args);
    }

    Widget CreateToolbar()
    {
      return new Toolbox(this, _sliceData);
    }

    Widget CreateCellProperties()
    {
      GimpFrame frame = new GimpFrame(_("Cell Properties"));

      VBox vbox = new VBox(false, 12);
      frame.Add(vbox);

      GimpTable table = new GimpTable(3, 2, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      
      vbox.Add(table);
			
      _url = new Entry();
      table.AttachAligned(0, 0, _("_Link:"), 0.0, 0.5, _url, 3, false);
      
      _altText = new Entry();
      table.AttachAligned(0, 1, _("Alt_ernative text:"), 0.0, 0.5, _altText, 3,
			  false);
      
      _target = new Entry();
      table.AttachAligned(0, 2, _("_Target:"), 0.0, 0.5, _target, 3, false);

      HBox hbox = new HBox(false, 12);
      vbox.Add(hbox);

      table = new GimpTable(3, 4, false)
	{ColumnSpacing = 6, RowSpacing = 6};
      hbox.PackStart(table, false, false, 0);

      _left = new Label("    ");
      table.AttachAligned(0, 0, _("Left:"), 0.0, 0.5, _left, 1, false);
      
      _right = new Label("    ");
      table.AttachAligned(0, 1, _("Right:"), 0.0, 0.5, _right, 1, false);
      
      _top = new Label("    ");
      table.AttachAligned(2, 0, _("Top:"), 0.0, 0.5, _top, 1, false);

      _bottom = new Label("    ");
      table.AttachAligned(2, 1, _("Bottom:"), 0.0, 0.5, _bottom, 1, false);
      
      _include = new CheckButton(_("_Include cell in table"));
      _include.Active = true;
      table.Attach(_include, 0, 3, 2, 3);
      
      return frame;
    }

    Widget CreateRollover()
    {
      GimpFrame frame = new GimpFrame(_("Rollovers"));

      VBox vbox = new VBox(false, 12);
      frame.Add(vbox);

      Button button = new Button(_("Rollover Creator..."));
      button.Clicked += OnRolloverCreate;
      vbox.Add(button);

      Label label = new Label(_("Rollover enabled: no"));
      vbox.Add(label);

      return frame;
    }

    void OnRolloverCreate(object o, EventArgs args)
    {
      RolloverDialog dialog = new RolloverDialog();
      dialog.SetRectangleData(_sliceData.Selected);
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      if (type == ResponseType.Ok)
	{
	  dialog.GetRectangleData(_sliceData.Selected);
	}
      dialog.Destroy();
    }

    void OnSaveSettings(object o, EventArgs args)
    {
      FileChooserDialog fc = 
	new FileChooserDialog(_("Save Settings"), Dialog,
			      FileChooserAction.Save,
			      "Cancel", ResponseType.Cancel,
			      "Save", ResponseType.Accept);
      
      if (fc.Run() == (int) ResponseType.Accept)
	{
	  _sliceData.SaveSettings(fc.Filename);
	}
      fc.Destroy();
    }

    void OnLoadSettings(object o, EventArgs args)
    {
      FileChooserDialog fc = 
	new FileChooserDialog(_("Load Settings"), Dialog,
			      FileChooserAction.Open,
			      "Cancel", ResponseType.Cancel,
			      "Open", ResponseType.Accept);
      
      if (fc.Run() == (int) ResponseType.Accept)
	{
	  _sliceData.LoadSettings(fc.Filename);
	  Redraw();
	}
      fc.Destroy();
    }

    void OnPreferences(object o, EventArgs args)
    {
      PreferencesDialog dialog = new PreferencesDialog();
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      if (type == ResponseType.Ok)
	{
	  Preview.Renderer.ActiveColor = dialog.ActiveColor;
	  Preview.Renderer.InactiveColor = dialog.InactiveColor;
	  Redraw();
	}
      dialog.Destroy();
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

      factory.Add(stockId, set);
    }

    void CreateStockIcons()
    {
      IconFactory factory = new IconFactory();
      factory.AddDefault();
      AddStockIcon(factory, "slice-tool-arrow", "stock-arrow.png");
    }

    void Redraw()
    {
      Preview.QueueDraw();
    }

    public void Redraw(PreviewRenderer renderer)
    {
      _sliceData.Draw(renderer);
    }

    public void SetRectangleData(Rectangle rectangle)
    {
      if (rectangle != null)
	{
	  rectangle.SetProperty("href", _url.Text);
	  rectangle.SetProperty("AltText", _altText.Text);
	  rectangle.SetProperty("Target", _target.Text);
	  rectangle.Include = _include.Active;
	  if (!_format.Apply)
	    {
	      rectangle.Extension = _format.Extension;
	    }
	}
    }

    public void GetRectangleData(Rectangle rectangle)
    {
      _url.Text = rectangle.GetProperty("href");
      _altText.Text = rectangle.GetProperty("AltText");
      _target.Text = rectangle.GetProperty("Target");
      _include.Active = rectangle.Include;		
      if (!_format.Apply)
	{
	  _format.Extension = rectangle.Extension;
	}

      _left.Text = rectangle.X1.ToString();
      _right.Text = rectangle.X2.ToString();
      _top.Text = rectangle.Y1.ToString();
      _bottom.Text = rectangle.Y2.ToString();
    }

    void OnShowCoordinates(object o, MotionNotifyEventArgs args)
    {
      int x, y;
      EventMotion ev = args.Event;
      
      if (ev.IsHint) 
	{
	  ModifierType s;
	  ev.Window.GetPointer(out x, out y, out s);
	} 
      else 
	{
	  x = (int) ev.X;
	  y = (int) ev.Y;
	}
      
      _xy.Text = "x: " + x + ", y: " + y;
      args.RetVal = true;

      SetCursor(new Coordinate<int>(x, y));
    }

    void SetCursor(Coordinate<int> c)
    {
      Preview.SetCursor(Func.GetCursor(c));
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Fix me. Only used to fill in _image and _drawable;
    }
  }
}
