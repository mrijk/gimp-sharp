using System;

using Gtk;

namespace Gimp.SliceTool
{
  public class RolloverEntry : FileEntry
  {
    public RolloverEntry(GimpTable table, string label, uint row) : 
      base("Select Image", "", false, true)
    {
      CheckButton button = new CheckButton(label);
      button.Clicked += new EventHandler(OnClick);
      table.Attach(button, 0, 1, row, row + 1);

      Sensitive = false;
      table.Attach(this, 1, 2, row, row + 1);
    }

    void OnClick(object o, EventArgs args)
    {
      bool active = (o as CheckButton).Active;
      Sensitive = active;

      if (!active)
	{
	FileName = "";
	}
    }
  }
  }
