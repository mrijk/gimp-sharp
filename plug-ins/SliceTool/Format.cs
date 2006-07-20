// The Slice Tool plug-in
// Copyright (C) 2004-2006 Maurits Rijk  m.rijk@chello.nl
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

using System;
using Mono.Unix;
using Gtk;

namespace Gimp.SliceTool
{
  public class Format : GimpFrame
  {
    ComboBox _format;
    CheckButton _apply;

    public Format() : base(Catalog.GetString("Format"))
    {
      Table table = new Table(2, 2, true);
      table.RowSpacing = 6;
      Add(table);

      _format = ComboBox.NewText();
      table.Attach(_format, 0, 1, 0, 1);

      _format.AppendText("gif");
      _format.AppendText("jpg");
      _format.AppendText("png");

      _apply = new CheckButton(Catalog.GetString("Apply to whole image"));
      _apply.Activated += OnApply;
      table.Attach(_apply, 0, 2, 1, 2);
    }

    void OnApply(object o, EventArgs args)
    {
      if ((o as CheckButton).Active)
	{
	  Rectangle.GlobalExtension = Extension;
	}
    }

    public string Extension
    {
      set
	{
	  if (value == ".gif")
	    {
	      _format.Active = 0;
	    }
	  else if (value == ".jpg" || value == ".jpeg")
	    {
	      _format.Active = 1;
	    }
	  else if (value == ".png")
	    {
	      _format.Active = 2;
	    }
	  else
	    {
	      _format.Active = 2;
	    }

	  if (_apply.Active)
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
      get
	{
	  return _apply.Active;
	}
    }
  }
}
