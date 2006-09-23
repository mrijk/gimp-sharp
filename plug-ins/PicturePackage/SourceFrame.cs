// The PicturePackage plug-in
// Copyright (C) 2004-2006 Maurits Rijk
//
// SourceFrame.cs
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

using Gtk;

namespace Gimp.PicturePackage
{
  public class SourceFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    ImageComboBox _imageBox;
    Button _refresh;
    CheckButton _include;
    FileChooserButton _choose;

    string _fileName = "";
    string _directory = "";
    bool _recursive = false;

    public SourceFrame(PicturePackage parent) : base(3, 2, "Source")
    {
      _parent = parent;

      _table.ColumnSpacing = 12;

      RadioButton button = new RadioButton(_("_Image"));
      button.Clicked += OnImageClicked;
      Table.Attach(button, 0, 1, 0, 1);

      HBox hbox = new HBox();
      Table.Attach(hbox, 1, 2, 0, 1);

      _imageBox = new ImageComboBox();
      _imageBox.Changed += OnImageChanged;
      hbox.Add(_imageBox);

      _refresh = new Button();
      Gtk.Image image = new Gtk.Image(Stock.Refresh, IconSize.Button);
      _refresh.Add(image);
      _refresh.Clicked += OnRefreshClicked;
      hbox.PackEnd(_refresh, false, false, 0);

      button = new RadioButton(button, _("_File"));
      button.Clicked += OnFileClicked;
      Table.Attach(button, 0, 1, 1, 2);

      button = new RadioButton(button, _("Fol_der"));
      button.Clicked += OnFolderClicked;
      Table.Attach(button, 0, 1, 2, 3);

      _include = new CheckButton(_("_Include All Subfolders"));
      _include.Active = _recursive;
      _include.Sensitive = false;
      _include.Toggled += OnIncludeToggled;
      Table.Attach(_include, 1, 2, 2, 3);

      SetFileEntry(false);
      _choose.Sensitive = false;
    }

    void SetFileEntry(bool isDir)
    {
      if (_choose != null)
	{
	  _choose.Hide();
	}

      if (isDir)
	{
	  _choose = new FileChooserButton(_("Open..."), 
					  FileChooserAction.SelectFolder);
	  _choose.SelectionChanged += OnDirNameChanged;
	  // _choose.FileName = _directory;
	}
      else
	{
	  _choose = new FileChooserButton(_("Open..."),
					  FileChooserAction.Open);
	  _choose.SelectionChanged += OnFileNameChanged;
	  // _choose.FileName = _fileName;
	}

      _choose.Show();
      Table.Attach(_choose, 1, 2, 1, 2, AttachOptions.Shrink,
		   AttachOptions.Fill, 0, 0);	
    }

    void OnImageChanged (object o, EventArgs args) 
    {
      _parent.Loader = new FrontImageProviderFactory(_imageBox.Active);
    }

    void OnImageClicked (object o, EventArgs args) 
    {
      _parent.Loader = new FrontImageProviderFactory(_imageBox.Active);
      _imageBox.Sensitive = true;
      _refresh.Sensitive = true;
      _include.Sensitive = false;
      _choose.Sensitive = false;
    }

    void OnRefreshClicked (object o, EventArgs args) 
    {
      HBox hbox = _imageBox.Parent as HBox;
      _imageBox.Destroy();
      _imageBox = new ImageComboBox();
      _imageBox.Changed += new EventHandler(OnImageChanged);
      hbox.Add(_imageBox);
      _imageBox.Show();
      _parent.Loader = new FrontImageProviderFactory(_imageBox.Active);
    }

    void OnFileClicked (object o, EventArgs args) 
    {
      SetFileEntry(false);
      _imageBox.Sensitive = false;
      _refresh.Sensitive = false;
      _include.Sensitive = false;
      _choose.Sensitive = true;
    }

    void OnFolderClicked (object o, EventArgs args) 
    {
      SetFileEntry(true);
      _imageBox.Sensitive = false;
      _refresh.Sensitive = false;
      _include.Sensitive = true;
      _choose.Sensitive = true;
    }

    void OnIncludeToggled (object o, EventArgs args) 
    {
      _recursive = (o as CheckButton).Active;
    }

    void OnFileNameChanged (object o, EventArgs args) 
    {
      _fileName = _choose.Filename;
      if (_fileName.Length > 0)
	{
	  _parent.Loader = new FileImageProviderFactory(_fileName);
	}
    }

    void OnDirNameChanged (object o, EventArgs args) 
    {
      _directory = _choose.Filename;
      if (_directory.Length > 0)
	{
	  _parent.Loader = new DirImageProviderFactory(_directory, _recursive);
	}
    }

    public Image Image
    {
      get {return _imageBox.Active;}
    }
  }
}
