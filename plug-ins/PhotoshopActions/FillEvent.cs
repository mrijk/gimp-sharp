// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// FillEvent.cs
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

namespace Gimp.PhotoshopActions
{
  public class FillEvent : ActionEvent
  {
    [Parameter("Usng")]
    EnumParameter _using;
    [Parameter("Opct")]
    double _opacity;
    [Parameter("Md")]
    EnumParameter _mode;

    protected override void FillParameters(TreeStore store, TreeIter iter)
    {
      if (_using != null)
	{
	  string color;
	  switch(_using.Value)
	    {
	    case "FrgC":
	      color = "foreground color";
	      break;
	    case "BckC":
	      color = "background color";
	      break;
	    default:
	      color = "Fixme: " + _using.Value;
	      break;
	    }
	  store.AppendValues(iter, "Using: " + color);
	}

      store.AppendValues(iter, "Opacity: " + _opacity + " %");

      if (_mode != null)
	{
	  string mode;
	  switch (_mode.Value)
	    {
	    case "Nrml":
	      mode = "Normal";
	      break;
	    default:
	      mode = "Fixme: " + _mode.Value;
	      break;
	    }
	  store.AppendValues(iter, "Mode: " + mode);
	}
    }

    override public bool Execute()
    {
      switch (_using.Value)
	{
	case "FrgC":
	  ActiveDrawable.EditFill(FillType.Foreground);
	  break;
	case "BckC":
	  ActiveDrawable.EditFill(FillType.Background);
	  break;
	default:
	  Console.WriteLine("FillEvent: with {0} not supported!", _using);
	  break;
	}
      return true;
    }
  }
}
