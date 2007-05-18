// The PhotoshopActions plug-in
// Copyright (C) 2006-2007 Maurits Rijk
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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class FillEvent : ActionEvent
  {
    [Parameter("From")]
    ObjcParameter _from;
    [Parameter("Tlrn")]
    int _tolerance;
    [Parameter("AntA")]
    bool _antiAlias;
    [Parameter("Usng")]
    EnumParameter _using;
    [Parameter("Opct")]
    double _opacity;
    [Parameter("Md")]
    EnumParameter _mode;

    public override bool IsExecutable
    {
      get 
	{
	  return _from == null || Gimp.Version > new Version("2.3.10");
	}
    }

    protected override IEnumerable ListParameters()
    {
      if (_from != null)
	{
	  yield return "From: ";
	}

      yield return "Tolerance: " + _tolerance;

      yield return Format(_antiAlias, "Anti-alias");

      if (_using != null)
	{
	  yield return Format(_using, "Usng");
	}

      yield return "Opacity: " + _opacity + " %";

      if (_mode != null)
	{
	  yield return Format(_mode, "Md");
	}
    }

    override public bool Execute()
    {
      Context.Push();

      FillType fillType;

      switch (_using.Value)
	{
	case "BckC":
	  fillType = FillType.Background;
	  break;
	case "Blck":
	  Context.Foreground = new RGB(0, 0, 0);
	  fillType = FillType.Foreground;
	  break;
	case "FrgC":
	  fillType = FillType.Foreground;
	  break;
	case "Ptrn":
	  fillType = FillType.Pattern;
	  break;
	case "Wht":
	  Context.Foreground = new RGB(255, 255, 255);
	  fillType = FillType.Foreground;
	  break;
	default:
	  Console.WriteLine("FillEvent: with {0} not supported!", 
			    _using.Value);
	  return false;
	}

      Drawable drawable = ActiveImage.ActiveDrawable;
      if (_from == null)
	{
	  drawable.EditFill(fillType);
	}
      else
	{
	  if (Gimp.Version > new Version("2.3.10"))
	    {
	      double x = _from.GetValueAsDouble("Hrzn");
	      double y = _from.GetValueAsDouble("Vrtc");

	      drawable.EditBucketFill(BucketFillMode.Foreground,
				      LayerModeEffects.Normal,
				      100.0, _tolerance, false, true,
				      SelectCriterion.Composite, x, y);
	    }
	}

      Context.Pop();

      return true;
    }
  }
}
