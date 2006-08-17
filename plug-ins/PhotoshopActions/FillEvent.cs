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
using System.Collections;

namespace Gimp.PhotoshopActions
{
  public class FillEvent : ActionEvent
  {
    [Parameter("From")]
    ObjcParameter _from;
    [Parameter("Tlrn")]
    int _tolerance;
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

      if (_using != null)
	{
	  yield return "Using: " + Abbreviations.Get(_using.Value);
	}

      yield return "Opacity: " + _opacity + " %";

      if (_mode != null)
	{
	  yield return "Mode: " + Abbreviations.Get(_mode.Value);
	}
    }

    override public bool Execute()
    {
      Context.Push();

      FillType fillType;

      switch (_using.Value)
	{
	case "Blck":
	  Context.Foreground = new RGB(0, 0, 0);
	  fillType = FillType.Foreground;
	  break;
	case "FrgC":
	  fillType = FillType.Foreground;
	  break;
	case "BckC":
	  fillType = FillType.Background;
	  break;
	default:
	  fillType = FillType.Foreground;
	  Console.WriteLine("FillEvent: with {0} not supported!", _using);
	  break;
	}

      if (_from == null)
	{
	  ActiveDrawable.EditFill(fillType);
	}
      else
	{
	  if (Gimp.Version > new Version("2.3.10"))
	    {
	      double x = _from.GetValueAsDouble("Hrzn");
	      double y = _from.GetValueAsDouble("Vrtc");
	      
	      ActiveDrawable.EditBucketFill(BucketFillMode.Foreground,
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
