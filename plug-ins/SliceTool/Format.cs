// The Slice Tool plug-in
// Copyright (C) 2004-2011 Maurits Rijk
//
// Format.cs
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

using Gtk;

namespace Gimp.SliceTool
{
  public class Format : GimpFrame
  {
    readonly ComboBox _format;
    readonly CheckButton _apply;

    public Format(RectangleSet rectangles) : base(_("File Type"))
    {
      var table = new Table(2, 2, true) {RowSpacing = 6};
      Add(table);

      _format = new ComboBox(new string[]{"gif", "jpg", "png"});
      table.Attach(_format, 0, 1, 0, 1);

      _apply = new CheckButton(_("Apply to _whole image"));
      _apply.Activated += delegate
	{
	  Rectangle.GlobalExtension = Extension;
	};
      table.Attach(_apply, 0, 2, 1, 2);

      rectangles.SelectedRectangleChanged += SelectedRectangleChanged;
    }

    void SelectedRectangleChanged(object sender, SelectedChangedEventArgs args)
    {
      if (!Apply)
	{
	  args.OldSelected.Extension = Extension;
	  Extension = args.NewSelected.Extension;
	}
    }

    public string Extension
    {
      set
	{
	  switch (value)
	    {
	    case "gif":
	      _format.Active = 0;
	      break;
	    case "jpg":
	      _format.Active = 1;
	      break;
	    case "jpeg":
	      _format.Active = 1;
	      break;
	    case "png":
	      _format.Active = 2;
	      break;
	    default:
	      _format.Active = 2;
	      break;
	    }

	  if (Apply)
	    {
	      Rectangle.GlobalExtension = value;
	    }
	}

      get
	{
	  switch (_format.Active)
	    {
	    case 0:
	      return "gif";
	    case 1:
	      return "jpg";
	    case 2:
	      return "png";
	    default:
	      return null;
	    }
	}
    }

    public bool Apply
    {
      get {return _apply.Active;}
    }
  }
}
