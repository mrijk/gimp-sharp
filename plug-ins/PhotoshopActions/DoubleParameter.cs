// The PhotoshopActions plug-in
// Copyright (C) 2006-2008 Maurits Rijk
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
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class DoubleParameter : Parameter
  {
    bool _hasUnits;
    public double Value {get; set;}
    public string Units {get; private set;}

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

    public override string Format()
    {
      switch (Units)
	{
	case "#Pxl":
	  return String.Format("{0}: {1} {2}", Abbreviations.Get(Name), 
			       Value, UnitString);
	default:
	  return String.Format("{0}: {1:F3}{2}", Abbreviations.Get(Name), 
			       Value, UnitString);
	}
    }

    string UnitString
    {
      get
	{
	  switch (Units)
	    {
	    case "#Prc":
	      return "%";
	    case "#Pxl":
	      return "pixels";
	    default:
	      Console.WriteLine("units: " + Units);
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
	default:
	  return Value;
	}
    }
  }
}
