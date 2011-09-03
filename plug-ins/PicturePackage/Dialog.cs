// The PicturePackage plug-in
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

    DialogStateType _currentDialogState = DialogStateType.SrcImgInvalid;

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

      _preview = new Preview(this, variables) 
	{
	  WidthRequest = 400, 
	  HeightRequest = 500,
	  TooltipText = _("Right click to select picture")
	};
      fbox.Add(_preview);
      
      _preview.ButtonPressEvent += PreviewClicked;
      _preview.DragDataReceived += OnDragDataReceived;

      layouts.Selected = layouts[0];
      _layout.Value = layouts[0];
      layouts.SelectEvent += SetLayout;

      DialogState = DialogStateType.SrcImgInvalid;

      loader.ValueChanged += delegate {RedrawPreview();};
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
	      var fc = new FileChooserDialog(_("Select image"), 
					     this, FileChooserAction.Open,
					     _("Cancel"), ResponseType.Cancel,
					     _("Open"), ResponseType.Accept);
	  
	      if (fc.Run() == (int) ResponseType.Accept)
		{
		  RenderRectangle(_rectangle, fc.Filename);
		  _preview.QueueDraw();
		}
	      fc.Destroy();
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
	  renderer.Cleanup();
	  provider.Release();
	}
      else
	{
	  //	  Console.WriteLine("Couldn't load: " + filename);
	  // Error dialog here.
	}		
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
