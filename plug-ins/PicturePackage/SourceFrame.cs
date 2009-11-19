// The PicturePackage plug-in
// Copyright (C) 2004-2009 Maurits Rijk
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

      Table.ColumnSpacing = 12;

      var imageButton = CreateImageButton();
      Attach(imageButton, 0, 1, 0, 1);

      var hbox = new HBox();
      Attach(hbox, 1, 2, 0, 1);

      _imageBox = CreateImageComboBox();
      hbox.Add(_imageBox);
      _refresh = CreateRefreshButton();
      hbox.PackEnd(_refresh, false, false, 0);

      var fileButton = CreateFileButton(imageButton);
      Attach(fileButton, 0, 1, 1, 2);

      var folderButton = CreateFolderButton(fileButton);
      Attach(folderButton, 0, 1, 2, 3);

      _include = CreateIncludeToggleButton();
      Attach(_include, 1, 2, 2, 3);

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

    ImageComboBox CreateImageComboBox()
    {
      var imageBox = new ImageComboBox();
      imageBox.Changed += delegate
	{
	  _parent.Loader = new FrontImageProviderFactory(imageBox.Active);
	};
      return imageBox;
    }

    RadioButton CreateImageButton()
    {
      var button = new RadioButton(_("_Image"));
      button.Clicked += delegate
	{
	  _parent.Loader = new FrontImageProviderFactory(_imageBox.Active);
	  _imageBox.Sensitive = true;
	  _refresh.Sensitive = true;
	  _include.Sensitive = false;
	  _choose.Sensitive = false;
	};
      return button;
    }

    Button CreateRefreshButton()
    {
      var refresh = new Button();
      var image = new Gtk.Image(Stock.Refresh, IconSize.Button);
      refresh.Add(image);
      refresh.Clicked += delegate
	{
	  var hbox = _imageBox.Parent as HBox;
	  _imageBox.Destroy();
	  _imageBox = CreateImageComboBox();
	  hbox.Add(_imageBox);
	  _imageBox.Show();
	  _parent.Loader = new FrontImageProviderFactory(_imageBox.Active);
	};
      return refresh;
    }

    RadioButton CreateFileButton(RadioButton previous)
    {
      var button = new RadioButton(previous, _("_File"));
      button.Clicked += delegate
	{
	  SetFileEntry(false);
	  _imageBox.Sensitive = false;
	  _refresh.Sensitive = false;
	  _include.Sensitive = false;
	  _choose.Sensitive = true;
	};
      return button;
    }

    RadioButton CreateFolderButton(RadioButton previous)
    {
      var button = new RadioButton(previous, _("Fol_der"));
      button.Clicked += delegate 
	{
	  SetFileEntry(true);
	  _imageBox.Sensitive = false;
	  _refresh.Sensitive = false;
	  _include.Sensitive = true;
	  _choose.Sensitive = true;
	};
      return button;
    }

    CheckButton CreateIncludeToggleButton()
    {
      var include = new CheckButton(_("_Include All Subfolders"))
	{Active = _recursive, Sensitive = false};
      include.Toggled += delegate {_recursive = include.Active;};
      return include;
    }

    void OnFileNameChanged(object o, EventArgs args) 
    {
      _fileName = _choose.Filename;
      if (_fileName.Length > 0)
	{
	  _parent.Loader = new FileImageProviderFactory(_fileName);
	}
    }

    void OnDirNameChanged(object o, EventArgs args) 
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
