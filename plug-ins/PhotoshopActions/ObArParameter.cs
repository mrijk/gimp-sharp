// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
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
using System.Reflection;

namespace Gimp.PhotoshopActions
{
  public class ObArParameter : Parameter
  {
    string _classID;
    string _classID2;

    public string ClassID2
    {
      get {return _classID2;}
    }

    public override void Parse(ActionParser parser)
    {
      int numberOfItems = parser.ReadInt32();
      DebugOutput.Dump("NumberOfItems: " + numberOfItems);

      _classID = parser.ReadUnicodeString();
      DebugOutput.Dump("ClassID: " + _classID);
      
      _classID2 = parser.ReadTokenOrString();
      DebugOutput.Dump("ClassID2: " + _classID2);

      int numberOfFields = parser.ReadInt32();
      DebugOutput.Dump("NumberOfItems: " + numberOfFields);

      for (int i = 0; i < numberOfFields; i++)
	{
	  string key = parser.ReadTokenOrString();
	  string type = parser.ReadFourByteString();
	  string units = parser.ReadFourByteString();
	  
	  DebugOutput.Dump("key: {0} ({1}) {2}", key, type, units);
	  
	  numberOfItems = parser.ReadInt32();
	  DebugOutput.Dump("NumberOfItems2: " + numberOfItems);
	  
	  switch (type)
	    {
	    case "UnFl":
	      for (int j = 0; j < numberOfItems; j++)
		{
		  parser.ReadDouble();
		}
	      break;
	    default:
	      Console.WriteLine("ObAr:Unknown type: " + type);
	      break;
	    }
	}

      // throw new GimpSharpException();
    }

    public override void Fill(Object obj, FieldInfo field)
    {
      field.SetValue(obj, this);
    }
  }
}
