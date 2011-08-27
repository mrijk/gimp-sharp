// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
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
using System.IO;
using System.Reflection;

using Gtk;

namespace Gimp.SliceTool
{
  public class Dialog : GimpDialog
  {
    public Preview Preview {private set; get;}

    Format _format;

    readonly SliceData _sliceData;
    readonly Image _image;
    readonly Drawable _drawable;

    string _filename = null;

    public Dialog(Image image, Drawable drawable, SliceData sliceData) :
      base(_("Slice Tool"), _("SliceTool"), 
	   IntPtr.Zero, 0, null, _("SliceTool"),
	   Stock.SaveAs, (Gtk.ResponseType) 0,
	   Stock.Save, (Gtk.ResponseType) 1,
	   Stock.Close, ResponseType.Close)
    {
      _image = image;
      _drawable = drawable;
      _sliceData = sliceData;

      SetTitle(null);

      var vbox = new VBox(false, 12) {BorderWidth = 12};
      VBox.PackStart(vbox, true, true, 0);

      var hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      
      var preview = CreatePreview(drawable, sliceData);
      var toolbox = Preview.CreateToolbox(sliceData);

      hbox.PackStart(toolbox, false, true, 0);
      hbox.PackStart(preview, true, true, 0);

      hbox = new HBox();
      vbox.PackStart(hbox, true, true, 0);
      hbox.PackStart(new CoordinatesDisplay(Preview), false, false, 0);

      hbox = new HBox(false, 24);
      vbox.PackStart(hbox, true, true, 0);
      
      var properties = new CellPropertiesFrame(sliceData.Rectangles);
      hbox.PackStart(properties, false, true, 0);
      
      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);
      
      var rollover = new RolloversFrame(sliceData);
      vbox.PackStart(rollover, false, true, 0);
      
      _format = new Format(sliceData.Rectangles);
      _format.Extension = System.IO.Path.GetExtension(image.Name).ToLower();
      vbox.PackStart(_format, false, true, 0);

      vbox = new VBox(false, 12);
      hbox.PackStart(vbox, false, true, 0);
      
      var save = new SaveSettingsButton(this, sliceData);
      vbox.PackStart(save, false, true, 0);
      
      var load = new LoadSettingsButton(this, sliceData);
      vbox.PackStart(load, false, true, 0);

      var preferences = new PreferencesButton(_("Preferences"), Preview);
      vbox.PackStart(preferences, false, true, 0);

      sliceData.Rectangles.SelectedRectangleChanged += delegate {Redraw();};
      sliceData.Init(drawable);
    }

    void SetTitle(string filename)
    {
      _filename = filename;	// Fix me!
      string p = (filename == null) 
	? _("<Untitled>") : System.IO.Path.GetFileName(filename);
      Title = string.Format(_("Slice Tool 0.6 - {0}"), p);
    }

    Widget CreatePreview(Drawable drawable, SliceData sliceData)
    {
      var window = new ScrolledWindow();
      window.SetSizeRequest(600, 400);

      var alignment = new Alignment(0.5f, 0.5f, 0, 0);

      Preview = new Preview(drawable, sliceData)
	{WidthRequest = drawable.Width, HeightRequest = drawable.Height};
      
      alignment.Add(Preview);
      window.AddWithViewport(alignment);

      return window;
    }

    public void Redraw()
    {
      Preview.QueueDraw();
    }

    public void DialogRun(ResponseType type)
    {
      if ((int) type == 0 || ((int) type == 1 && _filename == null))
	{
	  var fc = new FileChooserDialog(_("HTML Save As"), this,
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
	      Save(filename);
	    }
	  fc.Destroy();
	}
      else // type == 1
	{
	  Save(_filename);
	}
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
    
    void Save(string filename)
    {
      try
	{
	  _sliceData.Save(filename, _format.Apply, _image, _drawable);
	  SaveBlank(System.IO.Path.GetDirectoryName(filename));
	}
      catch (Exception)
	{
	  var message = new MessageDialog(null, DialogFlags.DestroyWithParent,
					  MessageType.Error, ButtonsType.Close,
					  _("Can't save to ") + filename);
	  message.Run();
	  message.Destroy();
	}
    }
  }
}
