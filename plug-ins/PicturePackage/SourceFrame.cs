using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class SourceFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    ImageComboBox _imageBox;
    CheckButton _include;
    FileEntry _choose;

    bool _recursive = false;

    public SourceFrame(PicturePackage parent) : base(2, 3, "Source")
    {
      _parent = parent;

      _table.ColumnSpacing = 12;

      RadioButton button = new RadioButton("Image");
      button.Clicked += new EventHandler(OnImageClicked);
      Table.Attach(button, 0, 1, 0, 1);

      _imageBox = new ImageComboBox();
      _imageBox.Changed += new EventHandler(OnImageChanged);
      Table.Attach(_imageBox, 1, 2, 0, 1);

      button = new RadioButton(button, "File");
      button.Clicked += new EventHandler(OnFileClicked);
      Table.Attach(button, 0, 1, 1, 2);

      button = new RadioButton(button, "Folder");
      button.Clicked += new EventHandler(OnFolderClicked);
      Table.Attach(button, 0, 1, 2, 3);

      _include = new CheckButton("_Include All Subfolders");
      _include.Active = _recursive;
      _include.Sensitive = false;
      _include.Toggled += new EventHandler(OnIncludeToggled);
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
	_choose = new FileEntry("Open...", "", true, true);
	_choose.FilenameChanged += new EventHandler(OnDirNameChanged);
	}
      else
	{
	_choose = new FileEntry("Open...", "", false, true);
	_choose.FilenameChanged += new EventHandler(OnFileNameChanged);
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
      // _parent.Loader = new FrontImageProviderFactory(_parent.Image);
      _imageBox.Sensitive = true;
      _include.Sensitive = false;
      _choose.Sensitive = false;
    }

    void OnFileClicked (object o, EventArgs args) 
    {
      SetFileEntry(false);
      _imageBox.Sensitive = false;
      _include.Sensitive = false;
      _choose.Sensitive = true;
    }

    void OnFolderClicked (object o, EventArgs args) 
    {
      SetFileEntry(true);
      _imageBox.Sensitive = false;
      _include.Sensitive = true;
      _choose.Sensitive = true;
    }

    void OnIncludeToggled (object o, EventArgs args) 
    {
      _recursive = (o as CheckButton).Active;
    }

    void OnFileNameChanged (object o, EventArgs args) 
    {
      string fileName = _choose.FileName;
      if (fileName.Length > 0)
	{
	_parent.Loader = new FileImageProviderFactory(fileName);
	}
    }

    void OnDirNameChanged (object o, EventArgs args) 
    {
      string directory = _choose.FileName;
      if (directory.Length > 0)
	{
	_parent.Loader = new DirImageProviderFactory(directory, _recursive);
	}
    }

    public Image Image
    {
      get {return _imageBox.Active;}
    }
  }
  }
