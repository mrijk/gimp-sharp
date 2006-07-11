// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// StrokeEvent.cs
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class StrokeEvent : ActionEvent
  {
    [Parameter("Wdth")]
    int _width;
    [Parameter("Lctn")]
    EnumParameter _location;
    [Parameter("Opct")]
    double _opacity;
    [Parameter("Md")]
    EnumParameter _mode;
    [Parameter("Clr")]
    ObjcParameter _color;

    public override bool IsExecutable
    {
      get {return false;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Width: " + _width;
      string location;
      switch (_location.Value)
	{
	case "Otsd":
	  location = "outside";
	  break;
	case "Cntr":
	  location = "inside";
	  break;
	default:
	  location = _location.Value;
	  break;
	}
      yield return "Location: " + location;
      yield return "Opacity: " + _opacity;
      yield return "Mode: " + _mode.Value;
    }

    override public bool Execute()
    {
      Context.Push();

      if (_color != null)
	{
	  RGB foreground = _color.GetColor();
#if true
	  if (foreground != null)
	    {
	      Console.WriteLine("Ok2!");
	      Context.Foreground = foreground;
	    }
#endif
	}
      else
	{
	  Console.WriteLine("No color!");
	}

      Context.Opacity = _opacity;
      ActiveDrawable.EditStroke();
      Context.Pop();

      return true;
    }
  }
}
