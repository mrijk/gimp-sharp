// The Slice Tool plug-in
// Copyright (C) 2004-2007 Maurits Rijk  m.rijk@chello.nl
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
    MouseFunc _func;
    
    SliceData _sliceData = new SliceData();
    
    ToggleToolButton _toggle;
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
    
    static void Main(string[] args)
    {
      new SliceTool(args);
    }
    
    public SliceTool(string[] args) : base(args, "SliceTool")
    {
    }

    override protected IEnumerable<Procedure> ListProcedures()
    {
      Procedure procedure = new Procedure("plug_in_slice_tool",
					  _("Slice Tool"),
					  _("The Image Slice Tool is used to apply image slicing and rollovers."),
					  "Maurits Rijk",
					  "(C) Maurits Rijk",
					  "2005-2006",
					  _("Slice Tool..."),
					  "RGB*, GRAY*");

      procedure.MenuPath = "<Image>/Filters/Web";
      procedure.IconFile = "SliceTool.png";

      yield return procedure;
    }
    
    override protected GimpDialog CreateDialog()
    {
      gimp_ui_init("SliceTool", true);
      
      CreateStockIcons();
      
      GimpDialog dialog = DialogNew(_("Slice Tool"), _("SliceTool"), 
				    IntPtr.Zero, 0, null, _("SliceTool"),
				    Stock.SaveAs, (Gtk.ResponseType) 0,
				    Stock.Save, (Gtk.ResponseType) 1,
				    Stock.Close, ResponseType.Close);
      
      SetTitle(null);
      
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
      hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      _xy = new Entry();
      _xy.WidthChars = 16;
      _xy.IsEditable = false;
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
      
      _func = new SelectFunc(this, _sliceData, _preview);
      
      return dialog;
    }
    
    // Fix me: move this to Plugin class?!
    void SetTitle(string filename)
    {
      _filename = filename;
      string p = (filename == null) 
	? "<Untitled>" : System.IO.Path.GetFileName(filename);
      string title = string.Format(_("Slice Tool 0.3 - {0}"), p);
      Dialog.Title = title;
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
      try
	{
	  _sliceData.Save(_filename, _format.Apply, _image, _drawable);
	  SaveBlank(System.IO.Path.GetDirectoryName(_filename));
	}
      catch (Exception e)
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
	  FileSelection fs = new FileSelection(_("HTML Save As"));
	  ResponseType response = (ResponseType) fs.Run();
	  if (response == ResponseType.Ok)
	    {
	      string filename = fs.Filename;
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
	  fs.Destroy();
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

      _preview = new Preview(_drawable, this);
      _preview.WidthRequest = _drawable.Width;
      _preview.HeightRequest = _drawable.Height;

      _preview.ButtonPressEvent += OnButtonPress;      
      _preview.MotionNotifyEvent += OnShowCoordinates;
      _preview.LeaveNotifyEvent += 
	delegate(object o, LeaveNotifyEventArgs args)
	{
	  _xy.Text = "";
	};
      
      alignment.Add(_preview);
      window.AddWithViewport(alignment);

      return window;
    }
    
    void OnButtonPress(object o, ButtonPressEventArgs args)
    {
      MouseFunc func = _func.GetActualFunc(this, (int) args.Event.X,
					   (int) args.Event.Y);
      func.OnButtonPress(o, args);
    }

    Widget CreateToolbar()
    {
      HandleBox handle = new HandleBox();
      
      Toolbar tools = new Toolbar();
      tools.Orientation = Gtk.Orientation.Vertical;
      tools.ToolbarStyle = Gtk.ToolbarStyle.Icons;
      handle.Add(tools);

      Tooltips tooltips = new Tooltips();
      // TODO: tootips don't work anymore :(

      ToggleToolButton toggle = new ToggleToolButton("slice-tool-arrow");
      _toggle = toggle;
      toggle.Active = true;
      toggle.SetTooltip(tooltips, _("Select Rectangle"), "arrow");
      tools.Insert(toggle, -1);
      toggle.Clicked += OnSelect;
      
      toggle = new ToggleToolButton(GimpStock.TOOL_CROP);
      tooltips.SetTip(toggle, _("Create a new Slice"), "create");
      // toggle.SetTooltip(tooltips, "Create a new Slice", "create");
      tools.Insert(toggle, -1);
      toggle.Clicked += OnCreateSlice;
      
      toggle = new ToggleToolButton(GimpStock.TOOL_ERASER);
      toggle.SetTooltip(tooltips, _("Remove Slice"), "delete");
      tools.Insert(toggle, -1);
      toggle.Clicked += OnRemoveSlice;

      toggle = new ToggleToolButton(GimpStock.GRID);
      toggle.SetTooltip(tooltips, _("Insert Table"), "grid");
      tools.Insert(toggle, -1);
      toggle.Clicked += OnCreateTable;

      return handle;
    }
    
    Widget CreateCellProperties()
    {
      GimpFrame frame = new GimpFrame(_("Cell Properties"));

      VBox vbox = new VBox(false, 12);
      frame.Add(vbox);

      GimpTable table = new GimpTable(3, 2, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      
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

      table = new GimpTable(3, 4, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
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
      FileSelection fs = new FileSelection(_("Save Settings"));
      ResponseType type = (ResponseType) fs.Run();
      if (type == ResponseType.Ok)
	{
	  _sliceData.SaveSettings(fs.Filename);
	}
      fs.Destroy();
    }

    void OnLoadSettings(object o, EventArgs args)
    {
      FileSelection fs = new FileSelection(_("Load Settings"));
      ResponseType type = (ResponseType) fs.Run();
      if (type == ResponseType.Ok)
	{
	  _sliceData.LoadSettings(fs.Filename);
	  Redraw();
	}
      fs.Destroy();
    }

    void OnPreferences(object o, EventArgs args)
    {
      PreferencesDialog dialog = new PreferencesDialog();
      dialog.ShowAll();
      ResponseType type = dialog.Run();
      if (type == ResponseType.Ok)
	{
	  _preview.Renderer.ActiveColor = dialog.ActiveColor;
	  _preview.Renderer.InactiveColor = dialog.InactiveColor;
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

    bool _lock;
    void OnFunc(object o, MouseFunc func)
    {
      if (!_lock)
	{
	  _lock = true;
	  ToggleToolButton toggle = (o as ToggleToolButton);
	  if (toggle != _toggle)
	    {
	      _toggle.Active = false;
	      _toggle = toggle;
	      _func = func;
	    } 
	  else
	    {
	      _toggle.Active = true;
	    }
	  _lock = false;
	}
    }

    void OnSelect(object o, EventArgs args)
    {
      OnFunc(o, new SelectFunc(this, _sliceData, _preview));
    }

    void OnCreateSlice(object o, EventArgs args)
    {
      OnFunc(o, new CreateFunc(_sliceData, _preview));
    }

    void OnRemoveSlice(object o, EventArgs args)
    {
      OnFunc(o, new RemoveFunc(_sliceData, _preview));
    }

    void OnCreateTable(object o, EventArgs args)
    {
      OnFunc(o, new CreateTableFunc(_sliceData, _preview));
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

      SetCursor(x, y);
    }

    void SetCursor(int x, int y)
    {
      _preview.SetCursor(_func.GetCursor(x, y));
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Fix me. Only used to fill in _image and _drawable;
    }
  }
}
