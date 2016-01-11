// The PhotoshopActions plug-in
// Copyright (C) 2006-2016 Maurits Rijk
//
// ObArParameter.cs
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
  public class ObArParameter : Parameter
  {
    string _classID;
    string _classID2;

    // Fix me: this is only working for point arrays!!!!
    CoordinateList<double> _value = new CoordinateList<double>();
    string _units;

    public string ClassID2 => _classID2;

    public CoordinateList<double> Value => _value;

    public string Units => _units;

    public override void Parse(ActionParser parser)
    {
      if (parser.PreSix)
	{
	  _classID2 = parser.ReadTokenOrString();
	  DebugOutput.Dump("ClassID2: " + _classID2);
	  
	  int numberOfItems = parser.ReadInt32();
	  DebugOutput.Dump("NumberOfItems: " + numberOfItems);
	}
      else
	{
	  int numberOfItems = parser.ReadInt32();
	  DebugOutput.Dump("NumberOfItems: " + numberOfItems);
	  
	  _classID = parser.ReadUnicodeString();
	  DebugOutput.Dump("ClassID: " + _classID);
	  
	  _classID2 = parser.ReadTokenOrString();
	  DebugOutput.Dump("ClassID2: " + _classID2);  
	}

      int numberOfFields = parser.ReadInt32();
      DebugOutput.Dump("NumberOfFields: " + numberOfFields);

      for (int i = 0; i < numberOfFields; i++)
	{
	  string key = parser.ReadTokenOrString();
	  string type = parser.ReadFourByteString();
	  _units = parser.ReadFourByteString();
	  
	  DebugOutput.Dump("key: {0} ({1}) {2}", key, type, _units);
	  
	  int numberOfItems = parser.ReadInt32();
	  DebugOutput.Dump("NumberOfItems2: " + numberOfItems);
	  
	  switch (type)
	    {
	    case "UnFl":
	      for (int j = 0; j < numberOfItems; j++)
		{
		  double val = parser.ReadDouble();

		  Coordinate<double> c;
		  if (i == 0)
		    {
		      c = new Coordinate<double>();
		      _value.Add(c);
		      c.X = val;
		    }
		  else
		    {
		      _value[j].Y = val;
		    }
		}
	      break;
	    default:
	      Console.WriteLine("ObAr:Unknown type: " + type);
	      break;
	    }
	}
    }

    public override IEnumerable<string> Format()
    {
      yield return "ObarParameter";
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }
  }
}
