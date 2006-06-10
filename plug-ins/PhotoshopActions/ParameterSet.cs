// The PhotoshopActions plug-in
// Copyright (C) 2006 Maurits Rijk
//
// ParameterSet.cs
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
using System.Collections.Generic;

namespace Gimp.PhotoshopActions
{
  public class ParameterSet
  {
    Dictionary<string, Parameter> _set = new Dictionary<string, Parameter>();

    public Parameter this[string name]
    {
      get {return _set[name];}
    }

    public void Parse(ActionParser parser, int numberOfItems)
    {
      for (int i = 0; i < numberOfItems; i++)
	{
	  ReadItem(parser);
	}
    }

    void ReadItem(ActionParser parser)
    {
      string key = parser.ReadTokenOrString();
      Console.WriteLine("\t\tkey: " + key);
		
      string type = parser.ReadFourByteString();
      Console.WriteLine("\t\ttype: " + type);

      Parameter parameter = null;

      if (type == "UntF")
	{
	  parameter = new DoubleParameter();
	  parameter.Parse(parser);

	  Console.WriteLine("\t\tval: " +(parameter as DoubleParameter).Value);

	}
      else if (type == "bool")
	{
	  parameter = new BoolParameter();
	  parameter.Parse(parser);

	  Console.WriteLine("\t\tval: " + (parameter as BoolParameter).Value);
	}
      else if (type == "enum")
	{
	  parameter = new EnumParameter();
	  parameter.Parse(parser);

	  Console.WriteLine("\t\tval: " + (parameter as EnumParameter).Value);
	}
      else if (type == "Enmr")
	{
	  parser.ParseEnmr();
	}
      else if (type == "long")
	{
	  int val = parser.ReadInt32();
	  Console.WriteLine("\t\tval: " + val);
	}
      else if (type == "VlLs")
	{
	  ReadVlLs(parser);
	}
      else
	{
	  Console.WriteLine("ReadItem: type {0} unknown!", type);
	  throw new GimpSharpException();
	}

      if (parameter != null)
	{
	  _set[key] = parameter;
	}
    }

    void ReadVlLs(ActionParser parser)
    {
      int number = parser.ReadInt32();
      Console.WriteLine("\t\tnumber: " + number);
      
      for (int i = 0; i < number; i++)
	{
	  string type = parser.ReadFourByteString();
	  Console.WriteLine("\t\ttype: " + type);
	  if (type == "Objc")
	    {
	      ReadDescriptor(parser);
	    }
	  else
	    {
	      Console.WriteLine("ReadVlLs: type {0} unknown!", type);
	      return;
	    }
	}
    }

    void ReadDescriptor(ActionParser parser)
    {
      string classID = parser.ReadUnicodeString();
      Console.WriteLine("\tClassID: " + classID);
      
      string classID2 = parser.ReadTokenOrString();
      Console.WriteLine("\tClassID2: " + classID2);

      int numberOfItems = parser.ReadInt32();
      Console.WriteLine("\tNumberOfItems: " + numberOfItems);
	  
      for (int i = 0; i < numberOfItems; i++)
	{
	  ReadItem(parser);
	}
    }
  }
}
