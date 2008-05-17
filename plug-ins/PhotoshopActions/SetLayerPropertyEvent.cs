// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
//
// SetLayerPropertyEvent.cs
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
  public class SetLayerPropertyEvent : SetEvent
  {
    [Parameter("T")]
    ObjcParameter _objc;
    
    readonly string _name;

    public SetLayerPropertyEvent(SetEvent srcEvent) : base(srcEvent)
    {
      Parameters.Fill(this);
    }

    public SetLayerPropertyEvent(SetEvent srcEvent, string name) : 
      this(srcEvent)
    {
      _name = name;
    }

    public override string EventForDisplay
    {
      get 
	{
	  return base.EventForDisplay + ((_name == null) 
					 ? " current layer"
					 : " layer \"" + _name + "\"");
	}
    }

    override public bool Execute()
    {
      Layer layer = (_name == null) ? SelectedLayer 
	: ActiveImage.Layers["_name"];

      foreach (Parameter parameter in _objc.Parameters)
	{
	  switch (parameter.Name)
	    {
	    case "Md":
	      string mode = (parameter as EnumParameter).Value;
	      switch (mode)
		{
		case "CBrn":
		  layer.Mode = LayerModeEffects.Burn;
		  break;
		case "CDdg":
		  layer.Mode = LayerModeEffects.Dodge;
		  break;
		case "Dfrn":
		  layer.Mode = LayerModeEffects.Difference;
		  break;
		case "Drkn":
		  // TODO: not a perfect match
		  layer.Mode = LayerModeEffects.DarkenOnly;
		  break;
		case "HrdL":
		  layer.Mode = LayerModeEffects.Hardlight;
		  break;
		case "Lghn":
		  // TODO: not a perfect match
		  layer.Mode = LayerModeEffects.LightenOnly;
		  break;
		case "Mltp":
		  layer.Mode = LayerModeEffects.Multiply;
		  break;
		case "Nrml":
		  layer.Mode = LayerModeEffects.Normal;
		  break;
		case "Ovrl":
		  layer.Mode = LayerModeEffects.Overlay;
		  break;
		case "Scrn":
		  layer.Mode = LayerModeEffects.Screen;
		  break;
		case "Strt":
		  layer.Mode = LayerModeEffects.Saturation;
		  break;
		case "Xclu":
		  // Fix me: not the best match
		  layer.Mode = LayerModeEffects.Difference;
		  break;
		default:
		  Console.WriteLine("Implement set layer mode: " + mode);
		  break;
		}
	      break;
	    case "Nm":
	      layer.Name = (parameter as TextParameter).Value;
	      break;
	    case "Opct":
	      layer.Opacity = (parameter as DoubleParameter).Value;
	      break;
	    default:
	      Console.WriteLine("SetLayerPropertyEvent: " + parameter.Name);
	      break;
	    }
	}
      return true;
    }
  }
}
