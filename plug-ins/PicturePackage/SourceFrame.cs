using System;

using Gtk;

namespace Gimp.PicturePackage
{
	public class SourceFrame : PicturePackageFrame
	{
		PicturePackage _parent;
		CheckButton _include;
		FileEntry _choose;

		bool _recursive = false;
		uint _useHistory = 2;

		public SourceFrame(PicturePackage parent) : base(2, 3, "Source")
		{
			_parent = parent;
			OptionMenu use = CreateOptionMenu(
				"File", "Folder", "Frontmost Document");
			use.SetHistory(_useHistory);
			Table.AttachAligned(0, 0, "_Use:", 0.0, 0.5, use, 1, false);
			use.Changed += new EventHandler(OnUseChanged);

			_include = new CheckButton("_Include All Subfolders");
			_include.Active = _recursive;
			_include.Toggled += new EventHandler(OnIncludeToggled);
			Table.Attach(_include, 1, 2, 1, 2);

			SetFileEntry(false);
			SetSourceFrameSensitivity();		
		}

		void SetSourceFrameSensitivity()
		{
			if (_useHistory == 0)
			{
				_include.Sensitive = false;
				_choose.Sensitive = true;
			}
			else if (_useHistory == 1)
			{
				_include.Sensitive = true;
				_choose.Sensitive = true;
			}
			else
			{
				_include.Sensitive = false;
				_choose.Sensitive = false;
			}
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
			Table.Attach(_choose, 1, 2, 2, 3, AttachOptions.Shrink,
				AttachOptions.Fill, 0, 0);	
		}

		void OnUseChanged (object o, EventArgs args) 
		{
			_useHistory = (uint) (o as OptionMenu).History;

			SetSourceFrameSensitivity();

			if (_useHistory == 0)
			{
				SetFileEntry(false);
			}
			else if (_useHistory == 1)
			{
				SetFileEntry(true);
			}
			else
			{
				_parent.Loader = new FrontImageProviderFactory(_parent.Image);
			}
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
	}
}