using System;

using Gtk;

namespace Gimp.PicturePackage
{
  public class SourceFrame : GimpFrame
  {
    CheckButton _include;
    FileEntry _choose;

    public SourceFrame() : base("Source")
    {
      GimpTable table = new GimpTable(2, 3, false);
      table.ColumnSpacing = 6;
      table.RowSpacing = 6;
      Add(table);

      OptionMenu use = new OptionMenu();
      Menu menu = new Menu();
      menu.Append(new MenuItem("File"));
      menu.Append(new MenuItem("Folder"));
      menu.Append(new MenuItem("Frontmost Document"));
      use.Menu = menu;
      use.SetHistory(2);
      table.AttachAligned(0, 0, "_Use:", 0.0, 0.5, use, 1, false);
      use.Changed += new EventHandler(OnUseChanged);

      _include = new CheckButton("_Include All Subfolders");
      table.Attach(_include, 1, 2, 1, 2);

      _choose = new FileEntry("Open...", "", true, true);
      table.Attach(_choose, 1, 2, 2, 3, AttachOptions.Shrink,
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
    }
  }
  }
