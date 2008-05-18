// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
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
// along with this program; if not, write to thzze Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//

using System;

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

    override public bool Execute()
    {
      Context.Push();

      Drawable drawable = ActiveImage.ActiveDrawable;
      if (_from == null && _mode == null)
	{
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
	      Console.WriteLine("FillEvent-1: with {0} not supported!", 
				_using.Value);
	      return false;
	    }

	  drawable.EditFill(fillType);
	}
      else
	{
	  if (Gimp.Version > new Version("2.3.10"))
	    {
	      LayerModeEffects layerMode = LayerModeEffects.Normal;
	      double x, y;

	      if (_mode != null)
		{
		  switch (_mode.Value)
		    {
		    case "Mltp":
		      layerMode = LayerModeEffects.Multiply;
		      break;
		    case "Nrml":
		      layerMode = LayerModeEffects.Normal ;
		      break;
		    default:
		      Console.WriteLine("FillEvent-2: with {0} not supported!", 
					_mode.Value);
		      break;
		    }
		  x = y = 0;
		}
	      else
		{
		  x = _from.GetValueAsDouble("Hrzn");
		  y = _from.GetValueAsDouble("Vrtc");
		}

	      BucketFillMode fillMode;

	      switch (_using.Value)
		{
		case "BckC":
		  fillMode = BucketFillMode.Background;
		  break;
		case "Blck":
		  Context.Foreground = new RGB(0, 0, 0);
		  fillMode = BucketFillMode.Foreground;
		  break;
		case "FrgC":
		  fillMode = BucketFillMode.Foreground;
		  break;
		case "Ptrn":
		  fillMode = BucketFillMode.Pattern;
		  break;
		default:
		  Console.WriteLine("FillEvent-3: with {0} not supported!", 
				    _using.Value);
		  return false;
		}

	      drawable.EditBucketFill(fillMode, layerMode,
				      _opacity, _tolerance, false, true,
				      SelectCriterion.Composite, x, y);
	    }
	}

      Context.Pop();

      return true;
    }
  }
}
