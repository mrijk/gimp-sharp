// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// DoubleParameter.cs
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
using System.Collections.Generic;
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class DoubleParameter : Parameter
  {
    bool _hasUnits;
    public double Value {get; set;}
    public string Units {get; private set;}
    static public double Resolution {private get; set;}

    public DoubleParameter(bool hasUnits)
    {
      _hasUnits = hasUnits;
    }

    public override void Parse(ActionParser parser)
    {
      if (_hasUnits)
	{
	  Units = parser.ReadFourByteString();
	}
      Value = parser.ReadDouble();
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, Value);
      // TODO: also fill the units!?
    }

    public override IEnumerable<string> Format()
    {
      switch (Units)
	{
	case "#Pxl":
	  yield return String.Format("{0}: {1} {2}", UppercaseName, Value, 
				     UnitString);
	  break;
	case "#Rlt":
	  yield return String.Format("{0}: {1:F3} {2}", UppercaseName, 
				     Value / 72,
				     UnitString);
	  break;
	case "#Rsl":
	  yield return String.Format("{0}: {1} {2}", UppercaseName, 
				     Value, UnitString);
	  break;
	default:
	  yield return String.Format("{0}: {1:F3} {2}", UppercaseName, Value,
				     UnitString);
	  break;
	}
    }

    string UnitString
    {
      get
	{
	  switch (Units)
	    {
	    case "#Ang":
	      return "";	// Fix me: should be angle character
	    case "#Prc":
	      return "%";
	    case "#Pxl":
	      return "pixels";
	    case "#Rlt":
	      return "inches";
	    case "#Rsl":
	      return "per inch";
	    default:
	      if (Units != null)
		{
		  Console.WriteLine("units: " + Units);
		}
	      return "";
	    }
	}
    }

    public double GetPixels(double x)
    {
      if (Units == null)
	{
	  return Value;
	}

      switch (Units)
	{
	case "#Prc":
	  return Value * x / 100.0;
	case "#Rlt":
	  return Value * Resolution / 72;
	default:
	  return Value;
	}
    }
  }
}
