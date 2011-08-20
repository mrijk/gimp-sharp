// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.IO;
using System.Reflection;

using Gtk;

namespace Gimp.SliceTool
{
  public class SliceTool : Plugin
  {
    public Preview Preview {private set; get;}
    
    SliceData _sliceData = new SliceData();

    Format _format;
    
    string _filename = null;
    
    static void Main(string[] args)
    {
      GimpMain<SliceTool>(args);
    }

    override protected Procedure GetProcedure()
    {
      return new Procedure("plug_in_slice_tool",
			   _("Slice Tool"),
			   _("The Image Slice Tool is used to apply image slicing and rollovers."),
			   "Maurits Rijk",
			   "(C) Maurits Rijk",
			   "2005-2011",
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
            
      var dialog = DialogNew(_("Slice Tool"), _("SliceTool"), 
			     IntPtr.Zero, 0, null, _("SliceTool"),
			     Stock.SaveAs, (Gtk.ResponseType) 0,
			     Stock.Save, (Gtk.ResponseType) 1,
			     Stock.Close, ResponseType.Close);
      
      SetTitle(null);

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      dialog.VBox.PackStart(vbox, true, true, 0);

      var hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      
      var preview = CreatePreview();
      var toolbox = Preview.CreateToolbox();

      hbox.PackStart(toolbox, false, true, 0);
      hbox.PackStart(preview, true, true, 0);
      
      hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      hbox.PackStart(new CoordinatesDisplay(Preview), false, false, 0);

      hbox = new HBox(false, 24);
      vbox.PackStart(hbox, true, true, 0);
      
      var properties = new CellPropertiesFrame(_sliceData.Rectangles);
      hbox.PackStart(properties, false, true, 0);
      
      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);
      
      var rollover = new RolloversFrame(_sliceData);
      vbox.PackStart(rollover, false, true, 0);
      
      _format = new Format(_sliceData.Rectangles);
      _format.Extension = System.IO.Path.GetExtension(_image.Name).ToLower();
      vbox.PackStart(_format, false, true, 0);
      
      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);
      
      var save = new Button(_("Save Settings..."));
      save.Clicked += OnSaveSettings;
      vbox.PackStart(save, false, true, 0);
      
      var load = new Button(_("Load Settings..."));
      load.Clicked += OnLoadSettings;
      vbox.PackStart(load, false, true, 0);

      var preferences = new PreferencesButton(_("Preferences"), Preview);
      vbox.PackStart(preferences, false, true, 0);

      _sliceData.Rectangles.SelectedRectangleChanged += delegate {Redraw();};
      _sliceData.Init(_drawable);
       
      return dialog;
    }
 
    // Fix me: move this to Plugin class?!
    void SetTitle(string filename)
    {
      _filename = filename;
      string p = (filename == null) 
	? _("<Untitled>") : System.IO.Path.GetFileName(filename);
      Dialog.Title = string.Format(_("Slice Tool 0.6 - {0}"), p);
    }
    
    void SaveBlank(string path)
    {
      var assembly = Assembly.GetExecutingAssembly();
      var input = assembly.GetManifestResourceStream("blank.png");
      var reader = new BinaryReader(input);
      var buffer = reader.ReadBytes((int) input.Length);
      string fileName = path + System.IO.Path.DirectorySeparatorChar + 
	"blank.png";
      var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
      var writer = new BinaryWriter(fs);
      writer.Write(buffer);
      writer.Close();
    }
    
    void Save()
    {
      try
	{
	  _sliceData.Save(_filename, _format.Apply, _image, _drawable);
	  SaveBlank(System.IO.Path.GetDirectoryName(_filename));
	}
      catch (Exception)
	{
	  var message = new MessageDialog(null, DialogFlags.DestroyWithParent,
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
	  var message = new MessageDialog(null, DialogFlags.DestroyWithParent,
					  MessageType.Warning, 
					  ButtonsType.YesNo, 
					  _("Some data has been changed!\n") + 
			      _("Do you really want to discard your changes?"));
	  var response = (ResponseType) message.Run();
	  return response == ResponseType.Yes;
	}
      return true;
    }
    
    override protected void DialogRun(ResponseType type)
    {
      if ((int) type == 0 || ((int) type == 1 && _filename == null))
	{
	  var fc = new FileChooserDialog(_("HTML Save As"), Dialog,
					 FileChooserAction.Save,
					 "Cancel", ResponseType.Cancel,
					 "Save", ResponseType.Accept);
      
	  if (fc.Run() == (int) ResponseType.Accept)
	    {
	      string filename = fc.Filename;
	      if (System.IO.File.Exists(filename))
		{
		  var message = new FileExistsDialog(filename);
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
      var window = new ScrolledWindow();
      window.SetSizeRequest(600, 400);

      var alignment = new Alignment(0.5f, 0.5f, 0, 0);

      Preview = new Preview(_drawable, _sliceData)
	{WidthRequest = _drawable.Width, HeightRequest = _drawable.Height};
      
      alignment.Add(Preview);
      window.AddWithViewport(alignment);

      return window;
    }

    void OnSaveSettings(object o, EventArgs args)
    {
      var fc = new FileChooserDialog(_("Save Settings"), Dialog,
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
      var fc = new FileChooserDialog(_("Load Settings"), Dialog,
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

    void Redraw()
    {
      Preview.QueueDraw();
    }

    override protected void Render(Image image, Drawable drawable)
    {
      // Fix me. Only used to fill in _image and _drawable;
    }
  }
}
