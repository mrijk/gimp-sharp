// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// GradientClassEvent.cs
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
  public class GradientClassEvent : ActionEvent
  {
    [Parameter("From")]
    ObjcParameter _from;
    [Parameter("T")]
    ObjcParameter _to;
    [Parameter("Type")]
    EnumParameter _type;
    [Parameter("Dthr")]
    bool _dither;
    [Parameter("UsMs")]
    bool _useMask;
    [Parameter("With")]
    ListParameter _with;
    [Parameter("Grad")]
    ObjcParameter _gradient;

    public override bool IsExecutable
    {
      get {return _gradient != null || _with != null;}
    }

    protected override IEnumerable ListParameters()
    {
      yield return "Type: " + _type.Value;
      yield return ((_dither) ? "With" : "Without") + " Dither";
      yield return ((_useMask) ? "With" : "Without") + " Use Mask";
    }

    static public Gradient CreateGradient(string name, ListParameter colors)
    {
      Gradient gradient = new Gradient("Photoshop." + name);
      gradient.SegmentRangeSplitUniform(0, -1, colors.Count);

      int segment = 0;
      
      foreach (ObjcParameter parameter in colors)
	{
	  RGB color;
	  
	  string type = parameter.GetValueAsString("Type");
	  switch (type)
	    {
	    case "BckC":
	      color = Context.Background;
	      break;
	    case "FrgC":
	      color = Context.Foreground;
	      break;
	    case "UsrS":
	      color = parameter.GetValueAsColor("Clr");
		  break;
	    default:
	      Console.WriteLine("Gradient-1: " + type);
	      color = new RGB(0, 0, 0);
	      break;
	    }
	  
	  long location = parameter.GetValueAsLong("Lctn");
	  long midpoint = parameter.GetValueAsLong("Mdpn");
	  Console.WriteLine("type: {0}, location: {1}, midpoint: {2}", 
			    type, location, midpoint);
	  gradient.SegmentSetLeftPosition(segment, location / 4096.0);
	  gradient.SegmentSetLeftColor(segment, color, 100);
	  if (segment > 0)
	    {
	      gradient.SegmentSetRightColor(segment - 1, color, 100);
	    }
	  segment++;
	}    
      return gradient;
    }

    override public bool Execute()
    {
      double x1 = _from.GetValueAsDouble("Hrzn");
      double y1 = _from.GetValueAsDouble("Vrtc");

      double x2 = _to.GetValueAsDouble("Hrzn");
      double y2 = _to.GetValueAsDouble("Vrtc");

      Console.WriteLine("from ({0}, {1}) to ({2}, {3})", x1, y1, x2, y2);

      // Fix me!
      GradientType gradientType;
      switch (_type.Value)
	{
	case "Lnr":
	  gradientType = GradientType.Linear;
	  break;
	default:
	  Console.WriteLine("Gradient-2: " + _type.Value);
	  gradientType = GradientType.Linear;
	  break;	      
	}
      
      Gradient gradient;

      if (_gradient != null)
	{
	  string name = _gradient.GetValueAsString("Nm");
	  Console.WriteLine("Name: " + name);
	  ListParameter colors = _gradient.Parameters["Clrs"] as ListParameter;

	  gradient = CreateGradient(name, colors);
	}
      else if (_with != null)
	{
	  gradient = CreateGradient("Photoshop.Temp", _with);
	}
      else
	{
	  Console.WriteLine("Gradient-3");
	  return false;
	}

      Context.Push();
      Context.Gradient = gradient;
      ActiveDrawable.EditBlend(BlendMode.Custom, LayerModeEffects.Normal,
			       gradientType, 100.0, 0.0, RepeatMode.None,
			       false, false, 0, 0, _dither, 
			       x1, y1, x2, y2);
      Context.Pop();

      return true;
    }
  }
}
