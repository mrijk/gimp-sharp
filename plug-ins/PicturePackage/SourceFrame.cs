using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class SourceFrame : PicturePackageFrame
  {
    PicturePackage _parent;
    CheckButton _include;
    FileEntry _choose;

    public SourceFrame(PicturePackage parent) : base(2, 3, "Source")
    {
      _parent = parent;
      OptionMenu use = CreateOptionMenu(
	"File", "Folder", "Frontmost Document");
      use.SetHistory(2);
      Table.AttachAligned(0, 0, "_Use:", 0.0, 0.5, use, 1, false);
      use.Changed += new EventHandler(OnUseChanged);

      _include = new CheckButton("_Include All Subfolders");
      Table.Attach(_include, 1, 2, 1, 2);

      _choose = new FileEntry("Open...", "", true, true);
      Table.Attach(_choose, 1, 2, 2, 3, AttachOptions.Shrink,
		   AttachOptions.Fill, 0, 0);	

      SetSourceFrameSensitivity(2);		
    }

    void SetSourceFrameSensitivity(int history)
    {
      if (history == 0)
	{
	_include.Sensitive = false;
	_choose.Sensitive = true;
	}
      else if (history == 1)
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

    void OnUseChanged (object o, EventArgs args) 
    {
      SetSourceFrameSensitivity((o as OptionMenu).History);

      string directory = _choose.FileName;
      if (directory.Length > 0)
	{
	_parent.LoadFromDirectory(directory);
	}
    }
  }
  }
